using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserIPAnalytics.Application.Pipeline.Queries.Handlers;
using UserIPAnalytics.Application.Pipeline.Queries;
using UserIPAnalytics.Domain.Abstractions;
using UserIPAnalytics.Domain.Entities;
using Xunit;
using UserIPAnalytics.Domain.Abstractions.IRepositories;

namespace UserConnectionTest
{
    public class GetUserLastConnectionQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetUserLastConnectionQueryHandler _handler;

        public GetUserLastConnectionQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new GetUserLastConnectionQueryHandler(_unitOfWorkMock.Object);

            _unitOfWorkMock.SetupGet(u => u.UserConnection).Returns(new Mock<IUseConnectionRepository>().Object);
        }

        [Fact]
        public async Task Handle_ReturnsLastConnection()
        {
            // Arrange
            var query = new GetUserLastConnectionQuery { UserId = 100001 };
            var lastConnection = new UserConnection(100001, "127.0.0.1") { CreatedDate = DateTime.UtcNow };
            _unitOfWorkMock.Setup(u => u.UserConnection.GetUserLastConnectionAsync(100001))
                .ReturnsAsync(lastConnection);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(100001, result.UserId);
            Assert.Equal("127.0.0.1", result.IpAddress);
            Assert.Equal(lastConnection.CreatedDate, result.ConnectionTime);
        }

        [Fact]
        public async Task Handle_NoConnection_ReturnsEmpty()
        {
            // Arrange
            var query = new GetUserLastConnectionQuery { UserId = 100001 };
            _unitOfWorkMock.Setup(u => u.UserConnection.GetUserLastConnectionAsync(100001))
                .ReturnsAsync((UserConnection)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(0, result.UserId);
            Assert.Null(result.IpAddress);
            Assert.Equal(default(DateTime), result.ConnectionTime);
        }
    }
}
