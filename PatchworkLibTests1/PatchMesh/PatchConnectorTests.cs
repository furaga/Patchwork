using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatchworkLib.PatchMesh;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace PatchworkLib.PatchMesh.Tests
{
    [TestClass()]
    public class PatchConnectorTests
    {
        [TestMethod()]
        public void CanConnectTest03()
        {
            List<PatchSkeletalMesh> smeshes;
            PatchSkeleton refSkeleton;
            FLib.ForceSerializer.Deserialize("../../../Patchwork/bin/Debug/Connect03", out smeshes, out refSkeleton);
            Assert.IsTrue(PatchConnector.CanConnect(smeshes, refSkeleton));
        }

        [TestMethod()]
        public void ConnectTest03()
        {
            List<PatchSkeletalMesh> smeshes;
            PatchSkeleton refSkeleton;
            FLib.ForceSerializer.Deserialize("../../../Patchwork/bin/Debug/Connect03", out smeshes, out refSkeleton);

            for (int i = 0; i < smeshes.Count; i++)
                PatchSkeletalMeshRenderer.ToBitmap(smeshes[i], alignment: true).Save("smesh[" + i + "].png");

            var combined = PatchConnector.Connect(smeshes, refSkeleton, null);
            Assert.IsNotNull(combined);

            PatchSkeletalMeshRenderer.ToBitmap(combined).Save("combined.png");
        }

    }
}
