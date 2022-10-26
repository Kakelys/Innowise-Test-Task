using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Errors;
using API.Interfaces;
using API.Interfaces.Logics;

namespace API.Services.Logics
{
    public class AccountService : ServiceBase, IAccountService
    {
        private readonly ITokenService _tokenService;
        public AccountService(IRepositoryManager repository, ITokenService tokenService) : base(repository)
        {
            this._tokenService = tokenService;
        }

        public async Task<UserDto> Login(LoginDto userDto)
        {
            var userName = userDto.UserName.ToLower();
            var user = await _repository.User.GetByNameAsync(userDto.UserName, false);
            if(user == null)
                throw new ApiException(400, "Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));
            
            for(var i = 0; i < passwordHash.Length; i++)
            {
                if(passwordHash[i] != user.PasswordHash[i])
                    throw new ApiException(400, "Invalid password");
            }

            return new UserDto
            {
                UserName = user.Name,
                Token = _tokenService.CreateToken(user)
            };
        }

        public async Task<UserDto> Register(LoginDto userDto)
        {
            using var hmac = new HMACSHA512();

            if(await _repository.User.IsUserExist(userDto.UserName)) 
                throw new ApiException(400, "Username is taken");

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

            return new UserDto
            {
                UserName = user.Name,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}