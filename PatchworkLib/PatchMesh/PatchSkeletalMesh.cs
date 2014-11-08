using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using PatchSection = System.Drawing.CharacterRange;

namespace PatchworkLib.PatchMesh
{
    /// <summary>
    /// パッチのポリゴン(PatchMesh)）と骨格(PatchSkeleton)を持つクラス
    /// パッチの接続はこれを使って行う
    /// </summary>
    public class PatchSkeletalMesh
    {
        public PatchMesh Mesh { get { return mesh;  } }

        internal PatchMesh mesh;
        internal PatchSkeleton skl;
        internal List<PatchSection> sections = new List<PatchSection>();
        internal Dictionary<PatchSkeletonBone, List<PointF>> skeletalControlPointDict = new Dictionary<PatchSkeletonBone, List<PointF>>();

        // コピーせずに参照をそのまま格納する
        public PatchSkeletalMesh(PatchMesh mesh, PatchSkeleton skl, List<PatchSection> sections)
        {
            this.mesh = mesh;
            this.skl = skl;
            this.sections = new List<PatchSection>(sections);
            this.skeletalControlPointDict = CreateSkeletalControlPoints(skl, 30);

            // 既存の制御点を消して、スケルトン周りの制御点を追加
            this.mesh.ClearControlPoints();
            foreach (var ls in this.skeletalControlPointDict.Values)
                foreach (var pt in ls)
                    this.mesh.AddControlPoint(pt, pt);
        }

        public static PatchSkeletalMesh Copy(PatchSkeletalMesh org)
        {
            var m = PatchMesh.Copy(org.mesh);
            var s = PatchSkeleton.Copy(org.skl);
            PatchSkeletalMesh copy = new PatchSkeletalMesh(m, s, org.sections);
            return copy;
        }

        static Dictionary<PatchSkeletonBone, List<PointF>> CreateSkeletalControlPoints(PatchSkeleton skl, int linearSpan)
        {
            Dictionary<PatchSkeletonBone, List<PointF>> dict = new Dictionary<PatchSkeletonBone, List<PointF>>();
            // ボーン沿いに制御点を追加
            if (skl != null && linearSpan >= 1)
            {
                foreach (var b in skl.bones)
                {
                    dict[b] = new List<PointF>();
                    float dist =FLib. FMath.Distance(b.src.position, b.dst.position);
                    int ptNum = Math.Max(2, (int)(dist / linearSpan) + 1);
                    for (int i = 0; i < ptNum; i++)
                    {
                        float t = (float)i / (ptNum - 1);
                        // 誤差の問題があるので i == 0, ptNum - 1のときは明示的に端点を代入する
                        PointF p;
                        if (i == 0)
                            p = b.src.position;
                        else if (i == ptNum - 1)
                            p = b.dst.position;
                        else
                            p = FLib.FMath.Interpolate(b.src.position, b.dst.position, t);
                        dict[b].Add(p);
                    }
                }
            }
            return dict;
        }

    }
}
