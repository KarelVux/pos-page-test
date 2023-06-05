using BLL.Base;
using BLL.Contracts.App;
using Contracts.Base;
using DAL.Contracts.App;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Services;

public class OrderService :
    BaseEntityService<BllDto.Order, DalDto.Order, IOrderRepository>,
    IOrderService
{
    protected IAppUOW Uow;

    public OrderService(IAppUOW uow, IMapper<BllDto.Order, DalDto.Order> mapper)
        : base(uow.OrderRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<BllDto.Order>> AllAsyncWithInclude()
    {
        return (await Repository.AllAsyncWithInclude()).Select(e => Mapper.Map(e))!;
    }

    public async Task<BllDto.Order?> FindAsyncWithInclude(Guid id)
    {
        return Mapper.Map(await Repository.FindAsyncWithInclude(id));
    }

}