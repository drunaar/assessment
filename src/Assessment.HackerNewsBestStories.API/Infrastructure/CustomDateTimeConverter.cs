namespace Assessment.HackerNewsBestStories.API.Infrastructure;

public class CustomDateTimeConverter : JsonConverter<DateTime> {
    private readonly string _format;

    public CustomDateTimeConverter(string format)
        => _format = format;

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTime.TryParseExact(reader.GetString(), _format, null, DateTimeStyles.None, out var result)
        ? result
        : throw new ArgumentException();

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(_format));
}
