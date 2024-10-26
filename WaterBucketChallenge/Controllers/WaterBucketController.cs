using Microsoft.AspNetCore.Mvc;
using WaterBucketChallenge.Application.Dtos;
using WaterBucketChallenge.Application.Dtos.BaseDtos;
using WaterBucketChallenge.Application.Interfaces;

namespace WaterBucketChallenge.API.Controllers
{
    public class WaterBucketController : ControllerBase
    {
        private readonly IWaterBucketService _solver;
        public WaterBucketController(IWaterBucketService solver)
        {
            _solver = solver;
        }
        
        [HttpPost("solve")]
        public IActionResult Solve([FromBody] WaterBucketRequestDto request)
        {
            if (request == null || !request.XCapacity.HasValue || !request.YCapacity.HasValue || !request.ZTarget.HasValue || request.XCapacity <= 0 || request.YCapacity <= 0 || request.ZTarget <= 0)
            {
                return BadRequest(new { error = "X, Y, and Z must be positive integers." });
            }
            else if (request.ZTarget > Math.Max(request.XCapacity.Value, request.YCapacity.Value))
            {
                return BadRequest(new { error = "Target volume cannot be larger than the largest jug." });
            }

            WaterBucketBaseResponseDto waterBucketResponseDto = _solver.Solve(request.XCapacity.Value, request.YCapacity.Value, request.ZTarget.Value);

            if (waterBucketResponseDto is WaterBucketErrorResponseDto || waterBucketResponseDto is WaterBucketNoSolutionResponseDto)
            {
                return BadRequest(waterBucketResponseDto);
            }
            return Ok(waterBucketResponseDto);
        }
    }
}
