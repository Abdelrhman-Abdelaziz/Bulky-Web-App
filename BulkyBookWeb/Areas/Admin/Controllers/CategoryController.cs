using BulkyBook.DataAccess.Repositories.IRepositories;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class CategoryController : Controller
{

    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
        return View(objCategoryList);
    }
    //GET
    public IActionResult Details(int? id)
    {

        if (id == null || id == 0)
        {
            return NotFound();
        }
        var category = _unitOfWork.Category.Get((int)id);
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
    public IActionResult Create(Category category)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(category);
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        return View(category);
    }

    //GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var category = _unitOfWork.Category.Get((int)id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category category)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();
            TempData["success"] = "Category updated successfully";
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
        var category = _unitOfWork.Category.Get((int)id);
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
        var category = _unitOfWork.Category.Get(id);
        if (category == null)
        {
            return NotFound();
        }

        _unitOfWork.Category.Remove(category);
        _unitOfWork.Save();
        TempData["success"] = "Category deleted successfully";
        return RedirectToAction("Index");
    }
}
