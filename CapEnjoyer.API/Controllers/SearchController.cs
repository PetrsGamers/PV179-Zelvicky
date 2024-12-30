namespace CapEnjoyer.API.Controllers;

using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SearchController(ISearchService searchService) : ControllerBase
{
    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] string searchField)
    {
        var searchResult = await searchService.GetSearchResult(searchField);
        return this.Ok(searchResult);
    }

}
