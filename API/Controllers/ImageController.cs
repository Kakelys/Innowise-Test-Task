using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Interfaces.Logics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace API.Controllers
{
    [Route("api/images")]
    public class PictureController : BaseApiController
    {
        private readonly IImageService _imageService;
        public PictureController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetImage(int id)
        {
            return await _imageService.GetImageStream(id);
        }
    }
}