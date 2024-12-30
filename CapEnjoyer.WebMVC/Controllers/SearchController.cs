namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Models;


public class SearchController(ISearchService searchService) : Controller
{

    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Search(SearchViewModel model)
    {
        if (!string.IsNullOrEmpty(model.SearchField))
        {
            return RedirectToAction(nameof(SearchResult), model);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> SearchResult(SearchViewModel model)
    {
        if (string.IsNullOrEmpty(model.SearchField))
        {
            return View(model);
        }

        var searchResult = await searchService.GetSearchResult(model.SearchField);

        model = searchResult.Adapt<SearchViewModel>();

        return View(model);
    }

}

