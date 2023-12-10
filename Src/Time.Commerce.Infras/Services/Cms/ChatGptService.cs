using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Time.Commerce.Application.Services.Cms;
using Time.Commerce.Contracts.Models.Cms;

namespace Time.Commerce.Infras.Services.Cms
{
    public class ChatGptService : IChatGptService
    {
        public async Task<string> ChatAsync(string message, CancellationToken cancellationToken = default)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "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");

            var messages = new List<ChatGptMessage>();
            messages!.Add(new()
            {
                Role = ChatGptRoles.User,
                Content = message
            });

            var request = new ChatGptRequest
            {
                Model = ChatGptModels.Gpt35Turbo,
                Messages = messages.ToArray()
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpResponse = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request, cancellationToken);
            var response = await httpResponse.Content.ReadFromJsonAsync<ChatGptResponse>(cancellationToken: cancellationToken);
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new ChatGptException(response.Error!, httpResponse.StatusCode);
            }

            // Adds the response message to the conversation cache.
            return response.Choices[0].Message.Content;
        }
    }
}
