using System.Text;
using DonationHub.Application;
using DonationHub.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });
builder.Services.AddControllers();  
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationService();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => {
        options.Theme = ScalarTheme.Purple;
        options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });

}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

