using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectDBZ.Controllers;
using ProjectDBZ.Data;
using ProjectDBZ.Models;

namespace ProjectDBZ.Tests;

public class PersonagemTests : IAsyncLifetime
{

    public Task InitializeAsync()
    {
        throw new NotImplementedException();
    }

    public Task DisposeAsync()
    {
        throw new NotImplementedException();
    }
    private readonly DbContextOptions<AppDbContext> _options;

    public PersonagemTests()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"db-{Guid.NewGuid()}")
            .Options;
    }

    private AppDbContext CreateContext() => new AppDbContext(_options);

    private static Personagem CreatePersonagem(Guid id, string description = "Saiyan") => new()
    {
        Id = id,
        Name = "Goku",
        Power = "Kamehameha",
        Description = description
    };

    [Fact]
    public async Task DeletePersonagem_RemovesEntity_ReturnsNoContent()
    {
        await using var dbContext = CreateContext();
        var personagemId = Guid.NewGuid();
        var existingPersonagem = CreatePersonagem(personagemId);

        dbContext.DBZ.Add(existingPersonagem);
        await dbContext.SaveChangesAsync();

        var controller = new PersonagensController(dbContext);

        var actionResult = await controller.DeletePersonagem(personagemId);

        Assert.IsType<NoContentResult>(actionResult);
        var deletedPersonagem = await dbContext.DBZ.FindAsync(personagemId);
        Assert.Null(deletedPersonagem);
    }

    [Fact]
    public async Task UpdatePersonagem()
    {
        await using var dbContext = CreateContext();
        var personagemId = Guid.NewGuid();
        var existingPersonagem = CreatePersonagem(personagemId, "Saiyan");

        dbContext.DBZ.Add(existingPersonagem);
        await dbContext.SaveChangesAsync();

        var controller = new PersonagensController(dbContext);

        var updatedPersonagem = CreatePersonagem(personagemId, "Muito Forte");

        var actionResult = await controller.UpdatePersonagem(updatedPersonagem, personagemId);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        var returnedPersonagem = Assert.IsType<Personagem>(okResult.Value);
        Assert.Equal(updatedPersonagem.Description, returnedPersonagem.Description);
    }


}
