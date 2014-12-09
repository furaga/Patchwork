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
        public void CanConnectTest()
        {
            var refSkeleton = FLib.ForceSerializer.Deserialize<PatchSkeleton>("../../../Patchwork/bin/Debug/CanConnect", "refSkeleton");
            var smeshes = FLib.ForceSerializer.Deserialize<List<PatchSkeletalMesh>>("../../../Patchwork/bin/Debug/CanConnect", "smeshes");
            Assert.IsTrue(PatchConnector.CanConnect(smeshes, refSkeleton));
        }

        [TestMethod()]
        public void ConnectTest()
        {
            PatchSkeletalMesh smesh1, smesh2;
            PatchSkeleton refSkeleton;
            FLib.ForceSerializer.Deserialize("../../../Patchwork/bin/Debug/Connect", out smesh1, out smesh2, out refSkeleton);
            var combined = PatchConnector.Connect(smesh1, smesh2, refSkeleton, null);
            PatchSkeletalMeshRenderer.ToBitmap(combined).Save("combined.png");
        }
    }
}
