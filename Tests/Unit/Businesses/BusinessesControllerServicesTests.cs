using API.Controllers;
using Application.Businesses.DTOs;
using Application.Businesses.Queries.GetServicesForBusiness;
using Application.Common;
using Application.Services.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Unit.Businesses
{
    public class BusinessesControllerServicesTests
    {
        [Fact]
        public async Task GetServicesBySlug_ReturnsOk_WhenSuccess()
        {
            var sender = new Mock<ISender>();

            var dto = new BusinessWithServicesDto
            {
                Id = Guid.NewGuid(),
                Name = "Pizza Palace",
                Slug = "pizza-palace",
                Services = new List<ServiceDto>()
            };

            sender.Setup(s => s.Send(It.Is<GetServicesForBusinessQuery>(q => q.Slug == "pizza-palace"),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<BusinessWithServicesDto>.Success(dto));

            var controller = new BusinessController(sender.Object);

            var result = await controller.GetServicesForBusiness("pizza-palace", CancellationToken.None);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(dto, ok.Value);
        }

        [Fact]
        public async Task GetServicesBySlug_ReturnsNotFound_WhenFailure()
        {
            var sender = new Mock<ISender>();

            sender.Setup(s => s.Send(It.IsAny<GetServicesForBusinessQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<BusinessWithServicesDto>.Failure("business not found"));

            var controller = new BusinessController(sender.Object);

            var result = await controller.GetServicesForBusiness("missing", CancellationToken.None);

            var nf = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("business not found", nf.Value);
        }
    }
}
