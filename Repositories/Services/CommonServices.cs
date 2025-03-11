using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace contact_management.Repositories.Services
{
    public class CommonServices
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly NpgsqlConnection _conn;

        public CommonServices(IWebHostEnvironment webHostEnvironment, NpgsqlConnection conn)
        {
            _webHostEnvironment = webHostEnvironment;
            _conn = conn;
        }

        #region FileUpload

        public  string ImageName(IFormFile ProfileImage, string email)
        {
            //1).
            string fileName = Guid.NewGuid() + "_" + email + Path.GetExtension(ProfileImage.FileName);
            return fileName;
        }

        public async Task UploadImage(IFormFile ProfileImage, string fileName)
        {
            //2) Folder Path create karna
            string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "profile_image");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //3) Create FilePath
            string filePath = Path.Combine(folderPath, fileName);
            //4). Server pe store karenge
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await ProfileImage.CopyToAsync(stream);
            }

        }
        #endregion


    }
}