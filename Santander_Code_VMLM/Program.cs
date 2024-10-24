using Santander_Code_VMLM.Controllers;
using Santander_Code_VMLM.Interfaces;
using Santander_Code_VMLM.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<IHackerNewsApi, HackerNewsApi>();
builder.Services.AddSingleton<IHackerNewsApi, HackerNewsApi>();
builder.Services.AddSingleton<IHackerNew, HackerNew>();

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




app.Run();
