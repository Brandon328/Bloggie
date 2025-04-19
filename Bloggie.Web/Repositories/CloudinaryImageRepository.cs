namespace Bloggie.Web.Repositories
{
    public interface CloudinaryImageRepository : IImageRepository
    {
        public async Task<string> UploadAsync(IFormFile file)
        {

        }
    }
}
