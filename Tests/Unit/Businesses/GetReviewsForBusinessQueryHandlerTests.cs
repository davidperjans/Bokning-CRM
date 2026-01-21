using Application.Businesses.DTOs;
using Application.Businesses.Queries.GetReviewsForBusiness;
using Application.Interface;
using Application.Reviews.DTOs;
using Moq;

namespace Tests.Unit.Businesses
{
    public class GetReviewsForBusinessQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsNotFound_WhenBusinessMissing()
        {
            // Arrange
            var repo = new Mock<IBusinessRepository>();

            repo.Setup(r => r.GetWithReviewsBySlugAsync("missing", It.IsAny<CancellationToken>()))
                .ReturnsAsync((BusinessWithReviewsDto?)null);

            var handler = new GetReviewsForBusinessQueryHandler(repo.Object);

            // Act
            var result = await handler.Handle(new GetReviewsForBusinessQuery("missing"), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("business not found.", result.ErrorMessage);

            repo.Verify(r => r.GetWithReviewsBySlugAsync("missing", It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsDto_WhenFound()
        {
            // Arrange
            var repo = new Mock<IBusinessRepository>();

            var dto = new BusinessWithReviewsDto
            {
                Business = new BusinessSummaryDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Pizza Palace",
                    Slug = "pizza-palace"
                },
                Reviews = new List<ReviewDto>
            {
                new ReviewDto
                {
                    Id = Guid.NewGuid(),
                    Rating = 5,
                    FullName = "John Doe",
                    Comment = "Great!",
                    CreatedAt = DateTime.UtcNow
                }
            }
            };

            repo.Setup(r => r.GetWithReviewsBySlugAsync("pizza-palace", It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto);

            var handler = new GetReviewsForBusinessQueryHandler(repo.Object);

            // Act
            var result = await handler.Handle(new GetReviewsForBusinessQuery("pizza-palace"), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("pizza-palace", result.Value.Business.Slug);
            Assert.Single(result.Value.Reviews);

            repo.Verify(r => r.GetWithReviewsBySlugAsync("pizza-palace", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
