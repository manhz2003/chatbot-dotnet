namespace Chatbot.Services
{
    public interface IChatService
    {
        Task<Chatbot.Models.ChatResponse> SendMessageAsync(Chatbot.Models.ChatRequest request);
    }
}
