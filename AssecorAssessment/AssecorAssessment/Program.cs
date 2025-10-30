using AssecorAssessment.Data;
using AssecorAssessment.Repositories;
using AssecorAssessment.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Scoped: One instance per HTTP request
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonValidator, PersonValidator>();

// Get the data source from configuration
var source = builder.Configuration["DataSource"];

if (source == "Database")
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite("Data Source=assecor.db"));
    builder.Services.AddScoped<IPersonRepository, DbPersonRepository>();
}
else
{
    builder.Services.AddSingleton<IPersonRepository, CsvPersonRepository>();
}

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();