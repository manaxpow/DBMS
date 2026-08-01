using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType<LoginResponse>(
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<User>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginAsync(request, cancellationToken);
        if (response == null)
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Unauthorized",
                Detail = "Invalid username or password.",
                Status = StatusCodes.Status401Unauthorized
            });
        }

        // Set the access token and refresh token in cookies
        CookieHelper.SetCookie(Response, "AccessToken", response.AccessToken, response.AccessTokenExpiresAt);
        CookieHelper.SetCookie(Response, "RefreshToken", response.RefreshToken, response.RefreshTokenExpiresAt);

        return Ok(response.User);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType<RegisterResponse>(
        StatusCodes.Status200OK)]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var response = await _authService.RegisterAsync(request, cancellationToken);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    [ProducesResponseType<RefreshTokenResponse>(
        StatusCodes.Status200OK)]
    public async Task<ActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        var refreshToken = CookieHelper.GetCookie(Request, "RefreshToken");
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Unauthorized",
                Detail = "Refresh token is missing.",
                Status = StatusCodes.Status401Unauthorized
            });
        }

        var response = await _authService.RefreshTokenAsync(refreshToken, cancellationToken);

        // Set the access token and refresh token in cookies
        CookieHelper.SetCookie(Response, "AccessToken", response.AccessToken, response.AccessTokenExpiresIn);
        CookieHelper.SetCookie(Response, "RefreshToken", response.RefreshToken, response.RefreshTokenExpiresIn);
        return Ok();
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType<CurrentUserResponse>(
        StatusCodes.Status200OK)]
    public async Task<ActionResult<CurrentUserResponse>> GetMe(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Unauthorized",
                Detail = "User ID claim is missing or invalid.",
                Status = StatusCodes.Status401Unauthorized
            });
        }

        var user = await _authService.GetMeAsync(userId, cancellationToken);
        return Ok(user);
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Logout()
    {
        // Delete the access token and refresh token cookies
        CookieHelper.DeleteCookie(Response, "AccessToken");
        CookieHelper.DeleteCookie(Response, "RefreshToken");
        return Ok(new { message = "Logged out successfully." });
    }
}
