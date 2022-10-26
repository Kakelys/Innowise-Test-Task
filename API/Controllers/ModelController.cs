using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces;
using API.Interfaces.Logics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/models")]
    public class ModelController : BaseApiController
    {
        private readonly IFridgeModelService _fridgeModelService;
        public ModelController(IFridgeModelService fridgeModelService)
        {
            _fridgeModelService = fridgeModelService;
        }

        [HttpGet]
        public async Task<ActionResult<List<FridgeModel>>> GetModels()
        {
            return Ok(await _fridgeModelService.GetModels());
        }
    }
}