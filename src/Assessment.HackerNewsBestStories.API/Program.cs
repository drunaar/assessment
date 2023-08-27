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
services.AddRefitClient<IHackerNewsAPI>()
    .ConfigureHttpClient(client => {
        client.BaseAddress = new Uri("https://hacker-news.firebaseio.com");
    })
    .AddPolicyHandler(Policy.BulkheadAsync<HttpResponseMessage>(5, 300));

TypeAdapterConfig.GlobalSettings.Apply(new MappingsProfile());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();

public partial class Program { }
