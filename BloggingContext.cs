using Microsoft.EntityFrameworkCore;

namespace DynamicEfCore
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Person> Persons { get; set; }

        public string DbPath { get; }

        public BloggingContext()
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseInMemoryDatabase("BloggingDatabase");
    }

    public class Blog
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
