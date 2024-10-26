using WaterBucketChallenge.Application.Dtos.BaseDtos;

namespace WaterBucketChallenge.Application.Dtos
{
    public class WaterBucketErrorResponseDto : WaterBucketBaseResponseDto
    {
        public string message { get; set; }

        public bool error { get; set; }
    }
}
