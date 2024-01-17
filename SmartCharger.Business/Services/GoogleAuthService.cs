using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;


    public class GoogleAuthService : IGoogleAuthService
    {
        public async Task<LoginResponseDTO> GetUserInfoFromAuthCodeAsync(string authorizationCode)
        {
            try
            {
                var clientSecrets = new ClientSecrets
                {
                    ClientId = "223586710221-3808p3ltsqf0e42ge6jun8mibsa2dt3k.apps.googleusercontent.com",
                    ClientSecret = "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"
                };

                var tokenResponse = await new GoogleAuthorizationCodeFlow(
                    new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets = clientSecrets
                    })
                    .ExchangeCodeForTokenAsync("user", authorizationCode, "https://developers.google.com/oauthplayground", CancellationToken.None);

                var credential = new UserCredential(
                    new GoogleAuthorizationCodeFlow(
                        new GoogleAuthorizationCodeFlow.Initializer
                        {
                            ClientSecrets = clientSecrets
                        }),
                    "user",
                    tokenResponse);

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

