using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;


public class GoogleAuthService : IGoogleAuthService { 
    public async Task<GoogleUserDTO> GetUserInfoAsync(string accessToken)
    {
        var credential = GoogleCredential.FromAccessToken(accessToken);
        var service = new Oauth2Service(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "Smart Charger",
        });

        var userInfo = await service.Userinfo.Get().ExecuteAsync();

        return new GoogleUserDTO
        {
            Email = userInfo.Email,
            Name = userInfo.Name,
            LastName = userInfo.FamilyName,

        };
    }
}
