using Assessment.HackerNewsBestStories.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions
            .Converters.Add(new CustomDateTimeConverter("yyyy-MM-ddTHH:mm:ssK"));
    });
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options => {
    options.SupportNonNullableReferenceTypes();
});
services.AddMediatR(config => {
    config.RegisterServicesFromAssemblyContaining<Program>();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();

public partial class Program { }
