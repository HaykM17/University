using Microsoft.AspNetCore.Identity;

namespace Application.Services.Abstract;

public interface ITokenService
{
    string CreateJWTToken(IdentityUser user, List<string> roles);
}