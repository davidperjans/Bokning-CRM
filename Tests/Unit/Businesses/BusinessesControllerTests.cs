using API.Controllers;
using Application.Businesses.DTOs;
using Application.Businesses.Queries.GetDetailsBySlug;
using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Unit.Businesses
{
    public class BusinessesControllerTests
    {
        [Fact]
        public async Task GetDetailsBySlug_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var mediator = new Mock<IMediator>();

            var dto = new BusinessDetailsDto
            {
                Id = Guid.NewGuid(),
                Name = "Pizza Palace",
                Slug = "pizza-palace",
                City = "Stockholm",
                Category = "Food",
                Rating = 4.5,
                ImageUrl = null,
                Description = "Nice pizza",
                Address = "Street 1",
                CreatedAt = DateTime.UtcNow
            };

            mediator.Setup(m => m.Send(
                    It.Is<GetDetailsBySlugQuery>(q => q.Slug == "pizza-palace"),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<BusinessDetailsDto>.Success(dto));

            var controller = new BusinessController(mediator.Object);

            // Act
            var result = await controller.GetDetailsBySlug("pizza-palace", CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(dto, ok.Value);

            mediator.Verify(m => m.Send(
                It.Is<GetDetailsBySlugQuery>(q => q.Slug == "pizza-palace"),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetDetailsBySlug_ReturnsNotFound_WhenFailure()
        {
            // Arrange
            var mediator = new Mock<IMediator>();

            mediator.Setup(m => m.Send(
                    It.Is<GetDetailsBySlugQuery>(q => q.Slug == "does-not-exist"),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<BusinessDetailsDto>.Failure("business not found"));

            var controller = new BusinessController(mediator.Object);

            // Act
            var result = await controller.GetDetailsBySlug("does-not-exist", CancellationToken.None);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("business not found", notFound.Value);

            mediator.Verify(m => m.Send(
                It.Is<GetDetailsBySlugQuery>(q => q.Slug == "does-not-exist"),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
