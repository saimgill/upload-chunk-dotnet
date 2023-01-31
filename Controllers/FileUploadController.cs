using FileUploadChunking.BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadChunking.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        [Route("UploadFileAsync")]
        [HttpPost]
        public async Task UploadFileAsync(IFormFile file)
        {
            FileUpload fileUpload = new FileUpload();
            fileUpload.UploadFileAsync(file).ConfigureAwait(false);
        }        
        
        [Route("UploadFile")]
        [HttpPost]
        public bool UploadFile(IList<IFormFile> file)
        {
            FileUpload fileUpload = new FileUpload();

            return fileUpload.UploadFile(file);
        }


    }
}
