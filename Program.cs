using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VacancyProAPI;
using VacancyProAPI.Models;
using VacancyProAPI.Services.ChatService;
using VacancyProAPI.Models.DbModels;
using VacancyProAPI.Services.MailService;
using VacancyProAPI.Services.UserService;

var builder = WebApplication.CreateBuilder(args);
var builderConf = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


var configuration = builderConf.Build();

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddCors(p => p.AddPolicy("VacancyPro", builder =>
{
    builder.WithOrigins(configuration.GetSection("CorsURL").Value).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));
builder.Services.AddSignalR();
var connectionString = configuration.GetConnectionString("PierreDb");


builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

// TODO : Remplacer UseSqlServer par la connctionString
/*builder.Services.AddDbContext<DatabaseContext>(opt =>
{
    opt.UseSqlServer("Server=192.168.132.200,11433;Database=Q200007;Uid=Q200007;Pwd=0007;TrustServerCertificate=True");
});*/

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
}).AddEntityFrameworkStores<DatabaseContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:JwtSecret").Value)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("./firebase.json"),
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
    var config = scope.ServiceProvider.GetService<IConfiguration>();

    await Seed.AddDefaultRole(roleManager);
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("VacancyPro");
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chatsocket");

app.Run();
