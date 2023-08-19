using MediatR;
using Moq;
using Timelogger.Application.Projects.Queries;
using Timelogger.Domain;
using Timelogger.Domain.Crosscutting;
using Timelogger.Domain.Repositories;
using Xunit;

namespace Timelogger.Unit.Tests.Projects;

public class GetProjectsQueryHandlerTest
{
    private readonly IRequestHandler<GetProjectsQuery, GetProjectsQueryResponse> _sut;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;

    public GetProjectsQueryHandlerTest()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();

        _sut = new GetProjectsQueryHandler(_projectRepositoryMock.Object);
    }

    [Fact]
    public async Task GetProjectsOrderedByDeadline_ReturnsExpected()
    {
        // Arrange
        var expectedProjects = new PagedList<Project>(
            items: new List<Project>
            {
                new (
                    id: 1,
                    freelancerId: 1,
                    name: "GTA6",
                    companyName: "Rockstar",
                    deadline: new DateTimeOffset(2023, 08, 19, 16, 30, 00, TimeSpan.Zero),
                    isCompleted: false),
                new (
                    id: 1,
                    freelancerId: 1,
                    name: "Accounting Software",
                    companyName: "e-conomic",
                    deadline: new DateTimeOffset(2023, 09, 19, 16, 30, 00, TimeSpan.Zero),
                    isCompleted: false),
                new (
                    id: 1,
                    freelancerId: 1,
                    name: "Food Delivery App",
                    companyName: "e-conomic",
                    deadline: new DateTimeOffset(2023, 10, 19, 16, 30, 00, TimeSpan.Zero),
                    isCompleted: false)
            },
            pageNumber: 1,
            pageSize: 10,
            count: 4);
        
        _projectRepositoryMock
            .Setup(p => p.GetProjectsOrderedByDeadlineAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync(expectedProjects);
        
        var queryRequest = new GetProjectsQuery(
            freelancerId: 1,
            pageNumber: 1,
            pageSize: 10);
        
        // Act
        var response = await _sut.Handle(queryRequest, new CancellationToken());

        // Assert
        Assert.Equal(expectedProjects[0].Name, response.Projects.Items.ToList()[0].Name);
        Assert.Equal(expectedProjects[1].Name, response.Projects.Items.ToList()[1].Name);
        Assert.Equal(expectedProjects[2].Name, response.Projects.Items.ToList()[2].Name);
    }
}