using Application.Businesses.DTOs;
using Application.Businesses.Queries.GetServicesForBusiness;
using Application.Interface;
using Application.Services.DTOs;
using Moq;

namespace Tests.Unit.Businesses
{
    public class GetServicesForBusinessQueryHandlerTests
    {
        //[Fact]
        //public async Task Handle_ReturnsNotFound_WhenBusinessMissing()
        //{
        //    var repo = new Mock<IBusinessRepository>();
        //    repo.Setup(r => r.GetWithServicesBySlugAsync("missing", It.IsAny<CancellationToken>()))
        //        .ReturnsAsync((BusinessWithServicesDto?)null);
        //    var db = new Mock<IAppDbContext>();

        //    var handler = new GetServicesForBusinessQueryHandler(repo.Object, db.Object);

        //    var result = await handler.Handle(new GetServicesForBusinessQuery("missing"), CancellationToken.None);

        //    Assert.False(result.IsSuccess);
        //    Assert.Equal("business not found.", result.ErrorMessage);
        //}

        //[Fact]
        //public async Task Handle_ReturnsDto_WhenFound()
        //{
        //    var repo = new Mock<IBusinessRepository>();
        //    var db = new Mock<IAppDbContext>();
        //    var businessId = Guid.NewGuid();

        //    // Vi skapar Service-objektet med konstruktorn (7 parametrar)
        //    var serviceDto = new ServiceDto(
        //        Guid.NewGuid(),      // Id
        //        "Herrklippning",     // Name
        //        "En klassisk klippning", // Description
        //        30,                  // DurationMinutes
        //        299m,                // Price
        //        true,                // IsActive
        //        businessId           // BusinessId
        //    );

        //    var dto = new BusinessWithServicesDto
        //    {
        //        Business = new BusinessSummaryDto
        //        {
        //            Id = businessId,
        //            Name = "Pizza Palace",
        //            Slug = "pizza-palace",
        //        },
        //        // Vi lägger till vår skapade service i listan
        //        Services = new List<ServiceDto> { serviceDto }
        //    };

        //    repo.Setup(r => r.GetWithServicesBySlugAsync("pizza-palace", It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(dto);

        //    var handler = new GetServicesForBusinessQueryHandler(repo.Object, db.Object);

        //    var result = await handler.Handle(new GetServicesForBusinessQuery("pizza-palace"), CancellationToken.None);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.NotNull(result.Value);
        //    Assert.Equal(1, result.Value.Services.Count);
        //    Assert.Equal("Herrklippning", result.Value.Services[0].Name);
        //}
    }
}
