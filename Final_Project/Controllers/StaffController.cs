using Final_Project.Models;
using Final_Project.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class StaffController : Controller
    {
        IStaffRepository StaffRepo;
        IBranchRepository BranchRepo;
        IRoleRepository RoleRepo;

        public StaffController(IStaffRepository staffRepo, IBranchRepository branchRepo, IRoleRepository roleRepo)
        {
            StaffRepo = staffRepo;
            BranchRepo = branchRepo;
            RoleRepo = roleRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Staff> staffList = StaffRepo.GetAll();
            return View("Index", staffList);
        }


        public IActionResult New()
        {
            ViewBag.Branches = BranchRepo.GetAll();
            ViewBag.Roles = RoleRepo.GetAll();
            return View("New");
        }

        [HttpPost]
        public IActionResult SaveNew(Staff staffFromReq)
        {
            if (ModelState.IsValid)
            {
                StaffRepo.Add(staffFromReq);
                StaffRepo.Save();
                return RedirectToAction("Index", "Staff");
            }
            ViewBag.Branches = BranchRepo.GetAll();
            ViewBag.Roles = RoleRepo.GetAll();
            return View("New", staffFromReq);
        }



        public IActionResult Details(int id)
        {
            Staff staff = StaffRepo.GetById(id);
            if (staff == null)
                return NotFound();

            return View("Details", staff);
        }



        public IActionResult Edit(int id)
        {
            Staff staff = StaffRepo.GetById(id);
            if (staff == null)
                return NotFound();

            ViewBag.Branches = BranchRepo.GetAll();
            ViewBag.Roles = RoleRepo.GetAll();
            return View("Edit", staff);
        }

        [HttpPost]
        public IActionResult SaveEdit(int id, Staff staffFromReq)
        {
            if (id != staffFromReq.StaffID)
                return BadRequest();

            if (ModelState.IsValid)
            {
                StaffRepo.Update(staffFromReq);
                StaffRepo.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Branches = BranchRepo.GetAll();
            ViewBag.Roles = RoleRepo.GetAll();
            return View("Edit", staffFromReq);
        }



        public IActionResult Delete(int id)
        {
            Staff staff = StaffRepo.GetById(id);
            if (staff == null)
                return NotFound();

            return View("Delete", staff);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            StaffRepo.Delete(id);
            StaffRepo.Save();
            return RedirectToAction("Index");
        }

    }
}