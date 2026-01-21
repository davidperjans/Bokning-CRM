using API.Controllers;
using Application.Businesses.DTOs;
using Application.Businesses.Queries.GetReviewsForBusiness;
using Application.Common;
using Application.Reviews.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Unit.Businesses
{
    public class BusinessesControllerReviewsTests
    {
        [Fact]
        public async Task GetReviewsBySlug_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var sender = new Mock<ISender>();

            var dto = new BusinessWithReviewsDto
            {
                Business = new BusinessSummaryDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Pizza Palace",
                    Slug = "pizza-palace"
                },
                Reviews = new List<ReviewDto>()
            };

            sender.Setup(s => s.Send(
                    It.Is<GetReviewsForBusinessQuery>(q => q.Slug == "pizza-palace"),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<BusinessWithReviewsDto>.Success(dto));

            var controller = new BusinessController(sender.Object);

            // Act
            var result = await controller.GetReviewsForBusiness("pizza-palace", CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(dto, ok.Value);

            sender.Verify(s => s.Send(
                It.Is<GetReviewsForBusinessQuery>(q => q.Slug == "pizza-palace"),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetReviewsBySlug_ReturnsNotFound_WhenFailure()
        {
            // Arrange
            var sender = new Mock<ISender>();

            sender.Setup(s => s.Send(It.IsAny<GetReviewsForBusinessQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<BusinessWithReviewsDto>.Failure("business not found"));

            var controller = new BusinessController(sender.Object);

            // Act
            var result = await controller.GetReviewsForBusiness("missing", CancellationToken.None);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("business not found", notFound.Value);
        }
    }
}
