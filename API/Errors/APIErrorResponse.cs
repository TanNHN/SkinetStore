using System;

namespace API.Errors;

public class APIErrorResponse(int statusCode, string exceptionMessage, string? detail)
{
public int StatusCode { get; set; } = statusCode;
public string ExceptionMessage { get; set; } = exceptionMessage;
public string? Detail { get; set; } = detail;
}
