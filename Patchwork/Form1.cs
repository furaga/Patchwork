
//
// TODO: canvas上のパッチの選択、削除、結合、分解
// TODO: スケルトンが動かしづらい
//
// todo: 3つ以上メッシュをつなげたときに動くか => OK?

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Windows;
using PatchworkLib.PatchMesh;
using FLib.SharpDX;
using FLib;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using DXColor = SharpDX.Color;

namespace Patchwork
{
    public partial class Form1 : Form
    {
        #region for debug
        /*
        static VertexPositionColorTexture[] rawVertices = new[]
        {
            new VertexPositionColorTexture(new Vector3(-1,-1,-1), DXColor.Red, new Vector2(0, 0)),
            new VertexPositionColorTexture(new Vector3(1,-1,-1), DXColor.Blue, new Vector2(1, 0)),
            new VertexPositionColorTexture(new Vector3(-1,1,-1), DXColor.Yellow, new Vector2(1, 0)),
            new VertexPositionColorTexture(new Vector3(-1,-1,1), DXColor.Green, new Vector2(1, 1)),
            new VertexPositionColorTexture(new Vector3(1,1,-1), DXColor.Purple, new Vector2(1, 1)),
            new VertexPositionColorTexture(new Vector3(1,-1,1), DXColor.Cyan, new Vector2(0, 1)),
            new VertexPositionColorTexture(new Vector3(-1,1,1), DXColor.White, new Vector2(1, 0)),
            new VertexPositionColorTexture(new Vector3(1,1,1), DXColor.LightGreen, new Vector2(0, 0)),
        };
        static int[] rawIndices = new[]
        {
            0, 2, 4,    0, 4, 1,
            3, 7, 6,    3, 5, 7,            
            2, 6, 7,    2, 7, 4,
            0, 5, 3,    0, 1, 5,
            0, 3, 6,    0, 6, 2,
            1, 7, 5,    1, 4, 7,
        };
         
        static int[] pathIndices = new[] { 1, 2, 3, 4 };
         */
        #endregion //for debug

        class SaveObjects
        {
            public float cameraX, cameraY, cameraZ;
            public PatchSkeleton refSkeleton;
            //            public Dictionary<string, List<string>> texturePathKeys = new Dictionary<string,List<string>>();
//            public Dictionary<string, RenderQuery> renderQueryPool = new Dictionary<string, RenderQuery>();
//            public List<RenderQuery> renderPatchQueries = new List<RenderQuery>();
        }

        //---------------------------------------------------------
        // 描画
        //---------------------------------------------------------

        class RenderQuery
        {
            public PatchSkeletalMesh patch;
            public PatchTree patchTree;
            public List<string> patchKeys;
            public RenderQuery(PatchSkeletalMesh patch, PatchTree patchTree, List<string> patchKeys)
            {
                this.patch = patch;
                this.patchKeys = new List<string>(patchKeys);
            }
        }

        /// <summary>
        /// 現在のパッチがどのように生成されたか。パッチを分解したりするときに使う
        /// </summary>
        class PatchTree
        {
            readonly string patchKey = null;
            readonly PatchTree subtree1 = null;
            readonly PatchTree subtree2 = null;
            public PatchTree(string patchKey)
            {
                this.patchKey = patchKey;
            }
            public PatchTree(PatchTree t1, PatchTree t2)
            {
                this.subtree1 = t1;
                this.subtree2 = t2;
            }
        }


        // 描画系
        PatchMeshRenderer renderer;
        PatchMeshRenderResources resources = new PatchMeshRenderResources();
        public Dictionary<SharpDX.Direct3D11.Texture2D, Bitmap> tex2bmp = new Dictionary<SharpDX.Direct3D11.Texture2D, Bitmap>();
        Vector3 cameraPosition = new Vector3(400, 400, -2000);


        // 変形先の骨格
        PatchSkeleton refSkeleton;

        // インタラクション
        List<RenderQuery> selectingPatchQueries = new List<RenderQuery>();

        Dictionary<string, RenderQuery> renderQueryPool = new Dictionary<string, RenderQuery>();
        List<RenderQuery> renderPatchQueries = new List<RenderQuery>();

        int combinedCount = 0;

        //---------------------------------------------------------

        public Form1()
        {
            InitializeComponent();

            List<PointF> input = new List<PointF>();
            List<PointF> outVert = new List<PointF>();
            List<int> indices = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                input.Add(new PointF(
                    500 * (float)Math.Cos(Math.PI * 2 * i / 100),
                    500 * (float)Math.Sin(Math.PI * 2 * i / 100)));
            }
            Triangle.Triangulate(input, outVert, indices, new Triangle.Parameters(false));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //            FLib.FileManager.OpenExplorer("output");

            canvas.AllowDrop = true;

            renderer = new PatchMeshRenderer(canvas.Handle, canvas.ClientSize, true);

            // Magic2Dで作ったセグメントをロードしてパッチに変換
            Dictionary<string, PatchSkeletalMesh> dict;
            Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();

            dict = Magic2D.SegmentToPatch.LoadPatches("../../../../..", "Patchwork_resources/GJ_ED3/3_segmentation", bitmaps, 2);            
            //dict = Magic2D.SegmentToPatch.LoadPatches(".", "3_segmentation", bitmaps, 4);

            System.Diagnostics.Debug.Assert(dict.Count == bitmaps.Count);


            // テクスチャをアセットに登録
            foreach (var kv in bitmaps)
            {
                var tex = SharpDXHelper.BitmapToTexture(kv.Value);
                resources.Add(kv.Key, tex);
                tex2bmp[tex] = kv.Value;
            }
            // パッチをキューに入れる
            for (int i = 0; i < dict.Count; i++)
            {
                string resourceKey = bitmaps.Keys.ElementAt(i);
                string textureKey = dict.Keys.ElementAt(i);
                PatchSkeletalMesh patch = dict.Values.ElementAt(i);
                renderQueryPool[resourceKey] = new RenderQuery(patch, new PatchTree(resourceKey), new List<string>() { textureKey });
            }


            // パッチをリストに表示する
            foreach (var kv in bitmaps)
            {
                patchImageList.Images.Add(kv.Key, kv.Value);
                patchView.Items.Add(kv.Key, kv.Key, kv.Key);
            }
            
            refSkeleton = PatchSkeleton.Load("refSkeleton.skl");
            
            canvas.MouseWheel += canvas_MouseWheel;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            DrawCanvas();
        }

        void DrawCanvas()
        {
            if (renderer == null)
                return;

            // renderer.rotateCamera = true;
            renderer.BeginDraw();

            foreach (var query in renderPatchQueries)
            {
                DXColor color = DXColor.White;
                if (selectingPatchQueries.Contains(query)) // 選択中は赤くする
                    color = DXColor.Red;
                foreach (var key in query.patchKeys)
                    renderer.DrawMesh(query.patch.Mesh, key, resources, color, canvas.ClientSize, cameraPosition);
            }

            for (int i = 0; i < 100; i++)
                renderer.DrawPoint(new PointF(100 * (i % 10), 100 * (i / 10)), 4, DXColor.Green, canvas.ClientSize, cameraPosition);

            foreach (var b in refSkeleton.bones)
                renderer.DrawLine(b.src.position, b.dst.position, 4, DXColor.White, canvas.ClientSize, cameraPosition);

            foreach (var j in refSkeleton.joints)
                renderer.DrawPoint(j.position, 8, DXColor.Yellow, canvas.ClientSize, cameraPosition);

            renderer.EndDraw();
        }

        // この中で描画するとバッファリングのせい？で画面がちらつく
        private void canvas_Paint(object sender, PaintEventArgs e)
        {
        }

        //--------------------------------------------------------------------
        // canvas内でのマウス操作
        //--------------------------------------------------------------------

        PointF prevMousePos = new PointF();
        PatchSkeletonJoint draggedJoint = null;

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            // マウスホイールができるようにフォーカスを当てる
            canvas.Focus();

            // 関節の移動
            draggedJoint = null;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Vector3 p = renderer.Unproject(e.Location, 0, canvas.ClientSize, cameraPosition);
                draggedJoint = refSkeleton.GetNearestJoint(new PointF(p.X, p.Y), 30, new System.Drawing.Drawing2D.Matrix());
            }

            // パッチの選択
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (draggedJoint == null)
                {
                    // 関節以外の部分がクリックされたらパッチの選択をチェックする
                    Vector3 p = renderer.Unproject(e.Location, 0, canvas.ClientSize, cameraPosition);
                    bool anyHit = false;
                    foreach (var q in renderPatchQueries)
                    {
                        // このパッチ（クエリ）がクリックされたら選択状態をトグルする
                        if (PatchMeshCollision.IsHit(q.patch.Mesh, new PointF(p.X, p.Y)))
                        {
                            anyHit = true;
                            if (selectingPatchQueries.Contains(q))
                                selectingPatchQueries.Remove(q);
                            else
                                selectingPatchQueries.Add(q);
                        }
                    }
                    if (!anyHit)
                    {
                        // なにもないところをクリックしたら選択解除
                        selectingPatchQueries.Clear();
                    }
                }
            }

            // マウスドラッグ用にクリック位置を保存
            prevMousePos = e.Location;
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            prevMousePos = PointF.Empty;

            draggedJoint = null;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (draggedJoint != null)
                {
                    Vector3 p = renderer.Unproject(e.Location, 0, canvas.ClientSize, cameraPosition);
                    draggedJoint.position = new PointF(p.X, p.Y);
                    foreach (var query in renderPatchQueries)
                        PatchSkeletonFitting.Fitting(query.patch, refSkeleton);
                }
            }


            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                PointF pos = e.Location;
                if (!prevMousePos.IsEmpty)
                {
                    // fix: ズーム度合いに合わせて移動量を増減させる
                    cameraPosition.X -= pos.X - prevMousePos.X;
                    cameraPosition.Y -= pos.Y - prevMousePos.Y;
                }
                prevMousePos = pos;
            }
        }

        void canvas_MouseWheel(object sender, MouseEventArgs e)
        {
            cameraPosition.Z += e.Delta * 0.3f;
        }


        //
        private void patchView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (patchView.SelectedItems.Count >= 0)
                AddQuery(patchView.SelectedItems[0].ImageKey);
        }

        private void patchView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            patchView.DoDragDrop(e.Item as ListViewItem, DragDropEffects.Move);
        }

        private void patchVieww_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void canvas_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void canvas_DragDrop(object sender, DragEventArgs e)
        {
            // リストビューからドラッグアンドドロップされた
            if (e.Data.GetData(typeof(ListViewItem)) != null)
            {
                List<string> imageKeys = new List<string>();
                var item = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;
                if (item != null)
                    AddQuery(item.ImageKey);
            }
        }

        void AddQuery(string resourceKey)
        {
            if (renderQueryPool.ContainsKey(resourceKey))
            {
                Console.WriteLine("D&D: " + resourceKey);
                var _q = renderQueryPool[resourceKey];
                var q = new RenderQuery(PatchSkeletalMesh.Copy(_q.patch), _q.patchTree, _q.patchKeys);
                resources.DuplicateResources(_q.patch.Mesh, q.patch.Mesh);

                // 初めて登録される場合、ARAP変形を有効にしてスケルトにフィットさせる
                if (!renderPatchQueries.Contains(q))
                {
                    q.patch.Mesh.BeginDeformation();
                    PatchSkeletonFitting.Fitting(q.patch, refSkeleton);
                }

                if (renderPatchQueries.Contains(q))
                    renderPatchQueries.Remove(q);
                renderPatchQueries.Add(q);

                // 追加したパッチを選択モードにする
                selectingPatchQueries.Add(q);
            }
        }

        // selectingPatchesをすべて結合する
        private void combineCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newKey =  "combined" + combinedCount;
            PatchSkeletalMesh newMesh = selectingPatchQueries[0].patch;
            PatchTree newTree = selectingPatchQueries[0].patchTree;
            HashSet<string> patchKeySet = new HashSet<string>();
            for (int i = 1; i < selectingPatchQueries.Count; i++)
            {
                newMesh = PatchConnector.Connect(newMesh, selectingPatchQueries[i].patch, refSkeleton, resources);
                newTree = new PatchTree(newTree, selectingPatchQueries[i].patchTree);
            }
            for (int i = 0; i < selectingPatchQueries.Count; i++)
            {
                foreach (string k in selectingPatchQueries[i].patchKeys)
                    patchKeySet.Add(k);
            }
            if (newMesh == null)
                return;

            newMesh.Mesh.BeginDeformation();

            renderQueryPool.Add(newKey, new RenderQuery(newMesh, newTree, patchKeySet.ToList()));
            combinedCount++;


            foreach (var q in selectingPatchQueries)
                renderPatchQueries.Remove(q);
            renderPatchQueries.Add(renderQueryPool[newKey]);

            selectingPatchQueries.Clear();
        }

        private void deleteDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var q in selectingPatchQueries)
            {
                renderPatchQueries.Remove(q);
                resources.RemoveResources(q.patch.Mesh);
            }
            selectingPatchQueries.Clear();
        }

        private void scaleSlider_Scroll(object sender, EventArgs e)
        {
            float scale = scaleSlider.Value * 0.1f;
            foreach (var q in selectingPatchQueries)
            {
                q.patch.Scale(scale, scale);
                PatchSkeletonFitting.Fitting(q.patch, refSkeleton);
            }
        }

        // スケルトンとカメラ位置だけ保存・復元する
        private void saveSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "*.xml|*.xml";
            if (saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            string dir = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);
            string filename = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName);

            var save = new SaveObjects()
            {
                cameraX = cameraPosition.X,
                cameraY = cameraPosition.Y,
                cameraZ = cameraPosition.Z,
                refSkeleton = refSkeleton
            };
            
            FLib.ForceSerializer.Serialize(dir, save, filename);
        }

        private void openOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "*.xml|*.xml";
            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            string dir = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
            string filename = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);

            var saved = FLib.ForceSerializer.Deserialize<SaveObjects>(dir, filename);
            cameraPosition = new Vector3(saved.cameraX, saved.cameraY, saved.cameraZ);
            refSkeleton = saved.refSkeleton;

        }

    }

}