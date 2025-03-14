﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using UserIPAnalytics.Application.Pipeline.Commands;

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
            //string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (ipAddress == "::1")
            {
                ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            }

            await _mediator.Send(new AddIPAddressCommand
            {
                IpAddress = ipAddress,
                UserId = userId
            });
            return Ok(new { Message = "Connection event published." });
        }

        [HttpGet("from-header")]
        public IActionResult GetUserIdFromHeader()
        {
            if (Request.Headers.TryGetValue("X-User-Id", out var userIdValue) &&
                long.TryParse(userIdValue, out long userId))
            {
                return Ok(new { UserId = userId });
            }

            return BadRequest("User ID is missing or invalid");
        }

        [Authorize]
        [HttpGet("from-token")]
        public IActionResult GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (long.TryParse(userIdClaim, out long userId))
            {
                return Ok(new { UserId = userId });
            }

            return Unauthorized("Invalid token");
        }

        //[HttpGet("find-users-by-ip")]
        //public async Task<IActionResult> FindUsersByIp([FromQuery] string ipPart)
        //{
        //    var users = await _service.FindUsersByIpPartAsync(ipPart);
        //    return Ok(users);
        //}

        //[HttpGet("{userId}/ips")]
        //public async Task<IActionResult> GetUserIps(long userId)
        //{
        //    var ips = await _service.GetAllUserIpsAsync(userId);
        //    return Ok(ips);
        //}

        //[HttpGet("{userId}/last-connection")]
        //public async Task<IActionResult> GetLastConnection(long userId)
        //{
        //    var result = await _service.GetLastUserConnectionAsync(userId);
        //    return result.IpAddress != null
        //        ? Ok(result)
        //        : NotFound("Нет подключений");
        //}
    }
}
