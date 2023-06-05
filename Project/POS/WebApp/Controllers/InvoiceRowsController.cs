using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public class InvoiceRowsController : Controller
    {
        /// <summary>
        /// Reference to the BLL service.
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// Controller constructor 
        /// </summary>
        /// <param name="bll"></param>
        public InvoiceRowsController( IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: InvoiceRows
        /// <summary>
        /// Get all controller records from DB
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var vm = await _bll.InvoiceRowService.AllAsyncWithInclude();
            return View(vm);
        }

        // GET: InvoiceRows/Details/5
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

            var element = await _bll.InvoiceRowService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // GET: InvoiceRows/Create
        /// <summary>
        /// Loads create view
        /// </summary>
        public async Task<IActionResult> Create()
        {
            var vm = new InvoiceRowCreateEditVM();

            vm = await InitializeSelectList(vm);
            return View(vm);
        }

        // POST: InvoiceRows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Loads create view to add new entity to DB
        /// </summary>
        /// <param name="vm">Record to be created</param>
        /// <returns>Index view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceRowCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _bll.InvoiceRowService.Add(vm.InvoiceRow);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm = await InitializeSelectList(vm, vm.InvoiceRow.InvoiceId, vm.InvoiceRow.ProductId);
            return View(vm);
        }

        // GET: InvoiceRows/Edit/5
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

            var element = await _bll.InvoiceRowService.FindAsync(id.Value);
            if (element == null)
            {
                return NotFound();
            }

            var vm = new InvoiceRowCreateEditVM()
            {
                InvoiceRow = element
            };

            vm = await InitializeSelectList(vm, vm.InvoiceRow.InvoiceId, vm.InvoiceRow.ProductId);
            return View(vm);
        }

        // POST: InvoiceRows/Edit/5
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
        public async Task<IActionResult> Edit(Guid id, InvoiceRowCreateEditVM vm)
        {
            if (id != vm.InvoiceRow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.InvoiceRowService.Update(vm.InvoiceRow);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await InvoiceRowExists(vm.InvoiceRow.Id)))
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

            vm = await InitializeSelectList(vm, vm.InvoiceRow.InvoiceId, vm.InvoiceRow.ProductId);
            return View(vm);
        }

        // GET: InvoiceRows/Delete/5
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

            var element = await _bll.InvoiceRowService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // POST: InvoiceRows/Delete/5
        /// <summary>
        /// Deletes specified record from db
        /// </summary>
        /// <param name="id">ID to be deleted</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await InvoiceRowExists(id))
            {
                await _bll.InvoiceRowService.RemoveAsync(id);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check id entity exists in DB
        /// </summary>
        /// <param name="id">Id to be checked</param>
        private async Task<bool> InvoiceRowExists(Guid id)
        {
            return (await _bll.InvoiceRowService.FindAsync(id)) != null;
        }

        /// <summary>
        /// Initialize select list
        /// </summary>
        /// <param name="vm">View model</param>
        /// <param name="invoiceId">Invoice ID</param>
        /// <param name="productId">Product ID</param>
        /// <returns></returns>
        private async Task<InvoiceRowCreateEditVM> InitializeSelectList(InvoiceRowCreateEditVM vm,
            Guid? invoiceId = null,
            Guid? productId = null)
        {
            if (invoiceId == null)
            {
                vm.InvoiceSelectList = new SelectList(await _bll.InvoiceService.AllAsync(),
                    nameof(vm.InvoiceRow.Invoice.Id),
                    nameof(vm.InvoiceRow.Invoice.Id));
            }
            else
            {
                vm.InvoiceSelectList = new SelectList(await _bll.InvoiceService.AllAsync(),
                    nameof(vm.InvoiceRow.Invoice.Id),
                    nameof(vm.InvoiceRow.Invoice.Id),
                    invoiceId.Value);
            }


            if (productId == null)
            {
                vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(),
                    nameof(vm.InvoiceRow.Product.Id),
                    nameof(vm.InvoiceRow.Product.Name));
            }
            else
            {
                vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(),
                    nameof(vm.InvoiceRow.Product.Id),
                    nameof(vm.InvoiceRow.Product.Name),
                    productId.Value);
            }

            return vm;
        }
    }
}