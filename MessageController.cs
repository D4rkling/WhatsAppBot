using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML;

namespace WhatsAppChatGPTBot;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IOpenAIService _openAIService;

    public MessageController(IOpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    [HttpPost]
    public async Task<IActionResult> ReceiveMessage([FromForm] string Body, [FromForm] string From)
    {
        var response = await _openAIService.GetReplyAsync(Body);
        var twilioResponse = new MessagingResponse();
        twilioResponse.Message(response);
        return Content(twilioResponse.ToString(), "text/xml");
    }
}
