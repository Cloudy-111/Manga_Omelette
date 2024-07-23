using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Manga_Omelette.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IConfiguration configuration)
        {
            var cloudinaryAccount = new Account(
               "dcs5nglks", "689156623666212", "_n5xGJgxx_RKaw82kSAEYxtIEEs"
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
