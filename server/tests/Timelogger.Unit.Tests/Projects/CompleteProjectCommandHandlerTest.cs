using MediatR;
using Moq;
using Timelogger.Application.Projects.Commands;
using Timelogger.Domain.Repositories;
using Xunit;

namespace Timelogger.Unit.Tests.Projects;

public class CompleteProjectCommandHandlerTest
{
    private readonly IRequestHandler<CompleteProjectCommandRequest> _sut;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;

    public CompleteProjectCommandHandlerTest()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();

        _sut = new CompleteProjectCommandHandler(_projectRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_CompleteProject_ShouldBeSuccessful()
    {
        // Arrange
        _projectRepositoryMock
            .Setup(p => p.CompleteProjectAsync(It.IsAny<int>()))
            .Callback(() => { });

        var request = new CompleteProjectCommandRequest(projectId: 1);

        // Act
        await _sut.Handle(request, new CancellationToken());

        // Assert
        _projectRepositoryMock
            .Verify(p => p.CompleteProjectAsync(It.IsAny<int>()), Times.Once);
    }
}