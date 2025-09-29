namespace Application.Dtos.Response.Authentication;

public record LoginResponseDto
{
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public string JwtToken { get; set; } = null!;
}