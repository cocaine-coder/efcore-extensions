namespace EFCore.Extensions.Models;

public interface IPageQueryRequest
{
    int Page { get; }

    int Count { get; }
}

public class PageQueryRequest : IPageQueryRequest
{
    private int _count;
    private int _page;

    public int Page
    {
        get => _page;

        set
        {
            if (value <= 0)
                value = 1;
            _page = value;
        }
    }

    public int Count
    {
        get => _count;

        set
        {
            if (value is <= 0 or > 20)
                value = 20;

            _count = value;
        }
    }
}

/// <summary>
/// 分页返回模板
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="Page">当前页</param>
/// <param name="Count">页面数据量</param>
/// <param name="Total">总数据量</param>
/// <param name="Data">数据</param>
public record PageQueryResponse<T>(int Page, int Count, int Total, IEnumerable<T> Data);