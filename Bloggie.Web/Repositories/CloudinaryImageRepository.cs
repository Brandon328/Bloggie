using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Bloggie.Web.Repositories
{
    public class CloudinaryImageRepository : IImageRepository
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryImageRepository(
            Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadAsync(IFormFile file)
        {

            // Upload an image and log the response to the console
            //=================

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName,
                Folder="/bloggie"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if(uploadResult != null && 
                uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
