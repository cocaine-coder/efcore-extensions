using EFCore.Extensions.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.Test.Tests;

public class IQueryableExtensionTest : IClassFixture<TestDbContextFixture>
{
    private readonly TestDbContext _dbContext;

    public IQueryableExtensionTest(TestDbContextFixture testDbContextFixture)
    {
        _dbContext = testDbContextFixture.DbContext;
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(null)]
    public void OrderByDefaultDesc(bool? desc)
    {
        var students = _dbContext.Students.OrderByDefaultDesc(x => x.Age, desc).ToList();
        var maxAge = students.Max(x => x.Age);
        var minAge = students.Min(x => x.Age);

        if (desc is false)
            Assert.Equal(minAge, students.First().Age);
        else
            Assert.Equal(maxAge, students.First().Age);
    }

    [Theory]
    [InlineData("11")]
    [InlineData(null)]
    public void WhereIfQuery(string? name)
    {
        var students = _dbContext.Students.WhereIf(name != null, student => student.Name == name).ToList();
        var count = _dbContext.Students.Count();

        if (name is null)
            Assert.Equal(count, students.Count);
        else
            Assert.Single(students);
    }

    [Theory]
    [InlineData(11)]
    [InlineData(null)]
    public void WhereIfNotNullQuery(int? age)
    {
        var students = _dbContext.Students.WhereIfNotNull(age, age => student => student.Age == age).ToList();

        Assert.NotEmpty(students);
    }

    [Theory]
    [InlineData(-1, 10)]
    [InlineData(1, 10)]
    [InlineData(100, 100)]
    public async Task PageQuery(int page, int count)
    {
        var studentPageResponse = await _dbContext.Students.QueryPageAsync(new PageQueryRequest()
        {
            Page = page,
            Count = count
        });
        var expectedCount = await _dbContext.Students.CountAsync();

        Assert.Equal(expectedCount, studentPageResponse.Total);

        if (page == 100)
            Assert.Empty(studentPageResponse.Data);
        else
            Assert.NotEmpty(studentPageResponse.Data);
    }
}