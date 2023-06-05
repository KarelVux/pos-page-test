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
    public class ProductPicturesController : Controller
    {
        /// <summary>
        /// Reference to the BLL service.
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// Controller constructor 
        /// </summary>
        /// <param name="bll"></param>
        public ProductPicturesController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: ProductPictures
        /// <summary>
        /// Get all controller records from DB
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var vm = await _bll.ProductPictureService.AllAsyncWithInclude();
            return View(vm);
        }

        // GET: ProductPictures/Details/5
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

            var element = await _bll.ProductPictureService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // GET: ProductPictures/Create
        /// <summary>
        /// Loads create view
        /// </summary>
        public async Task<IActionResult> Create()
        {
            var vm = new ProductPictureCreateEditVM();

            vm = await InitializeSelectList(vm);
            return View(vm);
        }

        // POST: ProductPictures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Loads create view to add new entity to DB
        /// </summary>
        /// <param name="vm">Record to be created</param>
        /// <returns>Index view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductPictureCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _bll.ProductPictureService.Add(vm.ProductPicture);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm = await InitializeSelectList(vm, vm.ProductPicture.PictureId, vm.ProductPicture.ProductId);

            return View(vm);
        }

        // GET: ProductPictures/Edit/5
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

            var element = await _bll.ProductPictureService.FindAsync(id.Value);
            if (element == null)
            {
                return NotFound();
            }

            var vm = new ProductPictureCreateEditVM()
            {
                ProductPicture = element
            };

            vm = await InitializeSelectList(vm, vm.ProductPicture.PictureId, vm.ProductPicture.ProductId);

            return View(vm);
        }

        // POST: ProductPictures/Edit/5
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
        public async Task<IActionResult> Edit(Guid id, ProductPictureCreateEditVM vm)
        {
            if (id != vm.ProductPicture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.ProductPictureService.Update(vm.ProductPicture);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await ProductPictureExists(vm.ProductPicture.Id)))
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

            vm = await InitializeSelectList(vm, vm.ProductPicture.PictureId, vm.ProductPicture.ProductId);
            return View(vm);
        }

        // GET: ProductPictures/Delete/5
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

            var element = await _bll.ProductPictureService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // POST: ProductPictures/Delete/5
        /// <summary>
        /// Deletes specified record from db
        /// </summary>
        /// <param name="id">ID to be deleted</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await ProductPictureExists(id))
            {
                await _bll.ProductPictureService.RemoveAsync(id);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// Check id entity exists in DB
        /// </summary>
        /// <param name="id">Id to be checked</param>
        private async Task<bool> ProductPictureExists(Guid id)
        {
            return (await _bll.ProductPictureService.FindAsync(id)) != null;
        }

        /// <summary>
        /// Initialize select list
        /// </summary>
        /// <param name="vm">View model</param>
        /// <param name="pictureId">Picture ID</param>
        /// <param name="productId">Product ID</param>
        /// <returns></returns>
        private async Task<ProductPictureCreateEditVM> InitializeSelectList(ProductPictureCreateEditVM vm,
            Guid? pictureId = null,
            Guid? productId = null)
        {
            if (pictureId == null)
            {
                vm.PictureSelectList = new SelectList(await _bll.PictureService.AllAsync(),
                    nameof(vm.ProductPicture.Picture.Id),
                    nameof(vm.ProductPicture.Picture.Title));
            }
            else
            {
                vm.PictureSelectList = new SelectList(await _bll.PictureService.AllAsync(),
                    nameof(vm.ProductPicture.Picture.Id),
                    nameof(vm.ProductPicture.Picture.Title),
                    pictureId.Value);
            }


            if (productId == null)
            {
                vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(),
                    nameof(vm.ProductPicture.Product.Id),
                    nameof(vm.ProductPicture.Product.Name));
            }
            else
            {
                vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(),
                    nameof(vm.ProductPicture.Product.Id),
                    nameof(vm.ProductPicture.Product.Name),
                    productId.Value);
            }

            return vm;
        }
    }
}