namespace CompanySystem.Shared.Requests;

public class PaginationFilterRequest
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;


    // Filtering
    public string? Search { get; set; }


    // Sorting
    public string? SortBy { get; set; }

    public bool IsDescending { get; set; } = false;
}