namespace app.Models;

public class FilteredBuildsResponse
{
    public int TotalCount { get; set; }
    public int CurrentCount { get; set; }
    public int PageNumber { get; set; }
    public bool IsLastPage { get; set; }
    public List<BuildOrderList> List { get; set; } = new();
}