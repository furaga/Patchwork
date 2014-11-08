
//
// TODO: 適当な２つのパッチを読み込んでつないでみる
//


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

        List<PatchSkeletalMesh> patchesList = new List<PatchSkeletalMesh>();
        List<string> patchKeyList = new List<string>();
        PatchMeshRenderer renderer;
        PatchMeshRenderResources resources = new PatchMeshRenderResources();
        Vector3 cameraPosition = new Vector3(100, 100, -1000);

        int mesh1, mesh2, mesh3;


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
            FLib.FileManager.OpenExplorer("output");

            renderer = new PatchMeshRenderer(canvas.Handle, canvas.ClientSize, true);

            // Magic2Dで作ったセグメントをロードする
            Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
            Dictionary<string, PatchSkeletalMesh> dict = Magic2D.SegmentToPatch.LoadPatches(".", "3_segmentation", bitmaps);

            int i = 0;
            foreach (var kv in dict)
            {
                patchesList.Add(kv.Value);
                patchKeyList.Add(kv.Key);

                if (kv.Key == "scr1.part1")
                    mesh1 = i;
                if (kv.Key == "scr1.part3")
                    mesh2 = i;
                i++;
            }


            //
            // for test
            //
            PatchSkeleton refSkeleton = PatchSkeleton.Load("refSkeleton.skl");
            var newMesh = PatchConnector.Connect(patchesList[mesh1], patchesList[mesh2], refSkeleton);
            patchesList.Add(newMesh);
            mesh3 = patchesList.Count - 1;

            var key31 =PatchMeshRenderResources.GenerateResourceKey(patchesList[ mesh3].Mesh, patchKeyList[mesh1]);
            var key32 =PatchMeshRenderResources.GenerateResourceKey(patchesList[ mesh3].Mesh, patchKeyList[mesh2]);
            var key11 =PatchMeshRenderResources.GenerateResourceKey(patchesList[ mesh1].Mesh, patchKeyList[mesh1]);
            var key22 =PatchMeshRenderResources.GenerateResourceKey(patchesList[ mesh2].Mesh, patchKeyList[mesh2]);
            resources.Add(key31, SharpDXHelper.BitmapToTexture(bitmaps[key11]));
            resources.Add(key32, SharpDXHelper.BitmapToTexture(bitmaps[key22]));

            // arap
            patchesList[mesh3].Mesh.BeginDeformation();


            foreach (var kv in bitmaps)
                resources.Add(kv.Key, SharpDXHelper.BitmapToTexture(kv.Value));


            PatchSkeletalMeshRenderer.ToBitmap(patchesList[mesh1]).Save("output/mesh1.png");
            PatchSkeletalMeshRenderer.ToBitmap(patchesList[mesh2]).Save("output/mesh2.png");


            canvas.MouseWheel += canvas_MouseWheel;

            
        }

        PointF pt1 = new PointF(150, 200);
        PointF pt2 = new PointF(350, 200);
        PointF pt3 = new PointF(150, 500);
        PointF pt4 = new PointF(350, 500);
        PointF pt5 = new PointF(150, 700);
        PointF pt6 = new PointF(350, 700);
        float time = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            time += 0.1f;
            float angle = time;
            float r = 30;
            PointF newpt3 = new PointF(150 + r * (float)Math.Cos(angle), 500 + r * (float)Math.Sin(angle));
            patchesList[mesh3].Mesh.TranslateControlPoint(pt3, newpt3, true);
            if (time <= 0.3)
                PatchSkeletalMeshRenderer.ToBitmap(patchesList[mesh3]).Save("output/mesh_combine.png");
            pt3 = newpt3;

            DrawCanvas();
        }

        //
        // メッシュを描画
        //
        void DrawCanvas()
        {
            if (renderer == null || patchesList == null)
                return;

//　           renderer.rotateCamera = true;
            renderer.BeginDraw();
//            renderer.Draw(patchesList[mesh1].Mesh, patchKeyList[mesh1], resources, canvas.ClientSize, cameraPosition);
            renderer.Draw(patchesList[mesh3].Mesh, patchKeyList[mesh1], resources, canvas.ClientSize, cameraPosition + new Vector3(0, 300, 0));
            renderer.Draw(patchesList[mesh3].Mesh, patchKeyList[mesh2], resources, canvas.ClientSize, cameraPosition + new Vector3(0, 300, 0));
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

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            // マウスホイールができるようにフォーカスを当てる
            canvas.Focus();

            // マウスドラッグ用にクリック位置を保存
            prevMousePos = e.Location;
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            prevMousePos = PointF.Empty;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
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


        private void segmentView_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

    }
  
}
