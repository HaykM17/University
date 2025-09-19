namespace Application.Dtos.Response;

public class BulkUpdateDto
{
    public int Updated { get; set; }
    public List<int> NotFoundIds { get; set; } = [];
}