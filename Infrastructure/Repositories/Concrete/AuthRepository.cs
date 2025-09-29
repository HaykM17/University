using Application.Repositories.GenericRepository;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories.Concrete;

public class AuthRepository(UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager) : IAuthRepository
{
    public async Task<IdentityUser?> FindByUserNameAsync(string userName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return null;
        }

        return await _userManager.FindByNameAsync(userName);
    }

    public async Task<bool> UserRoleExistsAsync(string role, CancellationToken ct = default)
    {
        return await _roleManager.RoleExistsAsync(role);
    }

    public async Task<bool> CheckUserPasswordAsync(IdentityUser user, string password, CancellationToken ct = default)
    {
        if (user is null)
        {
            return false;
        }

        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IdentityUser?> FindUserByEmailAsync(string email, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IdentityResult> CreateUserAsync(IdentityUser identityUser, string password, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return null!;
        }

        return await _userManager.CreateAsync(identityUser, password);
    }

    public async Task<IdentityResult> AddUserToRolesAsync(IdentityUser identityUser, IEnumerable<string> roles, CancellationToken ct = default)
    {
        if(roles == null)
        {
            return null!;
        }

        return await _userManager.AddToRolesAsync(identityUser, roles);
    }

    public async Task<IList<string>> GetUserRolesAsync(IdentityUser identityUser, CancellationToken ct = default)
    {
        return await _userManager.GetRolesAsync(identityUser);
    }
}