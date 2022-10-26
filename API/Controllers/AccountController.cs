using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Interfaces.Logics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/account")]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(LoginDto userDto)
        {
            return Ok(await _accountService.Register(userDto));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto userDto)
        {
            return Ok(await _accountService.Login(userDto));
        }
    }
}