using Flurl;
using Flurl.Http;
using DataBaseContext.DTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using TheTaleApiClient.Models.Responses;
using TheTaleApiClient.Models;
using System.Linq;
using System.Threading.Tasks;
using Resources.StringResources;

namespace TheTaleApiClient
{
    public class ApiClient
    {
        private readonly string _apiClient = "api_client=Badj-0.002";
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Login into The Tale game
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="password">User password</param>
        /// <param name="cookieFromResponseDto">Out cookie values</param>
        /// <returns>True if login successful</returns>
        public async Task<CookieDto> LoginAsync(string login, string password)
        {
            var cookieDto = new CookieDto()
            {
                CsrfToken = "JguoToqJzNzrwdPWTxtirsQ6v2AB410F7yKeyTi1qm3CL5cPew4oGcIFTtnkHMLe",
                SessionId = "g4rpav872lt5fzqk85376azl7qq1qmgf"
            };

            var loginURL = @"https://the-tale.org/accounts/auth/api/login?api_version=1.0&" + _apiClient;

            var respGet = "https://the-tale.org/accounts/auth/login?next_url=%2F"
                .WithHeader("referer", "https://the-tale.org/")
                .GetAsync().Result;

            var respPost = await loginURL
                .WithHeader("referer", "https://the-tale.org/")
                .WithHeader("X-CSRFToken", "a7aDU4URDgbDhgQ2TLtmup9izGvRtfFCsAv0clfNgzysDOh5sUcZPskN6YbJtFYT")
                .WithHeader("Origin", "https://the-tale.org")
                .WithCookies(new { csrftoken = "a7aDU4URDgbDhgQ2TLtmup9izGvRtfFCsAv0clfNgzysDOh5sUcZPskN6YbJtFYT", sessionid = "ebdwyzgikmatqqm7hl6q78v4pdvyhdrf" })
                .PostUrlEncodedAsync(new
                {
                    email = login,
                    password = password,
                    remember = "on"
                });

            var loginResponse = await respPost.GetJsonAsync<LoginResponse>();

            if (loginResponse != null && loginResponse.status == ResponseStatuses.Ok)
            {
                var cookieCsrfToken = respPost.Cookies.FirstOrDefault(x => nameof(CookieDto.CsrfToken).Equals(x.Name, StringComparison.OrdinalIgnoreCase));
                var cookieSessionId = respPost.Cookies.FirstOrDefault(x => nameof(CookieDto.SessionId).Equals(x.Name, StringComparison.OrdinalIgnoreCase));
                var cookieFromResponseDto = new CookieDto()
                {
                    CsrfToken = cookieCsrfToken?.Value,
                    SessionId = cookieSessionId?.Value,
                    AccountId = loginResponse.data.account_id
                };

                return cookieFromResponseDto;
            }
            else
            {
                CookieDto cookieFromResponseDto = null;
                return cookieFromResponseDto;
            }
        }

        /// <summary>
        /// Use card from hero hand
        /// </summary>
        /// <param name="cookie">Credentials</param>
        /// <param name="cardToUse">Card data</param>
        /// <returns></returns>
        public async Task<bool> UseCardsAsync(CookieDto cookie, Card cardToUse)
        {
            var cardGuid = new Guid(cardToUse.uid).ToString();
            var loginURL = @"https://the-tale.org/game/cards/api/use?api_version=2.0&" + _apiClient + $"&card={cardGuid}";

            var respPost = await loginURL
                .WithHeader("referer", "https://the-tale.org/")
                .WithHeader("X-CSRFToken", cookie.CsrfToken)
                .WithHeader("Origin", "https://the-tale.org")
                .WithCookies(new { csrftoken = cookie.CsrfToken, sessionid = cookie.SessionId })
                .PostUrlEncodedAsync(new { });

            var response = await respPost.GetJsonAsync<CommonResponse>();

            return response.status == ResponseStatuses.Processing;
        }

        /// <summary>
        /// Get cards in hero hand
        /// </summary>
        /// <param name="cookie">Credentials</param>
        /// <returns></returns>
        public async Task<CardsResponse> GetCardsAsync(CookieDto cookie)
        {
            var apiURL = "https://the-tale.org/game/cards/api/get-cards?api_version=2.0&" + _apiClient;

            var cardsResponse = await apiURL
                .WithHeader("referer", "https://the-tale.org/")
                .WithHeader("X-CSRFToken", cookie.CsrfToken)
                .WithHeader("Origin", "https://the-tale.org")
                .WithCookies(new { csrftoken = cookie.CsrfToken, sessionid = cookie.SessionId })
                .GetJsonAsync<CardsResponse>();

            return cardsResponse;
        }

        public async Task<bool> GetLoginStatusAsync(CookieDto cookie)
        {
            var apiURL = "https://the-tale.org/accounts/messages/api/new-messages-number?api_version=0.1&" + _apiClient;

            var newMessagesNumberResponse = await apiURL
                .WithHeader("referer", "https://the-tale.org/")
                .WithHeader("X-CSRFToken", cookie.CsrfToken)
                .WithHeader("Origin", "https://the-tale.org")
                .WithCookies(new { csrftoken = cookie.CsrfToken, sessionid = cookie.SessionId })
                .GetJsonAsync<NewMessagesNumberResponse>();

            return newMessagesNumberResponse.status == ResponseStatuses.Ok;
        }

        /// <summary>
        /// Get game info
        /// </summary>
        /// <param name="cookie">Game cookie with credentials</param>
        /// <returns>Game info</returns>
        public async Task<GameInfoResponse> GetGameInfoAsync(CookieDto cookie)
        {
            var loginStatus = await GetLoginStatusAsync(cookie);
            if (loginStatus)
            {
                var apiURL = "https://the-tale.org/game/api/info?api_version=1.9&" + _apiClient;

                var gameInfo = await apiURL
                    .WithHeader("referer", "https://the-tale.org/")
                    .WithHeader("X-CSRFToken", cookie.CsrfToken)
                    .WithHeader("Origin", "https://the-tale.org")
                    .WithCookies(new { csrftoken = cookie.CsrfToken, sessionid = cookie.SessionId })
                    .GetJsonAsync<GameInfoResponse>();

                return gameInfo;
            }

            return (GameInfoResponse)null;
        }
    }
}