using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using VacancyProAPI.Models;
using VacancyProAPI.Services.ChatService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddSignalR();

builder.Services.AddDbContext<ApplicationContext>(opt =>
{
    opt.UseSqlServer("Server=192.168.132.200,11433;Database=Q200007;Uid=Q200007;Pwd=0007;TrustServerCertificate=True");
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("./firebase.json"),
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chatsocket");

app.Run();
