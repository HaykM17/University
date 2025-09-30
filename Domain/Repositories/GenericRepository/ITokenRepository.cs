namespace Application.Repositories.GenericRepository;

public interface ITokenRepository<TToken, TUser, TRoles>
{
    TToken CreateJWTToken(TUser user, TRoles roles);
}