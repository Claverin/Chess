using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Chess.Models;

namespace Chess.Utility
{
    public class PieceConverter : JsonConverter<Piece>
    {
        public override Piece Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonDocument doc = JsonDocument.ParseValue(ref reader);
            if (doc.RootElement.TryGetProperty("Type", out JsonElement typeElement))
            {
                var type = Type.GetType(typeElement.GetString());
                return (Piece)JsonSerializer.Deserialize(doc.RootElement.GetRawText(), type, options);
            }
            throw new JsonException("Nie można zdeserializować obiektu Piece.");
        }

        public override void Write(Utf8JsonWriter writer, Piece value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Type", value.GetType().FullName);
            foreach (var prop in value.GetType().GetProperties())
            {
                var propValue = prop.GetValue(value);
                writer.WritePropertyName(prop.Name);
                JsonSerializer.Serialize(writer, propValue, options);
            }
            writer.WriteEndObject();
        }
    }
}