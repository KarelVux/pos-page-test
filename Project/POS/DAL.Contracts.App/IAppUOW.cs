using DAL.Contracts.Base;

namespace DAL.Contracts.App;

public interface IAppUOW : IBaseUOW
{
    // list your repositories here
    IBusinessRepository BusinessRepository { get; }
    IPictureRepository PictureRepository { get; }
    IProductCategoryRepository ProductCategoryRepository { get; }
    ISettlementRepository SettlementRepository { get; }
    IBusinessCategoryRepository BusinessCategoryRepository { get; }
    IBusinessPictureRepository BusinessPictureRepository { get; }
    IProductRepository ProductRepository { get; }
    IProductPictureRepository ProductPictureRepository { get; }
    IOrderRepository OrderRepository { get; }
    IOrderFeedbackRepository OrderFeedbackRepository { get; }
    IInvoiceRepository InvoiceRepository { get; }
    IInvoiceRowRepository InvoiceRowRepository { get; }
    IBusinessManagerRepository BusinessManagerRepository { get; }
    IAppRefreshTokenRepository AppRefreshTokenRepository { get; }
}