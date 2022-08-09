using BulkyBook.DataAccess.Repositories.IRepositories;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class CompanyController : Controller
{

    private readonly IUnitOfWork _unitOfWork;

    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        return View();
    }
    //GET
    public IActionResult Details(int? id)
    {

        if (id == null || id == 0)
        {
            return NotFound();
        }
        var Company = _unitOfWork.Company.Get(id);
        if (Company == null)
        {
            return NotFound();
        }

        return View(Company);
    }


    //GET
    public IActionResult Upsert(int? id)
    {
        Company company = new();
        if (id == null || id == 0)
        {
            // Create Company
            return View(company);
        }

        // Update Preoduct
        company = _unitOfWork.Company.Get(id);
        return View(company);


    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Company company)
    {
        if (ModelState.IsValid)
        {
            if (company.Id == 0)
            {
                _unitOfWork.Company.Add(company);
                TempData["success"] = "Company created successfully";
            }
            else
            {
                _unitOfWork.Company.Update(company);
                TempData["success"] = "Company updated successfully";
            }
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
        return View(company);
    }

    #region API Calls
    [HttpGet]
    public IActionResult GetAll()
    {
        var companyList = _unitOfWork.Company.GetAll();
        return Json(new { data = companyList });
    }

    //POST
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var company = _unitOfWork.Company.Get(id);
        if (company == null)
        {
            return Json(new { success = false, message = "Error While Deleting" });
        }
        _unitOfWork.Company.Remove(company);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Delete Succesful" });
    }
    #endregion
}

