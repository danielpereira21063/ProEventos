﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.API.Helpers
{
    public class Util : IUtil
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public Util(IWebHostEnvironment webHostEnvironment)
        {
            _hostEnvironment = webHostEnvironment;
        }


        public void DeleteImage(string imageName, string destino)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/{destino}", imageName);

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        public async Task<string> SaveImage(IFormFile imageFile, string destino)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                .Take(10)
                .ToArray())
                .Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yyyymmddhhmmss")}{Path.GetExtension(imageFile.FileName)}";


            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/{destino}", imageName);

            using (var fileStrem = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStrem);
            }

            return imageName;
        }
    }
}
