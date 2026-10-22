using lab12v11;

IRepository<Movie> movieRepo = new InMemoryRepository<Movie>();

// 1. Add
movieRepo.Add(new Movie { Id = 1, Title = "Inception", Director = "Christopher Nolan", ReleaseYear = 2010 });
movieRepo.Add(new Movie { Id = 2, Title = "The Matrix", Director = "Lana & Lilly Wachowski", ReleaseYear = 1999 });

Console.WriteLine("\n Усі фільми після додавання");
foreach (var m in movieRepo.GetAll())
{
    Console.WriteLine($"[{m.Id}] {m.Title} ({m.ReleaseYear}) - Режисер: {m.Director}");
}

// 2. Update
movieRepo.Update(new Movie { Id = 1, Title = "Inception (Updated)", Director = "Christopher Nolan", ReleaseYear = 2010 });

// 3. GetById
var updatedMovie = movieRepo.GetById(1);
Console.WriteLine($"\nОновлений фільм (Id=1)\n{updatedMovie?.Title}");

// 4. Delete
movieRepo.Delete(2);
Console.WriteLine("\nСписок після видалення (Id=2)");
foreach (var m in movieRepo.GetAll())
{
    Console.WriteLine($"[{m.Id}] {m.Title}");
}
