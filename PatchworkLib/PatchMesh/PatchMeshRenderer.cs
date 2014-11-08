using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using FLib.SharpDX;
using DXColor = SharpDX.Color;

namespace PatchworkLib.PatchMesh
{
    public class PatchMeshRenderer : IDisposable
    {
        public bool rotateCamera = false;

        SharpDXInfo RenderInfo;
        Dictionary<string, Texture2D> textureDict = new Dictionary<string, Texture2D>();

        float time = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="size"></param>
        /// <param name="setDefaultRenderInfo">PatchMeshRenderer.RenderInfoをSharpDXHelperがデフォルトで使用するrendering infoとして設定するか</param>
        public PatchMeshRenderer(IntPtr handle, Size size, bool setDefaultRenderInfo)
        {
            RenderInfo = SharpDXHelper.Initialize(handle, size, rawVertices, rawIndices, new Matrix(), "test.png");
            if (setDefaultRenderInfo)
                SharpDXHelper.SetDefaultRenderInfo(RenderInfo);
        }

        public void BeginDraw()
        {
            if (rotateCamera)
                time += 0.02f;
            SharpDXHelper.BeginDraw(RenderInfo);   
        }

        public void EndDraw()
        {
            SharpDXHelper.EndDraw(RenderInfo);
        }

        /// <summary>
        /// cameraPositionは x, y : [-∞, +∞], z : [-∞, 0)
        /// x, y が大きいほどカメラが右下に移動する
        /// zが小さいほどズームアウトする
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="patchKey"></param>
        /// <param name="resources"></param>
        /// <param name="formSize"></param>
        /// <param name="cameraPosition">x, y : [-∞, +∞], z : [-∞, 0).x, y が大きいほどカメラが右下に移動する.zが小さいほどズームアウトする</param>
        public void Draw(PatchMesh mesh, string patchKey, PatchMeshRenderResources resources, Size formSize, Vector3 cameraPosition)
        {
            List<VertexPositionColorTexture> rawVertices = new List<VertexPositionColorTexture>();
            for (int i = 0; i < mesh.vertices.Count; i++)
            {
                Vector3 pos = vec3(mesh.vertices[i].position);
                pos.Y *= -1;
                DXColor col = DXColor.White;
                Vector2 coord = vec2(mesh.vertices[i].GetTexcoord(patchKey));
                rawVertices.Add(new VertexPositionColorTexture(pos, col, coord));
            }

            List<int> rawIndices = new List<int>();
            for (int i = 0; i < mesh.triangles.Count; i++)
            {
                if (mesh.triangles[i].TextureKey != patchKey)
                    continue;
                rawIndices.Add(mesh.triangles[i].Idx0);
                rawIndices.Add(mesh.triangles[i].Idx1);
                rawIndices.Add(mesh.triangles[i].Idx2);
            }

            var view = Matrix.LookAtLH(
                new Vector3(cameraPosition.X, -cameraPosition.Y, cameraPosition.Z), 
                new Vector3(cameraPosition.X, -cameraPosition.Y, 0),
                Vector3.UnitY);
            var proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, formSize.Width / (float)formSize.Height, 0.1f, 10000.0f);
            var viewProj = Matrix.Multiply(view, proj);
            var worldViewProj = Matrix.RotationX(time) * Matrix.RotationY(time * 2) * Matrix.RotationZ(time * .7f) * viewProj;
            worldViewProj.Transpose();

            var texture = resources.GetTexture(PatchMeshRenderResources.GenerateResourceKey(mesh, patchKey));

            SharpDXHelper.UpdateVertexBuffer(RenderInfo, rawVertices);
            SharpDXHelper.UpdateIndexBuffer(RenderInfo, rawIndices);
            SharpDXHelper.UpdateCameraBuffer(RenderInfo, worldViewProj);
            if (texture != null)
                SharpDXHelper.SwitchTexture(RenderInfo, texture);
            SharpDXHelper.DrawMesh(RenderInfo);
        }

        Vector2 vec2(PointF pt)
        {
            return new Vector2(pt.X, pt.Y);
        }

        Vector3 vec3(PointF pt)
        {
            return new Vector3(pt.X, pt.Y, 0);
        }

        public void Dispose()
        {
            if (RenderInfo != null)
                RenderInfo.Dispose();
        }

        #region デバッグ用
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
            0, 2, 4,
            0, 4, 1,

            3, 7, 6,
            3, 5, 7,
            
            2, 6, 7,
            2, 7, 4,

            0, 5, 3,
            0, 1, 5,

            0, 3, 6,
            0, 6, 2,

            1, 7, 5,
            1, 4, 7,
        };
        #endregion
    }
}