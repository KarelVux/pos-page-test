using BLL.Contracts.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.Admin;

namespace WebApp.Controllers
{
    /// <summary>
    ///   Admin view controller
    /// </summary>
    [Authorize(Roles = "Root")]
    public class ProductsController : Controller
    {
        /// <summary>
        /// Reference to the BLL service.
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// Controller constructor 
        /// </summary>
        /// <param name="bll"></param>
        public ProductsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Products
        /// <summary>
        /// Get all controller records from DB
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var vm = await _bll.ProductService.AllAsyncWithInclude();
            return View(vm);
        }

        // GET: Products/Details/5
        /// <summary>
        /// Produces view for single record
        /// </summary>
        /// <param name="id">ID to be loaded from DB</param>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _bll.ProductService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // GET: Products/Create
        /// <summary>
        /// Loads create view
        /// </summary>
        public async Task<IActionResult> Create()
        {
            var vm = new ProductCreateEditVM();

            vm = await InitializeSelectList(vm);
            return View(vm);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Loads create view to add new entity to DB
        /// </summary>
        /// <param name="vm">Record to be created</param>
        /// <returns>Index view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _bll.ProductService.Add(vm.Product);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm = await InitializeSelectList(vm, vm.Product.BusinessId, vm.Product.ProductCategoryId);

            return View(vm);
        }

        // GET: Products/Edit/5
        /// <summary>
        /// Loads edit view To edit DB records
        /// </summary>
        /// <param name="id">ID to be edited</param>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _bll.ProductService.FindAsync(id.Value);
            if (element == null)
            {
                return NotFound();
            }

            var vm = new ProductCreateEditVM()
            {
                Product = element
            };

            vm = await InitializeSelectList(vm, vm.Product.BusinessId, vm.Product.ProductCategoryId);

            return View(vm);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Saves edited values if everything is okay
        /// </summary>
        /// <param name="id">Id to be edited</param>
        /// <param name="vm">Edited object</param>
        /// <returns>Index view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductCreateEditVM vm)
        {
            if (id != vm.Product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.ProductService.Update(vm.Product);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await ProductExists(vm.Product.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            vm = await InitializeSelectList(vm, vm.Product.BusinessId, vm.Product.ProductCategoryId);
            return View(vm);
        }

        // GET: Products/Delete/5
        /// <summary>
        /// Loads details view of a record that will be deleted
        /// </summary>
        /// <param name="id">ID to be loaded</param>
        /// <returns>Delete view</returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _bll.ProductService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // POST: Products/Delete/5
        /// <summary>
        /// Deletes specified record from db
        /// </summary>
        /// <param name="id">ID to be deleted</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await ProductExists(id))
            {
                await _bll.ProductService.RemoveAsync(id);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check id entity exists in DB
        /// </summary>
        /// <param name="id">Id to be checked</param>
        private async Task<bool> ProductExists(Guid id)
        {
            return (await _bll.ProductService.FindAsync(id)) != null;
        }

        /// <summary>
        /// Initialize select list
        /// </summary>
        /// <param name="vm">View model</param>
        /// <param name="businessId">Business ID</param>
        /// <param name="productCategoryId">ProductCategory ID</param>
        /// <returns></returns>
        private async Task<ProductCreateEditVM> InitializeSelectList(ProductCreateEditVM vm,
            Guid? businessId = null,
            Guid? productCategoryId = null)
        {
            if (businessId == null)
            {
                vm.BusinessSelectList = new SelectList(await _bll.BusinessService.AllAsync(),
                    nameof(vm.Product.Business.Id),
                    nameof(vm.Product.Business.Name));
            }
            else
            {
                vm.BusinessSelectList = new SelectList(await _bll.BusinessService.AllAsync(),
                    nameof(vm.Product.Business.Id),
                    nameof(vm.Product.Business.Name),
                    businessId.Value);
            }


            if (productCategoryId == null)
            {
                vm.ProductCategorySelectList = new SelectList(await _bll.ProductCategoryService.AllAsync(),
                    nameof(vm.Product.ProductCategory.Id),
                    nameof(vm.Product.ProductCategory.Title));
            }
            else
            {
                vm.ProductCategorySelectList = new SelectList(await _bll.ProductCategoryService.AllAsync(),
                    nameof(vm.Product.ProductCategory.Id),
                    nameof(vm.Product.ProductCategory.Title),
                    productCategoryId.Value);
            }

            return vm;
        }
    }
}