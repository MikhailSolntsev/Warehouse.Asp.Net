using System.Text.Json;

namespace Warehouse.Web.Dto;

public class ResponseMessage
{
    public ResponseMessage(string message)
    {
        Message = message;
    }

    public ResponseMessage(IDictionary<string, string[]> dictionary)
    {
        var options = new JsonSerializerOptions() {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        Message = JsonSerializer.Serialize(dictionary, options);
    }

    public string Message { get; set; }
}
