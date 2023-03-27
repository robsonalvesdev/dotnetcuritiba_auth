using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Authority = "https://oauth2.prosperainovacao.com.br/realms/consilux";
            options.RequireHttpsMetadata = false;
            options.MetadataAddress = "http://localhost:8080/realms/dotnetcuritiba/.well-known/openid-configuration";
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidAudience = "",
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true
            };
        });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Stock", policy => policy.RequireClaim("scope", "stock"));
        options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
    });

    builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();