using AutoMapper;
using BLL.Contracts.App;
using DAL.Contracts.App;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using DomainApp = Domain.App;
using DalEf = DAL.EF;
using BllApp = BLL.App;
using BllDto = BLL.DTO;

namespace Tests.Services;

public class SettlementService
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly DalEf.App.ApplicationDbContext _ctx;
    private readonly IAppBLL _bll;
    private readonly DomainApp.Settlement _settlement;

    public SettlementService(ITestOutputHelper testOutputHelper)
    {
        // --------------------------------------------------------------------
        _testOutputHelper = testOutputHelper;

        // set up mock db - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<DalEf.App.ApplicationDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _ctx = new DalEf.App.ApplicationDbContext(optionsBuilder.Options);

        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        var dalMapperConfiguration =
            new MapperConfiguration(cfg => { cfg.AddProfile(new DalEf.App.AutoMapperConfig()); });
        var dalMapper = dalMapperConfiguration.CreateMapper();

        //auto mapper configuration
        var bllMapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile(new BllApp.AutoMapperConfig()); });
        var bllMapper = bllMapperConfiguration.CreateMapper();

        IAppUOW uow = new DalEf.App.AppUOW(_ctx, dalMapper);
        _bll = new BllApp.AppBLL(uow, bllMapper);
        // --------------------------------------------------------------------


        _settlement = new DomainApp.Settlement
        {
            Id = Guid.NewGuid(),
            Name =
                "Tallinn",
        };
        _ctx.Settlements.Add(_settlement);
        _ctx.SaveChanges();
        _ctx.Entry(_settlement).State = EntityState.Detached;
        _ctx.SaveChanges();
    }

    [Fact]
    public async void SettlementsService__All_Async_Should_Have_2_Elements()
    {
        // Arrange
        _ctx.Settlements.Add(
            new DomainApp.Settlement
            {
                Id = Guid.NewGuid(),
                Name = "Keila",
            });

        await _ctx.SaveChangesAsync();

        // Act
        var result = (await _bll.SettlementService.AllAsync()).ToList();

        // Assert
        _testOutputHelper.WriteLine(string.Join(", ", result.Select(x => x.Name)));
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }


    [Fact]
    public async void SettlementsService__Should_Find_Element_By_Id_Async()
    {
        // Arrange

        // Act
        var idToBeSearched = _settlement.Id;
        var result = await _bll.SettlementService.FindAsync(idToBeSearched);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(idToBeSearched, result.Id);
    }

    [Fact]
    public async void SettlementsService__Should_Remove_Element_By_Id_Async()
    {
        // Act
        var idToBeSearched = _settlement.Id;
        var result = await _bll.SettlementService.RemoveAsync(idToBeSearched);
        await _bll.SaveChangesAsync();

        // Assert
        Assert.NotNull(result);

        var stillExists = _ctx.Settlements.Any(x => x.Id.Equals(idToBeSearched));
        Assert.False(stillExists);
    }

    [Fact]
    public async void SettlementsService__Should_Remove_Element_By_Entity()
    {
        // Act
        var idToBeSearched = _settlement.Id;
        var result = _bll.SettlementService.Remove(new BllDto.Settlement
        {
            Id = _settlement.Id,
            Name = _settlement.Name,
        });
        await _ctx.SaveChangesAsync();

        // Assert
        Assert.NotNull(result);

        var stillExists = _ctx.Settlements.Any(x => x.Id.Equals(idToBeSearched));
        Assert.False(stillExists);
    }


    [Fact]
    public void SettlementsService__Should_Add_New_Element_To_Db()
    {
        // Arrange
        var value = new BllDto.Settlement()
            { Id = Guid.NewGuid(), Name = "Tallinn" };
        // Act
        var result = _bll.SettlementService.Add(value);
        _ctx.SaveChanges();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(value.Id, result.Id);
        Assert.Equal(value.Name, result.Name);

        var stillExists = _ctx.Settlements.Any(x => x.Id.Equals(value.Id));
        Assert.True(stillExists);
    }

    [Fact]
    public async Task SettlementsService__Should_Update_Element_In_Db()
    {
        // Arrange
        // Act

        var stringToBeChecked = "updated";
        _settlement.Name = stringToBeChecked;

        _bll.SettlementService.Update(new BllDto.Settlement
        {
            Id = _settlement.Id,
            Name = _settlement.Name,
        });
        await _ctx.SaveChangesAsync();

        // Assert
        var elementToBeChecked = await _bll.SettlementService.FindAsync(_settlement.Id);

        Assert.NotNull(elementToBeChecked);
        Assert.Equal(_settlement.Id, elementToBeChecked.Id);
        Assert.Equal(stringToBeChecked, elementToBeChecked.Name);
    }
}