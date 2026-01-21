using API.Controllers;
using Application.Businesses.Queries.GetAllBusinesses;
using Application.Common;
using Application.SuperAdmin.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Unit.SuperAdmin
{
    public class SuperAdminControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly SuperAdminController _controller;

        public SuperAdminControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new SuperAdminController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllBusinesses_ShouldReturnOk_WhenBusinessesExist()
        {
            // Arrange
            var businesses = new List<BusinessAdminListItemDto>
            {
                new BusinessAdminListItemDto(Guid.NewGuid(), "Test Business", "Active", DateTime.Now, "owner@test.com"),
                new BusinessAdminListItemDto(Guid.NewGuid(), "Pending Business", "Pending", DateTime.Now, "pending@test.com")
            };

            var operationResult = OperationResult<List<BusinessAdminListItemDto>>.Success(businesses);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllBusinessesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.GetAllBusinesses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<OperationResult<List<BusinessAdminListItemDto>>>(okResult.Value);
            
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(returnedResult.IsSuccess);
            Assert.Equal(2, returnedResult.Value.Count);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllBusinessesQuery>(), default), Times.Once);
        }

        [Fact]
        public async Task GetAllBusinesses_ShouldReturnOk_EvenIfListIsEmpty()
        {
            // Arrange
            var operationResult = OperationResult<List<BusinessAdminListItemDto>>.Success(new List<BusinessAdminListItemDto>());

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllBusinessesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.GetAllBusinesses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<OperationResult<List<BusinessAdminListItemDto>>>(okResult.Value);

            Assert.Empty(returnedResult.Value);
        }
    }
}