using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DemoService
{
    public interface IFileService
    {
        public Tuple<int, string> SaveFile(IFormFile file);
        public bool DeleteImage(string fileName);
    }
    public class FileService:IFileService
    {
        public IWebHostEnvironment environment { get; set; }
        public FileService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        public Tuple<int, string> SaveFile(IFormFile imageFile)
        {
            try
            {
                var contentPath = this.environment.WebRootPath;
               
                var path = Path.Combine(contentPath, "images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

               
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                // we are trying to create a unique filename here
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, "Error has occured");
            }
        }

        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
