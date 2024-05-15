namespace Omini.Opme.Be.Api.Dtos;

public record ResponseDto
{
    public static ResponseDto ApiSuccess<T>(T body)
    {
        return new ResponseDto<T>()
        {
            Success = true,
            Data = body!
        };
    }
}

public record ResponseDto<T> : ResponseDto
{
    public bool Success { get; init; }
    public T Data { get; init; }
}