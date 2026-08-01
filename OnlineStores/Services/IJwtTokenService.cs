public interface IJwtTokenService
{
    JwtTokenResult GenerateAccessToken(User user);

}
