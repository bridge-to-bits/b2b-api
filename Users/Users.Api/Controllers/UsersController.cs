﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Core.DTOs;
using Users.Core.Interfaces;
using Users.Core.Models;

namespace Users.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController (IUserService userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO registrationDTO)
        {
            var createdUser = await userService.Register(registrationDTO);
            return Ok(createdUser.Id.ToString());
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var token = await userService.Login(loginDTO);
            return Ok(token);
        }

        [Authorize]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok(1);
        }

        [HttpGet("userInfo")]
        public async Task<IActionResult> GetUser(string userId)
        {

            return Ok(await _userService.GetUser(userId));
        }
    }
}
