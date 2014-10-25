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
using ARAPDeform = PatchworkLib.ARAPDeformation.ARAPDeformation;

namespace PatchworkLib.PatchMesh
{
    public class PatchMesh
    {
        internal List<PatchVertex> vertices = new List<PatchVertex>();
        internal List<PatchControlPoint> controlPoints = new List<PatchControlPoint>();
        internal List<PatchTriangle> mesh = new List<PatchTriangle>();

        // verticesのうち、メッシュの外周をなすもののインデックスのリスト
        internal List<int> pathVertices = new List<int>();

        ARAPDeform arap;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawVertices"></param>
        /// <param name="rawPathVertices"></param>
        /// <param name="vertToPart">nullならすべての頂点が"-1"に所属する</param>
        /// <param name="patchKey"></param>
        /// <param name="textureSize"></param>
        public PatchMesh(List<PointF> rawVertices, List<int> rawIndices, List<int> rawPathVertices, List<int> vertToPart, string patchKey, SizeF textureSize)
        {
            if (rawVertices == null || rawPathVertices == null)
                return;
            vertices.Clear();
            for (int i = 0; i < rawVertices.Count; i++)
            {
                int part = -1;
                if (vertToPart != null && vertToPart.Count > i)
                    part = vertToPart[i];
                PointF coord = new PointF(rawVertices[i].X / textureSize.Width, rawVertices[i].Y / textureSize.Height);
                vertices.Add(new PatchVertex(rawVertices[i], part, patchKey, coord));
            }
            pathVertices = new List<int>(rawPathVertices);

            for (int i = 0; i + 2 < rawIndices.Count; i += 3)
                mesh.Add(new PatchTriangle(rawIndices[i], rawIndices[i + 1], rawIndices[i + 2], patchKey));

            arap = new ARAPDeform(rawVertices, rawPathVertices);
        }

        //-------------------------------------------------------------------------
        // 外周の取得
        //-------------------------------------------------------------------------

        List<PatchVertex> GetPath()
        {
            if (pathVertices == null)
                return new List<PatchVertex>();
            List<PatchVertex> path = new List<PatchVertex>();
            foreach (var idx in pathVertices)
            {
                if (vertices.Count <= idx)
                    return new List<PatchVertex>();
                path.Add(vertices[idx]);
            }
            return path;
        }

        //-------------------------------------------------------------------------
        // メッシュの変形
        //-------------------------------------------------------------------------

        public void AddControlPoint(PointF controlPoint, PointF orgControlPoint)
        {
            arap.AddControlPoint(controlPoint, orgControlPoint);
        }

        public void RemoveControlPoint(PointF pt)
        {
            arap.RemoveControlPoint(pt);
        }

        public void TranslateControlPoint(PointF pt, PointF to, bool flush)
        {
            arap.TranslateControlPoint(pt, to, flush);
        }

        public void FlushDefomation()
        {
            arap.FlushDefomation();
        }

        public void BeginDeformation()
        {
            arap.BeginDeformation();
        }

        public void EndDeformation()
        {
            arap.EndDeformation();
        }

    }
}
