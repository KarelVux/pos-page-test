using BLL.Base;
using BLL.Contracts.App;
using Contracts.Base;
using DAL.Contracts.App;
using  BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Services;

public class SettlementService :
    BaseEntityService<BllDto.Settlement, DalDto.Settlement, ISettlementRepository>, ISettlementService
{
    protected IAppUOW Uow;

    public SettlementService(IAppUOW uow, IMapper<BllDto.Settlement, DalDto.Settlement> mapper)
        : base(uow.SettlementRepository, mapper)
    {
        Uow = uow;
    }
}