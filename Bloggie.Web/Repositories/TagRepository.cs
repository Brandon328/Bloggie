using Azure.Core;
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BloggieDbContext _context;
        public TagRepository(BloggieDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            var tags = await _context.Tag.ToListAsync();

            return tags;
        }
        public async Task<Tag?> GetByIdAsync(Guid id)
        {
            var tag = await _context.Tag.FirstOrDefaultAsync(x => x.Id == id);

            return tag;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await _context.Tag.AddAsync(tag);
            await _context.SaveChangesAsync(); // Save changes to the database

            return tag;
        }
        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await _context.Tag.FirstOrDefaultAsync(x => x.Id == tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await _context.SaveChangesAsync(); // Save changes to the database
            }

            return existingTag;
        }
        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var tag = await _context.Tag.FindAsync(id);

            if (tag != null)
            {
                _context.Tag.Remove(tag);
                await _context.SaveChangesAsync(); // Save changes to the database
            }

            return tag;
        }
    }
}
