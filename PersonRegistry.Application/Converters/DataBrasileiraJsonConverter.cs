using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PersonRegistry.Application.Converters
{
    public class DataBrasileiraJsonConverter : JsonConverter<DateTime?>
    {
        private const string Formato = "dd/MM/yyyy";

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            var valor = reader.GetString();

            if (string.IsNullOrWhiteSpace(valor))
            {
                return null;
            }

            if (DateTime.TryParseExact(valor, Formato, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dataBrasileira))
            {
                return dataBrasileira;
            }

            if (DateTime.TryParse(valor, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dataIso))
            {
                return dataIso;
            }

            throw new JsonException($"A data '{valor}' é inválida. Utilize o formato {Formato}.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString(Formato, CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
