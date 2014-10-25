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

namespace PatchworkLib.PatchMesh
{
    class PatchControlPoint
    {
        public PointF point;
        public readonly PointF orgPoint;
        public readonly int part;

        public PatchControlPoint(PointF orgPoint, int part)
        {
            this.point = this.orgPoint = orgPoint;
            this.part = part;
        }
    }
}
