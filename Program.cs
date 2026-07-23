using System.Text;
using System.Text.Json;
using CsvHelper;
using System.Globalization;
using System.Text.Json.Serialization;
using Zadanie___KAMSOFT.Models;

namespace Zadanie___KAMSOFT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(
                    new JsonStringEnumConverter()
                );
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapPost("/api/v1/parse-content", RequestValidationConfirm);

            static IResult RequestValidationConfirm(ContentFormat request)
            {

                if(Enum.IsDefined(request.FormatType) == false)
                {
                    return Results.BadRequest("Wrong format of content");
                }

                if (string.IsNullOrWhiteSpace(request.Content))
                {
                    return Results.BadRequest("Content cannot be empty");
                }

                string decodedContent;

                try
                {
                    decodedContent = Encoding.UTF8.GetString(Convert.FromBase64String(request.Content));
                }

                catch (FormatException)
                {
                    return Results.BadRequest("Content is not valid Base64");
                }

                List<Dictionary<string, object>> data = new();

                switch (request.FormatType)
                {
                    case FormatType.CSV:

                        using (StringReader reader = new(decodedContent))
                        using (CsvReader csv = new(reader, CultureInfo.InvariantCulture))
                        {
                            foreach (var row in csv.GetRecords<dynamic>())
                            {
                                var dictionary = (IDictionary<string, object>)row;

                                Dictionary<string, object> newRow = new();

                                foreach (var item in dictionary)
                                {
                                    newRow.Add(item.Key, item.Value);
                                }

                                data.Add(newRow);
                            }
                        }

                        break;

                    case FormatType.INTERNAL_JSON:

                        try
                        {
                            data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(decodedContent);
                        }
                        catch (JsonException)
                        {
                            return Results.BadRequest("Content is not valid JSON");
                        }

                        break;

                    default:
                        return Results.BadRequest("Wrong format of content");
                }

                OutputDataDetails response = new()
                {
                    Status = "success",
                    Count = data.Count,
                    Data = data
                };

                return Results.Ok(response);

            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
