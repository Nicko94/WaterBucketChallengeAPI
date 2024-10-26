using WaterBucketChallenge.Application.Dtos;
using WaterBucketChallenge.Application.Dtos.BaseDtos;
using WaterBucketChallenge.Application.Interfaces;
using WaterBucketChallenge.Application.Services;

namespace WaterBucketChallenge.Test.Application.Unit
{
    [TestFixture]
    public class WaterBucketServiceTests
    {
        private IWaterBucketService _waterBucketService;

        [SetUp]
        public void SetUp()
        {
            // Initialize the WaterBucketService instance before each test
            _waterBucketService = new WaterBucketService();
        }

        // Test case for a successful solution scenario
        [Test]
        [TestCase(2, 10, 4, TestName = "Solve_SuccessfulSolution")]
        public void Solve_WaterBucketService_Success(int XCapacity, int YCapacity, int ZTarget)
        {
            // Act: Call the Solve method with valid inputs that should yield a solution
            WaterBucketBaseResponseDto result = _waterBucketService.Solve(XCapacity, YCapacity, ZTarget);

            // Assert: Check that the result is a successful solution and contains steps
            Assert.Multiple(() =>
            {
                // Verify that the result is of type WaterBucketSuccessResponseDto
                Assert.IsInstanceOf<WaterBucketSuccessResponseDto>(result, "Expected a success response");

                // Cast result and check for non-nullity
                var successResult = result as WaterBucketSuccessResponseDto;
                Assert.IsNotNull(successResult, "Success result should not be null");

                // Ensure solution contains steps
                Assert.IsTrue(successResult.solution.Any(), "Expected solution steps but found none.");
                Assert.IsNotEmpty(successResult.solution, "Solution steps should not be empty.");
            });
        }

        // Test case for handling invalid inputs that should produce an error
        /*[Test]
        [TestCase(0, 0, 0, TestName = "Solve_ErrorWithInvalidInputs")]
        public void Solve_WaterBucketService_Error(int XCapacity, int YCapacity, int ZTarget)
        {
            // Act: Call the Solve method with inputs that should generate an error response
            WaterBucketBaseResponseDto result = _waterBucketService.Solve(XCapacity, YCapacity, ZTarget);

            // Assert: Check that the result is an error response and contains an error message
            Assert.Multiple(() =>
            {
                // Verify that the result is of type WaterBucketErrorResponseDto
                Assert.IsInstanceOf<WaterBucketErrorResponseDto>(result, "Expected an error response");

                // Cast result and check for non-nullity
                var errorResult = result as WaterBucketErrorResponseDto;
                Assert.IsNotNull(errorResult, "Error result should not be null");

                // Check that the error flag is set to true and an error message is present
                Assert.IsTrue(errorResult.error, "Error flag should be set to true");
                Assert.IsNotEmpty(errorResult.message, "Error message should not be empty");
            });
        }*/

        // Test case for a scenario where no solution exists for the given inputs
        [Test]
        [TestCase(2, 1, 5, TestName = "Solve_NoSolutionFound")]
        public void Solve_WaterBucketService_NoSolution(int XCapacity, int YCapacity, int ZTarget)
        {
            // Act: Call the Solve method with inputs that should result in no possible solution
            WaterBucketBaseResponseDto result = _waterBucketService.Solve(XCapacity, YCapacity, ZTarget);

            // Assert: Check that the result is a no-solution response with the expected message
            Assert.Multiple(() =>
            {
                // Verify that the result is of type WaterBucketNoSolutionResponseDto
                Assert.IsInstanceOf<WaterBucketNoSolutionResponseDto>(result, "Expected a no-solution response");

                // Cast result and check for non-nullity
                var noSolutionResult = result as WaterBucketNoSolutionResponseDto;
                Assert.IsNotNull(noSolutionResult, "No-solution result should not be null");

                // Confirm that the solution message matches the expected "No solution" message
                Assert.That(noSolutionResult.solution, Is.EqualTo("No solution"), "Expected 'No solution' in solution message");
            });
        }
    }
}