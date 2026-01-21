using Application.Reviews.DTOs;

namespace Application.Businesses.DTOs
{
    public class BusinessWithReviewsDto
    {
        public BusinessSummaryDto Business { get; set; } = new();
        public List<ReviewDto> Reviews { get; set; } = new();
    }
}
