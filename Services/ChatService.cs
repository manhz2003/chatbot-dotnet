using Chatbot.Models;
using Newtonsoft.Json;
using System.Text;

namespace Chatbot.Services
{
    public class ChatService : IChatService
    {
        private static readonly HttpClient Http = new HttpClient();

        public async Task<ChatResponse> SendMessageAsync(ChatRequest request)
        {
            var apiUrl = "https://mentoroid-api.geniam.com/client/chatGPT/test";

            var jsonContent = new
            {
                question = request.Question,
                model = "gpt-4o-2024-05-13"
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(jsonContent), Encoding.UTF8, "application/json");
            var response = await Http.PostAsync(apiUrl, requestBody);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"API call failed: {errorMessage}");
            }

            var resContext = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(resContext);

            if (data != null && data.data != null && data.data.choices != null && data.data.choices.Count > 0)
            {
                var text = data.data.choices[0].message.content.ToString();
                return new ChatResponse { ResponseText = text };
            }

            throw new Exception("No choices returned");
        }
    }
}
