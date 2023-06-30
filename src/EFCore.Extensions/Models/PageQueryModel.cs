namespace EFCore.Extensions.Models;

public interface IPageQueryRequest
{
    int Page { get; }

    int Count { get; }
}

public class PageQueryRequest : IPageQueryRequest
{
    public int Page { get; set; }

    public int Count { get; set; }
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