using System.Text;
using System.Text.Json.Serialization;
using leave_a_note_core.Models.Authentication;
using leave_a_note_core.Services;
using leave_a_note_core.Services.Authenticators;
using leave_a_note_core.Services.PasswordHasher;
using leave_a_note_data;
using leave_a_note_data.Entities;
using leave_a_note_data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AuthenticationConfiguration>(builder.Configuration.GetSection("Authentication"));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:3000",
                    "https://witty-wave-02367ea03.1.azurestaticapps.net")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add data storage services
builder.Services.AddDbContext<LeaveANoteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<DataSeeder>();

// Add data repository services
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRepository<Note>, NoteRepository>();

// Add service layer services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<INoteService, NoteService>();

// Authentication services
builder.Services.AddTransient<Authenticator>();
builder.Services.AddTransient<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:AccessTokenSecret"])),
        ValidIssuer = builder.Configuration["Authentication:Audience"],
        ValidAudience = builder.Configuration["Authentication:Issuer"],
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var initializer = services.GetRequiredService<DataSeeder>();
initializer.Seed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
