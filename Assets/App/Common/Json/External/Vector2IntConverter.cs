using App.Common.Algorithms.Runtime;
using Newtonsoft.Json;

namespace App.Common.Json.External
{
    public class Vector2IntConverter : JsonConverter<Vector2Int>
    {
        public override void WriteJson(JsonWriter writer, Vector2Int value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("x");
            writer.WriteValue(value.X);

            writer.WritePropertyName("y");
            writer.WriteValue(value.Y);

            writer.WriteEndObject();
        }

        public override Vector2Int ReadJson(
            JsonReader reader,
            System.Type objectType,
            Vector2Int existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            int x = 0;
            int y = 0;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string property = reader.Value.ToString();

                    reader.Read();

                    switch (property)
                    {
                        case "x":
                            x = System.Convert.ToInt32(reader.Value);
                            break;

                        case "y":
                            y = System.Convert.ToInt32(reader.Value);
                            break;
                    }
                }

                if (reader.TokenType == JsonToken.EndObject)
                    break;
            }

            return new Vector2Int(x, y);
        }
    }
}