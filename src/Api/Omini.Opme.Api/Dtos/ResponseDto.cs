using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Api.Dtos;

internal record ResponseDto
{
    public static ResponseDto ApiSuccess<T>(T body)
    {
        return new ResponseDto<T>()
        {
            Success = true,
            Data = body!
        };
    }

    public static ResponseDto ApiSuccess<T>(PagedResult<T> body)
    {
        return new ResponsePagedDto<T>()
        {
            Success = true,
            Data = body?.Response!,
            PageNumber = body.PageNumber,
            PageSize = body.PageSize,
            TotalCount = body.TotalCount,
            TotalPages = body.TotalPages,
        };
    }
}

internal record ResponseDto<T> : ResponseDto
{
    public bool Success { get; init; }
    public T Data { get; init; }
}

internal record ResponsePagedDto<T> : ResponseDto
{
    public bool Success { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public IEnumerable<T> Data { get; init; }
}