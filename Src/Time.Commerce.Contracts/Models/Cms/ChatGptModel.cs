using System.Buffers;
using System.Buffers.Text;
using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Time.Commerce.Contracts.Models.Cms;
public class Choice
{
    public string text { get; set; }
    public int index { get; set; }
    public object logprobs { get; set; }
    public string finish_reason { get; set; }
}
public class Root
{
    public string id { get; set; }
    public string @object { get; set; }
    public int created { get; set; }
    public string model { get; set; }
    public List<Choice> choices { get; set; }
    public Usage usage { get; set; }
}
public class Usage
{
    public int prompt_tokens { get; set; }
    public int completion_tokens { get; set; }
    public int total_tokens { get; set; }
}
public class OpenAIChoice
{
    public string text { get; set; }
    public float probability { get; set; }
    public float[] logprobs { get; set; }
    public int[] finish_reason { get; set; }
}
public class OpenAIRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }

    [JsonPropertyName("temperature")]
    public float Temperature { get; set; }

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }
    public bool stream { get; set; }

}
public class OpenAIErrorResponse
{
    [JsonPropertyName("error")]
    public OpenAIError Error { get; set; }
}
public class OpenAIError
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("param")]
    public string Param { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }
}


//FOR CHAT GPT
/// <summary>
/// Represents a single chat message.
/// </summary>
public class ChatGptMessage
{
    /// <summary>
    /// Gets or sets the role (source) of the message. Valid values are: <em>system</em>, <em>user</em> and <em>assistant</em>.
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// The content of the message.
    /// </summary>
    public string Content { get; set; } = string.Empty;
}

/// <summary>
/// Contains constants for all the possible roles.
/// </summary>
public class ChatGptRoles
{
    /// <summary>
    /// The system role.
    /// </summary>
    public const string System = "system";

    /// <summary>
    /// The user role.
    /// </summary>
    public const string User = "user";

    /// <summary>
    /// The assistant role.
    /// </summary>
    public const string Assistant = "assistant";
}

/// <summary>
/// Represents a request for a chat completions.
/// </summary>
public class ChatGptRequest
{
    /// <summary>
    /// Gets or sets the ID of the model to use. Currently, only <em>gpt-3.5-turbo</em> and <em>gpt-3.5-turbo-0301</em> are supported.
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the messages to generate chat completions for.
    /// </summary>
    /// <seealso cref="ChatGptMessage"/>
    public ChatGptMessage[] Messages { get; set; } = Array.Empty<ChatGptMessage>();
    public bool Stream { get; set; }
}


/// <summary>
/// Contains all the currently supported chat completion models.
/// </summary>
public static class ChatGptModels
{
    /// <summary>
    /// The model used by the official ChatGPT.
    /// </summary>
    public const string Gpt35Turbo = "gpt-3.5-turbo";
}


public class ChatGptException : HttpRequestException
{
    /// <summary>
    /// Gets the detailed error information.
    /// </summary>
    /// <seealso cref="ChatGptError"/>
    public ChatGptError Error { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatGptException"/> class with the specified <paramref name="error"/> details.
    /// </summary>
    /// <param name="error">The detailed error information</param>
    /// <param name="statusCode">The HTTP status code</param>
    /// <seealso cref="ChatGptError"/>
    /// <seealso cref="HttpRequestException"/>
    public ChatGptException(ChatGptError error, HttpStatusCode statusCode) : base(error.Message, null, statusCode)
    {
        Error = error;
    }
}



/// <summary>
/// Contains information about the error occurred while invoking the service.
/// </summary>
/// <remarks>
/// Refer to https://platform.openai.com/docs/guides/error-codes/api-errors for more information.
/// </remarks>
public class ChatGptError
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    /// <remarks>
    /// Refer to https://platform.openai.com/docs/guides/error-codes/api-errors for more information.
    /// </remarks>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    /// <remarks>
    /// Refer to https://platform.openai.com/docs/guides/error-codes/api-errors for more information.
    /// </remarks>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the parameter that caused the error.
    /// </summary>
    /// <remarks>
    /// Refer to https://platform.openai.com/docs/guides/error-codes/api-errors for more information.
    /// </remarks>
    [JsonPropertyName("param")]
    public string? Parameter { get; set; }

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    /// <remarks>
    /// Refer to https://platform.openai.com/docs/guides/error-codes/api-errors for more information.
    /// </remarks>
    public string? Code { get; set; }
}




/// <summary>
/// Represents a chat completion response.
/// </summary>
public class ChatGptResponse
{
    /// <summary>
    /// Gets or sets the Id of the response
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source object for this response.
    /// </summary>
    public string Object { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Conversation Id, that is used to group messages of the same conversation.
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time at which the response has been generated.
    /// </summary>
    [JsonPropertyName("created")]
    [JsonConverter(typeof(UnixToDateTimeConverter))]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets information about token usage.
    /// </summary>
    public ChatGptUsage Usage { get; set; } = new();

    /// <summary>
    /// Gets or sets the error occurred during the chat completion execution, if any.
    /// </summary>
    public ChatGptError? Error { get; set; }

    /// <summary>
    /// Gets or sets the list of choices that has been provided by chat completion.
    /// </summary>
    public ChatGptChoice[] Choices { get; set; } = Array.Empty<ChatGptChoice>();

    /// <summary>
    /// Gets a value that determines if the response was successful.
    /// </summary>
    public bool IsSuccessful => Error is null;

    /// <summary>
    /// Gets the content of the first choice, if available.
    /// </summary>
    /// <returns>The content of the first choice, if available</returns>
    public string? GetMessage() => Choices.FirstOrDefault()?.Message.Content.Trim();
}

public class ChatGptStreamResponse
{
    public ChatGptResponse Data { get; set; }
}

internal class UnixToDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                // try to parse number directly from bytes
                var span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out long number, out var bytesConsumed) && span.Length == bytesConsumed)
                {
                    var date = UnixTimeStampToDateTime(number);
                    return date;
                }
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();
                if (DateTime.TryParseExact(value, "O", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var date))
                {
                    return date;
                }
            }
        }
        catch
        {
        }

        return DateTime.MinValue;

        static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTime;
        }
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // The "O" standard format will always be 28 bytes.
        Span<byte> utf8Date = new byte[28];

        Utf8Formatter.TryFormat(value, utf8Date, out _, new StandardFormat('O'));
        writer.WriteStringValue(utf8Date);
    }
}


/// <summary>
/// Contains information about the API usage.
/// </summary>
/// <remarks>
/// Refer to https://help.openai.com/en/articles/4936856-what-are-tokens-and-how-to-count-them for more information.
/// </remarks>
public class ChatGptUsage
{
    /// <summary>
    /// Gets or sets the number of tokens of the request.
    /// </summary>
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }

    /// <summary>
    /// Gets or sets the number of token of the response.
    /// </summary>
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }

    /// <summary>
    /// Gets the total number of tokens.
    /// </summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}


/// <summary>
/// Represent a chat completion choice.
/// </summary>
public class ChatGptChoice
{
    /// <summary>
    /// Gets or sets the index of the choice in the list.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Gets or sets the message associated with this <see cref="ChatGptChoice"/>.
    /// </summary>
    /// <seealso cref="ChatGptChoice"/>
    public ChatGptMessage Message { get; set; } = new();

    /// <summary>
    /// Gets or sets a value specifying why the choice has been returned. Possible values are: <em>stop</em> (API returned complete model output), <em>length</em> (incomplete model output due to max_tokens parameter or token limit), <em>content_filter</em> (omitted content due to a flag from content filters) or <em>null</em> (API response still in progress or incomplete).
    /// </summary>
    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; } = string.Empty;
}


/// <summary>
/// Represents a chat completion response.
/// </summary>
public class ChatGptStremingResponse
{
    /// <summary>
    /// Gets or sets the Id of the response
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source object for this response.
    /// </summary>
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;


    /// <summary>
    /// Gets or sets the UTC date and time at which the response has been generated.
    /// </summary>
    [JsonPropertyName("created")]
    [JsonConverter(typeof(UnixToDateTimeConverter))]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    /// <summary>
    /// Gets or sets the error occurred during the chat completion execution, if any.
    /// </summary>
    public ChatGptError? Error { get; set; }

    /// <summary>
    /// Gets or sets the list of choices that has been provided by chat completion.
    /// </summary>
    [JsonPropertyName("choices")]
    public ChatGptChoiceStreaming[] Choices { get; set; } = Array.Empty<ChatGptChoiceStreaming>();

    /// <summary>
    /// Gets a value that determines if the response was successful.
    /// </summary>
    public bool IsSuccessful => Error is null;

    /// <summary>
    /// Gets the content of the first choice, if available.
    /// </summary>
    /// <returns>The content of the first choice, if available</returns>
    public string? GetMessage() => Choices.FirstOrDefault()?.Delta.Content.Trim();
}

/// <summary>
/// Represent a chat completion choice.
/// </summary>
public class ChatGptChoiceStreaming
{
    /// <summary>
    /// Gets or sets the index of the choice in the list.
    /// </summary>
    [JsonPropertyName("index")]
    public int Index { get; set; }

    /// <summary>
    /// Gets or sets the message associated with this <see cref="ChatGptChoice"/>.
    /// </summary>
    /// <seealso cref="ChatGptChoice"/>
    [JsonPropertyName("delta")]
    public ChatGptMessageStream Delta { get; set; } = new();

    /// <summary>
    /// Gets or sets a value specifying why the choice has been returned. Possible values are: <em>stop</em> (API returned complete model output), <em>length</em> (incomplete model output due to max_tokens parameter or token limit), <em>content_filter</em> (omitted content due to a flag from content filters) or <em>null</em> (API response still in progress or incomplete).
    /// </summary>
    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; } = string.Empty;
}

/// <summary>
/// Represents a single chat message.
/// </summary>
public class ChatGptMessageStream
{
    /// <summary>
    /// Gets or sets the role (source) of the message. Valid values are: <em>system</em>, <em>user</em> and <em>assistant</em>.
    /// </summary>
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// The content of the message.
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}