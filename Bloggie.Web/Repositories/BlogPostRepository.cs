using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Bloggie.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private BloggieDbContext _dbContext;
        public BlogPostRepository(BloggieDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await _dbContext.AddAsync(blogPost);
            await _dbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var deleteBlog = await _dbContext.BlogPost.FindAsync(id);

            if (deleteBlog != null)
            {
                _dbContext.BlogPost.Remove(deleteBlog);
                await _dbContext.SaveChangesAsync();
            }

            return deleteBlog;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        => await _dbContext.BlogPost.Include(x=>x.Tags)
                .ToListAsync();


        public async Task<BlogPost?> GetByIdAsync(Guid id)
        => await _dbContext.BlogPost.Include(x=>x.Tags)
            .FirstOrDefaultAsync(x=>x.Id==id);

        public async Task<BlogPost?> UpdateAsync(BlogPost updatedBlogPost)
        {
            var blogPost= await _dbContext.BlogPost.FirstOrDefaultAsync(x => x.Id == updatedBlogPost.Id);

            if (blogPost != null)
            {
                blogPost.Heading = updatedBlogPost.Heading;
                blogPost.PageTitle = updatedBlogPost.PageTitle;
                blogPost.Content = updatedBlogPost.Content;
                blogPost.ShortDescription = updatedBlogPost.ShortDescription;
                blogPost.FeaturedImgUrl = updatedBlogPost.FeaturedImgUrl;
                blogPost.UrlHandle = updatedBlogPost.UrlHandle;
                blogPost.PublishedDate = updatedBlogPost.PublishedDate;
                blogPost.Author = updatedBlogPost.Author;
                blogPost.Visible = updatedBlogPost.Visible;
                blogPost.Tags = updatedBlogPost.Tags;

                await _dbContext.SaveChangesAsync();
            }
            return updatedBlogPost;
        }
    }
}
