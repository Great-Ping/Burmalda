using Burmalda.DataAccess;
using Burmalda.Routing;
using Burmalda.Routing.Auctions;
using Burmalda.Routing.Donates;
using Burmalda.Routing.Identity;
using Burmalda.Routing.Users;
using Burmalda.Services.Auctions;
using Burmalda.Services.Identification;
using Burmalda.Services.Payment;
using Burmalda.Services.PaymentsService;
using Burmalda.Services.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = IdentificationService.AccessValidationParameters;
    });

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    OpenApiSecurityScheme securitySchema = new()
    {
        Description = "JWT авторизация",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };
    OpenApiSecurityRequirement securityRequirement = new()
    {
        { securitySchema, [JwtBearerDefaults.AuthenticationScheme] }
    };
    
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);
    options.AddSecurityRequirement(securityRequirement);
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.ConstraintMap.Add(
        ULongRouteConstraint.Name,
        typeof(ULongRouteConstraint)
    );
});

builder.Services.AddCors(options => 
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<BurmaldaDbContext>(options =>
{
    options
        .UseSqlite("Data Source=burmalda.db")
        .UseLazyLoadingProxies();
});


builder.Services
    .AddScoped<IUsersRepository, UsersEfRepository>()
    .AddScoped<IDonatesRepository, DonatesEfRepository>()
    .AddScoped<ILotsRepository, LotsEfRepository>()
    .AddScoped<IAuctionsRepository, AuctionsEfRepository>()
    .AddScoped<IBetsRepository, BetsEfRepository>();

builder.Services
    .AddScoped<IIdentificationService, IdentificationService>()
    .AddScoped<IUsersService, UsersService>()
    .AddScoped<IAuctionService, AuctionService>()
    .AddScoped<IAuctionPaymentService, AuctionPaymentService>()
    .AddScoped<IPaymentService, StupidPaymentService>();


WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi();
    
app.UseCors(policy =>
{
    policy.AllowAnyOrigin();
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
});
app.UseAuthentication();
app.UseAuthorization();

app.UseBurmaldaExceptionHandling();

app.MapIdentityRoutes();
app.MapAuctinsRoutes();
app.MapDonationRoutes();
app.MapUserEndpoints();

app.Run();