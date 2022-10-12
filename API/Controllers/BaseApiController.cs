using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected readonly IRepositoryManager _repository;

        public BaseApiController(IRepositoryManager repository)
        {
            _repository = repository;
        }

        protected async Task<bool> IsFridgeExist(int id)
        {
            if((await _repository.Fridge.GetByIdAsync(id,false)) == null)
                return false;

            return true;
        }
    }
}