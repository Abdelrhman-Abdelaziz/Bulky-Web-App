using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repositories.IRepositories;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class CoverTypeController : Controller
{

    private readonly IUnitOfWork _unitOfWork;

    public CoverTypeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
        return View(objCoverTypeList);
    }
    //GET
    public IActionResult Details(int? id)
    {

        if (id == null || id == 0)
        {
            return NotFound();
        }
        var category = _unitOfWork.CoverType.Get((int)id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
    //GET
    public IActionResult Create()
    {
        return View();
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CoverType category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }
        _unitOfWork.CoverType.Add(category);
        _unitOfWork.Save();
        TempData["success"] = "CoverType created successfully";
        return RedirectToAction("Index");
    }

    //GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var category = _unitOfWork.CoverType.Get((int)id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CoverType category)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Update(category);
            _unitOfWork.Save();
            TempData["success"] = "CoverType updated successfully";
            return RedirectToAction("Index");
        }
        return View(category);
    }
    //GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var category = _unitOfWork.CoverType.Get((int)id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int id)
    {
        var category = _unitOfWork.CoverType.Get(id);
        if (category == null)
        {
            return NotFound();
        }

        _unitOfWork.CoverType.Remove(category);
        _unitOfWork.Save();
        TempData["success"] = "CoverType deleted successfully";
        return RedirectToAction("Index");
    }
}
