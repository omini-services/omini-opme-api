namespace Omini.Opme.Shared.Entities;

public class PagedResult<TValue>
{
    public int PageNumber { get; protected set; }
    public int PageSize { get; protected set; }
    public int TotalCount { get; protected set; }
    public int TotalPages { get; protected set; }

    private readonly List<TValue>? _value;

    public List<TValue>? Response { get { return _value; } }

    public PagedResult(IEnumerable<TValue> source, int pageNumber, int pageSize)
    {
        _value = new();
        Initialize(source, pageNumber, pageSize);
    }

    public PagedResult(IEnumerable<TValue> source, int pageNumber, int pageSize, int totalCount)
    {
        _value = new();
        Initialize(source, pageNumber, pageSize, totalCount);
    }

    // public static implicit operator PagedResult<TValue, TError>((IEnumerable<TValue> value, int pageNumber, int pageSize) data) => new(data.value, data.pageNumber, data.pageSize);
    // public static implicit operator PagedResult<TValue, TError>((IEnumerable<TValue> value, int pageNumber, int pageSize, int totalCount) data) => new(data.value, data.pageNumber, data.pageSize, data.totalCount);
    // public static implicit operator PagedResult<TValue, TError>(TError value) => new(value);

    private void Initialize(IEnumerable<TValue>? source, int pageNumber, int pageSize, int? totalCount = null)
    {
        if (source is null)
        {
            source = new List<TValue>();
        }

        if (pageSize <= 0)
            pageSize = 1;

        TotalCount = totalCount ?? source.Count();

        if (pageSize > 0)
        {
            TotalPages = TotalCount / pageSize;
            if (TotalCount % pageSize > 0)
                TotalPages++;
        }

        PageSize = pageSize;
        PageNumber = pageNumber;
        source = totalCount == null ? source.Skip(pageNumber * pageSize).Take(pageSize) : source;
        _value.AddRange(source);
    }
}