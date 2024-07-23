using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Manga_Omelette.Models_Secondary;
using Microsoft.Extensions.Options;

namespace Manga_Omelette.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var cloudinaryAccount = new Account(
               config.Value.CloudName,
               config.Value.ApiKey,
               config.Value.ApiSecret
            );
            
            _cloudinary = new Cloudinary(cloudinaryAccount);
        }

        public ImageUploadResult UploadImage(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file != null)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream)
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            return uploadResult;
        }
    }
}
