namespace Caller
{
    public class UrlCaller
    {
        public async Task<string> GetResponseString(string uri)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(uri);
            var contents = await response.Content.ReadAsStringAsync();

            return contents;
        }

    }
}