using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace API.Controllers
{
    [Route("api/images")]
    public class PictureController : BaseApiController
    {
        private readonly IWebHostEnvironment _env;
        public PictureController(IRepositoryManager repository, IWebHostEnvironment env) : base(repository)
        {
            _env = env;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var image = await _repository.Image.GetByIdAsync(id,false);

            if(image == null)
                return BadRequest("No image with this ID");

            var path = _env.WebRootPath + "\\Images\\" + image.Path;

            byte[] bytes = await System.IO.File.ReadAllBytesAsync(path);

            Stream stream = new MemoryStream(bytes);

            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(image.Path, out contentType);

            return new FileStreamResult(stream, contentType);
        }
    }
}