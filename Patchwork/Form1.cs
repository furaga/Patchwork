
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
using SharpDX.Direct3D11;
using SharpDX.DXGI;
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

        PatchMeshRenderer renderer;
        PatchMeshRenderResources resources = new PatchMeshRenderResources();
        Vector3 cameraPosition = new Vector3(400, 400, -2000);

        PatchSkeleton refSkeleton;
        /*
        PatchSkeletalMesh rawPatch1, rawPatch2, rawPatch3;
        PatchSkeletalMesh patch;
        List<string> patchKeys = new List<string>();
        */

        Dictionary<string, RenderQuery> renderQueryPool = new Dictionary<string, RenderQuery>();
        List<RenderQuery> renderQueries = new List<RenderQuery>();

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
            dict = Magic2D.SegmentToPatch.LoadPatches(".", "3_segmentation", bitmaps);

            System.Diagnostics.Debug.Assert(dict.Count == bitmaps.Count);


            // テクスチャをアセットに登録
            foreach (var kv in bitmaps)
                resources.Add(kv.Key, SharpDXHelper.BitmapToTexture(kv.Value));


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

            //
            // for test:
            //
            /*
            // ２つのパッチを結合する
            rawPatch1 = dict["scr1.part1"]; // 胴体
            rawPatch2 = dict["scr1.part3"]; // 肩周り
            rawPatch3 = dict["scr9.part00"]; // 左腕


            patch = PatchConnector.Connect(rawPatch1, rawPatch2, refSkeleton, resources);
#if DEBUG
            PatchSkeletalMeshRenderer.ToBitmap(rawPatch1).Save("output/rawPatch1.png");
            PatchSkeletalMeshRenderer.ToBitmap(rawPatch2).Save("output/rawPatch2.png");
            PatchSkeletalMeshRenderer.ToBitmap(patch, showPath:true).Save("output/patch1-2.png");
#endif
            patch = PatchConnector.Connect(patch, rawPatch3, refSkeleton, resources);
#if DEBUG
            PatchSkeletalMeshRenderer.ToBitmap(rawPatch3).Save("output/rawPatch3.png");
            PatchSkeletalMeshRenderer.ToBitmap(patch, showPath: true).Save("output/patch1-2-3.png");
#endif
//            patch = PatchConnector.Connect(rawPatch3, patch, refSkeleton, resources);
//            patch = PatchConnector.Connect(rawPatch2, rawPatch3, refSkeleton, resources);

            patchKeys.Add("scr1.part1");
            patchKeys.Add("scr1.part3");
            patchKeys.Add("scr9.part00");

            patch.Mesh.BeginDeformation();
*/



            canvas.MouseWheel += canvas_MouseWheel;
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            DrawCanvas();
        }

        //
        // メッシュを描画
        //
        void DrawCanvas()
        {
            if (renderer == null)
                return;

            //            renderer.rotateCamera = true;
            renderer.BeginDraw();

            foreach (var query in renderQueries)
                foreach (var key in query.patchKeys)
                    renderer.DrawMesh(query.patch.Mesh, key, resources, canvas.ClientSize, cameraPosition);

            //            foreach (var key in patchKeys)
            //                renderer.DrawMesh(patch.Mesh, key, resources, canvas.ClientSize, cameraPosition + new Vector3(0, 0, 0));

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

            draggedJoint = null;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Vector3 p = renderer.Unproject(e.Location, 0, canvas.ClientSize, cameraPosition);
                draggedJoint = refSkeleton.GetNearestJoint(new PointF(p.X, p.Y), 30, new System.Drawing.Drawing2D.Matrix());
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
                    foreach (var query in renderQueries)
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
                var q = renderQueryPool[resourceKey];

                // 初めて登録される場合、ARAP変形を有効にしてスケルトにフィットさせる
                if (!renderQueries.Contains(q))
                {
                    q.patch.Mesh.BeginDeformation();
                    PatchSkeletonFitting.Fitting(renderQueryPool[resourceKey].patch, refSkeleton);
                }

                if (renderQueries.Contains(q))
                    renderQueries.Remove(q);
                renderQueries.Add(q);
            }
        }

    }

}