using System.Collections.Generic;
using IdentityServer4.Models;

namespace Id4Server;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResources.Address(),
            new IdentityResources.Phone(),
        };

    public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
    {
        new("room_api"),
        new("room_signalr"),
        new("user_api")
    };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new()
            {
                ClientId = "client1",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "room_api" }
            },

            // interactive client using code flow + pkce
            // 授权码模式
            new()
            {
                ClientId = "client2",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "room_api", "room_signalr", "user_api" }
            },
            
            new()
            {
                ClientId = "client3",
                ClientName = "Resource Owner Password Client",
                
                ClientSecrets = { new Secret("E99D62B0-0D20-4F74-8C4E-8247D5806F0A".Sha256()) },
                RequireClientSecret = true,
                
                AllowRememberConsent = false,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                
                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "email", "address", "phone", "offline_access", "room_api", "room_signalr", "user_api" },
                
                AlwaysIncludeUserClaimsInIdToken = true,
                IncludeJwtId = true,
            }
        };
}