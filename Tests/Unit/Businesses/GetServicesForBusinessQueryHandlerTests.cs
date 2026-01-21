using Application.Businesses.DTOs;
using Application.Businesses.Queries.GetServicesForBusiness;
using Application.Interface;
using Application.Services.DTOs;
using Moq;

namespace Tests.Unit.Businesses
{
    public class GetServicesForBusinessQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsNotFound_WhenBusinessMissing()
        {
            var repo = new Mock<IBusinessRepository>();
            repo.Setup(r => r.GetWithServicesBySlugAsync("missing", It.IsAny<CancellationToken>()))
                .ReturnsAsync((BusinessWithServicesDto?)null);

            var handler = new GetServicesForBusinessQueryHandler(repo.Object);

            var result = await handler.Handle(new GetServicesForBusinessQuery("missing"), CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("business not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ReturnsDto_WhenFound()
        {
            var repo = new Mock<IBusinessRepository>();

            var dto = new BusinessWithServicesDto
            {
                Id = Guid.NewGuid(),
                Name = "Pizza Palace",
                Slug = "pizza-palace",
                Services = new List<ServiceDto>
            {
                new() { Id = Guid.NewGuid(), Name = "Herrklippning", Description = "Cut", DurationMinutes = 30, Price = 299m }
            }
            };

            repo.Setup(r => r.GetWithServicesBySlugAsync("pizza-palace", It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto);

            var handler = new GetServicesForBusinessQueryHandler(repo.Object);

            var result = await handler.Handle(new GetServicesForBusinessQuery("pizza-palace"), CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Services.Count);
        }
    }
}
