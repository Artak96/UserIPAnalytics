using Moq;
using UserIPAnalytics.Application.Pipeline.Queries;
using UserIPAnalytics.Application.Pipeline.Queries.Handlers;
using UserIPAnalytics.Domain.Abstractions;
using UserIPAnalytics.Domain.Abstractions.IRepositories;
using UserIPAnalytics.Domain.Entities;
using Xunit;

namespace UserConnectionTest
{
    public class GetByIpAddressQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetByIpAddressQueryHandler _handler;

        public GetByIpAddressQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new GetByIpAddressQueryHandler(_unitOfWorkMock.Object);

            _unitOfWorkMock.SetupGet(u => u.UserConnection).Returns(new Mock<IUseConnectionRepository>().Object);
        }

        [Fact]
        public async Task Handle_ReturnsUsersMatchingIpPrefix()
        {
            // Arrange
            var query = new GetByIpAddressQuery { IpAddress = "31.214" };
            var connections = new List<UserConnection>
        {
            new UserConnection(1234567, "31.214.157.141"),
            new UserConnection(100001, "31.214.12.34")
        };
            _unitOfWorkMock.Setup(u => u.UserConnection.FindUsersByIpPartAsync("31.214"))
                .ReturnsAsync(connections);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.UserId == 1234567 && r.IpAddress == "31.214.157.141");
            Assert.Contains(result, r => r.UserId == 100001 && r.IpAddress == "31.214.12.34");
        }

        [Fact]
        public async Task Handle_NoMatches_ReturnsEmptyList()
        {
            // Arrange
            var query = new GetByIpAddressQuery { IpAddress = "192.168" };
            _unitOfWorkMock.Setup(u => u.UserConnection.FindUsersByIpPartAsync("192.168"))
                .ReturnsAsync(new List<UserConnection>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}
