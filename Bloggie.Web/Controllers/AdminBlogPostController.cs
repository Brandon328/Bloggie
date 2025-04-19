using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
    public class AdminBlogPostController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        public AdminBlogPostController(
            ITagRepository tagRepository,
            IBlogPostRepository blogPostRepository)
        {
            _tagRepository = tagRepository;
            _blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await _tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.DisplayName
                })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest request)
        {
            var blogPost = new BlogPost
            {
                Heading = request.Heading,
                PageTitle = request.PageTitle,
                Content = request.Content,
                ShortDescription = request.ShortDescription,
                FeaturedImgUrl = request.FeaturedImgUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                Author = request.Author,
                Visible = request.Visible
            };

            var selectedTags = new List<Tag>();
            foreach (var tagId in request.SelectedTags)
            {
                var tag = await _tagRepository.GetByIdAsync(Guid.Parse(tagId));
                if (tag != null)
                {
                    selectedTags.Add(tag);
                }
            }

            blogPost.Tags = selectedTags;

            await _blogPostRepository.AddAsync(blogPost);

            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogs = await _blogPostRepository.GetAllAsync();

            return View(blogs);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blog = await _blogPostRepository.GetByIdAsync(id);
            var tags = await _tagRepository.GetAllAsync();

            if (blog != null)
            {
                var blogRequest = new EditBlogPostRequest
                {
                    Id = blog.Id,
                    Heading = blog.Heading,
                    PageTitle = blog.PageTitle,
                    Content = blog.Content,
                    ShortDescription = blog.ShortDescription,
                    FeaturedImgUrl = blog.FeaturedImgUrl,
                    UrlHandle = blog.UrlHandle,
                    PublishedDate = blog.PublishedDate,
                    Author = blog.Author,
                    Visible = blog.Visible,
                    Tags = tags.Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.DisplayName
                    }),
                    SelectedTags = blog.Tags.Select(t => t.Id.ToString()).ToArray()
                };

                return View(blogRequest);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest request)
        {
            var blog = await _blogPostRepository.GetByIdAsync(request.Id);

            if (blog == null)
            {
                return NotFound();
            }

            blog.Heading = request.Heading;
            blog.PageTitle = request.PageTitle;
            blog.Content = request.Content;
            blog.ShortDescription = request.ShortDescription;
            blog.FeaturedImgUrl = request.FeaturedImgUrl;
            blog.UrlHandle = request.UrlHandle;
            blog.PublishedDate = request.PublishedDate;
            blog.Author = request.Author;
            blog.Visible = request.Visible;

            var selectedTags = new List<Tag>();
            foreach (var tagId in request.SelectedTags)
            {
                var tag = await _tagRepository.GetByIdAsync(Guid.Parse(tagId));
                if (tag != null)
                {
                    selectedTags.Add(tag);
                }
            }

            blog.Tags = selectedTags;

            await _blogPostRepository.UpdateAsync(blog);

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var blog = await _blogPostRepository.DeleteAsync(id);

            if (blog == null)
            {
                return NotFound();
            }

            return RedirectToAction("List");
        }
    }
}
