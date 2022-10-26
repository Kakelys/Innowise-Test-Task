using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using API.Interfaces;
using API.Interfaces.Logics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace API.Services.Logics
{
    public class ImageService : ServiceBase, IImageService
    {
        private readonly IWebHostEnvironment _env;
        public ImageService(IRepositoryManager repository, IWebHostEnvironment env) : base(repository)
        {
            _env = env;
        }

        public async Task<FileStreamResult> GetImageStream(int imgId)
        {
            var image = await _repository.Image.GetByIdAsync(imgId,false);

            if(image == null)
                throw new ApiException(204, "No image with this ID");

            var path = _env.WebRootPath + "\\Images\\" + image.Path;

            byte[] bytes = await System.IO.File.ReadAllBytesAsync(path);

            Stream stream = new MemoryStream(bytes);

            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(image.Path, out contentType);

            return new FileStreamResult(stream, contentType);
        }
    }
}