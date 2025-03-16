using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Text.Json;
using UserIPAnalytics.Infrustructure.Data.Context;
using Xunit;

namespace UserConnectionIntegrationTests
{
    public class UserControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public UserControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            try
            {
                _client = factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<UserIpAnalysticDbContext>));
                        if (descriptor != null) services.Remove(descriptor);

                        services.AddDbContext<UserIpAnalysticDbContext>(options =>
                            options.UseInMemoryDatabase("TestDb"));
                    });
                }).CreateClient();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting up test client: {ex.Message}");
            }
        }

        [Fact]
        public async Task ConnectUser_ValidUserId_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/user/connect?userId=100001");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            Xunit.Assert.Equal("Connection event published.", result.GetProperty("Message").GetString());
        }

        [Fact]
        public async Task GetUserIdFromHeader_ValidHeader_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/user/connection-user-from-header");
            request.Headers.Add("X-User-Id", "100002");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            Xunit.Assert.Equal(100002, result.GetProperty("UserId").GetInt64());
        }

        [Fact]
        public async Task GetUserIdFromHeader_MissingHeader_ReturnsBadRequest()
        {
            // Act
            var response = await _client.GetAsync("/user/connection-user-from-header");

            // Assert
            Xunit.Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Xunit.Assert.Equal("User ID is missing or invalid", content);
        }

        [Fact]
        public async Task FindUsersByIp_ValidIpPrefix_ReturnsUsers()
        {
            // Arrange:
            await SeedTestData(100003, "31.214.157.141");
            await SeedTestData(100004, "31.214.12.34");

            // Act
            var response = await _client.GetAsync("/user/find-users-by-ip?IpAddress=31.214");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<FindUserIpAddress>>(content);
            Xunit.Assert.Equal(2, result.Count);
            Xunit.Assert.Contains(result, r => r.UserId == 100003 && r.IpAddress == "31.214.157.141");
            Xunit.Assert.Contains(result, r => r.UserId == 100004 && r.IpAddress == "31.214.12.34");
        }

        [Fact]
        public async Task GetUserIps_ValidUserId_ReturnsIpList()
        {
            // Arrange
            await SeedTestData(100005, "127.0.0.1");
            await SeedTestData(100005, "192.168.1.1");

            // Act
            var response = await _client.GetAsync("/user/get-user-ips?userId=100005");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<string>>(content);
            Xunit.Assert.Equal(2, result.Count);
            Xunit.Assert.Contains("127.0.0.1", result);
            Xunit.Assert.Contains("192.168.1.1", result);
        }

        [Fact]
        public async Task GetUserLastConnection_ValidUserId_ReturnsLastConnection()
        {
            // Arrange
            await SeedTestData(100006, "127.0.0.1");
            await Task.Delay(100); 
            await SeedTestData(100006, "192.168.1.1");

            // Act
            var response = await _client.GetAsync("/user/user-last-connection?UserId=100006");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<UserLastConnection>(content);
            Xunit.Assert.Equal(100006, result.UserId);
            Xunit.Assert.Equal("192.168.1.1", result.IpAddress);
        }

        [Fact]
        public async Task GetUserLastConnection_NoConnections_ReturnsEmpty()
        {
            // Act
            var response = await _client.GetAsync("/user/user-last-connection?UserId=999999");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<UserLastConnection>(content);
            Xunit.Assert.Equal(0, result.UserId);
            Xunit.Assert.Null(result.IpAddress);
        }

        private async Task SeedTestData(long userId, string ipAddress)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/user/connect?userId={userId}");
            // Эмулируем IP-адрес через заголовок, если нужно
            request.Headers.Add("X-Forwarded-For", ipAddress);
            await _client.SendAsync(request);
        }
    }

    public class FindUserIpAddress
    {
        public long UserId { get; set; }
        public string IpAddress { get; set; }
    }

    public class UserLastConnection
    {
        public long UserId { get; set; }
        public string IpAddress { get; set; }
        public DateTime ConnectionTime { get; set; }
    }
}
