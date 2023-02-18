using Typeahead.DAL;
using Typeahead.Models;
using Typeahead.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.Configure<EnvironmentSettings>(options =>
{
    options.Port = builder.Configuration.GetValue<string>(Constants.Port);
    options.SuggestionNumber = builder.Configuration.GetValue<int>(Constants.SuggestionNumber);
    options.Host = $"{builder.Configuration.GetValue<string>(Constants.Host)}:{builder.Configuration.GetValue<string>(Constants.Port)}`";
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<INameRepository, NameRepository>();
builder.Services.AddScoped<ITypeaheadService, TypeaheadService>();
builder.Services.AddAutoMapper(typeof(NameProfile));
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
