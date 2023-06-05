using BLL.Contracts.Base;
using DAL.Contracts.App;

namespace BLL.Contracts.App;

public interface IAppBLL : IBaseBLL
{
    ISettlementService SettlementService { get; }
    IPictureService PictureService { get; }
    IProductCategoryService ProductCategoryService { get; }
    IBusinessCategoryService BusinessCategoryService { get; }
    IBusinessService BusinessService { get; }
    IBusinessPictureService BusinessPictureService { get; }
    IProductPictureService ProductPictureService { get; }
    IProductService ProductService { get; }
    IInvoiceRowService InvoiceRowService { get; }
    IOrderService OrderService { get; }
    IOrderFeedbackService OrderFeedbackService { get; }
    IBusinessManagerService BusinessManagerService { get; }
    IInvoiceService InvoiceService { get; }
    IAppRefreshTokenService AppRefreshTokenService { get; }
}