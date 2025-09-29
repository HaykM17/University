using Application.Repositories.GenericRepository;
using Application.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Concrete;

public class TokenService(ITokenRepository<string, IdentityUser, List<string>> _tokenRepository) : ITokenService
{
    public string CreateJWTToken(IdentityUser user, List<string> roles)
    {
        return _tokenRepository.CreateJWTToken(user, roles);
    }
}