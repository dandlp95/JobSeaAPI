using JobSeaAPI;
using JobSeaAPI.Database;
using JobSeaAPI.Repository;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

string secretKey = "";
string ApiUrl = "";
string clientUrl = "";

if (builder.Environment.IsDevelopment())
{
    secretKey = builder.Configuration.GetValue<string>("AppSettings:SecretKey") ?? throw new Exception("Unable to access key to connect to the database.");
    ApiUrl = builder.Configuration.GetValue<string>("AppSettings:ApiUrl") ?? throw new Exception("Unable to feth Api Url value"); 
    clientUrl = builder.Configuration.GetValue<string>("AppSettings:clientUrl") ?? throw new Exception("Unable to access client url variable");
}
else
{
    secretKey = builder.Configuration.GetValue<string>("SECRET_KEY") ?? throw new Exception("Unable to access key to connect to the database.");
    ApiUrl = builder.Configuration.GetValue<string>("API_URL") ?? throw new Exception("Unable to feth Api Url value"); 
    clientUrl = builder.Configuration.GetValue<string>("CLIENT_URL") ?? throw new Exception("Unable to access client url variable");
}

var TokenValidationParameters = new TokenValidationParameters
{
    ValidIssuer = ApiUrl,
    ValidAudience = ApiUrl,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});

if (builder.Environment.IsDevelopment()) 
{
    builder.Services.AddDbContext<ApplicationDbContext>(option =>
    {
        option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
    });


}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")));
}



builder.Services.AddSingleton<ILoggerCustom, LoggerCustom>();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IModalityRepository, ModalityRepository>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddScoped<IJobApplicationsRepository, JobApplicationsRepository>();
builder.Services.AddScoped<IUpdateRepository, UpdateRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddSingleton<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ISqlBuilder, SqlBuilder
    >();



//// Add services to the container.
builder.Services.AddAuthentication(
    options =>
    {
        // Technically not necessary to add default schema as there is only 1
        // authentication service registered.
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }
    )
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = TokenValidationParameters;
    });

builder.Services.AddAuthorization(cfg =>
{
    cfg.AddPolicy("User", policy => policy.RequireClaim("type", "User"));
    cfg.AddPolicy("Admin", policy => policy.RequireClaim("type", "Admin"));
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JobSea", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    { new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
            },
        Array.Empty<string>()
    }
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors(MyAllowSpecificOrigins);
// This is an alternative to do it without having to add a cors policy separately.
//app.UseCors(x => x
//            .SetIsOriginAllowed(origin => true)
//            .AllowAnyMethod()
//            .AllowAnyHeader()
//            .AllowCredentials());

app.UseAuthorization();

// Middleware to handle 404 Not Found responses
app.Use(async (context, next) =>
{
    await next(); // Process the request and response

    // Check if the response status code is 404
    if (context.Response.StatusCode == StatusCodes.Status404NotFound)
    {
        // Set the content type to JSON
        context.Response.ContentType = "application/json";
        // Write a custom 404 response
        await context.Response.WriteAsync("{\"message\": \"Resource not found.\"}");
    }
});

app.MapControllers();

app.Run();
