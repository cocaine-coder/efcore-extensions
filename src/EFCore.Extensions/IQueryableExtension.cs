namespace EFCore.Extensions;

public static class IQueryableExtension
{
    #region QueryPage

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entities"></param>
    /// <param name="queryParams"></param>
    /// <returns></returns>
    public static async Task<PageQueryResponse<TEntity>> QueryPageAsync<TEntity>(
        this IQueryable<TEntity> entities,
        IPageQueryRequest queryParams)
    where TEntity : class
    {
        var (total, query) = await entities.QueryPage(queryParams);

        return new PageQueryResponse<TEntity>(queryParams.Page, queryParams.Count, total, await query.ToListAsync());
    }

    /// <summary>
    /// 分页查询 并在数据库查询期间映射
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="entities"></param>
    /// <param name="queryParams"></param>
    /// <param name="dbSelector"></param>
    /// <returns></returns>
    public static async Task<PageQueryResponse<TDto>> QueryPageAsync<TEntity, TDto>(
        this IQueryable<TEntity> entities,
        IPageQueryRequest queryParams,
        Expression<Func<TEntity, TDto>> dbSelector)
    where TEntity : class
    where TDto : class
    {
        var (total, query) = await entities.QueryPage(queryParams);
        var ret = await query.Select(dbSelector).ToListAsync();

        return new PageQueryResponse<TDto>(queryParams.Page, queryParams.Count, total, ret);
    }

    /// <summary>
    /// 分页查询 并在数据库查询之后映射
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="entities"></param>
    /// <param name="queryParams"></param>
    /// <param name="afterDbSelector"></param>
    /// <returns></returns>
    public static async Task<PageQueryResponse<TDto>> QueryPageAsync<TEntity, TDto>(
        this IQueryable<TEntity> entities,
        IPageQueryRequest queryParams,
        Func<TEntity, TDto> afterDbSelector)
    where TEntity : class
    where TDto : class
    {
        var (total, query) = await entities.QueryPage(queryParams);
        var ret = (await query.ToListAsync()).Select(afterDbSelector);

        return new PageQueryResponse<TDto>(queryParams.Page, queryParams.Count, total, ret);
    }

    private static async Task<(int total, IQueryable<TEntity> query)> QueryPage<TEntity>(
        this IQueryable<TEntity> entities,
        IPageQueryRequest queryParams)
    where TEntity : class
    {
        return (await entities.CountAsync(),
            entities.Skip((queryParams.Page - 1) * queryParams.Count).Take(queryParams.Count));
    }

    #endregion QueryPage

    /// <summary>
    /// 满足TProperty不为null时使用TProperty构造where的predicate执行where
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="entities"></param>
    /// <param name="property"></param>
    /// <param name="predicateCreator"></param>
    /// <returns></returns>
    public static IQueryable<TEntity> WhereIfNotNull<TEntity, TProperty>(
        this IQueryable<TEntity> entities,
        TProperty? property,
        Func<TProperty, Expression<Func<TEntity, bool>>> predicateCreator)
    where TEntity : class
    {
        return property == null ?
            entities :
            entities.Where(predicateCreator.Invoke(property));
    }

    /// <summary>
    /// 满足一定条件执行where
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entities"></param>
    /// <param name="flag"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IQueryable<TEntity> WhereIf<TEntity>(
        this IQueryable<TEntity> entities,
        bool flag,
        Expression<Func<TEntity, bool>> predicate)
    {
        return flag ? entities.Where(predicate) : entities;
    }

    /// <summary>
    /// 排序
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="entities"></param>
    /// <param name="keySelector"></param>
    /// <param name="desc">默认从大到小</param>
    /// <returns></returns>
    public static IQueryable<TEntity> OrderByDefaultDesc<TEntity, TKey>(
        this IQueryable<TEntity> entities,
        Expression<Func<TEntity, TKey>> keySelector,
        bool? desc = null)
    {
        return desc is false ? entities.OrderBy(keySelector) : entities.OrderByDescending(keySelector);
    }
}