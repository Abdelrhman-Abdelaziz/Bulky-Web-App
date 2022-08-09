
using BulkyBook.DataAccess.Repositories.IRepositories;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class ProductController : Controller
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }
    public IActionResult Index()
    {
        return View();
    }
    //GET
    public IActionResult Upsert(int? id)
    {
        ProductVM productVM = new()
        {
            Product = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(cat => new SelectListItem { 
                Text = cat.Name,
                Value = cat.Id.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(cat => new SelectListItem
            {
                Text = cat.Name,
                Value = cat.Id.ToString()
            })
        };
        if (id == null || id == 0)
        {
            // Create Product
            return View(productVM);
        }
        else
        {
            // Update Preoduct
            productVM.Product = _unitOfWork.Product.Get(id);
            return View(productVM);
        }

        
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM ProductVM,IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if(file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                string uploads = Path.Combine(wwwRootPath, @"imags\products");
                string extension = Path.GetExtension(file.FileName);
                if(ProductVM.Product.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, ProductVM.Product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using(var fileStream = new FileStream(Path.Combine(uploads, fileName + extension),FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                ProductVM.Product.ImageUrl = @"\imags\products\" + fileName + extension;
            }
            if(ProductVM.Product.Id == 0)
            {
                _unitOfWork.Product.Add(ProductVM.Product);
                TempData["success"] = "Product created successfully";
            }
            else
            {
                _unitOfWork.Product.Update(ProductVM.Product);
                TempData["success"] = "Product updated successfully";
            }
            _unitOfWork.Save();
            
            return RedirectToAction("Index");
        }

        return View(ProductVM);
    }

    #region API Calls
    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
        return Json(new { data = productList });
    }

    //POST
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var Product = _unitOfWork.Product.Get(id);
        if (Product == null)
        {
            return Json(new { success = false, message = "Error While Deleting" });
        }

        var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, Product.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }
        _unitOfWork.Product.Remove(Product);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Delete Succesful" });
    }
    #endregion
}

