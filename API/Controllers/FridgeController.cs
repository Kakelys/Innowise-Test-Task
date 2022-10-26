using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Errors;
using API.Interfaces;
using API.Interfaces.Logics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/fridges")]
    public class FridgeController : BaseApiController
    {
        private readonly IFridgeService _fridgeService;

        public FridgeController(IFridgeService fridgeService)
        {
            _fridgeService = fridgeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fridge>>> Get()
        {
            return Ok(await _fridgeService.GetFridges(false));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fridge>> GetById(int id)
        {
            return Ok(await _fridgeService.GetFridgeById(id, false));
        }

        [HttpGet("{id}/detail")]
        public async Task<ActionResult<Fridge>> GetFridgeDetail(int id)
        {
            return Ok(await _fridgeService.GetFridgeDetail(id));
        }

        [Authorize]
        [HttpPost]
        public ActionResult<Fridge> PostFridge(CreateFridgeDto fridge)
        {
            return Ok(_fridgeService.Add(fridge));
        }

        [Authorize]
        [HttpPost("updproducts")]
        public async Task<ActionResult> UpdProducts()
        {
            return Ok(await _fridgeService.UpdateFridgesProducts());
        }

        [Authorize(Policy = "ForAdmins")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Fridge>> PutFridge(int id, EditFridgeDto fridge)
        {
            return Ok(await _fridgeService.Update(id, fridge));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteFridge(int id)
        {
            return Ok(await _fridgeService.Delete(id));
        }
    }
}