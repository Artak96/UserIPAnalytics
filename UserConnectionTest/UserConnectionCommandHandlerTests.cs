//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Moq;
//using Xunit;
//using System.Threading;
//using System.Threading.Tasks;
//using UserIPAnalytics.Domain.Abstractions;
//using UserIPAnalytics.Infrustructure.Repositories;
//using UserIPAnalytics.Domain.Abstractions.IRepositories;
//using UserIPAnalytics.Application.Pipeline.Commands;
//using UserIPAnalytics.Domain.Entities;
//using UserIPAnalytics.Application.Pipeline.Commands.Handler;


//namespace UserConnectionTest
//{
//    public class UserConnectionCommandHandlerTests
//    {
//        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
//        private readonly UserConnectionCommandHandler _handler;

//        public UserConnectionCommandHandlerTests()
//        {
//            _unitOfWorkMock = new Mock<IUnitOfWork>();
//            _handler = new UserConnectionCommandHandler(_unitOfWorkMock.Object);

//            // Настройка моков для вложенных интерфейсов
//            _unitOfWorkMock.SetupGet(u => u.User).Returns(new Mock<UserReposirory>().Object);
//            _unitOfWorkMock.SetupGet(u => u.UserConnection).Returns(new Mock<IUseConnectionRepository>().Object);
//        }

//        [Fact]
//        public async Task Handle_NewUser_AddsUserAndConnection()
//        {
//            // Arrange
//            var command = new UserConnectionCommand { UserId = 100001, IpAddress = "127.0.0.1" };
//            _unitOfWorkMock.Setup(u => u.User.GetUserByIdAsync(100001)).ReturnsAsync((User)null);

//            // Act
//            await _handler.Handle(command, CancellationToken.None);

//            // Assert
//            _unitOfWorkMock.Verify(u => u.User.AddAsync(It.Is<User>(user => user.Id == 100001)), Times.Once);
//            _unitOfWorkMock.Verify(u => u.UserConnection.AddAsync(It.Is<UserConnection>(c =>
//                c.UserId == 100001 && c.IpAddress == "127.0.0.1")), Times.Once);
//            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
//        }

//        [Fact]
//        public async Task Handle_ExistingUser_AddsOnlyConnection()
//        {
//            // Arrange
//            var command = new UserConnectionCommand { UserId = 100001, IpAddress = "127.0.0.1" };
//            var existingUser = new User("Name_100001") { Id = 100001 };
//            _unitOfWorkMock.Setup(u => u.User.GetUserByIdAsync(100001)).ReturnsAsync(existingUser);

//            // Act
//            await _handler.Handle(command, CancellationToken.None);

//            // Assert
//            _unitOfWorkMock.Verify(u => u.User.AddAsync(It.IsAny<User>()), Times.Never);
//            _unitOfWorkMock.Verify(u => u.UserConnection.AddAsync(It.Is<UserConnection>(c =>
//                c.UserId == 100001 && c.IpAddress == "127.0.0.1")), Times.Once);
//            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
//        }
//    }
//}
