using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using PatchworkLib.PatchMesh;

namespace Magic2D
{
    public class SegmentToPatch
    {
        public static Dictionary<string, PatchMesh> LoadPatches(string root, string segmentationDirName, Dictionary<string, Bitmap> bitmaps)
        {
            var segmentDict = SegmentLoader.LoadSegments(root, segmentationDirName);
            Dictionary<string, PatchMesh> patchDict = new Dictionary<string, PatchMesh>();
            foreach (var kv in segmentDict)
            {
                patchDict[kv.Value.name] = ToPatchMesh(kv.Value, bitmaps);
                kv.Value.Dispose();
            }
            return patchDict;
        }

        // todo: segmentからpatchmeshへの変換
        // seg.bmpはどこに格納する?patchmesh?pathcmeshrenderer?
        // => pathcMeshRendererResourcesに格納する
        static PatchMesh ToPatchMesh(Segment seg, Dictionary<string, Bitmap> bitmaps)
        {
            List<PointF> rawPath = new List<PointF>(seg.path);
            // メッシュを生成
            var shiftPath = ShiftPath(seg.path, -seg.offset.X, -seg.offset.Y);
            var subdividedPath = PathSubdivision.Subdivide(shiftPath, 10);
            subdividedPath.RemoveAt(subdividedPath.Count - 1); // 終点（始点と同じ）は消す

            List<PointF> vertices = new List<PointF>();
            List<int> indices = new List<int>();
            FLib.Triangle.Triangulate(subdividedPath, vertices, indices, FLib.Triangle.Parameters.Default);

            List<int> path = Enumerable.Range(0, subdividedPath.Count).ToList();

            // 正しい?
            for (int i = 0; i < path.Count; i++)
            {
                System.Diagnostics.Debug.Assert(vertices[path[i]].X == subdividedPath[i].X);
                System.Diagnostics.Debug.Assert(vertices[path[i]].Y == subdividedPath[i].Y);
            }

            // todo
            List<int> vert2part = Enumerable.Repeat(0, subdividedPath.Count).ToList();
            
            string patchKey = seg.name;
            SizeF textureSize = seg.bmp.Size;

            var patch = new PatchMesh(vertices, indices, path, vert2part, patchKey, textureSize);

            // todo
            // pathcMeshRendererResourcesに格納する
            if (seg.bmp != null)
                bitmaps[PatchMeshRenderResources.GenerateResourceKey(patch, patchKey)] = new Bitmap(seg.bmp);

            return patch;
        }

        static List<PointF> ShiftPath(List<PointF> path, float offsetx, float offsety)
        {
            if (path == null)
                return new List<PointF>();
            List<PointF> _path = new List<PointF>();
            for (int i = 0; i < path.Count; i++)
                _path.Add(new PointF(path[i].X + offsetx, path[i].Y + offsety));
            return _path;
        }

    }
}
