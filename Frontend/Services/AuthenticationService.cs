//using Frontend.Helpers;
using Frontend.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using Frontend.Services;
using System.Net.Http.Headers;

namespace Frontend.Services
{
    public class AuthenticationService : AuthenticationStateProvider
    {
        private ILocalStorageService _localStorageService;
        private HttpClient _http;
        private IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();
        public AuthenticationService(ILocalStorageService localStorage, HttpClient http, IEnumerable<Claim> claims)
        {
            _localStorageService = localStorage;
            _http = http;
            _claims = claims;
        }
        public async Task GetClaimsPrincipalData<User>()
        {
            var authState = await GetAuthenticationStateAsync();
            var user = authState.User;

            if (!user.Identity.IsAuthenticated)
            {
                return;
                //authMessage = $"{user.Identity.Name} is authenticated.";
                //claims = user.Claims;
                //surnameMessage =
                //    $"Surname: {user.FindFirst(c => c.Type == ClaimTypes.SerialNumber).Value}";
            }
            else
            {
                //_user.Username = user.Identity.Name;
                //_user.Id = Convert.ToInt32(user.FindFirst(c => c.Type == ClaimTypes.SerialNumber)?.Value);
                //_claims = user.Claims;
            }
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var jtoken = await _localStorageService.GetItem<string>("user");

            //var handler = new JwtSecurityTokenHandler(); good tools here
            //var token = handler.ReadJwtToken(jtoken);

            //not authorized user below
            var identity = new ClaimsIdentity();
            _http.DefaultRequestHeaders.Authorization = null;

            //if user is logged in
            if (!string.IsNullOrEmpty(jtoken))
            {
                identity = new ClaimsIdentity((ParseClaimsFromJWT(jtoken)), "jwt"); //authorized user is present
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jtoken.Replace("\"", "")); //remove {"}token{"}
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state)); //this is where context.User.Identity comes from, the state checks token claims

            return state;
        }

        //manual jwt parsing below, check out ms docs ModelIdentity for easier methods, this one is just the most detailed way to check a token
        public static IEnumerable<Claim> ParseClaimsFromJWT(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            ExtractRolesFromJWT(claims, keyValuePairs);

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }
        private static void ExtractRolesFromJWT(List<Claim> claims, Dictionary<string, object> keyValuePairs)
        {
            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles is not null)
            {
                var parsedRoles = roles.ToString().Trim().TrimStart(trimChars: '[').TrimEnd(trimChars: ']').Split(separator: ',');

                if (parsedRoles.Length > 1)
                {
                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole.Trim('"')));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRoles[0]));
                }
                keyValuePairs.Remove(ClaimTypes.Role);
            }
        }
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }
            return Convert.FromBase64String(base64);
        }
    }
    
}