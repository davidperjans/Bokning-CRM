using Application.Businesses.Queries.GetBusinessSetting;
using Application.Interface;
using Domain.Models;
using Moq;

namespace Tests.Unit.Businesses;

public class GetBusinessSettingsHandlerTests
{
    private readonly Mock<IBusinessRepository> _repoMock = new();
    private readonly Mock<IUserContext> _userContextMock = new();
    private readonly GetBusinessSettingsHandler _handler;

    public GetBusinessSettingsHandlerTests()
    {
        _handler = new GetBusinessSettingsHandler(_repoMock.Object, _userContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSettings_WhenBusinessExists()
    {
        // Arrange
        var businessId = Guid.NewGuid();
        var business = new Business { Id = businessId, Name = "Test Shop", Category = "Wellness" };
        
        _userContextMock.Setup(u => u.BusinessId).Returns(businessId);
        _repoMock.Setup(r => r.GetByIdAsync(businessId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(business);

        // Act
        var result = await _handler.Handle(new GetBusinessSettingsQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Test Shop", result.Value.Name);
    }
}