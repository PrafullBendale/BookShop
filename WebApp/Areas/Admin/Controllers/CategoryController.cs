using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pp.DataAccess.Data;
using pp.DataAccess.Repository.IRepository;
using pp.Models;
using pp.Utility;


namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)] //without authorization user can access admin pages using url
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //list of categories using category model and storing in objCategoryList list
            List<Category> objCategoryList = _UnitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult CreateNewCategory()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateNewCategory(Category obj)
        {
            //if (obj.CategoryName == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("CategoryName","Category name and display order cannot be same");
            //}
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Add(obj); //this will keep track of all objectes needed to add to db
                _UnitOfWork.Save();  //using saveChanges we will actually save all objects to db
                TempData["success"] = "Category created successfully";
                //TempData["success"] = "Category {"@obj.CategoryName"} created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }


        //Edit get and post
        public IActionResult EditCategory(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category categoryFromDb = _UnitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound(categoryFromDb);
            }

            return View(categoryFromDb);
        }


        [HttpPost]
        public IActionResult EditCategory(Category obj)
        {

            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Update(obj); //this will kupdate all objectes needed to updated to db
                _UnitOfWork.Save();  //using saveChanges we will actually save all objects to db
                TempData["success"] = "Category Edited successfully";
                return RedirectToAction("Index");
            }
            return View();
        }


        //Delete get and post
        public IActionResult DeleteCategory(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category categoryFromDb = _UnitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound(categoryFromDb);
            }

            return View(categoryFromDb);
        }



        [HttpPost, ActionName("DeleteCategory")]  //adding action nane to avaoid function overloading because of same parameters
        public IActionResult DeleteCategoryPOST(int? id)
        {
            Category? obj = _UnitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _UnitOfWork.Category.Remove(obj); //this will remove objectes from db
            _UnitOfWork.Save();  //using saveChanges we will actually save all objects to db
            TempData["success"] = "Category Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
