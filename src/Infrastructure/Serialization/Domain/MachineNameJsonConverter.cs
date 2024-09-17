namespace MicroDotNet.Services.StateMachines.Infrastructure.Serialization.Domain;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using MicroDotNet.Services.StateMachines.Domain;

public sealed class MachineNameJsonConverter : JsonConverter<MachineName>
{
    public override MachineName? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        string code = default!;
        short version = default;
        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            switch (reader.TokenType) {
                case JsonTokenType.PropertyName:
                    var propertyName = reader.GetString();
                    reader.Read();
                    if (propertyName == "code")
                    {
                        code = reader.GetString()!;
                    }
                    else if (propertyName == "version")
                    {
                        version = reader.GetInt16();
                    }

                    break;
            }
        }

        return MachineName.Create(code, version);
    }

    public override void Write(Utf8JsonWriter writer, MachineName value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("code", value.Code);
        writer.WriteNumber("version", value.Version);
        writer.WriteEndObject();
        writer.Flush();
    }
}
