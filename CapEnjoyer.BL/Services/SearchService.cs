namespace CapEnjoyer.BL.Services;

using DTOs;
using Interfaces;

public class SearchService(IBottleService bottleService, IProducerService producerService, ICapService capService) : ISearchService
{
    public async Task<SearchDto> GetSearchResult(string searchField)
    {
        var searchResult = new SearchDto
        {
            Bottles = await bottleService.GetBottlesbySearchFieldAsync(searchField),
            Producers = await producerService.GetProducersBySearchFieldAsync(searchField),
            Caps = await capService.GetCapsBySearchFieldAsync(searchField)
        };
        return searchResult;
    }
}
