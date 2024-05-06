using HackerNewsApi;
using HackerNewsStories.Api;
using HackerNewsStories.StoriesLoading;
using Scrutor;
using Serilog;

Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(new ConfigurationBuilder().AddJsonFile("serilog.json").Build())
        .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddSingleton<IStoriesService, StoriesService>();
builder.Services.AddHostedService<StoriesLoaderBackgroundService>();
builder.Services.AddHackerNewsApi();

builder.Services.Scan(scan => scan.FromCallingAssembly()
                              .AddClasses(classes => classes.Where(x => !typeof(Exception).IsAssignableFrom(x)))
                              .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                              .AsImplementedInterfaces()
                              .WithTransientLifetime());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddResponseCaching();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseResponseCaching();

app.UseAuthorization();

app.UseMiddleware<StoriesNotYetAvailableMiddleware>();

app.MapControllers();

app.Run();
