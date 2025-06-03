using Newtonsoft.Json;
using System.Text;

namespace WhatsAppChatGPTBot;

public interface IOpenAIService
{
    Task<string> GetReplyAsync(string message);
}

public class OpenAIService : IOpenAIService
{
    private readonly string _apiKey;

    public OpenAIService(IConfiguration config)
    {
        _apiKey = config["OpenAI:ApiKey"];
    }

    public async Task<string> GetReplyAsync(string message)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

        var request = new
        {
            model = "gpt-4",
            messages = new[] {
            new { role = "user", content = message }
        }
        };

        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
        var result = await response.Content.ReadAsStringAsync();

        dynamic json = JsonConvert.DeserializeObject(result);
        return json.choices[0].message.content;
    }
}
