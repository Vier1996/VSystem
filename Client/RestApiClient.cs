using VSystem.Internal.Keys;

namespace VSystem.Client;

public class RestApiClient : IDisposable
{
    private readonly HttpClient _httpClient;

    public RestApiClient(RunClientKey runKey)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(runKey.ServerUrl.TrimEnd('/') + "/")
        };
    }

    public async Task<string> GetPingAsync(CancellationToken cancellationToken)
    {
        return await GetAsync("api/server/ping", cancellationToken);
    }

    public async Task<string> GetTimeAsync(CancellationToken cancellationToken)
    {
        return await GetAsync("api/server/time", cancellationToken);
    }

    private async Task<string> GetAsync(string relativePath, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(relativePath, cancellationToken);

        if (response.IsSuccessStatusCode == false)
        {
            string errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"Request [{relativePath}] failed with status {(int)response.StatusCode}: {errorBody}");
        }

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
