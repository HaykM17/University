namespace Application.Dtos.Response.Authentication;

public record RegisterResponseDto
{
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }
    public List<string>? MissingRoles { get; set; }
}