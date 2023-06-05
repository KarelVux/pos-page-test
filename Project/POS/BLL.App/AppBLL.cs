using AutoMapper;
using BLL.App.Mappers;
using BLL.App.Services;
using BLL.Base;
using BLL.Contracts.App;
using DAL.Contracts.App;

namespace BLL.App;

public class AppBLL : BaseBLL<IAppUOW>, IAppBLL
{
    protected readonly IAppUOW UOW;
    private readonly AutoMapper.IMapper _mapper;

    public AppBLL(IAppUOW uow, IMapper mapper) : base(uow)
    {
        UOW = uow;
        _mapper = mapper;
    }

    private ISettlementService? _settlements;

    public ISettlementService SettlementService =>
        _settlements ??= new SettlementService(Uow, new SettlementMapper(_mapper));

    private IPictureService? _picture;


    public IPictureService PictureService =>
        _picture ??= new PictureService(Uow, new PictureMapper(_mapper));

    private IProductCategoryService? _productCategory;

    public IProductCategoryService ProductCategoryService =>
        _productCategory ??= new ProductCategoryService(Uow, new ProductCategoryMapper(_mapper));

    private IBusinessCategoryService? _businessCategory;

    public IBusinessCategoryService BusinessCategoryService =>
        _businessCategory ??= new BusinessCategoryService(Uow, new BusinessCategoryMapper(_mapper));

    private IBusinessService? _business;

    public IBusinessService BusinessService =>
        _business ??= new BusinessService(Uow, new BusinessMapper(_mapper));

    private IBusinessPictureService? _businessPicture;


    public IBusinessPictureService BusinessPictureService =>
        _businessPicture ??= new BusinessPictureService(Uow, new BusinessPictureMapper(_mapper));

    private IProductPictureService? _productPicture;

    public IProductPictureService ProductPictureService =>
        _productPicture ??= new ProductPictureService(Uow, new ProductPictureMapper(_mapper));

    private IProductService? _productService;

    public IProductService ProductService =>
        _productService ??= new ProductService(Uow, new ProductMapper(_mapper));

    private IInvoiceRowService? _invoiceRowService;

    public IInvoiceRowService InvoiceRowService =>
        _invoiceRowService ??= new InvoiceRowService(Uow, new InvoiceRowMapper(_mapper));

    private IOrderService? _orderService;

    public IOrderService OrderService =>
        _orderService ??= new OrderService(Uow, new OrderMapper(_mapper));

    private IOrderFeedbackService? _orderFeedbackService;

    public IOrderFeedbackService OrderFeedbackService =>
        _orderFeedbackService ??= new OrderFeedbackService(Uow, new OrderFeedbackMapper(_mapper));

    private IBusinessManagerService? _businessManagerService;

    public IBusinessManagerService BusinessManagerService =>
        _businessManagerService ??= new BusinessManagerService(Uow, new BusinessManagerMapper(_mapper));


    private IInvoiceService? _invoiceService;

    public IInvoiceService InvoiceService =>
        _invoiceService ??= new InvoiceService(Uow, new InvoiceMapper(_mapper));
    
    private IAppRefreshTokenService? _appRefreshTokenService;

    public IAppRefreshTokenService AppRefreshTokenService =>
        _appRefreshTokenService ??= new AppRefreshTokenService(Uow, new AppRefreshTokenMapper(_mapper));
}