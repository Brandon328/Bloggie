using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private ITagRepository _tagRepository;

        public AdminTagsController(
            ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest request)
        {
            // Mapping AddTagRequest to Tag domain model
            var tag = new Tag
            {
                Name = request.Name,
                DisplayName = request.DisplayName
            };

            await _tagRepository.AddAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            // use db context to get all tags
            var tags = await _tagRepository.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // 1rst method
            // var tag = _bloggieDbContext.Tag.Find(id);

            // 2nd method
            var tag = await _tagRepository.GetByIdAsync(id);

            if (tag != null)
            {
                var request = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(request);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest request)
        {
            var tag = new Tag
            {
                Id = request.Id,
                Name = request.Name,
                DisplayName = request.DisplayName
            };

            var updatedTag = await _tagRepository.UpdateAsync(tag);

            if(updatedTag != null)
            {
                // Show success notification
            }
            else
            {
                // Show error notification
            }

            return RedirectToAction("Edit", new { id = request.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedTag = await _tagRepository.DeleteAsync(id);

            if (deletedTag != null)
            {
                // Show success message
            }
            else
            {
                // Show error message
            }

            return RedirectToAction("List");
        }
    }
}
