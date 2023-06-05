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
    public class BusinessPicturesController : Controller
    {
        /// <summary>
        /// Reference to the BLL service.
        /// </summary>
        private readonly IAppBLL _bll;

        /// <summary>
        /// Controller constructor 
        /// </summary>
        /// <param name="bll"></param>
        public BusinessPicturesController( IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: BusinessPictures
        /// <summary>
        /// Get all controller records from DB
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var vm = await _bll.BusinessPictureService.AllAsyncWithInclude();
            return View(vm);
        }

        // GET: BusinessPictures/Details/5
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

            var element = await _bll.BusinessPictureService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // GET: BusinessPictures/Create
        /// <summary>
        /// Loads create view
        /// </summary>
        public async Task<IActionResult> Create()
        {
            var vm = new BusinessPictureCreateEditVM();

            vm = await InitializeSelectList(vm);
            return View(vm);
        }

        // POST: BusinessPictures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Loads create view to add new entity to DB
        /// </summary>
        /// <param name="vm">Record to be created</param>
        /// <returns>Index view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BusinessPictureCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _bll.BusinessPictureService.Add(vm.BusinessPicture);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm = await InitializeSelectList(vm, vm.BusinessPicture.BusinessId, vm.BusinessPicture.PictureId);
            return View(vm);
        }

        // GET: BusinessPictures/Edit/5
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

            var element = await _bll.BusinessPictureService.FindAsync(id.Value);
            if (element == null)
            {
                return NotFound();
            }

            var vm = new BusinessPictureCreateEditVM
            {
                BusinessPicture = element
            };

            vm = await InitializeSelectList(vm, vm.BusinessPicture.BusinessId, vm.BusinessPicture.PictureId);

            return View(vm);
        }

        // POST: BusinessPictures/Edit/5
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
        public async Task<IActionResult> Edit(Guid id, BusinessPictureCreateEditVM vm)
        {
            if (id != vm.BusinessPicture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.BusinessPictureService.Update(vm.BusinessPicture);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await BusinessPictureExists(vm.BusinessPicture.Id)))
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

            vm = await InitializeSelectList(vm, vm.BusinessPicture.BusinessId, vm.BusinessPicture.PictureId);
            return View(vm);
        }

        // GET: BusinessPictures/Delete/5
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

            var element = await _bll.BusinessPictureService.FindAsyncWithInclude(id.Value);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // POST: BusinessPictures/Delete/5
        /// <summary>
        /// Deletes specified record from db
        /// </summary>
        /// <param name="id">ID to be deleted</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await BusinessPictureExists(id))
            {
                await _bll.BusinessPictureService.RemoveAsync(id);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check id entity exists in DB
        /// </summary>
        /// <param name="id">Id to be checked</param>
        private async Task<bool> BusinessPictureExists(Guid id)
        {
            return (await _bll.BusinessPictureService.FindAsync(id)) != null;
        }

        /// <summary>
        /// Initializes select list
        /// </summary>
        /// <param name="vm">View model</param>
        /// <param name="businessId">Business ID</param>
        /// <param name="pictureId">Picture ID</param>
        /// <returns></returns>
        private async Task<BusinessPictureCreateEditVM> InitializeSelectList(BusinessPictureCreateEditVM vm,
            Guid? businessId = null,
            Guid? pictureId = null)
        {
            if (businessId == null)
            {
                vm.BusinessSelectList = new SelectList(await _bll.BusinessService.AllAsync(),
                    nameof(vm.BusinessPicture.Business.Id),
                    nameof(vm.BusinessPicture.Business.Name));
            }
            else
            {
                vm.BusinessSelectList = new SelectList(await _bll.BusinessService.AllAsync(),
                    nameof(vm.BusinessPicture.Business.Id),
                    nameof(vm.BusinessPicture.Business.Name),
                    businessId.Value);
            }


            if (pictureId == null)
            {
                vm.PictureSelectList = new SelectList(await _bll.PictureService.AllAsync(),
                    nameof(vm.BusinessPicture.Picture.Id),
                    nameof(vm.BusinessPicture.Picture.Title));
            }
            else
            {
                vm.PictureSelectList = new SelectList(await _bll.PictureService.AllAsync(),
                    nameof(vm.BusinessPicture.Picture.Id),
                    nameof(vm.BusinessPicture.Picture.Title),
                    pictureId.Value);
            }

            return vm;
        }
    }
}