using System;

using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;

public class UserConnectionServiceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UserConnectionCommandHandler _commandHandler;
    private readonly GetByIpAddressQueryHandler _ipQueryHandler;
    private readonly GetUserConnectionsIpsQueryHandler _ipsQueryHandler;
    private readonly GetUserLastConnectionQueryHandler _lastConnectionHandler;
    private readonly HttpClient _client;

    public UserConnectionServiceTests(WebApplicationFactory<Program> factory = null)
    {
        // Настройка моков для юнит-тестов
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.SetupGet(u => u.User).Returns(new Mock<IUserRepository>().Object);
        _unitOfWorkMock.SetupGet(u => u.UserConnection).Returns(new Mock<IUserConnectionRepository>().Object);

        _commandHandler = new UserConnectionCommandHandler(_unitOfWorkMock.Object);
        _ipQueryHandler = new GetByIpAddressQueryHandler(_unitOfWorkMock.Object);
        _ipsQueryHandler = new GetUserConnectionsIpsQueryHandler(_unitOfWorkMock.Object);
        _lastConnectionHandler = new GetUserLastConnectionQueryHandler(_unitOfWorkMock.Object);
         
        // Настройка клиента для интеграционных тестов
        if (factory != null)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.Remove(services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<IpTrackingContext>)));
                    services.AddDbContext<IpTrackingContext>(options => options.UseInMemoryDatabase("TestDb"));
                });
            }).CreateClient();
        }
    }

    // Тесты для UserConnectionCommandHandler
    [Fact]
    public async Task Handle_NewUser_AddsUserAndConnection()
    {
        var command = new UserConnectionCommand { UserId = 100001, IpAddress = "127.0.0.1" };
        _unitOfWorkMock.Setup(u => u.User.GetUserByIdAsync(100001)).ReturnsAsync((User)null);

        await _commandHandler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.User.AddAsync(It.Is<User>(user => user.Id == 100001)), Times.Once);
        _unitOfWorkMock.Verify(u => u.UserConnection.AddAsync(It.Is<UserConnection>(c =>
            c.UserId == 100001 && c.IpAddress == "127.0.0.1")), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingUser_AddsOnlyConnection()
    {
        var command = new UserConnectionCommand { UserId = 100001, IpAddress = "127.0.0.1" };
        var existingUser = new User("Name_100001") { Id = 100001 };
        _unitOfWorkMock.Setup(u => u.User.GetUserByIdAsync(100001)).ReturnsAsync(existingUser);

        await _commandHandler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.User.AddAsync(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.UserConnection.AddAsync(It.Is<UserConnection>(c =>
            c.UserId == 100001 && c.IpAddress == "127.0.0.1")), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    // Тесты для GetByIpAddressQueryHandler
    [Fact]
    public async Task GetByIpAddress_ReturnsUsersMatchingIpPrefix()
    {
        var query = new GetByIpAddressQuery { IpAddress = "31.214" };
        var connections = new List<UserConnection>
        {
            new UserConnection(1234567, "31.214.157.141"),
            new UserConnection(100001, "31.214.12.34")
        };
        _unitOfWorkMock.Setup(u => u.UserConnection.FindUsersByIpPartAsync("31.214")).ReturnsAsync(connections);

        var result = await _ipQueryHandler.Handle(query, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.UserId == 1234567 && r.IpAddress == "31.214.157.141");
        Assert.Contains(result, r => r.UserId == 100001 && r.IpAddress == "31.214.12.34");
    }

    [Fact]
    public async Task GetByIpAddress_NoMatches_ReturnsEmptyList()
    {
        var query = new GetByIpAddressQuery { IpAddress = "192.168" };
        _unitOfWorkMock.Setup(u => u.UserConnection.FindUsersByIpPartAsync("192.168")).ReturnsAsync(new List<UserConnection>());

        var result = await _ipQueryHandler.Handle(query, CancellationToken.None);

        Assert.Empty(result);
    }

    // Тесты для GetUserConnectionsIpsQueryHandler
    [Fact]
    public async Task GetUserConnectionsIps_ReturnsUserIpAddresses()
    {
        var query = new GetUserConnectionsIpsQuery { userId = 100001 };
        var ips = new List<string> { "127.0.0.1", "192.168.1.1" };
        _unitOfWorkMock.Setup(u => u.UserConnection.GetUserAllIpsAsync(100001)).ReturnsAsync(ips);

        var result = await _ipsQueryHandler.Handle(query, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Contains("127.0.0.1", result);
        Assert.Contains("192.168.1.1", result);
    }

    // Тесты для GetUserLastConnectionQueryHandler
    [Fact]
    public async Task GetUserLastConnection_ReturnsLastConnection()
    {
        var query = new GetUserLastConnectionQuery { UserId = 100001 };
        var lastConnection = new UserConnection(100001, "127.0.0.1") { CreatedDate = DateTime.UtcNow };
        _unitOfWorkMock.Setup(u => u.UserConnection.GetUserLastConnectionAsync(100001)).ReturnsAsync(lastConnection);

        var result = await _lastConnectionHandler.Handle(query, CancellationToken.None);

        Assert.Equal(100001, result.UserId);
        Assert.Equal("127.0.0.1", result.IpAddress);
        Assert.Equal(lastConnection.CreatedDate, result.ConnectionTime);
    }

    [Fact]
    public async Task GetUserLastConnection_NoConnection_ReturnsEmpty()
    {
        var query = new GetUserLastConnectionQuery { UserId = 100001 };
        _unitOfWorkMock.Setup(u => u.UserConnection.GetUserLastConnectionAsync(100001)).ReturnsAsync((UserConnection)null);

        var result = await _lastConnectionHandler.Handle(query, CancellationToken.None);

        Assert.Equal(0, result.UserId);
        Assert.Null(result.IpAddress);
        Assert.Equal(default(DateTime), result.ConnectionTime);
    }

    // Интеграционный тест для UserController
    [Fact]
    public async Task ConnectUser_AddsConnection_Integration()
    {
        if (_client == null) return; // Пропускаем, если нет WebApplicationFactory

        var response = await _client.GetAsync("/user/connect?userId=100001");

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Connection event published", content);
    }
}
