using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace lab12v11.Tests;

public class InMemoryRepositoryTests
{
    private readonly InMemoryRepository<Movie> _repository;

    public InMemoryRepositoryTests()
    {
        _repository = new InMemoryRepository<Movie>();
    }

    [Fact]
    public void Add_ShouldAddItemToRepository()
    {
        // Arrange
        var movie = new Movie { Id = 1, Title = "Interstellar", Director = "Nolan", ReleaseYear = 2014 };

        // Act
        _repository.Add(movie);

        // Assert
        var result = _repository.GetById(1);
        Assert.NotNull(result);
        Assert.Equal("Interstellar", result.Title);
    }

    [Fact]
    public void Add_DuplicateId_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var movie1 = new Movie { Id = 1, Title = "Movie 1", Director = "Dir 1", ReleaseYear = 2020 };
        var movie2 = new Movie { Id = 1, Title = "Movie 2", Director = "Dir 2", ReleaseYear = 2021 };
        _repository.Add(movie1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _repository.Add(movie2));
    }

    [Fact]
    public void GetById_ExistingId_ShouldReturnItem()
    {
        // Arrange
        var movie = new Movie { Id = 1, Title = "Dune", Director = "Denis Villeneuve", ReleaseYear = 2021 };
        _repository.Add(movie);

        // Act
        var result = _repository.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void GetById_NonExistingId_ShouldReturnNull()
    {
        // Act
        var result = _repository.GetById(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_ShouldReturnAllItems()
    {
        // Arrange
        _repository.Add(new Movie { Id = 1, Title = "Movie A", Director = "Dir A", ReleaseYear = 2000 });
        _repository.Add(new Movie { Id = 2, Title = "Movie B", Director = "Dir B", ReleaseYear = 2005 });

        // Act
        var result = _repository.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void Update_ExistingItem_ShouldUpdateData()
    {
        // Arrange
        var movie = new Movie { Id = 1, Title = "Old Title", Director = "Director", ReleaseYear = 2000 };
        _repository.Add(movie);

        var updatedMovie = new Movie { Id = 1, Title = "New Title", Director = "Director", ReleaseYear = 2000 };

        // Act
        _repository.Update(updatedMovie);

        // Assert
        var result = _repository.GetById(1);
        Assert.NotNull(result);
        Assert.Equal("New Title", result.Title);
    }

    [Fact]
    public void Update_NonExistingItem_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var movie = new Movie { Id = 999, Title = "Ghost", Director = "Unknown", ReleaseYear = 2000 };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _repository.Update(movie));
    }

    [Fact]
    public void Delete_ExistingItem_ShouldRemoveFromRepository()
    {
        // Arrange
        var movie = new Movie { Id = 1, Title = "ToDelete", Director = "Dir", ReleaseYear = 2010 };
        _repository.Add(movie);

        // Act
        _repository.Delete(1);

        // Assert
        var result = _repository.GetById(1);
        Assert.Null(result);
    }

    [Fact]
    public void Delete_NonExistingItem_ShouldThrowKeyNotFoundException()
    {
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _repository.Delete(999));
    }
}