using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuoteGenerationMS.Application.Interface;
using QuoteGenerationMS.Application.Mapper;
using QuoteGenerationMS.Infrastructure.Data;
using QuoteGenerationMS.Infrastructure.Services;
using QuoteGenerationMS.Infrastructure.Repositories;
using QuoteGenerationMS.Infrastructure.Clients;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("dbconn")));

builder.Services.AddAutoMapper(typeof(MappingData));

builder.Services.AddHttpClient<IProductManagementClient, ProductManagementClient>(client =>
{
    var baseUrl = builder.Configuration["ServiceUrls:ProductManagement"];

    if (string.IsNullOrWhiteSpace(baseUrl) ||
        !Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri))
    {
        throw new InvalidOperationException(
            "Product Management service URL is missing or invalid.");
    }

    client.BaseAddress = uri;
});

builder.Services.AddHttpClient<IUnderwritingClient, UnderwritingClient>(client =>
{
    var baseUrl = builder.Configuration["ServiceUrls:Underwriting"];

    if (string.IsNullOrWhiteSpace(baseUrl) ||
        !Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri))
    {
        throw new InvalidOperationException(
            "Underwriting service URL is missing or invalid.");
    }

    client.BaseAddress = uri;
});

builder.Services.AddHttpClient<ICustomerRegistrationClient, CustomerRegistrationClient>(
    client =>
    {
        var baseUrl = builder.Configuration["ServiceUrls:CustomerRegistration"];

        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new InvalidOperationException(
                "Customer Registration service URL is missing.");

        client.BaseAddress = new Uri(baseUrl);
    });


builder.Services.AddScoped<IQuoteService, QuoteService>();
builder.Services.AddScoped<IPremiumCalculationService,PremiumCalculationService>();
builder.Services.AddScoped<IRiskAssessmentService,RiskAssessmentService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IQuoteRiderService, QuoteRiderService>();
builder.Services.AddScoped<IQuoteApprovalService,QuoteApprovalService>();
builder.Services.AddScoped<IQuoteLifecycleService,QuoteLifecycleService>();
builder.Services.AddScoped<IQuoteHistoryService,QuoteHistoryService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var jwtKey = builder.Configuration["JwtSettings:Key"]
    ?? throw new InvalidOperationException("JWT signing key is missing.");

var issuer = builder.Configuration["JwtSettings:Issuer"]
    ?? throw new InvalidOperationException("JWT issuer is missing.");

var audience = builder.Configuration["JwtSettings:Audience"]
    ?? throw new InvalidOperationException("JWT audience is missing.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,

            ValidateAudience = true,
            ValidAudience = audience,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AngularFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();