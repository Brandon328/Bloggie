using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bloggie.Web.Models;
using Bloggie.Web.Repositories;
using Bloggie.Web.Models.ViewModels;

namespace Bloggie.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly ITagRepository _tagRepository;

    public HomeController(ILogger<HomeController> logger,
        IBlogPostRepository blogPostRepository,
        ITagRepository tagRepository)
    {
        _logger = logger;
        _blogPostRepository = blogPostRepository;
        _tagRepository = tagRepository;
    }

    public async Task<IActionResult> Index()
    {
        var blogPosts = await _blogPostRepository.GetAllAsync();
        var tags = await _tagRepository.GetAllAsync();

        var homeViewModel = new HomeViewModel
        {
            BlogPosts = blogPosts,
            Tags = tags
        };

        return View(homeViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
