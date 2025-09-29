using Microsoft.AspNetCore.Identity;

namespace Application.Repositories.GenericRepository;

public interface IAuthRepository
{
    Task<IdentityUser?> FindByUserNameAsync(string userName, CancellationToken ct = default);

    Task<bool> UserRoleExistsAsync(string role, CancellationToken ct = default);

    Task<bool> CheckUserPasswordAsync(IdentityUser user, string password, CancellationToken ct = default);

    Task<IdentityUser?> FindUserByEmailAsync(string email, CancellationToken ct = default);

    Task<IdentityResult> CreateUserAsync(IdentityUser identityUser, string password, CancellationToken ct = default);

    Task<IdentityResult> AddUserToRolesAsync(IdentityUser identityUser, IEnumerable<string> roles, CancellationToken ct = default);
    Task<IList<string>> GetUserRolesAsync(IdentityUser identityUser, CancellationToken ct = default);
}