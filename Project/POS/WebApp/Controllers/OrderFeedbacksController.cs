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
    public class OrderFeedbacksController : Controller
    {
        /// <summary>
        /// Reference to the BLL service.
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// Controller constructor 
        /// </summary>
        /// <param name="bll"></param>
        public OrderFeedbacksController( IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: OrderFeedbacks
        /// <summary>
        /// Get all controller records from DB
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var vm = await _bll.OrderFeedbackService.AllAsyncWithInclude();
            return View(vm);
        }

        // GET: OrderFeedbacks/Details/5
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

            var element = await _bll.OrderFeedbackService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // GET: OrderFeedbacks/Create
        /// <summary>
        /// Loads create view
        /// </summary>
        public async Task<IActionResult> Create()
        {
            var vm = new OrderFeedbackCreateEditVM();

            vm = await InitializeSelectList(vm);
            return View(vm);
        }

        // POST: OrderFeedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Loads create view to add new entity to DB
        /// </summary>
        /// <param name="vm">Record to be created</param>
        /// <returns>Index view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderFeedbackCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _bll.OrderFeedbackService.Add(vm.OrderFeedback);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm = await InitializeSelectList(vm, vm.OrderFeedback.OrderId);

            return View(vm);
        }

        // GET: OrderFeedbacks/Edit/5
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

            var element = await _bll.OrderFeedbackService.FindAsync(id.Value);
            if (element == null)
            {
                return NotFound();
            }

            var vm = new OrderFeedbackCreateEditVM()
            {
                OrderFeedback = element
            };

            vm = await InitializeSelectList(vm, vm.OrderFeedback.OrderId);

            return View(vm);
        }

        // POST: OrderFeedbacks/Edit/5
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
        public async Task<IActionResult> Edit(Guid id, OrderFeedbackCreateEditVM vm)
        {
            if (id != vm.OrderFeedback.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.OrderFeedbackService.Update(vm.OrderFeedback);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await OrderFeedbackExists(vm.OrderFeedback.Id)))
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

            vm = await InitializeSelectList(vm, vm.OrderFeedback.OrderId);
            return View(vm);
        }

        // GET: OrderFeedbacks/Delete/5
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

            var element = await _bll.OrderFeedbackService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // POST: OrderFeedbacks/Delete/5
        /// <summary>
        /// Deletes specified record from db
        /// </summary>
        /// <param name="id">ID to be deleted</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await OrderFeedbackExists(id))
            {
                await _bll.OrderFeedbackService.RemoveAsync(id);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check id entity exists in DB
        /// </summary>
        /// <param name="id">Id to be checked</param>
        private async Task<bool> OrderFeedbackExists(Guid id)
        {
            return (await _bll.OrderFeedbackService.FindAsync(id)) != null;
        }


        /// <summary>
        /// Initialize select list
        /// </summary>
        /// <param name="vm">View model</param>
        /// <param name="orderId">Order ID</param>
        /// <returns></returns>
        private async Task<OrderFeedbackCreateEditVM> InitializeSelectList(OrderFeedbackCreateEditVM vm,
            Guid? orderId = null)
        {
            if (orderId == null)
            {
                vm.OrderSelectList = new SelectList(await _bll.OrderService.AllAsync(),
                    nameof(vm.OrderFeedback.Order.Id),
                    nameof(vm.OrderFeedback.Order.Id));
            }
            else
            {
                vm.OrderSelectList = new SelectList(await _bll.OrderService.AllAsync(),
                    nameof(vm.OrderFeedback.Order.Id),
                    nameof(vm.OrderFeedback.Order.Id),
                    orderId.Value);
            }

            return vm;
        }
    }
}