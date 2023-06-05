# General db migration

~~~bash
 dotnet ef database drop  --project DAL.EF.App --startup-project WebApp --context ApplicationDbContext -f
 dotnet ef migrations remove --project DAL.EF.App --startup-project WebApp --context ApplicationDbContext -f
 
 
 dotnet ef migrations add Initial  --project DAL.EF.App --startup-project WebApp --context ApplicationDbContext
 dotnet ef database update --project DAL.EF.App --startup-project WebApp --context ApplicationDbContext




~~~


V2
~~~bash
 dotnet ef database update --project DAL.EF.App --startup-project WebApp --context ApplicationDbContext
 dotnet ef migrations add Initial  --project DAL.EF.App --startup-project WebApp --context ApplicationDbContext
 
 dotnet ef migrations remove --project DAL.EF.App --startup-project WebApp --context ApplicationDbContext -f
 dotnet ef database drop  --project DAL.EF.App --startup-project WebApp --context ApplicationDbContext -f
 
 
~~~



# Generate rest controllers

Add nuget packeages
Microsoft.VisualStudio.Web.CodeGeneration.Design
Microsoft.EntityFrameworkCore.SqlServer
~~~bash
dotnet  tool install --global dotnet-aspnet-codegenerator

# scaffold 

# MVC
dotnet aspnet-codegenerator controller -name BusinessesController             -actions -m  Business             -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name BusinessCategoriesController     -actions -m  BusinessCategory     -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name BusinessManagersController       -actions -m  BusinessManager      -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name BusinessPicturesController       -actions -m  BusinessPicture      -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name InvoicesController               -actions -m  Invoice              -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name InvoiceRowsController            -actions -m  InvoiceRow           -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name OrdersController                 -actions -m  Order                -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name BusinessesController             -actions -m  Business             -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name OrderFeedbacksController         -actions -m  OrderFeedback        -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name PicturesController               -actions -m  Picture              -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ProductsController               -actions -m  Product              -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ProductCategoriesController      -actions -m  ProductCategory      -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ProductPicturesController        -actions -m  ProductPicture       -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ProductTagsController            -actions -m  ProductTag           -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name SettlementsController            -actions -m  Settlement           -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f



# Rest API 
dotnet aspnet-codegenerator controller -name PicturesController -actions -m  Domain.App.Picture -dc DAL.EF.App.ApplicationDbContext -outDir Api -api --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name SettlementsController -actions -m  Domain.App.Settlement -dc DAL.EF.App.ApplicationDbContext -outDir Api -api --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ProductCategoriesController -actions -m  Domain.App.ProductCategory -dc DAL.EF.App.ApplicationDbContext -outDir Api -api --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name BusinessCategoriesController -actions -m  Domain.App.BusinessCategory -dc DAL.EF.App.ApplicationDbContext -outDir Api -api --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name BusinessesController -actions -m  Domain.App.Business -dc DAL.EF.App.ApplicationDbContext -outDir Api -api --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f

dotnet aspnet-codegenerator controller -m Domain.App.Picture -name PicturesController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f
dotnet aspnet-codegenerator controller -m Domain.App.Settlement -name SettlementsController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f
dotnet aspnet-codegenerator controller -m Domain.App.ProductCategory -name ProductCategoriesController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f
dotnet aspnet-codegenerator controller -m Domain.App.BusinessCategory -name BusinessCategoriesController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f
dotnet aspnet-codegenerator controller -m Domain.App.Business -name BusinessesController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f
dotnet aspnet-codegenerator controller -m Domain.App.Product -name ProductController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f
dotnet aspnet-codegenerator controller -m Domain.App.ProductTag -name ProductTagsController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f
dotnet aspnet-codegenerator controller -m Domain.App.Invoice -name InvoicesController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f
dotnet aspnet-codegenerator controller -m Domain.App.Order -name OrdersController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f
dotnet aspnet-codegenerator controller -m Domain.App.OrderFeedback -name OrderFeedbacksController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f
dotnet aspnet-codegenerator controller -m Domain.App.BusinessPicture -name BusinessPicturesController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f
dotnet aspnet-codegenerator controller -m Domain.App.ProductPicture -name ProductPictureController -outDir ApiControllers -api -dc ApplicationDbContext  -udl -f

~~~ 


# Docker local run
~~~bash
# 2:12:00
docker build -t pos_webapp .
docker run --name pos_webapp --rm -it -p 8000:80 pos_webapp


docker build -t pos_webapp .

~~~

# Docker publish automatic
~~~bash
docker build -t pos_webapp .


~~~
