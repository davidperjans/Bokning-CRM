using Application.Businesses.DTOs;
using Application.Businesses.Queries.GetBusinesses;
using Application.Common;
using Application.Interface;
using Domain.Enum;
using Infrastructure.Repositories;
using Moq;
using Xunit;

namespace Tests.Unit.Businesses
{
    public class GetBusinessesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Returns_PagedResult_From_Repository()
        {
            // Arrange
            var repo = new Mock<IBusinessRepository>();

            var expected = new PagedResult<BusinessListItemDto>(
                Items: new[]
                {
                new BusinessListItemDto { Id = Guid.NewGuid(), Name = "Test", Slug = "test", City="Stockholm", Category="Food", Rating=4.5 }
                },
                Page: 1,
                PageSize: 20,
                TotalCount: 1
            );

            repo.Setup(r => r.SearchAsync(
                    It.IsAny<BusinessSearchParams>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(expected);

            var handler = new GetBusinessesQueryHandler(repo.Object);

            var query = new GetBusinessesQuery(
                City: "Stockholm",
                Category: "Food",
                Query: "pizza",
                Sort: BusinessSort.RatingDesc,
                Page: 1,
                PageSize: 20
            );

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.TotalCount);
            Assert.Single(result.Value.Items);

            repo.Verify(r => r.SearchAsync(
                It.Is<BusinessSearchParams>(p =>
                    p.City == "Stockholm" &&
                    p.Category == "Food" &&
                    p.Query == "pizza" &&
                    p.Sort == BusinessSort.RatingDesc &&
                    p.Page == 1 &&
                    p.PageSize == 20),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
