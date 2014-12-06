using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatchworkLib.ARAPDeformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace PatchworkLib.ARAPDeformation.Tests
{
    [TestClass()]
    public class ARAPDeformationTests
    {
        [TestMethod()]
        public void BalancePartCountTest()
        {
            List<int> parts = new List<int>()
            {
                0, 0, 0, // 0-2
                0, 0, 0, // 3-5
                1, 1, 1, // 6-8
                1, 1, 1, // 9-11
                2, 2, 2, // 12-14
                2, 2, 2, // 15-17
            };
            List<int> cparts = new List<int>() { 0, 2 };

            Dictionary<int, List<int>> edges = new Dictionary<int, List<int>>();
            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (!edges.ContainsKey(3 * j + i))
                        edges[3 * j + i] = new List<int>();
                    // 上下左右
                    if (j - 1 >= 0)
                      edges[3 * j + i].Add(3 * (j - 1) + i);
                    if (j + 1 < 6)
                        edges[3 * j + i].Add(3 * (j + 1) + i);
                    if (i - 1 >= 0)
                        edges[3 * j + i].Add(3 * j + i - 1);
                    if (i + 1 < 3)
                        edges[3 * j + i].Add(3 * j + i + 1);
                }
            }

            var vparts = ARAPDeformation.BalancePartCount(parts, cparts, edges);

            Assert.Fail();
        }
    }
}