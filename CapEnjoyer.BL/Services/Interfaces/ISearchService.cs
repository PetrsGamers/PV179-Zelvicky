namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;

public interface ISearchService
{
    Task<SearchDto> GetSearchResult(string searchField);

}
