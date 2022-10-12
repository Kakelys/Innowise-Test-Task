using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/models")]
    public class ModelController : BaseApiController
    {
        public ModelController(IRepositoryManager repository) : base(repository)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<FridgeModel>>> GetModels()
        {
            return Ok(await _repository.FridgeModel.GetAll(false));
        }
    }
}