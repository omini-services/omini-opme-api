namespace Omini.Opme.Shared.Entities;

public class PagedResult<TValue>
{
    public int CurrentPage { get; protected set; }
    public int PageSize { get; protected set; }
    public int RowCount { get; protected set; }
    public int PageCount { get; protected set; }

    private readonly List<TValue>? _value;

    public List<TValue>? Response { get { return _value; } }

    public PagedResult(IEnumerable<TValue> source, int currentPage, int pageSize)
    {
        _value = new();
        Initialize(source, currentPage, pageSize);
    }

    public PagedResult(IEnumerable<TValue> source, int currentPage, int pageSize, int rowCount)
    {
        _value = new();
        Initialize(source, currentPage, pageSize, rowCount);
    }

    private void Initialize(IEnumerable<TValue>? source, int currentPage, int pageSize, int? rowCount = null)
    {
        if (source is null)
        {
            source = new List<TValue>();
        }

        if (pageSize <= 0)
            pageSize = 1;

        RowCount = rowCount ?? source.Count();

        if (pageSize > 0)
        {
            PageCount = RowCount / pageSize;
            if (RowCount % pageSize > 0)
                PageCount++;
        }

        PageSize = pageSize;
        CurrentPage = currentPage;
        source = rowCount == null ? source.Skip(currentPage * pageSize).Take(pageSize) : source;
        _value.AddRange(source);
    }
}