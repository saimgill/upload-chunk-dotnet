using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FileUploadChunking.BusinessLogic
{
    public class FileUpload
    {
        public async Task UploadFileAsync(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Upload\\Files"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                    {
                        file.CopyToAsync(fileStream).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }


        public bool UploadFile(IList<IFormFile> chunkFile)
        {
            long size = 0;
            try
            {
                // for chunk-upload
                foreach (var file in chunkFile)
                {

                    var filename = ContentDispositionHeaderValue
                                        .Parse(file.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                    string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Upload\\Files"));
                    filename = path + $@"\{filename}";
                    //filename = hostingEnv.WebRootPath + $@"\{filename}";
                    size += file.Length;
                    if (!System.IO.File.Exists(filename))
                    {
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    else
                    {
                        using (FileStream fs = System.IO.File.Open(filename, FileMode.Append))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                //Response.Clear();
                //Response.StatusCode = 204;
                //Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                //Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
                throw new Exception("File Copy Failed", e);
            }
        }
    }
}
