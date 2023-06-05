using DAL.Contracts.App;
using DAL.Contracts.Base;
using BllDto = BLL.DTO;

namespace BLL.Contracts.App;

public interface ISettlementService : IBaseRepository<BllDto.Settlement>,
    ISettlementRepositoryCustom<BllDto.Settlement>
{
    // add your custom service methods here
}