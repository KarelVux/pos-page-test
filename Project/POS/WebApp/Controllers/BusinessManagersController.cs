using BLL.Contracts.App;
using DAL.Contracts.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.App.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Models.Admin;

namespace WebApp.Controllers
{
    /// <summary>
    ///   Admin view controller
    /// </summary>
    [Authorize(Roles = "Root")]
    public class BusinessManagersController : Controller
    {
        /// <summary>
        /// Reference to the BLL service.
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// User manager instance
        /// </summary>
        private readonly UserManager<AppUser> _userManager;


        /// <summary>
        /// Controller constructor 
        /// </summary>
        /// <param name="userManager">User manager </param>
        /// <param name="bll"></param>
        public BusinessManagersController( UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
        }

        // GET: BusinessManagers
        // GET: Settlements
        /// <summary>
        /// Get all controller records from DB
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var vm = (await _bll.BusinessManagerService.AllAsyncWithInclude())
                .Select(b => new BusinessManagerDetails
                {
                    BusinessManager = b,
                    UserName = _userManager.Users.FirstOrDefault(u => u.Id == b.AppUserId)?.UserName
                });


            return View(vm);
        }

        // GET: BusinessManagers/Details/5
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

            var element = await _bll.BusinessManagerService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            var returnable = new BusinessManagerDetails
            {
                BusinessManager = element,
                UserName = _userManager.Users.FirstOrDefault(u => u.Id == element.AppUserId)?.UserName
            };

            return View(returnable);
        }

        // GET: BusinessManagers/Create
        /// <summary>
        /// Loads create view
        /// </summary>
        public async Task<IActionResult> Create()
        {
            var vm = new BusinessManagerCreateEditVM();

            vm = await InitializeSelectList(vm);
            return View(vm);
        }

        // POST: BusinessManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Loads create view to add new entity to DB
        /// </summary>
        /// <param name="vm">Record to be created</param>
        /// <returns>Index view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BusinessManagerCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _bll.BusinessManagerService.Add(vm.BusinessManager);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm = await InitializeSelectList(vm, vm.BusinessManager!.AppUserId, vm.BusinessManager.BusinessId);
            return View(vm);
        }

        // GET: BusinessManagers/Edit/5
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

            var element = await _bll.BusinessManagerService.FindAsync(id.Value);
            if (element == null)
            {
                return NotFound();
            }

            var vm = new BusinessManagerCreateEditVM()
            {
                BusinessManager = element
            };

            vm = await InitializeSelectList(vm, vm.BusinessManager.AppUserId, vm.BusinessManager.BusinessId);
            return View(vm);
        }

        // POST: BusinessManagers/Edit/5
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
        public async Task<IActionResult> Edit(Guid id, BusinessManagerCreateEditVM vm)
        {
            if (id != vm.BusinessManager.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.BusinessManagerService.Update(vm.BusinessManager);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await BusinessManagerExists(vm.BusinessManager.Id)))
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

            vm = await InitializeSelectList(vm, vm.BusinessManager.AppUserId, vm.BusinessManager.BusinessId);
            return View(vm);
        }

        // GET: BusinessManagers/Delete/5
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

            var element = await _bll.BusinessManagerService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            var returnable = new BusinessManagerDetails
            {
                BusinessManager = element,
                UserName = _userManager.Users.FirstOrDefault(u => u.Id == element.AppUserId)?.UserName
            };

            return View(returnable);
        }

        // POST: BusinessManagers/Delete/5
        /// <summary>
        /// Deletes specified record from db
        /// </summary>
        /// <param name="id">ID to be deleted</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await BusinessManagerExists(id))
            {
                await _bll.BusinessManagerService.RemoveAsync(id);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check id entity exists in DB
        /// </summary>
        /// <param name="id">Id to be checked</param>
        private async Task<bool> BusinessManagerExists(Guid id)
        {
            return (await _bll.BusinessManagerService.FindAsync(id)) != null;
        }


        /// <summary>
        /// Initialize select list
        /// </summary>
        /// <param name="vm">View model</param>
        /// <param name="appUserId">AppUser ID</param>
        /// <param name="businessId">Business ID</param>
        /// <returns></returns>
        private async Task<BusinessManagerCreateEditVM> InitializeSelectList(BusinessManagerCreateEditVM vm,
            Guid? appUserId = null,
            Guid? businessId = null)
        {
            if (appUserId == null)
            {
                var users = _userManager.Users;
                vm.AppUserSelectList = new SelectList(_userManager.Users,
                    nameof(vm.AppUser.Id),
                    nameof(vm.AppUser.UserName));
            }
            else
            {
                vm.AppUserSelectList = new SelectList(_userManager.Users,
                    nameof(vm.AppUser.Id),
                    nameof(vm.AppUser.UserName),
                    appUserId.Value);
            }


            if (businessId == null)
            {
                vm.BusinessSelectList = new SelectList(await _bll.BusinessService.AllAsync(),
                    nameof(vm.BusinessManager.Business.Id),
                    nameof(vm.BusinessManager.Business.Name));
            }
            else
            {
                vm.BusinessSelectList = new SelectList(await _bll.BusinessService.AllAsync(),
                    nameof(vm.BusinessManager.Business.Id),
                    nameof(vm.BusinessManager.Business.Name),
                    businessId.Value);
            }

            return vm;
        }
    }
}