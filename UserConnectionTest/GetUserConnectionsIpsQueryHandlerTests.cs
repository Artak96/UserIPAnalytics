using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserIPAnalytics.Application.Pipeline.Queries.Handlers;
using UserIPAnalytics.Application.Pipeline.Queries;
using UserIPAnalytics.Domain.Abstractions;
using Xunit;
using UserIPAnalytics.Domain.Abstractions.IRepositories;

namespace UserConnectionTest
{
    public class GetUserConnectionsIpsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetUserConnectionsIpsQueryHandler _handler;

        public GetUserConnectionsIpsQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new GetUserConnectionsIpsQueryHandler(_unitOfWorkMock.Object);

            _unitOfWorkMock.SetupGet(u => u.UserConnection).Returns(new Mock<IUseConnectionRepository>().Object);
        }

        [Fact]
        public async Task Handle_ReturnsUserIpAddresses()
        {
            // Arrange
            var query = new GetUserConnectionsIpsQuery { userId = 100001 };
            var ips = new List<string> { "127.0.0.1", "192.168.1.1" };
            _unitOfWorkMock.Setup(u => u.UserConnection.GetUserAllIpsAsync(100001)).ReturnsAsync(ips);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("127.0.0.1", result);
            Assert.Contains("192.168.1.1", result);
        }
    }
}
