using Final_Project.Models;
using Final_Project.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class BranchController : Controller
    {
        IBranchRepository branchRepo;

        public BranchController(IBranchRepository _branchRepo)
        {
            branchRepo = _branchRepo;
        }

        public IActionResult Index()
        {
            List<Branch> BranchList = branchRepo.GetAll();
            return View("Index", BranchList);
        }

        public IActionResult Details(int id)
        {
            Branch Branch = branchRepo.GetById(id);
            if (Branch == null)
            {
                return NotFound();
            }
            return View("Details", Branch);
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Branch branch)
        {
            if (ModelState.IsValid == true)
            {
                branchRepo.Add(branch);
                branchRepo.Save();
                return RedirectToAction("Index");
            }
            return View("Create", branch);
        }

        public IActionResult Edit(int id)
        {
            Branch Branch = branchRepo.GetById(id);
            if (Branch == null)
            {
                return NotFound();
            }
            return View("Edit", Branch);
        }

        [HttpPost]
        public IActionResult Edit(Branch branch)
        {
            if (ModelState.IsValid == true)
            {
                branchRepo.Update(branch);
                branchRepo.Save();
                return RedirectToAction("Index");
            }
            return View("Edit", branch);
        }

        public IActionResult Delete(int id)
        {
            Branch Branch = branchRepo.GetById(id);
            if (Branch == null)
            {
                return NotFound();
            }
            return View("Delete", Branch);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            branchRepo.Delete(id);
            branchRepo.Save();
            return RedirectToAction("Index");
        }
    }
}