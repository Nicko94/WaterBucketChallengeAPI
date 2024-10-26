using System;
using System.Collections.Generic;
using WaterBucketChallenge.Application.Dtos;
using WaterBucketChallenge.Application.Dtos.BaseDtos;
using WaterBucketChallenge.Application.Interfaces;
using WaterBucketChallenge.Domain;

namespace WaterBucketChallenge.Application.Services
{
    public class WaterBucketService : IWaterBucketService
    {
        private const int MaxVisitedSteps = 10000;
        /* MaxVisitedSteps is defined as a safety mechanism to avoid endless execution in special cases
         not accounted for. It also protects the algorithm of arbitrarily large inputs that could clutter the response times.*/
        private const string OpFillX = "Fill bucket X";
        private const string OpFillY = "Fill bucket Y";
        private const string OpDumpX = "Empty bucket X";
        private const string OpDumpY = "Empty bucket Y";
        private const string OpTransferXY = "Transfer from bucket X to Y";
        private const string OpTransferYX = "Transfer from bucket Y to X";

        public WaterBucketBaseResponseDto Solve(int XCapacity, int YCapacity, int ZTarget)
        {
            try
            {
                List<WaterBucketStepDto> steps = new List<WaterBucketStepDto>();
                var (path, found) = SearchSolution(XCapacity, YCapacity, ZTarget);

                if (found)
                {
                    for (int i = 1; i < path.Count; i++)
                    {
                        var step = path[i];

                        steps.Add(new WaterBucketStepDto
                        {
                            StepNumber = i,
                            BucketX = step.X,
                            BucketY = step.Y,
                            Action = step.Operation,
                            Status = (i == path.Count - 1) ? "Solved" : null // Adding status field only in the last step
                        });
                    }
                }
                else
                {
                    return new WaterBucketNoSolutionResponseDto() { solution = "No solution" };
                }
                return new WaterBucketSuccessResponseDto() { solution = steps };
            }
            catch(Exception ex)
            {
                return new WaterBucketErrorResponseDto() { error = true, message = ex.Message };
            }
        }

        private (List<Node> Path, bool Found) SearchSolution(int x, int y, int z)
        {
            // Initial node starts with both buckets empty
            var initialNode = new Node(0, 0, "");
            // Creating a list of paths to explore
            var paths = new List<List<Node>> { new List<Node> { initialNode } };
            // Keeping track of visited nodes with a HashSet
            var visitedNodes = new HashSet<string>();

            // While there are still paths unexplored, execute the loop
            while (paths.Count > 0)
            {
                var path = paths[0]; // 1st path
                paths.RemoveAt(0); // Explored path removed

                // Get the most recent step
                var lastNode = path[path.Count - 1];

                if (visitedNodes.Count > MaxVisitedSteps) break; // When visit limit is reached end search
                
                visitedNodes.Add(GetIndex(lastNode)); // Added to visited list

                // Check if the solution was found
                if (IsSolution(lastNode, z)) return (path, true);

                var nextMoves = NextTransitions(x, y, path, visitedNodes); // Add possible next move from the current one
                paths.AddRange(nextMoves); // Add the generaeted states to the list of the next moves
            }
            return (null, false); // Return message id no solution was found
        }

        private List<List<Node>> NextTransitions(int x, int y, List<Node> path, HashSet<string> visitedNodes)
        {
            var result = new List<List<Node>>();
            var nextNodes = new List<Node>();
            int aMax = x;
            int bMax = y;
            
            // Previous step (node)
            int a = path[path.Count - 1].X;
            int b = path[path.Count - 1].Y;

            // Fill bucket X
            var node = new Node(aMax, b, OpFillX);
            if (!BeenThere(node, visitedNodes)) nextNodes.Add(node);

            // Fill bucket Y
            node = new Node(a, bMax, OpFillY);
            if (!BeenThere(node, visitedNodes)) nextNodes.Add(node);

            // Transfer from X to Y
            node = new Node(a - IntMin(a, bMax - b), IntMin(a + b, bMax), OpTransferXY);
            if (!BeenThere(node, visitedNodes)) nextNodes.Add(node);

            // Transfer from Y to X
            node = new Node(IntMin(a + b, aMax), b - IntMin(b, aMax - a), OpTransferYX);
            if (!BeenThere(node, visitedNodes)) nextNodes.Add(node);

            // Empty bucket X
            node = new Node(0, b, OpDumpX);
            if (!BeenThere(node, visitedNodes)) nextNodes.Add(node);

            // Empty bucket Y
            node = new Node(a, 0, OpDumpY);
            if (!BeenThere(node, visitedNodes)) nextNodes.Add(node);

            // Create a list of next paths
            foreach (var nextNode in nextNodes)
            {
                var temp = new List<Node>(path) { nextNode };
                result.Add(temp);
            }
            return result;
        }

        private string GetIndex(Node node)
        {
            return $"{node.X}:{node.Y}";
        }

        private bool IsSolution(Node node, int z)
        {
            return node.X == z || node.Y == z;
        }

        private bool BeenThere(Node node, HashSet<string> visitedNodes)
        {
            return visitedNodes.Contains(GetIndex(node));
        }

        private int IntMin(int a, int b)
        {
            return Math.Min(a, b);
        }
    }
}
