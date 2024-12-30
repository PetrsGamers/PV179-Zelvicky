namespace CapEnjoyer.BL.DTOs;

public class PaginatedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
}

