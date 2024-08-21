using MagicSystem;
using MagicSystem.BFF;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InjectMagicSystem(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", static builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.MapGroup("loom").BuildApiEndpoints();

app.Run();
