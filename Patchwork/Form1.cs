
//
// TODO: スケルトンを動かすことでメッシュを操作
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

        PatchMeshRenderer renderer;
        PatchMeshRenderResources resources = new PatchMeshRenderResources();
        Vector3 cameraPosition = new Vector3(100, 100, -1000);

        PatchSkeletalMesh rawPatch1, rawPatch2;
        PatchSkeleton refSkeleton;
        PatchSkeletalMesh patch;
        List<string> patchKeys = new List<string>();

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

            renderer = new PatchMeshRenderer(canvas.Handle, canvas.ClientSize, true);

            // Magic2Dで作ったセグメントをロードする
            Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
            Dictionary<string, PatchSkeletalMesh> dict = Magic2D.SegmentToPatch.LoadPatches(".", "3_segmentation", bitmaps);

            // テクスチャをアセットに登録
            foreach (var kv in bitmaps)
                resources.Add(kv.Key, SharpDXHelper.BitmapToTexture(kv.Value));

            // ２つのパッチを結合する
            rawPatch1 = dict["scr1.part1"];
            rawPatch2 = dict["scr1.part3"];

            refSkeleton = PatchSkeleton.Load("refSkeleton.skl");

            patch = PatchConnector.Connect(rawPatch1, rawPatch2, refSkeleton, resources);
                      
            patchKeys.Add("scr1.part1");
            patchKeys.Add("scr1.part3");

            patch.Mesh.BeginDeformation();

#if DEBUG
            PatchSkeletalMeshRenderer.ToBitmap(rawPatch1).Save("output/rawPatch1.png");
            PatchSkeletalMeshRenderer.ToBitmap(rawPatch2).Save("output/rawPatch2.png");
            PatchSkeletalMeshRenderer.ToBitmap(patch).Save("output/patch.png");
#endif

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

       //     foreach (var key in patchKeys)
         //       renderer.DrawMesh(patch.Mesh, key, resources, canvas.ClientSize, cameraPosition + new Vector3(0, 0, 0));

            foreach (var key in patchKeys)
                renderer.DrawWireframe(patch.Mesh, key, DXColor.LightGray, canvas.ClientSize, cameraPosition + new Vector3(0, 0, 0));
            
            for (int i = 0; i < 100; i++)
                renderer.DrawPoint(new PointF(100 * (i % 10), 100 * (i / 10)), DXColor.Green, canvas.ClientSize, cameraPosition);

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
