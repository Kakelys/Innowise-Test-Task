using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Interfaces.Logics
{
    public interface IAccountService
    {
        Task<UserDto> Register(LoginDto userDto);
        Task<UserDto> Login(LoginDto userDto);

    }
}