using MediatR;
using Moq;
using Timelogger.Application.TimeRegistrations.Queries;
using Timelogger.Domain.Crosscutting;
using Timelogger.Domain.dtos;
using Timelogger.Domain.Repositories;
using Xunit;

namespace Timelogger.Unit.Tests.TimeRegistrations;

public class GetRegisteredTimesQueryHandlerTest
{
    private readonly IRequestHandler<GetRegisteredTimesQuery, GetRegisteredTimesQueryResponse> _sut;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;

    public GetRegisteredTimesQueryHandlerTest()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();

        _sut = new GetRegisteredTimesQueryHandler(_projectRepositoryMock.Object);
    }

    [Fact]
    public async Task GetRegisteredTimes_PerProject_ReturnsExpected()
    {
        // Arrange
        var expectedRegisteredTimes = new PagedList<GetRegisteredTimeModel>(
            items: new List<GetRegisteredTimeModel>
            {
                new (
                    "GTA6",
                    "Added main character",
                    new DateTimeOffset(2023, 08, 19, 16, 30, 00, TimeSpan.Zero),
                    new DateTimeOffset(2023, 08, 19, 18, 30, 00, TimeSpan.Zero),
                    2),
                new (
                    "GTA6",
                    "Added secondary character",
                    new DateTimeOffset(2023, 08, 19, 16, 30, 00, TimeSpan.Zero),
                    new DateTimeOffset(2023, 08, 19, 18, 30, 00, TimeSpan.Zero),
                    2),
                new (
                    "GTA6",
                    "Added map",
                    new DateTimeOffset(2023, 08, 19, 16, 30, 00, TimeSpan.Zero),
                    new DateTimeOffset(2023, 08, 19, 18, 30, 00, TimeSpan.Zero),
                    2),
                new (
                    "GTA6",
                    "Added music",
                    new DateTimeOffset(2023, 08, 19, 16, 30, 00, TimeSpan.Zero),
                    new DateTimeOffset(2023, 08, 19, 18, 30, 00, TimeSpan.Zero),
                    2)
            },
            pageNumber: 1,
            pageSize: 10,
            count: 4);

        _projectRepositoryMock
            .Setup(p => p.GetTimeRegistrationsAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync(expectedRegisteredTimes);

        var queryRequest = new GetRegisteredTimesQuery(
            projectId: 1,
            pageNumber: 1,
            pageSize: 10);

        // Act
        var response = await _sut.Handle(queryRequest, new CancellationToken());

        // Assert
        Assert.Equal(expectedRegisteredTimes.ItemsCount, response.TimeRegistrations.Pagination.ItemsCount);
    }
}