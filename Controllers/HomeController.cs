using Microsoft.AspNetCore.Mvc;
using Chatbot.Models;
using Chatbot.Services;

namespace ChatbotApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IChatService _chatService;

        public HomeController(IChatService chatService)
        {
            _chatService = chatService;
        }

        public IActionResult Index()
        {
            // Lấy danh sách các tin nhắn từ TempData
            ViewBag.Messages = TempData["Messages"] as List<string> ?? new List<string>();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string message)
        {
            var messages = TempData["Messages"] as List<string> ?? new List<string>();
            string botResponseText = string.Empty; // Khởi tạo biến để lưu phản hồi của chatbot

            if (string.IsNullOrWhiteSpace(message))
            {
                messages.Add("Please enter a message.");
            }
            else
            {
                try
                {
                    var chatRequest = new ChatRequest { Question = message };
                    var chatResponse = await _chatService.SendMessageAsync(chatRequest);

                    messages.Add($"User: {message}");
                    botResponseText = chatResponse.ResponseText; // Gán giá trị phản hồi của chatbot
                    messages.Add($"Chatbot: {botResponseText}");
                }
                catch (Exception ex)
                {
                    messages.Add($"An error occurred: {ex.Message}");
                }
            }

            TempData["Messages"] = messages;
            TempData.Keep("Messages");

            // Trả về chỉ HTML của tin nhắn mới, không phải toàn bộ container
            return PartialView("_Messages", new List<string> { $"User: {message}", $"Chatbot: {botResponseText}" });
        }

    }
}
