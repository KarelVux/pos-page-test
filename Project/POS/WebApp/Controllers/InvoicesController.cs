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
    ///
    [Authorize(Roles = "Root")]

    public class InvoicesController : Controller
    {
        /// <summary>
        /// Reference to the BLL service.
        /// </summary>
        private readonly IAppBLL _bll;

        private readonly UserManager<AppUser> _userManager;

        /// <summary>
        /// Controller constructor 
        /// </summary>
        /// <param name="userManager">USer manager</param>
        /// <param name="bll"></param>
        public InvoicesController(UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
        }

        // GET: Invoices
        /// <summary>
        /// Get all controller records from DB
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var vm = (await _bll.InvoiceService.AllAsyncWithInclude()).Select(x => new InvoiceDetails
            {
                Invoice = x,
                UserName = _userManager.Users.FirstOrDefault(u => u.Id == x.AppUserId)?.UserName
            });
            return View(vm);
        }

        // GET: Invoices/Details/5
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

            var element = await _bll.InvoiceService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }
            var returnable = new InvoiceDetails
            {
                Invoice = element,
                UserName = _userManager.Users.FirstOrDefault(u => u.Id == element.AppUserId)?.UserName
            };
            

            return View(returnable);
        }

        // GET: Invoices/Create

        /// <summary>
        /// Loads create view
        /// </summary>
        public async Task<IActionResult> Create()
        {
            var vm = new InvoiceCreateEditVM();

            vm = await InitializeSelectList(vm);
            return View(vm);
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Loads create view to add new entity to DB
        /// </summary>
        /// <param name="vm">Record to be created</param>
        /// <returns>Index view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _bll.InvoiceService.Add(vm.Invoice);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm = await InitializeSelectList(vm, vm.Invoice.AppUserId, vm.Invoice.BusinessId);
            return View(vm);
        }

        // GET: Invoices/Edit/5
        /// <summary>
        /// Loads edit view To edit DB records
        /// </summary>
        /// <param name="id">ID to be edited</param>
        /// 
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _bll.InvoiceService.FindAsync(id.Value);
            if (element == null)
            {
                return NotFound();
            }

            var vm = new InvoiceCreateEditVM()
            {
                Invoice = element
            };

            vm = await InitializeSelectList(vm, vm.Invoice.AppUserId, vm.Invoice.BusinessId);
            return View(vm);
        }

        // POST: Invoices/Edit/5
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
        public async Task<IActionResult> Edit(Guid id, InvoiceCreateEditVM vm)
        {
            if (id != vm.Invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.InvoiceService.Update(vm.Invoice);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await InvoiceExists(vm.Invoice.Id)))
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

            vm = await InitializeSelectList(vm, vm.Invoice.AppUserId, vm.Invoice.BusinessId);
            return View(vm);
        }

        // GET: Invoices/Delete/5
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

            var element = await _bll.InvoiceService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }
            
            var returnable = new InvoiceDetails
            {
                Invoice = element,
                UserName = _userManager.Users.FirstOrDefault(u => u.Id == element.AppUserId)?.UserName
            };

            return View(returnable);
        }

        // POST: Invoices/Delete/5
        /// <summary>
        /// Deletes specified record from db
        /// </summary>
        /// <param name="id">ID to be deleted</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await InvoiceExists(id))
            {
                await _bll.InvoiceService.RemoveAsync(id);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check id entity exists in DB
        /// </summary>
        /// <param name="id">Id to be checked</param>
        private async Task<bool> InvoiceExists(Guid id)
        {
            return (await _bll.InvoiceService.FindAsync(id)) != null;
        }


        private async Task<InvoiceCreateEditVM> InitializeSelectList(InvoiceCreateEditVM vm,
            Guid? appUserId = null,
            Guid? businessId = null)
        {
            if (appUserId == null)
            {
                vm.AppUserSelectList = new SelectList(_userManager.Users,
                    nameof(AppUser.Id),
                    nameof(AppUser.UserName));
            }
            else
            {
                vm.AppUserSelectList = new SelectList(_userManager.Users,
                    nameof(AppUser.Id),
                    nameof(AppUser.UserName),
                    appUserId.Value);
            }


            if (businessId == null)
            {
                vm.BusinessSelectList = new SelectList(await _bll.BusinessService.AllAsync(),
                    nameof(vm.Invoice.Business.Id),
                    nameof(vm.Invoice.Business.Name));
            }
            else
            {
                vm.BusinessSelectList = new SelectList(await _bll.BusinessService.AllAsync(),
                    nameof(vm.Invoice.Business.Id),
                    nameof(vm.Invoice.Business.Name),
                    businessId.Value);
            }

            return vm;
        }
    }
}