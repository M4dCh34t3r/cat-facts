using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Shared.Utils;

public class JsonUtil
{
    private static readonly JsonSerializerOptions _serializerOptions =
        new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Default,
            PropertyNameCaseInsensitive = true,
        };

    /// <summary>
    /// Determines whether the specified JSON string represents a JSON array.
    /// </summary>
    /// <param name="json">The JSON string to evaluate. Can be null or empty.</param>
    /// <returns>
    /// <c>true</c> if the JSON string represents a valid JSON array; otherwise, <c>false</c>.
    /// Returns <c>false</c> for null, empty, or invalid JSON strings.
    /// </returns>
    public static bool IsArray(string? json)
    {
        if (IsInvalidOrEmpty(json))
            return false;

        try
        {
            // GUARANTEE: the 'json' validation occurs on 'IsInvalidOrEmpty' call
            return JsonNode.Parse(json!) is JsonArray;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Determines whether the specified JSON string represents a JSON object.
    /// </summary>
    /// <param name="json">The JSON string to evaluate. Can be null or empty.</param>
    /// <returns>
    /// <c>true</c> if the JSON string represents a valid JSON object; otherwise, <c>false</c>.
    /// Returns <c>false</c> for null, empty, or invalid JSON strings.
    /// </returns>
    public static bool IsObject(string? json)
    {
        if (IsInvalidOrEmpty(json))
            return false;

        try
        {
            // GUARANTEE: the 'json' validation occurs on 'IsInvalidOrEmpty' call
            return JsonNode.Parse(json!) is JsonObject;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Deserializes a JSON string into an object of the specified type.
    /// </summary>
    /// <typeparam name="TValue">The type of the object to deserialize to.</typeparam>
    /// <param name="json">The JSON string to deserialize. Must be a valid JSON string.</param>
    /// <returns>
    /// An object of type <typeparamref name="TValue"/> if the deserialization is successful;
    /// otherwise, <c>null</c> if the JSON is invalid or the deserialization fails.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="json"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown if the JSON is malformed or does not match the structure of <typeparamref name="TValue"/>.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// Thrown if the type <typeparamref name="TValue"/> is not supported for deserialization.
    /// </exception>
    public static TValue? Deserialize<TValue>([StringSyntax("Json")] string json) =>
        JsonSerializer.Deserialize<TValue>(json, _serializerOptions);

    /// <summary>
    /// Serializes an object of the specified type into a JSON string.
    /// </summary>
    /// <typeparam name="TValue">The type of the object to serialize.</typeparam>
    /// <param name="value">The object to serialize. Must not be <c>null</c>.</param>
    /// <returns>A JSON string representation of the <paramref name="value"/>.</returns>
    /// <exception cref="NotSupportedException">
    /// Thrown if the type <typeparamref name="TValue"/> is not supported for serialization.
    /// </exception>
    /// <remarks>
    /// This method uses predefined serialization options (<c>_serializerOptions</c>) to customize the JSON output.
    /// </remarks>
    public static string Serialize<TValue>(TValue value) =>
        JsonSerializer.Serialize(value, _serializerOptions);

    /// <summary>
    /// Determines whether a JSON string is invalid or does not represent a JSON object or array.
    /// </summary>
    /// <param name="json">The JSON string to validate. Can be <c>null</c> or empty.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="json"/> is <c>null</c>, empty, whitespace,
    /// or not a valid JSON object or array; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the input JSON string contains prohibited characters or fails a custom validation.
    /// </exception>
    /// <remarks>
    /// The method first checks if the input is <c>null</c>, empty, or whitespace. If not,
    /// it attempts to parse the JSON string. The result is considered valid only if the root element
    /// is of type <see cref="JsonValueKind.Object"/> or <see cref="JsonValueKind.Array"/>.
    /// </remarks>
    public static bool IsInvalid(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return true;

        try
        {
            var jsonDoc = JsonDocument.Parse(json);
            return jsonDoc.RootElement.ValueKind != JsonValueKind.Object
                && jsonDoc.RootElement.ValueKind != JsonValueKind.Array;
        }
        catch (JsonException)
        {
            return true;
        }
    }

    /// <summary>
    /// Determines whether the provided JSON string is invalid or represents an empty JSON structure.
    /// </summary>
    /// <param name="json">The JSON string to evaluate. Can be <c>null</c> or empty.</param>
    /// <returns>
    /// <c>true</c> if the JSON string is <c>null</c>, empty, invalid, or represents an empty JSON object (<c>{}</c>)
    /// or array (<c>[]</c>); otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the input JSON string contains prohibited characters or fails a custom validation.
    /// </exception>
    /// The method first checks if the input is <c>null</c>, empty, or whitespace. If not,
    /// it attempts to parse the JSON string. The result is considered valid only if the root element
    /// is of type <see cref="JsonValueKind.Object"/> or <see cref="JsonValueKind.Array"/>.
    /// </remarks>
    public static bool IsInvalidOrEmpty(string? json) => IsInvalid(json) || IsEmpty(json);

    /// <summary>
    /// Determines whether the provided JSON string is empty or represents an empty JSON object or array.
    /// </summary>
    /// <param name="json">The JSON string to evaluate. Can be <c>null</c> or empty.</param>
    /// <returns>
    /// <c>true</c> if the JSON string is <c>null</c>, empty, or represents an empty JSON object (<c>{}</c>)
    /// or array (<c>[]</c>); otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the input JSON string contains prohibited characters or fails a custom validation.
    /// </exception>
    public static bool IsEmpty(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return true;

        try
        {
            using var jsonDoc = JsonDocument.Parse(json);
            var root = jsonDoc.RootElement;

            return root.ValueKind == JsonValueKind.Object && !root.EnumerateObject().Any()
                || root.ValueKind == JsonValueKind.Array && !root.EnumerateArray().Any();
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
