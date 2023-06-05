using BLL.Contracts.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{
    /// <summary>
    ///   Admin view controller
    /// </summary>
    [Authorize(Roles = "Root")]
    public class ProductCategoriesController : Controller
    {
        /// <summary>
        /// Reference to the BLL service.
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// Controller constructor 
        /// </summary>
        /// <param name="bll"></param>
        public ProductCategoriesController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: ProductCategories
        /// <summary>
        /// Get all controller records from DB
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var vm = await _bll.ProductCategoryService.AllAsync();
            return View(vm);
        }

        // GET: ProductCategories/Details/5
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

            var element = await _bll.ProductCategoryService.FindAsync(id.Value);
            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // GET: ProductCategories/Create
        /// <summary>
        /// Loads create view
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Loads create view to add new entity to DB
        /// </summary>
        /// <param name="element">Record to be created</param>
        /// <returns>Index view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] BLL.DTO.ProductCategory element)
        {
            if (ModelState.IsValid)
            {
                _bll.ProductCategoryService.Add(element);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(element);
        }

        // GET: ProductCategories/Edit/5
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

            var element = await _bll.ProductCategoryService.FindAsync(id.Value);
            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Saves edited values if everything is okay
        /// </summary>
        /// <param name="id">Id to be edited</param>
        /// <param name="element">Edited object</param>
        /// <returns>Index view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, BLL.DTO.ProductCategory element)
        {
            if (id != element.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.ProductCategoryService.Update(element);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await ProductCategoryExists(element.Id)))
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

            return View(element);
        }

        // GET: ProductCategories/Delete/5
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

            var element = await _bll.ProductCategoryService.FindAsync(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // POST: ProductCategories/Delete/5
        /// <summary>
        /// Deletes specified record from db
        /// </summary>
        /// <param name="id">ID to be deleted</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await ProductCategoryExists(id))
            {
                await _bll.ProductCategoryService.RemoveAsync(id);
            }

            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check id entity exists in DB
        /// </summary>
        /// <param name="id">Id to be checked</param>
        private async Task<bool> ProductCategoryExists(Guid id)
        {
            return (await _bll.ProductCategoryService.FindAsync(id)) != null;
        }
    }
}