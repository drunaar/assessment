namespace Assessment.HackerNewsBestStories.FunctionalTests.Fixtures;

public class WebAppFactory : WebApplicationFactory<Program> {
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        base.ConfigureWebHost(builder);

        builder.UseContentRoot(".");
        builder.ConfigureAppConfiguration(config => config
            .AddJsonFile("appsettings.Testing.json", optional: true));
    }
}
