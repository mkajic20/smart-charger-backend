using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;


public class GoogleAuthService : IGoogleAuthService {

    public async Task<LoginResponseDTO> GetUserInfoAsync(string accessToken)
    {
        try
        {
            var credential = GoogleCredential.FromAccessToken(accessToken);
            var service = new Oauth2Service(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Smart Charger",
            });

            var userInfo = await service.Userinfo.Get().ExecuteAsync();

            if (userInfo == null)
            {
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = "Invalid access token."
                };
            }

            return new LoginResponseDTO
            {
                Success = true,
                User = new UserDTO
                {
                    Email = userInfo.Email,
                    FirstName = userInfo.Name,
                    LastName = userInfo.FamilyName,
                }
            };
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return new LoginResponseDTO
            {
                Success = false,
                Message = "Invalid access token."
            };
        }
        catch (Exception ex)
        {
            return new LoginResponseDTO
            {
                Success = false,
                Message = "An error occurred: " + ex.Message
            };
        }
    }

}

