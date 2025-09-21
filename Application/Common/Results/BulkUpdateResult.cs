namespace Application.Common.Results;

public class BulkUpdateResult
{
    public int Updated { get; set; }
    public List<int> NotFoundIds { get; set; } = [];
}