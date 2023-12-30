using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using pp.DataAccess.Data;
using pp.DataAccess.Repository.IRepository;
using pp.Models;
using pp.Models.ViewModels;
using pp.Utility;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)] //without authorization user can access admin pages using url
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            //list of categories using product model and storing in objProductList list
            List<Product> objProductList = _UnitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _UnitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.Id.ToString()
                }),
                Product = new Product()

            };
            if (id == null || id == 0) {
                //create
                return View(productVM);
            }
            else
            {
                //Update
                productVM.Product = _UnitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }

        }


        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;  //this path will give path of wwwroot folder
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);  //final image name
                    string productPath = Path.Combine(wwwRootPath, @"images\product");               //final location for saving


                    //for updating existing image 
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    _UnitOfWork.Product.Add(productVM.Product); //this will keep track of all objectes needed to add to db
                }
                else
                {
                    _UnitOfWork.Product.Update(productVM.Product);
                }

                _UnitOfWork.Save();  //using saveChanges we will actually save all objects to db
                TempData["success"] = "Product created successfully";
                //TempData["success"] = "Product {"@obj.ProductName"} created successfully";
                return RedirectToAction("Index");
            }
            else
            {

                productVM.CategoryList = _UnitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }


        ////Delete get and post
        //public IActionResult DeleteProduct(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Product productFromDb = _UnitOfWork.Product.Get(u => u.Id == id);
        //    if (productFromDb == null)
        //    {
        //        return NotFound(productFromDb);
        //    }

        //    return View(productFromDb);
        //}



        //[HttpPost, ActionName("DeleteProduct")]  //adding action nane to avaoid function overloading because of same parameters
        //public IActionResult DeleteProductPOST(int? id)
        //{
        //    Product? obj = _UnitOfWork.Product.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }

        //    _UnitOfWork.Product.Remove(obj); //this will remove objectes from db
        //    _UnitOfWork.Save();  //using saveChanges we will actually save all objects to db
        //    TempData["success"] = "Product Deleted successfully";
        //    return RedirectToAction("Index");
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _UnitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _UnitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new {success=false, message="Error while deleting"});
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                                            productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _UnitOfWork.Product.Remove(productToBeDeleted);
            _UnitOfWork.Save();

            return Json(new { success = true, message = "Deleted Successfully " });
        }
        #endregion
    }
}
