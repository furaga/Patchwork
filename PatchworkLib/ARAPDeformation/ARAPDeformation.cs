using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using FLib;

namespace PatchworkLib.ARAPDeformation
{
    class ARAPDeformation
    {
        List<PointF> meshPointList = new List<PointF>();
        List<PointF> orgMeshPointList = new List<PointF>();
        List<int> meshPtToPart = new List<int>();

        List<PointF> controls = new List<PointF>();
        List<PointF> orgControls = new List<PointF>();
        List<int> controlsToPart = new List<int>();

        float[] weights;
        float[] A00;
        float[] A01;
        float[] A10;
        float[] A11;
        PointF[] D;

        public ARAPDeformation(List<PointF> orgPoints, List<int> pointToPart)
        {
            this.meshPointList = new List<PointF>(orgPoints);
            this.orgMeshPointList = new List<PointF>(orgPoints);
            this.meshPtToPart = new List<int>(pointToPart);
        }

        public void ClearControlPoints()
        {
            controls.Clear();
            controlsToPart.Clear();
            orgControls.Clear();
            weights = A00 = A01 = A10 = A11 = null;
            D = null;
        }

        public void AddControlPoint(PointF pt, PointF orgPt)
        {
            if (controls.Contains(pt))
                return;

            controls.Add(pt);
            orgControls.Add(orgPt);

            int minIdx;
            FMath.GetMinElement(meshPointList, p => FMath.SqDistance(p, orgPt), out minIdx);

            if (minIdx >= 0)
                controlsToPart.Add(meshPtToPart[minIdx]);
            else
                controlsToPart.Add(-1);
        }

        public void RemoveControlPoint(PointF pt)
        {
            if (controls.Contains(pt))
            {
                orgControls.RemoveAt(controls.IndexOf(pt));
                controls.Remove(pt);
            }
        }

        public void TranslateControlPoint(PointF pt, PointF to, bool flush)
        {
            if (!controls.Contains(pt))
                return;
            controls[controls.IndexOf(pt)] = to;
            if (flush)
                FlushDefomation();
        }

        public void FlushDefomation()
        {
            RigidMLS();
        }

        public void BeginDeformation()
        {
            Precompute();
        }

        public void EndDeformation()
        {
            weights = null;
            A00 = null;
            A01 = null;
            A10 = null;
            A11 = null;
            D = null;
        }

        void Precompute()
        {
            if (controls.Count < 3)
                return;

            weights = new float[meshPointList.Count * controls.Count];
            A00 = new float[meshPointList.Count * controls.Count];
            A01 = new float[meshPointList.Count * controls.Count];
            A10 = new float[meshPointList.Count * controls.Count];
            A11 = new float[meshPointList.Count * controls.Count];
            D = new PointF[meshPointList.Count];

            for (int vIdx = 0; vIdx < meshPointList.Count; vIdx++)
            {
                int offset = vIdx * controls.Count;
                for (int i = 0; i < controls.Count; i++)
                {
                    if (meshPtToPart[vIdx] == controlsToPart[i])
                    {
                        // ぴったり同じ位置だったら無限の重みを与える
                        if (FMath.Distance(orgControls[i], orgMeshPointList[vIdx]) <= 1e-4)
                            weights[i + offset] = float.PositiveInfinity;
                        else
                            weights[i + offset] = (float)(1 / (0.01 + Math.Pow(FMath.Distance(orgControls[i], orgMeshPointList[vIdx]), 2)));
                    }
                    else
                    {
                        weights[i + offset] = 0;
                    }
                }

                PointF? Pa = AverageWeight(orgControls, weights, vIdx);
                if (Pa == null || !Pa.HasValue)
                    return;

                PointF[] Ph = new PointF[orgControls.Count];
                for (int i = 0; i < orgControls.Count; i++)
                {
                    if (!orgControls[i].IsEmpty)
                    {
                        Ph[i].X = orgControls[i].X - Pa.Value.X;
                        Ph[i].Y = orgControls[i].Y - Pa.Value.Y;
                    }
                }

                float mu = 0;
                for (int i = 0; i < controls.Count; i++)
                    mu += (float)(Ph[i].X * Ph[i].X + Ph[i].Y * Ph[i].Y) * weights[i + offset];

                D[vIdx].X = orgMeshPointList[vIdx].X - Pa.Value.X;
                D[vIdx].Y = orgMeshPointList[vIdx].Y - Pa.Value.Y;
                for (int i = 0; i < controls.Count; i++)
                {
                    int idx = i + offset;
                    A00[idx] = weights[idx] / mu * (Ph[i].X * D[vIdx].X + Ph[i].Y * D[vIdx].Y);
                    A01[idx] = -weights[idx] / mu * (Ph[i].X * (-D[vIdx].Y) + Ph[i].Y * D[vIdx].X);
                    A10[idx] = -weights[idx] / mu * (-Ph[i].Y * D[vIdx].X + Ph[i].X * D[vIdx].Y);
                    A11[idx] = weights[idx] / mu * (Ph[i].Y * D[vIdx].Y + Ph[i].X * D[vIdx].X);
                }
            }
        }

        float[] Ortho(float[] v, int i) { return new float[] { -v[3 * i + 1], v[3 * i], 0 }; }

        float LengthSquared(float[] vecs, int i)
        {
            float x = vecs[3 * i];
            float y = vecs[3 * i + 1];
            float z = vecs[3 * i + 2];
            return x * x + y * y + z * z;
        }

        float Dot(float[] v0, int i, float[] v1, int j)
        {
            return v0[3 * i + 0] * v1[3 * j + 0] +
                v0[3 * i + 1] * v1[3 * j + 1] +
                v0[3 * i + 2] * v1[3 * j + 2];
        }

        void RigidMLS()
        {
            if (controls.Count < 3)
                return;

            if (weights == null || A00 == null || A01 == null || A10 == null || A11 == null || D == null)
                return;

            for (int vIdx = 0; vIdx < meshPointList.Count; vIdx++)
            {
                int offset = vIdx * controls.Count;
                bool flg = false;
                for (int i = offset; i < offset + controls.Count; i++)
                {
                    if (float.IsInfinity(weights[i]))
                    {
                        // infに飛んでたらcontrols[i]自体を返す
                        meshPointList[vIdx] = controls[i - offset];
                        flg = true;
                        break;
                    }
                }
                if (flg)
                    continue;

                PointF? Qa = AverageWeight(controls, weights, vIdx);
                if (Qa == null || !Qa.HasValue)
                    continue;

                meshPointList[vIdx] = Qa.Value;
                float fx = 0;
                float fy = 0;
                for (int i = 0; i < controls.Count; i++)
                {
                    int idx = i + vIdx * controls.Count;
                    float qx = controls[i].X - Qa.Value.X;
                    float qy = controls[i].Y - Qa.Value.Y;
                    fx += qx * A00[idx] + qy * A10[idx];
                    fy += qx * A01[idx] + qy * A11[idx];
                }
                float lenD = (float)Math.Sqrt(D[vIdx].X * D[vIdx].X + D[vIdx].Y * D[vIdx].Y);
                float lenf = (float)Math.Sqrt(fx * fx + fy * fy);
                float k = lenD / (0.01f + lenf);
                PointF pt = meshPointList[vIdx];
                pt.X += fx * k;
                pt.Y += fy * k;
                meshPointList[vIdx] = pt;
            }
        }

        PointF? AverageWeight(List<PointF> controls, float[] weight, int vIdx)
        {
            PointF pos_sum = PointF.Empty;

            int offset = vIdx * controls.Count;

            float w_sum = 0;
            for (int i = offset; i < offset + controls.Count; i++)
                w_sum += weight[i];

            if (w_sum <= 0)
                return null;

            for (int i = offset; i < offset + controls.Count; i++)
            {
                var p = controls[i - offset];
                pos_sum.X += p.X * weight[i];
                pos_sum.Y += p.Y * weight[i];
            }

            pos_sum.X /= w_sum;
            pos_sum.Y /= w_sum;

            return pos_sum;
        }

        public PointF? OrgToCurControlPoint(PointF orgPt)
        {
            if (!orgControls.Contains(orgPt))
                return null;
            int idx = orgControls.IndexOf(orgPt);
            return controls[idx];
        }

    }
}