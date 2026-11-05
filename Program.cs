using DrustvenaMreza.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<KorisnikRepository>();
builder.Services.AddScoped<ClanstvoRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "Drustvena mreza API"
        );
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("Frontend");
app.UseAuthorization();
app.MapControllers();
app.Run();
