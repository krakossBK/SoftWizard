using Microsoft.AspNetCore.ResponseCompression;
using SoftWizard.Services;
using System.IO.Compression;


// .....................
//using Microsoft.AspNetCore.OutputCaching;   // для атрибута [OutputCache]

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;

    // исключаем из сжатия простой текст
    options.ExcludedMimeTypes = ["text/plain"];

    options.Providers.Add<BrotliCompressionProvider>();
    // добавляем провайдер gzip-сжатия
    options.Providers.Add(new GzipCompressionProvider(new GzipCompressionProviderOptions()));
    // добавляем провайдер сжатия DeflateCompressionProvider
    options.Providers.Add(new SoftWizard.AppCode.DeflateCompressionProvider());
});

// устанавливаем уровень сжатия
services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

services.AddOutputCache();  // добавляем сервисы
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

app.UseOutputCache(); // добавляем OutputCacheMiddleware

app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
    }
});


// подключаем сжатие
app.UseResponseCompression();

app.UseRouting();
app.MapControllers();

//app.MapGet("/", () => okpdCategory).CacheOutput();  // применяем кэширование

//app.MapGet("/", async () =>
//{
//    await Task.Delay(5000);     // имитация долгой обработки
//    return OkpdCategory;
//}).CacheOutput();   // применяем кэширование к результату обработки метода app.MapGet("/")

// .....................
// app.MapGet("/", [OutputCache] () => OkpdCategory);

app.Run();
