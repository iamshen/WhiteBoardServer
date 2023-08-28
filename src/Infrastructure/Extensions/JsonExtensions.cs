using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace System.Text.Json;

/// <summary>
///     Json辅助扩展操作
/// </summary>
public static class JsonExtensions
{
    /// <summary>序列化</summary>
    /// <param name="obj"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static string Serialize(this object? obj, JsonSerializerOptions? options = null)
    {
        options ??= new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };
        return JsonSerializer.Serialize(obj, options);
    }

    /// <summary>反序列化</summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="json">    </param>
    /// <param name="options"> </param>
    /// <returns> </returns>
    public static T Deserialize<T>(this string json, JsonSerializerOptions? options = null)
    {
        options ??= new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };
        return JsonSerializer.Deserialize<T>(json, options);
    }
}