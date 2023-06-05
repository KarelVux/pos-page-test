using BLL.Base;
using BLL.Contracts.App;
using Contracts.Base;
using DAL.Contracts.App;
using  BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Services;

public class  PictureService :
    BaseEntityService<BllDto. Picture, DalDto. Picture, IPictureRepository>, IPictureService
{
    protected IAppUOW Uow;

    public PictureService(IAppUOW uow, IMapper<BllDto. Picture, DalDto. Picture> mapper)
        : base(uow.PictureRepository, mapper)
    {
        Uow = uow;
    }
}