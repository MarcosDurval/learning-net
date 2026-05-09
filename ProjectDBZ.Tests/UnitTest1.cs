using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBZ.Application.DTOs;
using DBZ.Application.Services;
using DBZ.Presentation.Controllers;
using DBZ.Domain.Entities;
using DBZ.Infrastructure.Persistence;
using DBZ.Infrastructure.Repositories;
using DBZ.Application.Races;
using DBZ.Presentation.DTOs;

namespace ProjectDBZ.Tests;

public class PersonagemTests : IAsyncLifetime
{

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    private readonly DbContextOptions<AppDbContext> _options;

    public PersonagemTests()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"db-{Guid.NewGuid()}")
            .Options;
    }

    private AppDbContext CreateContext() => new AppDbContext(_options);

    private static PersonagensController CreateController(AppDbContext dbContext)
    {
        var repository = new PersonagemRepository(dbContext);
        var strategy = new RaceStrategy(new RaceProfileProvider());
        var service = new PersonagemService(repository, strategy);
        return new PersonagensController(service);
    }

    private static Personagem CreatePersonagem(Guid id, string description = "Saiyan", string race = "Saiyan") => new()
    {
        Id = id,
        Name = "Goku",
        Power = 80,
        Race = race,
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

        var controller = CreateController(dbContext);

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

        var controller = CreateController(dbContext);

        var updatedPersonagem = new EditPersonagemDTO
        {
            Name = "Goku",
            Power = 80,
            Race = "Saiyan",
            Description = "Muito Forte"
        };

        var actionResult = await controller.UpdatePersonagem(updatedPersonagem, personagemId);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        var returnedPersonagem = Assert.IsType<Personagem>(okResult.Value);
        Assert.Equal(updatedPersonagem.Description, returnedPersonagem.Description);
        Assert.Null(returnedPersonagem.Transformation);
    }

    [Fact]
    public async Task TransformPersonagem_WhenSaiyanTransformsToSuperSaiyan_ReturnsOk()
    {
        await using var dbContext = CreateContext();
        var personagemId = Guid.NewGuid();
        var existingPersonagem = CreatePersonagem(personagemId);

        dbContext.DBZ.Add(existingPersonagem);
        await dbContext.SaveChangesAsync();

        var controller = CreateController(dbContext);

        var actionResult = await controller.TransformPersonagem(personagemId, "Super Saiyan");

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var returnedPersonagem = Assert.IsType<PersonagemResult>(okResult.Value);
        Assert.Equal("Super Saiyan", returnedPersonagem.Transformation);
        Assert.Equal(140, returnedPersonagem.EffectivePower);
    }

    [Fact]
    public async Task TransformPersonagem_WhenHumanTransformsToSuperSaiyan_ReturnsBadRequest()
    {
        await using var dbContext = CreateContext();
        var personagemId = Guid.NewGuid();
        var existingPersonagem = CreatePersonagem(personagemId, race: "Humano");

        dbContext.DBZ.Add(existingPersonagem);
        await dbContext.SaveChangesAsync();

        var controller = CreateController(dbContext);

        var actionResult = await controller.TransformPersonagem(personagemId, "Super Saiyan");

        Assert.IsType<BadRequestObjectResult>(actionResult);
    }

    [Fact]
    public async Task TransformPersonagem_WhenNamekuseiTransformsToSuperSaiyan_ReturnsBadRequest()
    {
        await using var dbContext = CreateContext();
        var personagemId = Guid.NewGuid();
        var existingPersonagem = CreatePersonagem(personagemId, race: "Namekusei");

        dbContext.DBZ.Add(existingPersonagem);
        await dbContext.SaveChangesAsync();

        var controller = CreateController(dbContext);

        var actionResult = await controller.TransformPersonagem(personagemId, "Super Saiyan");

        Assert.IsType<BadRequestObjectResult>(actionResult);
    }

    [Fact]
    public async Task TransformPersonagem_WhenNamekuseiTransformsToGigante_ReturnsOk()
    {
        await using var dbContext = CreateContext();
        var personagemId = Guid.NewGuid();
        var existingPersonagem = CreatePersonagem(personagemId, race: "Namekusei");

        dbContext.DBZ.Add(existingPersonagem);
        await dbContext.SaveChangesAsync();

        var controller = CreateController(dbContext);

        var actionResult = await controller.TransformPersonagem(
            personagemId,
            new TransformPersonagemDTO { Transformation = "Gigante" });

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var returnedPersonagem = Assert.IsType<PersonagemResult>(okResult.Value);
        Assert.Equal("Gigante", returnedPersonagem.Transformation);
        Assert.Equal(123, returnedPersonagem.EffectivePower);
    }

    [Fact]
    public void GetRaces_ReturnsAvailableRaceProfiles()
    {
        var controller = new RacesController(new RaceProfileProvider());

        var actionResult = controller.GetRaces();

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var races = Assert.IsAssignableFrom<IEnumerable<RaceResult>>(okResult.Value);
        Assert.Contains(races, race => race.Race == "Saiyan" && race.Transformations.Contains("Super Saiyan"));
        Assert.Contains(races, race => race.Race == "Namekusei" && race.Transformations.Contains("Gigante"));
    }

}
