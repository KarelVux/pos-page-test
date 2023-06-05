using Contracts.Base;
using DAL.Contracts.App;
using DAL.EF.Base;

namespace DAL.EF.App.Repositories;

public class PictureRepository : EFBaseRepository<DAL.DTO.Picture, Domain.App.Picture, ApplicationDbContext>, IPictureRepository
{
    public PictureRepository(ApplicationDbContext dataContext, IMapper<DAL.DTO.Picture, Domain.App.Picture> mapper) : base(dataContext, mapper)
    {
    }
}