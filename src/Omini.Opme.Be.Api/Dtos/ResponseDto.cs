using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Omini.Opme.Be.Api.Dtos;

public record ResponseDto
{
    public bool Success { get; init; }
    public object Data { get; init; }
    public List<string> Errors { get; set; }

    public static ResponseDto ApiSuccess<T>(T? body)
    {
        return new ResponseDto()
        {
            Success = true,
            Data = body
        };
    }

    public static ResponseDto ApiError(params string[] errors)
    {
        return new ResponseDto()
        {
            Success = false,
            Errors = errors.ToList()
        };
    }
}