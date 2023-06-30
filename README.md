# efcore-extensions
[![Nuget](https://img.shields.io/nuget/v/CC.EFCore.Extensions)](https://www.nuget.org/packages/CC.EFCore.Extensions)
![Nuget](https://img.shields.io/nuget/dt/CC.EFCore.Extensions)

some business logic extension with efcore

## Usage

```
dotnet add package CC.EFCore.Extensions
```

### PageQuery
 
```cs
var pageStudents = await appDbContext.Set<Student>().QueryPageAsync(new PageQueryRequest()
{
	Page = 1,
	Count = 10
});

var students = pageStudents.Data;
var total = pageStudents.Total;
var page = pageStudents.Page;
var count = pageStudents.Count;


// map to dto in database execute sql
await appDbContext.Set<Student>().QueryPageAsync(new PageQueryRequest()
{
	Page = 1,
	Count = 10
},dbSelector:s => new {
	s.Id, s.FirstName, s.LastName, s.Age
});

// map to dto after databse execute sql
await appDbContext.Set<Student>().QueryPageAsync(new PageQueryRequest()
{
	Page = 1,
	Count = 10
},dbSelector:s => {
	var fullName = $"{s.FirstName} {s.LastName}";
	return new { s.Id, Name=fullName , s.Age };
});

```

### WhereIf

```cs

List<Student> QueryStudent(int? minAge) =>
appDbContext.Set<Student>()
	.WhereIf(minAge != null, s => s.Age > minAge.Value)
	.ToList();

```

### WhereIfNotNull

```cs

List<Student> QueryStudent(int? minAge) =>
appDbContext.Set<Student>()
	.WhereIfNotNull(minAge, age=> s => s.Age > age)
	.ToList();

```

### OrderByDefaultDesc

```cs

List<Student> QueryStudentOrderByAge(bool? desc) =>
appDbContext.Set<Student>()
	.OrderByDefaultDesc(s => s.Age, desc)
	.ToList();

```