using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using UserIPAnalytics.Application.Pipeline.Commands;
using UserIPAnalytics.Application.Pipeline.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UserIPAnalytics.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("connect")]
        public async Task<IActionResult> ConnectUser(long userId)
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ipAddress == "::1")
                ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

            await _mediator.Send(new AddIPAddressCommand
            {
                IpAddress = ipAddress,
                UserId = userId
            });
            return Ok(new { Message = "Connection event published." });
        }

        /// <summary>
        /// Need to send UserId by header
        /// </summary>
        /// <returns></returns>
        [HttpGet("connection-user-from-header")]
        public async Task<IActionResult> GetUserIdFromHeader()
        {
            if (Request.Headers.TryGetValue("X-User-Id", out var userIdValue) &&
                long.TryParse(userIdValue, out long userId))
            {
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                if (ipAddress == "::1")
                    ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

                await _mediator.Send(new AddIPAddressCommand
                {
                    IpAddress = ipAddress,
                    UserId = userId
                });
                return Ok(new { UserId = userId });
            }

            return BadRequest("User ID is missing or invalid");
        }

        /// <summary>
        /// Need to add e.g JWT in project
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("connection-user-from-token")]
        public async Task<IActionResult> GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (long.TryParse(userIdClaim, out long userId))
            {
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                if (ipAddress == "::1")
                    ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

                await _mediator.Send(new AddIPAddressCommand
                {
                    IpAddress = ipAddress,
                    UserId = userId
                });
                return Ok(new { UserId = userId });
            }

            return Unauthorized("Invalid token");
        }

        [HttpGet("find-users-by-ip")]
        public async Task<IActionResult> FindUsersByIp(GetByIpAddressQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{UserId}/ips")]
        public async Task<IActionResult> GetUserIps(GetUserConnectionsIpsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        //[HttpGet("{UserId}/last-connection")]
        //public async Task<IActionResult> GetLastConnection(long UserId)
        //{
        //    var result = await _service.GetLastUserConnectionAsync(UserId);
        //    return result.IpAddress != null
        //        ? Ok(result)
        //        : NotFound("Нет подключений");
        //}
    }
}
