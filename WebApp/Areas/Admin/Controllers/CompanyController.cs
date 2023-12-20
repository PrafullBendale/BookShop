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
   // [Authorize(Roles = SD.Role_Admin)] //without authorization user can access admin pages using url
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //list of categories using Company model and storing in objCompanyList list
            List<Company> objCompanyList = _UnitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            
            if (id == null || id == 0) {
                //create
                return View(new Company());
            }
            else
            {
                //Update
                Company companyObj = _UnitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }

        }


        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {
                if (companyObj.Id == 0)
                {
                    _UnitOfWork.Company.Add(companyObj); //this will keep track of all objectes needed to add to db
                }
                else
                {
                    _UnitOfWork.Company.Update(companyObj);
                }

                _UnitOfWork.Save();  //using saveChanges we will actually save all objects to db
                TempData["success"] = "Company created successfully";
                //TempData["success"] = "Company {"@obj.CompanyName"} created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(companyObj);
            }
        }


        ////Delete get and post
        //public IActionResult DeleteCompany(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Company CompanyFromDb = _UnitOfWork.Company.Get(u => u.Id == id);
        //    if (CompanyFromDb == null)
        //    {
        //        return NotFound(CompanyFromDb);
        //    }

        //    return View(CompanyFromDb);
        //}



        //[HttpPost, ActionName("DeleteCompany")]  //adding action nane to avaoid function overloading because of same parameters
        //public IActionResult DeleteCompanyPOST(int? id)
        //{
        //    Company? obj = _UnitOfWork.Company.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }

        //    _UnitOfWork.Company.Remove(obj); //this will remove objectes from db
        //    _UnitOfWork.Save();  //using saveChanges we will actually save all objects to db
        //    TempData["success"] = "Company Deleted successfully";
        //    return RedirectToAction("Index");
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _UnitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _UnitOfWork.Company.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new {success=false, message="Error while deleting"});
            }

            _UnitOfWork.Company.Remove(CompanyToBeDeleted);
            _UnitOfWork.Save();

            return Json(new { success = true, message = "Deleted Successfully " });
        }
        #endregion
    }
}
