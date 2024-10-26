using System.Collections.Generic;
using WaterBucketChallenge.Application.Dtos.BaseDtos;

namespace WaterBucketChallenge.Application.Dtos
{
    public class WaterBucketSuccessResponseDto : WaterBucketBaseResponseDto
    {
        public List<WaterBucketStepDto> solution { get; set; }
    }
}
