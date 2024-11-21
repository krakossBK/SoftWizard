using SoftWizard.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


string connStr = "Data Source=ms-sql-10.in-solve.ru;Initial Catalog=1gb_vladimirpiter;Integrated Security=False;User ID=1gb_olga-arsi;Password=4uC8s47Ke6i5;TrustServerCertificate=True";
services.AddTransient<IUserRepository, UserRepository>(provider => new UserRepository(connStr));

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
