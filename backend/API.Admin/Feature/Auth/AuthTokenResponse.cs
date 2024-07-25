using poc.core.api.net8;

namespace API.Admin.Feature.Auth;

public class AuthTokenResponse : BaseResponse
{
    public AuthTokenResponse(string token)
    {
        Token = token;
    }
    public string Token { get; }
}
