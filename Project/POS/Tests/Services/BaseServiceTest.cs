using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BLL.Base;
using DAL.Base;
using DAL.Contracts.Base;
using Domain.Contracts.Base;
using Helpers;
using Moq;
using Xunit.Abstractions;

namespace Tests.Services;

public class BaseServiceTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Mock<IBaseRepository<DalBusinessCategory>> _baseRepositoryMock;

    private readonly BaseEntityService<BllBusinessCategoryData, DalBusinessCategory,
            IBaseRepository<DalBusinessCategory>>
        _baseEntityService;


    public BaseServiceTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _baseRepositoryMock = new Mock<IBaseRepository<DalBusinessCategory>>();
        _baseEntityService =
            new BaseEntityService<BllBusinessCategoryData, DalBusinessCategory, IBaseRepository<DalBusinessCategory>>(
                _baseRepositoryMock.Object,
                new BaseMapper<BllBusinessCategoryData, DalBusinessCategory>(
                    new MapperConfiguration(cfg => cfg
                            .CreateMap<DalBusinessCategory, BllBusinessCategoryData>().ReverseMap()
                        )
                        .CreateMapper()));
    }

    [Fact]
    public async void BaseService__All_Async_Should_Have_4_Elements()
    {
        var expectedDatas = new List<DalBusinessCategory>()
        {
            new DalBusinessCategory()
            {
                Id = Guid.NewGuid(),
                Title = "Tallinn"
            },
            new DalBusinessCategory()
            {
                Id = Guid.NewGuid(),
                Title = "Tartu"
            },
            new DalBusinessCategory()
            {
                Id = Guid.NewGuid(),
                Title = "Parnu"
            },
            new DalBusinessCategory()
            {
                Id = Guid.NewGuid(),
                Title = "Keila"
            }
        };
        // Arrange
        _baseRepositoryMock.Setup(r => r.AllAsync()).ReturnsAsync(expectedDatas);

        // Act
        var result = (await _baseEntityService.AllAsync()).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.Count());

        foreach (var expectedData in expectedDatas)
        {
            var resultData = result.ToList().FirstOrDefault(x => x.Id == expectedData.Id);
            Assert.NotNull(resultData);
            Assert.Equal(expectedData.Title, resultData.Title);
        }
    }

    [Fact]
    public async void BaseService__Find_Async_Should_Should_Item_By_Item_Id()
    {
        var expectedElement = new BllBusinessCategoryData()
        {
            Title = "Tallinn",
            Id = Guid.NewGuid()
        };
        var tempDb = new List<DalBusinessCategory>()
        {
            new DalBusinessCategory
            {
                Title = expectedElement.Title,
                Id = expectedElement.Id
            }
        };

        _baseRepositoryMock.Setup(r =>
                r.FindAsync(It.IsAny<Guid>()))
            .Returns<Guid>((id) =>
            {
                var res = tempDb.FirstOrDefault(x => x.Id == id);
                return Task.FromResult(res);
            });


        var result = (await _baseEntityService.FindAsync(expectedElement.Id));

        // Assert
        Assert.NotNull(result);
        Assert.Single(tempDb.ToList());

        Assert.Equal(expectedElement.Title, result.Title);
        Assert.Equal(expectedElement.Id, result.Id);
    }

    [Fact]
    public void BaseService__Add_Should_Add_Item_To_Memory()
    {
        var tempDb = new List<DalBusinessCategory>();
        var expectedElement = new BllBusinessCategoryData()
        {
            Title = "Tallinn",
            Id = Guid.NewGuid()
        };
        _baseRepositoryMock.Setup(r =>
                r.Add(It.IsAny<DalBusinessCategory>()))
            .Returns<DalBusinessCategory>((entity) =>
            {
                tempDb.Add(entity);
                return entity;
            });

        var result = _baseEntityService
            .Add(expectedElement);


        // Assert
        Assert.NotNull(result);
        Assert.Single(tempDb.ToList());

        Assert.Equal(expectedElement.Title, result.Title);
        Assert.Equal(expectedElement.Id, result.Id);

        foreach (var item in tempDb)
        {
            var searchableItem = tempDb.FirstOrDefault(x => x.Id == item.Id);
            Assert.NotNull(searchableItem);
            Assert.Equal(expectedElement.Title, item.Title);
        }
    }

    [Fact]
    public  void BaseService__Update_Should_Update_Item_In_Memory()
    {
        var expectedElement = new BllBusinessCategoryData()
        {
            Title = "Tallinn",
            Id = Guid.NewGuid()
        };
        var tempDb = new List<DalBusinessCategory>()
        {
            new DalBusinessCategory
            {
                Title = expectedElement.Title,
                Id = expectedElement.Id
            }
        };
        var updatedValue = "FooBar";

        _baseRepositoryMock.Setup(r =>
                r.Update(It.IsAny<DalBusinessCategory>()))
            .Returns<DalBusinessCategory>((entity) =>
            {
                var returnable = tempDb.FirstOrDefault(x => x.Id == entity.Id);
                returnable!.Title = entity.Title;
                return entity;
            });

        expectedElement.Title = updatedValue;
        var result = (_baseEntityService.Update(expectedElement));

        // Assert
        Assert.NotNull(result);

        Assert.Equal(expectedElement.Title, result.Title);
        Assert.Equal(expectedElement.Id, result.Id);

        foreach (var item in tempDb)
        {
            var searchableItem = tempDb.FirstOrDefault(x => x.Id == item.Id);
            Assert.NotNull(searchableItem);
            Assert.Equal(expectedElement.Title, item.Title);
        }
    }

    [Fact]
    public async void BaseService__Update_Async_Should_Update_Item_In_Memory()
    {
        var expectedElement = new BllBusinessCategoryData()
        {
            Title = "Tallinn",
            Id = Guid.NewGuid()
        };
        var tempDb = new List<DalBusinessCategory>()
        {
            new DalBusinessCategory
            {
                Title = expectedElement.Title,
                Id = expectedElement.Id
            }
        };
        var updatedValue = "FooBar";

        _baseRepositoryMock.Setup(r =>
                r.UpdateAsync(It.IsAny<DalBusinessCategory>()))
            .Returns<DalBusinessCategory>( (entity) =>
            {
                var returnable = tempDb.FirstOrDefault(x => x.Id == entity.Id);
                returnable!.Title = entity.Title;
                return Task.FromResult(entity)!;
            });

        expectedElement.Title = updatedValue;
        var result = (await _baseEntityService.UpdateAsync(expectedElement));

        // Assert
        Assert.NotNull(result);

        Assert.Equal(expectedElement.Title, result.Title);
        Assert.Equal(expectedElement.Id, result.Id);

        foreach (var item in tempDb)
        {
            var searchableItem = tempDb.FirstOrDefault(x => x.Id == item.Id);
            Assert.NotNull(searchableItem);
            Assert.Equal(expectedElement.Title, item.Title);
        }
    }

    [Fact]
    public async void BaseService__Remove_Async_Remove_Item_From_Memory()
    {
        var expectedElement = new BllBusinessCategoryData()
        {
            Title = "Tallinn",
            Id = Guid.NewGuid()
        };
        var tempDb = new List<DalBusinessCategory>()
        {
            new DalBusinessCategory
            {
                Title = expectedElement.Title,
                Id = expectedElement.Id
            }
        };

        _baseRepositoryMock.Setup(r =>
                r.RemoveAsync(It.IsAny<Guid>()))
            .Returns<Guid>( (id) =>
            {
                var returnable = tempDb.FirstOrDefault(x => x.Id == id);
                tempDb.Remove(returnable!);
                return  Task.FromResult(returnable);
            });

        var result = (await _baseEntityService.RemoveAsync(expectedElement.Id));

        // Assert
        Assert.NotNull(result);

        Assert.Equal(expectedElement.Title, result.Title);
        Assert.Equal(expectedElement.Id, result.Id);
    
        Assert.Empty(tempDb.ToList());        
    }

    [Fact]
    public  void BaseService__Remove_Remove_Item_From_Memory()
    {
        
        var expectedElement = new BllBusinessCategoryData()
        {
            Title = "Tallinn",
            Id = Guid.NewGuid()
        };
        var tempDb = new List<DalBusinessCategory>()
        {
            new DalBusinessCategory
            {
                Title = expectedElement.Title,
                Id = expectedElement.Id
            }
        };

        _baseRepositoryMock.Setup(r =>
                r.Remove(It.IsAny<DalBusinessCategory>()))
            .Returns<DalBusinessCategory>( ( entity) =>
            {
                var returnable = tempDb.FirstOrDefault(x => x.Id == entity.Id);
                tempDb.Remove(returnable!);
                return  returnable!;
            });

        var result = ( _baseEntityService.Remove(expectedElement));

        // Assert
        Assert.NotNull(result);

        Assert.Equal(expectedElement.Title, result.Title);
        Assert.Equal(expectedElement.Id, result.Id);
    
        Assert.Empty(tempDb.ToList()); 
    }
}

public class BllBusinessCategoryData : IDomainEntityId
{
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Title { get; set; } = default!;

    public Guid Id { get; set; }
}

public class DalBusinessCategory : IDomainEntityId<Guid>, IDomainEntityId
{
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Title { get; set; } = default!;

    public Guid Id { get; set; }
}