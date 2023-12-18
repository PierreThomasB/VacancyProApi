using Microsoft.AspNetCore.SignalR;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin.Messaging;

namespace VacancyProAPI.Services.ChatService;

public class ChatHub : Hub
{
    
    private readonly FirebaseClient firebaseClient = new FirebaseClient("https://votre-projet-id.firebaseio.com/");

        public   Task SendMessage1(string user, string message)             
        {
             firebaseClient.Child("messages").PostAsync(new Message());

            return Clients.All.SendAsync("ReceiveOne", user, message);
        }
    }

