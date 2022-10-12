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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/account")]
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        public AccountController(IRepositoryManager repository, ITokenService tokenService) : base(repository)
        {
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(LoginDto userDto)
        {
            using var hmac = new HMACSHA512();

            if(await _repository.User.IsUserExist(userDto.UserName)) 
                return BadRequest("Username is taken");

            var user = new User()
            {
                Name = userDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password)),
                PasswordSalt = hmac.Key
            };

            _repository.User.Create(user);

            _repository.Save();
            var ifno = new UserDto
            {
                UserName = user.Name,
                Token = _tokenService.CreateToken(user)
            };

            return Ok(new UserDto
            {
                UserName = user.Name,
                Token = _tokenService.CreateToken(user)
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto userDto)
        {
            var userName = userDto.UserName.ToLower();
            var user = await _repository.User.GetByNameAsync(userDto.UserName, false);
            if(user == null)
                return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));
            
            for(var i = 0; i < passwordHash.Length; i++)
            {
                if(passwordHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid password");
            }

            return Ok(new UserDto
            {
                UserName = user.Name,
                Token = _tokenService.CreateToken(user)
            });
        }
    }
}