using BLL.Contracts.App;
using DAL.Contracts.App;
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
    public class BusinessesController : Controller
    {
        /// <summary>
        /// Reference to the BLL service
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="bll">References to BLL</param>
        public BusinessesController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Businesses
        /// <summary>
        /// Get all controller records from DB
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var vm = await _bll.BusinessService.AllAsyncWithInclude();
            return View(vm);
        }

        // GET: Businesses/Details/5
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

            var element = await _bll.BusinessService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // GET: Businesses/Create
        /// <summary>
        /// Loads create view
        /// </summary>
        public async Task<IActionResult> Create()
        {
            var vm = new BusinessCreateEditVM();

            vm = await InitializeSelectList(vm);
            return View(vm);
        }

        // POST: Businesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Loads create view to add new entity to DB
        /// </summary>
        /// <param name="vm">Record to be created</param>
        /// <returns>Index view</returns>
        [HttpPost]
        public async Task<IActionResult> Create(BusinessCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _bll.BusinessService.Add(vm.Business);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm = await InitializeSelectList(vm, vm.Business.BusinessCategoryId, vm.Business.SettlementId);

            return View(vm);
        }

        // GET: Businesses/Edit/5
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

            var element = await _bll.BusinessService.FindAsync(id.Value);
            if (element == null)
            {
                return NotFound();
            }

            var vm = new BusinessCreateEditVM
            {
                Business = element
            };

            vm = await InitializeSelectList(vm, vm.Business.BusinessCategoryId, vm.Business.SettlementId);

            return View(vm);
        }

        // POST: Businesses/Edit/5
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
        public async Task<IActionResult> Edit(Guid id, BusinessCreateEditVM vm)
        {
            if (id != vm.Business.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.BusinessService.Update(vm.Business);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await BusinessExists(vm.Business.Id)))
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

            vm = await InitializeSelectList(vm, vm.Business.BusinessCategoryId, vm.Business.SettlementId);
            return View(vm);
        }

        // GET: Businesses/Delete/5
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

            var element = await _bll.BusinessService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // POST: Businesses/Delete/5
        /// <summary>
        /// Deletes specified record from db
        /// </summary>
        /// <param name="id">ID to be deleted</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await BusinessExists(id))
            {
                await _bll.BusinessService.RemoveAsync(id);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check id entity exists in DB
        /// </summary>
        /// <param name="id">Id to be checked</param>
        private async Task<bool> BusinessExists(Guid id)
        {
            return (await _bll.BusinessService.FindAsync(id)) != null;
        }

        /// <summary>
        /// Initializes select list
        /// </summary>
        /// <param name="vm">View model element</param>
        /// <param name="businessCategoryId">BusinessCategories ID</param>
        /// <param name="settlementId">Settlement Id</param>
        /// <returns></returns>
        private async Task<BusinessCreateEditVM> InitializeSelectList(BusinessCreateEditVM vm,
            Guid? businessCategoryId = null,
            Guid? settlementId = null)
        {
            if (businessCategoryId == null)
            {
                vm.BusinessCategorySelectList = new SelectList(await _bll.BusinessCategoryService.AllAsync(),
                    nameof(vm.Business.BusinessCategory.Id),
                    nameof(vm.Business.BusinessCategory.Title));
            }
            else
            {
                vm.BusinessCategorySelectList = new SelectList(await _bll.BusinessCategoryService.AllAsync(),
                    nameof(vm.Business.BusinessCategory.Id),
                    nameof(vm.Business.BusinessCategory.Title),
                    businessCategoryId.Value);
            }


            if (settlementId == null)
            {
                vm.SettlementSelectList = new SelectList(await _bll.SettlementService.AllAsync(),
                    nameof(vm.Business.Settlement.Id),
                    nameof(vm.Business.Settlement.Name));
            }
            else
            {
                vm.SettlementSelectList = new SelectList(await _bll.SettlementService.AllAsync(),
                    nameof(vm.Business.Settlement.Id),
                    nameof(vm.Business.Settlement.Name),
                    settlementId.Value);
            }

            return vm;
        }
    }
}