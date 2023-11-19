
using System.Net;
using Microsoft.AspNetCore.Mvc;
using PusherServer;

namespace VacancyProAPI.Controllers;

public class ChatController : ControllerBase {
    
    
    [HttpPost]
    public async Task<ActionResult> HelloWorld() {
        var options = new PusherOptions
        {
            Cluster = "eu",
            Encrypted = true
        };

        var pusher = new Pusher(
            "1708130",
            "74f1716b51dbbc6c19ca",
            "c3341eb1f00700d5711a",
            options);

        var result = await pusher.TriggerAsync(
            "my-channel",
            "my-event",
            new { message = "hello world" } );

        return Ok();
    }
}