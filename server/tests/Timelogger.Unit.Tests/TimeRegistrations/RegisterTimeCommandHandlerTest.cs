using MediatR;
using Moq;
using Timelogger.Application.TimeRegistrations.Commands;
using Timelogger.Domain;
using Timelogger.Domain.Crosscutting;
using Timelogger.Domain.Exceptions;
using Timelogger.Domain.Repositories;
using Xunit;

namespace Timelogger.Unit.Tests.TimeRegistrations;

public class RegisterTimeCommandHandlerTest
{
    private readonly IRequestHandler<RegisterTimeCommand> _sut;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<IClock> _clockMock;

    public RegisterTimeCommandHandlerTest()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _clockMock = new Mock<IClock>();

        _sut = new RegisterTimeCommandHandler(
            _projectRepositoryMock.Object,
            _clockMock.Object);
    }

    [Fact]
    public async Task Handle_ValidTimeRegistry_WhenProjectExists_ShouldBeSuccessful()
    {
        // Arrange
        var lastInsertedTimeRegistration = new DateTimeOffset(2023, 08, 19, 16, 30, 00, TimeSpan.Zero);
        var now = new DateTimeOffset(2023, 08, 19, 17, 00, 00, TimeSpan.Zero);
        
        var commandRequest = new RegisterTimeCommand(
            projectId: 1,
            description: "Clean Architecture setup",
            startDate: new DateTimeOffset(2023, 08, 19, 17, 30, 55, TimeSpan.Zero),
            endDate: new DateTimeOffset(2023, 08, 19, 19, 30, 00, TimeSpan.Zero));

        _projectRepositoryMock
            .Setup(p => p.VerifyIfExistsAsync(It.IsAny<int>()))
            .Callback(() => { });

        _clockMock
            .Setup(c => c.UtcNow)
            .Returns(now);

        _projectRepositoryMock
            .Setup(p => p.GetLastInsertedTimeRegistrationDateAsync(It.IsAny<int>()))
            .ReturnsAsync(lastInsertedTimeRegistration);

        _projectRepositoryMock
            .Setup(p => p.CreateTimeRegistrationAsync(It.IsAny<TimeRegistration>()));

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await _sut.Handle(commandRequest, new CancellationToken()));

        // Assert
        Assert.Null(exception);
        
        _projectRepositoryMock
            .Verify(p => p.VerifyIfExistsAsync(It.IsAny<int>()), Times.Once);
        
        _clockMock
            .Verify(c => c.UtcNow, Times.Once);
        
        _projectRepositoryMock
            .Verify(p => p.GetLastInsertedTimeRegistrationDateAsync(It.IsAny<int>()), Times.Once);
        
        _projectRepositoryMock
            .Verify(p => p.CreateTimeRegistrationAsync(It.IsAny<TimeRegistration>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ValidTimeRegistry_WhenProjectDoesntExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var commandRequest = new RegisterTimeCommand(
            projectId: 1,
            description: "Clean Architecture setup",
            startDate: new DateTimeOffset(2023, 08, 19, 17, 30, 55, TimeSpan.Zero),
            endDate: new DateTimeOffset(2023, 08, 19, 19, 30, 00, TimeSpan.Zero));

        var exceptionToBeThrown = new EntityNotFoundException(1, "Entity 'Project' with id 1 was not found.");

        _projectRepositoryMock
            .Setup(p => p.VerifyIfExistsAsync(It.IsAny<int>()))
            .ThrowsAsync(exceptionToBeThrown);

        // Act
        async Task Action() => await _sut.Handle(commandRequest, new CancellationToken());

        // Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(Action);
        
        _projectRepositoryMock
            .Verify(p => p.VerifyIfExistsAsync(It.IsAny<int>()), Times.Once);
        
        _clockMock
            .Verify(c => c.UtcNow, Times.Never);
        
        _projectRepositoryMock
            .Verify(p => p.GetLastInsertedTimeRegistrationDateAsync(It.IsAny<int>()), Times.Never);
        
        _projectRepositoryMock
            .Verify(p => p.CreateTimeRegistrationAsync(It.IsAny<TimeRegistration>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ValidTimeRegistry_WhenAnotherRequestWasMadeLessThanOneMinuteAgo_ShouldThrow()
    {
        // Arrange
        var lastInsertedTimeRegistration = new DateTimeOffset(2023, 08, 19, 16, 30, 00, TimeSpan.Zero);
        var now = new DateTimeOffset(2023, 08, 19, 16, 30, 45, TimeSpan.Zero);
        var expectedErrorMessage = "Cant submit two requests in a row for the same project.";
        
        var commandRequest = new RegisterTimeCommand(
            projectId: 1,
            description: "Clean Architecture setup",
            startDate: new DateTimeOffset(2023, 08, 19, 17, 30, 55, TimeSpan.Zero),
            endDate: new DateTimeOffset(2023, 08, 19, 19, 30, 00, TimeSpan.Zero));

        _projectRepositoryMock
            .Setup(p => p.VerifyIfExistsAsync(It.IsAny<int>()))
            .Callback(() => { });

        _clockMock
            .Setup(c => c.UtcNow)
            .Returns(now);

        _projectRepositoryMock
            .Setup(p => p.GetLastInsertedTimeRegistrationDateAsync(It.IsAny<int>()))
            .ReturnsAsync(lastInsertedTimeRegistration);

        // Act
        async Task Action() => await _sut.Handle(commandRequest, new CancellationToken());

        // Assert
        var exception = await Assert.ThrowsAsync<TimeloggerBusinessException>(Action);
        
        Assert.Equal(expected: expectedErrorMessage, actual: exception.Errors[0]);
        
        _projectRepositoryMock
            .Verify(p => p.VerifyIfExistsAsync(It.IsAny<int>()), Times.Once);
        
        _clockMock
            .Verify(c => c.UtcNow, Times.Once);
        
        _projectRepositoryMock
            .Verify(p => p.GetLastInsertedTimeRegistrationDateAsync(It.IsAny<int>()), Times.Once);
        
        _projectRepositoryMock
            .Verify(p => p.CreateTimeRegistrationAsync(It.IsAny<TimeRegistration>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_TimeRegistryWithLessThan30Min_ShouldThrow()
    {
        // Arrange
        var lastInsertedTimeRegistration = new DateTimeOffset(2023, 08, 19, 16, 30, 00, TimeSpan.Zero);
        var now = new DateTimeOffset(2023, 08, 19, 18, 30, 45, TimeSpan.Zero);
        var expectedErrorMessage = "Time Registration in a project must be at least 30 minutes long!";
        
        var commandRequest = new RegisterTimeCommand(
            projectId: 1,
            description: "Clean Architecture setup",
            startDate: new DateTimeOffset(2023, 08, 19, 17, 30, 00, TimeSpan.Zero),
            endDate: new DateTimeOffset(2023, 08, 19, 17, 50, 00, TimeSpan.Zero));

        _projectRepositoryMock
            .Setup(p => p.VerifyIfExistsAsync(It.IsAny<int>()))
            .Callback(() => { });

        _clockMock
            .Setup(c => c.UtcNow)
            .Returns(now);

        _projectRepositoryMock
            .Setup(p => p.GetLastInsertedTimeRegistrationDateAsync(It.IsAny<int>()))
            .ReturnsAsync(lastInsertedTimeRegistration);

        // Act
        async Task Action() => await _sut.Handle(commandRequest, new CancellationToken());

        // Assert
        var exception = await Assert.ThrowsAsync<TimeloggerBusinessException>(Action);
        
        Assert.Equal(expected: expectedErrorMessage, actual: exception.Errors[0]);
        
        _projectRepositoryMock
            .Verify(p => p.VerifyIfExistsAsync(It.IsAny<int>()), Times.Once);
        
        _clockMock
            .Verify(c => c.UtcNow, Times.Once);
        
        _projectRepositoryMock
            .Verify(p => p.GetLastInsertedTimeRegistrationDateAsync(It.IsAny<int>()), Times.Once);
        
        _projectRepositoryMock
            .Verify(p => p.CreateTimeRegistrationAsync(It.IsAny<TimeRegistration>()), Times.Never);
    }
}