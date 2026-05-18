using App.Common.Algorithms.Runtime;
using Newtonsoft.Json;

namespace App.Common.Json.External
{
    public class Vector2Converter : JsonConverter<Vector2>
    {
        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("x");
            writer.WriteValue(value.X);

            writer.WritePropertyName("y");
            writer.WriteValue(value.Y);

            writer.WriteEndObject();
        }

        public override Vector2 ReadJson(
            JsonReader reader,
            System.Type objectType,
            Vector2 existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            float x = 0;
            float y = 0;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string property = reader.Value.ToString();

                    reader.Read();

                    switch (property)
                    {
                        case "x":
                            x = System.Convert.ToSingle(reader.Value);
                            break;

                        case "y":
                            y = System.Convert.ToSingle(reader.Value);
                            break;
                    }
                }

                if (reader.TokenType == JsonToken.EndObject)
                    break;
            }

            return new Vector2(x, y);
        }
    }
}