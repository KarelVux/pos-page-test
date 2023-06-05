using AutoMapper;
using DAL.Contracts.App;
using DAL.EF.App.Mappers;
using DAL.EF.App.Repositories;
using DAL.EF.Base;

namespace DAL.EF.App;

public class AppUOW : EFBaseUOW<ApplicationDbContext>, IAppUOW
{
    private readonly AutoMapper.IMapper _mapper;

    public AppUOW(ApplicationDbContext dataContext, IMapper mapper) : base(dataContext)
    {
        _mapper = mapper;
    }

    private IBusinessRepository? _businessRepository;

    public IBusinessRepository BusinessRepository =>
        _businessRepository ??= new BusinessRepository(UowDbContext, new BusinessMapper(_mapper));

    private IPictureRepository? _pictureRepository;

    public IPictureRepository PictureRepository =>
        _pictureRepository ??= new PictureRepository(UowDbContext, new PictureMapper(_mapper));

    private IProductCategoryRepository? _productCategoryRepository;

    public IProductCategoryRepository ProductCategoryRepository =>
        _productCategoryRepository ??= new ProductCategoryRepository(UowDbContext, new ProductCategoryMapper(_mapper));

    private ISettlementRepository? _settlementRepository;

    public ISettlementRepository SettlementRepository =>
        _settlementRepository ??= new SettlementRepository(UowDbContext, new SettlementMapper(_mapper));

    private IBusinessCategoryRepository? _businessCategoryRepository;

    public IBusinessCategoryRepository BusinessCategoryRepository =>
        _businessCategoryRepository ??=
            new BusinessCategoryRepository(UowDbContext, new BusinessCategoryMapper(_mapper));

    private IBusinessPictureRepository? _businessPictureRepository;

    public IBusinessPictureRepository BusinessPictureRepository =>
        _businessPictureRepository ??= new BusinessPictureRepository(UowDbContext, new BusinessPictureMapper(_mapper));

    private IProductRepository? _productRepository;

    public IProductRepository ProductRepository =>
        _productRepository ??= new ProductRepository(UowDbContext, new ProductMapper(_mapper));

    private IProductPictureRepository? _productPictureRepository;

    public IProductPictureRepository ProductPictureRepository =>
        _productPictureRepository ??= new ProductPictureRepository(UowDbContext, new ProductPictureMapper(_mapper));

    private IOrderRepository? _orderRepository;

    public IOrderRepository OrderRepository =>
        _orderRepository ??= new OrderRepository(UowDbContext, new OrderMapper(_mapper));

    private IOrderFeedbackRepository? _orderFeedbackRepository;

    public IOrderFeedbackRepository OrderFeedbackRepository =>
        _orderFeedbackRepository ??= new OrderFeedbackRepository(UowDbContext, new OrderFeedbackMapper(_mapper));


    private IInvoiceRepository? _invoiceRepository;

    public IInvoiceRepository InvoiceRepository =>
        _invoiceRepository ??= new InvoiceRepository(UowDbContext, new InvoiceMapper(_mapper));


    private IInvoiceRowRepository? _invoiceRowRepository;

    public IInvoiceRowRepository InvoiceRowRepository =>
        _invoiceRowRepository ??= new InvoiceRowRepository(UowDbContext, new InvoiceRowMapper(_mapper));


    private IBusinessManagerRepository? _businessManagerRepository;

    public IBusinessManagerRepository BusinessManagerRepository =>
        _businessManagerRepository ??= new BusinessManagerRepository(UowDbContext, new BusinessManagerMapper(_mapper));

    private IAppRefreshTokenRepository? _appRefreshTokenRepository;

    public IAppRefreshTokenRepository AppRefreshTokenRepository =>
        _appRefreshTokenRepository ??= new AppRefreshTokenRepository(UowDbContext, new AppRefreshTokenMapper(_mapper));
}