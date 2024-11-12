using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Utils;

public static class OAuth
{
  private static DateTime lastTokenRefresh = DateTime.MinValue;
  private static TokenObject? currentToken = null;
  public static async Task<string> GetAccessTokenAsync(string clientId, string clientSecret, string redirectUri)
  {
    if (currentToken is null)
    {
      try
      {
        // Try to read "refresh_token.json"
        // Refresh and return on success
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText("refresh_token.json")) ?? new Dictionary<string, string>();

        if (dict.ContainsKey("refresh_token") && dict["refresh_token"] != "")
        {
          currentToken = await RefreshToken(dict["refresh_token"], clientId, clientSecret);
          return currentToken.accessToken;
        }
        else
        {
          // Continue to catch
          throw new Exception();
        }
      }
      catch
      {
        // Couldn't read refresh token
        // Prompt user for full OAuth2 flow
        Console.WriteLine("Nem található korábbról elmentett token. Hitelesítés szükséges.");
        currentToken = await DoAuthFlow(clientId, clientSecret, redirectUri);
        return currentToken.accessToken;
      }

    }
    else if (DateTime.Now > currentToken.expiration)
    {
      // Current token is present but expired
      currentToken = await RefreshToken(currentToken.refreshToken, clientId, clientSecret);
      return currentToken.accessToken;
    }
    else
    {
      // All correct, simply return the token
      return currentToken.accessToken;
    }
  }

  private static async Task<TokenObject> DoAuthFlow(string clientId, string clientSecret, string redirectUri)
  {
    var authLink = $"https://accounts.google.com/o/oauth2/auth?client_id={clientId}&redirect_uri={redirectUri}&scope=https://www.googleapis.com/auth/youtube&response_type=code&access_type=offline&prompt=consent";
    Process.Start(new ProcessStartInfo { FileName = authLink, UseShellExecute = true });

    var httpListener = new HttpListener();
    httpListener.Prefixes.Add(redirectUri + "/");
    httpListener.Start();

    Console.WriteLine("Várakozás a hitelesítésre...");
    var context = await httpListener.GetContextAsync();

    var response = context.Response;
    string responseString = "Hitelesitve. Bezarhatod ezt az oldalt.";
    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
    response.ContentLength64 = buffer.Length;
    var responseoutput = response.OutputStream;
    await responseoutput.WriteAsync(buffer, 0, buffer.Length);
    responseoutput.Close();
    httpListener.Stop();

    // Error handling
    string? error = context.Request.QueryString.Get("error");
    if (error != null)
    {
      Console.WriteLine($"OAuth hitelesítési hiba: {error}.");
      throw new Exception("OAuth hitelesítési hiba.");
    }
    if (context.Request.QueryString.Get("code") is null)
    {
      Console.WriteLine($"Malformed authorization response. {context.Request.QueryString}");
      throw new Exception("OAuth authorization error.");
    }


    Console.WriteLine("Hitelesítve.");
    var code = context.Request.QueryString.Get("code");

    var exchangeUrl = $"https://oauth2.googleapis.com/token?client_id={clientId}&client_secret={clientSecret}&code={code}&grant_type=authorization_code&redirect_uri={redirectUri}";

    var httpClient = new HttpClient();

    var content = new FormUrlEncodedContent(new Dictionary<string, string> { }); // Empty content
    var responseMessage = await httpClient.PostAsync(exchangeUrl, content);

    if (!responseMessage.IsSuccessStatusCode)
    {
      Console.WriteLine($"Hiba a tokenváltásnál. {responseMessage.StatusCode}\n{responseMessage.ReasonPhrase}");
      throw new Exception("Hiba a tokenváltásnál.");
    }

    var responseContent = await responseMessage.Content.ReadAsStringAsync();
    var responseContentJson = JsonSerializer.Deserialize<TokenExchangeResponse>(responseContent);

    if (responseContentJson is null)
    {
      Console.WriteLine($"Hiba a tokenváltásnál. {responseContent}");
      throw new Exception("Hiba a tokenváltásnál.");
    }

    var token = new TokenObject
    {
      accessToken = responseContentJson.access_token,
      refreshToken = responseContentJson.refresh_token,
      expiration = DateTime.Now.AddSeconds(responseContentJson.expires_in)
    };

    lastTokenRefresh = DateTime.Now;

    var refreshToken = responseContentJson.refresh_token;
    var refreshTokenJson = JsonSerializer.Serialize(new Dictionary<string, string> { { "refresh_token", refreshToken } });
    File.WriteAllText("refresh_token.json", refreshTokenJson);

    return token;
  }

  private static async Task<TokenObject> RefreshToken(string token, string clientId, string clientSecret)
  {
    var refreshUrl = $"https://oauth2.googleapis.com/token?client_id={clientId}&client_secret={clientSecret}&refresh_token={token}&grant_type=refresh_token";

    var httpClient = new HttpClient();
    var content = new FormUrlEncodedContent(new Dictionary<string, string> { }); // Empty content
    var responseMessage = await httpClient.PostAsync(refreshUrl, content);

    if (!responseMessage.IsSuccessStatusCode)
    {
      Console.WriteLine($"Hiba a tokenfrissítésnél. {responseMessage.StatusCode}\n{responseMessage.ReasonPhrase}");
      throw new Exception("Hiba a tokenfrissítésnél.");
    }

    var responseContent = await responseMessage.Content.ReadAsStringAsync();
    var responseContentJson = JsonSerializer.Deserialize<TokenRefreshResponse>(responseContent);

    if (responseContentJson is null)
    {
      Console.WriteLine($"Hiba a tokenfrissítésnél. {responseContent}");
      throw new Exception("Hiba a tokenfrissítésnél.");
    }

    var newToken = new TokenObject
    {
      accessToken = responseContentJson.access_token,
      refreshToken = token,
      expiration = DateTime.Now.AddSeconds(responseContentJson.expires_in)
    };

    lastTokenRefresh = DateTime.Now;

    return newToken;
  }
}