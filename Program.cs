using Microsoft.AspNetCore.ResponseCompression;
using SoftWizard.Services;
using System.IO.Compression;


// .....................
//using Microsoft.AspNetCore.OutputCaching;   // for attribute [OutputCache]

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.ExcludedMimeTypes = ["text/plain"];

    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add(new GzipCompressionProvider(new GzipCompressionProviderOptions()));
    options.Providers.Add(new SoftWizard.AppCode.DeflateCompressionProvider());
});

// set level Compress
services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

services.AddOutputCache();  
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

app.UseOutputCache(); // add OutputCacheMiddleware

app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
    }
});


// add Compress
app.UseResponseCompression();

app.UseRouting();
app.MapControllers();
app.Run();
