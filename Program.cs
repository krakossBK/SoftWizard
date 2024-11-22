using Microsoft.Extensions.Configuration;
using SoftWizard.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


services.AddMemoryCache();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddTransient<IOkpdCategoryRepository, OkpdCategoryService>();
services.AddTransient<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "";
    });
}

app.UseRouting();
app.MapControllers();

app.Run();
