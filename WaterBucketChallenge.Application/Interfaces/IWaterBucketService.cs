using WaterBucketChallenge.Application.Dtos.BaseDtos;

namespace WaterBucketChallenge.Application.Interfaces
{
    public interface IWaterBucketService
    {
        WaterBucketBaseResponseDto Solve(int XCapacity, int YCapacity, int ZTarget);
    }
}
