﻿using System.Threading.Tasks;
using EventApp.Models.Communication;
using EventApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest data)
        {
            var result = await _userService.LoginAsync(data);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Init()
        {
            await _userService.InitAsync();
            return Ok();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_userService.GetUsers());
        }
    }
}
