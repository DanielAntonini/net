using System.Net;
using Newtonsoft.Json;

namespace Example.Core.Exceptions;

public class BusinessException : Exception
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
    public string CustomCode { get; set; }
    
    public BusinessException()
    {
    }

    public BusinessException(string message)
        : base(message)
    {
    }

    public BusinessException(string message, Exception inner)
        : base(message, inner)
    {
    }
    
    public BusinessException(string code ,string message)
        : base(message)
    {
        CustomCode = code;
    }

    public BusinessException(string code, string message, Exception inner)
        : base(message, inner)
    {
        CustomCode = code;
    }
}