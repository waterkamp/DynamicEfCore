using DynamicEfCore;
using System.Text.Json;

using var db = new BloggingContext();

// add some dummy data
db.Blogs.Add(new Blog() { Name = "Blog number 1" });
db.Blogs.Add(new Blog() { Name = "Blog number 2" });
db.Blogs.Add(new Blog() { Name = "Blog number 3" });
db.Blogs.Add(new Blog() { Name = "Blog number 4" });

db.Persons.Add(new Person() { Name = "Jack" });
db.Persons.Add(new Person() { Name = "Thomas" });
db.Persons.Add(new Person() { Name = "Steven" });
db.Persons.Add(new Person() { Name = "Frank" });
db.SaveChanges();


// access the dbSet by a string and call a specific search-method
var blogs = db.GetDynamicDbSet("Blogs").SearchByTerm("number 1", nameof(Blog.Name));
var persons = db.GetDynamicDbSet("Persons").SearchByTerm("Thomas", nameof(Person.Name));


// show the result
Console.WriteLine(JsonSerializer.Serialize(blogs));
Console.WriteLine(JsonSerializer.Serialize(persons));

