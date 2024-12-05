using AoCToolbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOCToolBoxTests
{
    public class SortHelperTests
    {
        [Fact]
        public void TopologicalSort_Test()
        {
            // Arrange
            var nodes = new List<int> { 53,75, 97, 47, 61 };
            var edges = createEdges();

            // Act
            var actual = SortHelpers.TopologicalSort(nodes, edges);

            // Assert
            Assert.Equal(97, actual.First());
            Assert.Equal(53, actual.Last());
        }

        private List<(int,int)> createEdges()
        {
            return new List<(int, int)>
            {
                (47,53),
                (97,13),
                (97,61),
                (97,47),
                (75,29),
                (61,13),
                (75,53),
                (29,13),
                (97,29),
                (53,29),
                (61,53),
                (97,53),
                (61,29),
                (47,13),
                (75,47),
                (97,75),
                (47,61),
                (75,61),
                (47,29),
                (75,13),
                (53,13)
            };
        }
    }
}
