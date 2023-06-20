using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.Test
{
    public class TestDbContext : DbContext
    {
        public DbSet<Student> Students => Set<Student>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseInMemoryDatabase("test");
        }
    }

    public class Student
    {
        public Student(int id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }

    public class TestDbContextFixture : IDisposable
    {
        public TestDbContextFixture()
        {
            DbContext = new TestDbContext();

            DbContext.Students.AddRange(
                new Student(1, "1", 1),
                new Student(2, "2", 2),
                new Student(3, "3", 3),
                new Student(4, "4", 4),
                new Student(5, "5", 5),
                new Student(6, "6", 6),
                new Student(7, "7", 7),
                new Student(8, "8", 8),
                new Student(9, "9", 9),
                new Student(10, "10", 10),
                new Student(11, "11", 11),
                new Student(12, "12", 12),
                new Student(13, "13", 13),
                new Student(14, "14", 14),
                new Student(15, "15", 15),
                new Student(16, "16", 16),
                new Student(17, "17", 17),
                new Student(18, "18", 18),
                new Student(19, "19", 19),
                new Student(20, "20", 20),
                new Student(21, "21", 21),
                new Student(22, "22", 22),
                new Student(23, "23", 23),
                new Student(24, "24", 24),
                new Student(25, "25", 25),
                new Student(26, "26", 26),
                new Student(27, "27", 27),
                new Student(28, "28", 28),
                new Student(29, "29", 29),
                new Student(30, "30", 30),
                new Student(31, "31", 31),
                new Student(32, "32", 32),
                new Student(33, "33", 33),
                new Student(34, "34", 34),
                new Student(35, "35", 35),
                new Student(36, "36", 36),
                new Student(37, "37", 37)
                );

            DbContext.SaveChanges();
        }

        public TestDbContext DbContext { get; init; }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}