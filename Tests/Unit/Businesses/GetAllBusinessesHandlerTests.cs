using Application.Businesses.Queries.GetAllBusinesses;
using Application.Interface;
using Domain.Enum;
using Domain.Models;
using Moq;

namespace Tests.Unit.Businesses
{
    public class GetAllBusinessesHandlerTests
    {
        private readonly Mock<IBusinessRepository> _repositoryMock;
        private readonly GetAllBusinessesHandler _handler;

        public GetAllBusinessesHandlerTests()
        {
            _repositoryMock = new Mock<IBusinessRepository>();
            _handler = new GetAllBusinessesHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedDtos_WhenBusinessesExist()
        {
            // Arrange
            var owner = new Domain.Models.User { Email = "owner@test.com" };
            var businesses = new List<Business>
            {
                new Business 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Business 1", 
                    Status = BusinessStatus.Active, 
                    CreatedAt = DateTime.UtcNow, 
                    Owner = owner 
                },
                new Business 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Business 2", 
                    Status = BusinessStatus.Pending, 
                    CreatedAt = DateTime.UtcNow, 
                    Owner = null // Testar fallback för Owner
                }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(businesses);

            var query = new GetAllBusinessesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);
            
            // Kontrollera första företaget (med ägare)
            Assert.Equal("owner@test.com", result.Value[0].OwnerEmail);
            Assert.Equal("Active", result.Value[0].Status);

            // Kontrollera andra företaget (utan ägare - ska bli "No Owner")
            Assert.Equal("No Owner", result.Value[1].OwnerEmail);
            Assert.Equal("Pending", result.Value[1].Status);
            
            _repositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoBusinessesFound()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Business>());

            // Act
            var result = await _handler.Handle(new GetAllBusinessesQuery(), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }
    }
}