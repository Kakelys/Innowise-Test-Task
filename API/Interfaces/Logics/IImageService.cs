using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces.Logics
{
    public interface IImageService
    {
        Task<FileStreamResult> GetImageStream(int imgId);
    }
}