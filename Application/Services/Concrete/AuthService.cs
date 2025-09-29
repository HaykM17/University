using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.Authentication;
using Application.Repositories.GenericRepository;
using Application.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Concrete;

public class AuthService(IAuthRepository _authRepository, ITokenService _tokenService) : IAuthService
{
    public async Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto registerDto, CancellationToken cancellationToken = default)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.UserName,
        };

        var existingByUserName = await _authRepository.FindByUserNameAsync(registerDto.UserName);

        if (existingByUserName != null)
        {
            return new RegisterResponseDto
            {
                Succeeded = false,
                Errors = new List<string> { "User with this username already exists." }
            };
        }

        if (registerDto.Roles == null || registerDto.Roles.Count == 0)
        {
            return new RegisterResponseDto
            {
                Succeeded = false,
                Errors = new List<string> { "At least one role is required." }
            };
        }

        var roles = registerDto.Roles
            .Where(r => !string.IsNullOrWhiteSpace(r))
            .ToList();

        var missing = new List<string>();

        foreach (var role in roles)
        {
            if (!await _authRepository.UserRoleExistsAsync(role))
                missing.Add(role);
        }

        if (missing.Count > 0)
        {
            return new RegisterResponseDto
            {
                Succeeded = false,
                Errors = new List<string> { "Some roles do not exist." },
                MissingRoles = missing
            };
        }

        var identityResult = await _authRepository.CreateUserAsync(identityUser, registerDto.Password);

        if (identityResult.Succeeded)
        {
            if (registerDto.Roles != null && registerDto.Roles.Any())
            {
                identityResult = await _authRepository.AddUserToRolesAsync(identityUser, registerDto.Roles);

                if (identityResult.Succeeded)
                {
                    return new RegisterResponseDto
                    {
                        Succeeded = true,
                        Message = "User was register. Please login"
                    };
                }
            }
        }

        var errorDescriptions = identityResult.Errors.Select(e => e.Description).ToList();

        return new RegisterResponseDto
        {
            Succeeded = false,
            Errors = errorDescriptions
        };
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto logintDto, CancellationToken cancellationToken = default)
    {
        var user = await _authRepository.FindUserByEmailAsync(logintDto.UserName);

        if (user != null)
        {
            var checkPasswordResult = await _authRepository.CheckUserPasswordAsync(user, logintDto.Password);

            if (checkPasswordResult)
            {
                var roles = await _authRepository.GetUserRolesAsync(user);

                if (roles != null && roles.Any())
                {
                    var jwtToken = _tokenService.CreateJWTToken(user, roles.ToList());

                    return new LoginResponseDto
                    {
                        Succeeded = true,
                        Message = "User was login",
                        JwtToken = jwtToken
                    };
                }
            }
        }

        return new LoginResponseDto
        {
            Succeeded = false,
            Message = "Username or Password incorrect"
        };
    }
}