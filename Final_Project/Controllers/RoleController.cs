using Final_Project.Models;
using Final_Project.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class RoleController : Controller
    {
        IRoleRepository RoleRepo;

        public RoleController(IRoleRepository roleRepo)
        {
            RoleRepo = roleRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Role> roleList = RoleRepo.GetAll();
            return View("Index", roleList);
        }


        public IActionResult New()
        {
            return View("New");
        }

        [HttpPost]
        public IActionResult SaveNew(Role roleFromReq)
        {
            if (ModelState.IsValid)
            {
                RoleRepo.Add(roleFromReq);
                RoleRepo.Save();
                return RedirectToAction("Index", "Role");
            }
            return View("New", roleFromReq);
        }



        public IActionResult Details(int id)
        {
            Role role = RoleRepo.GetById(id);
            if (role == null)
                return NotFound();

            return View("Details", role);
        }
     
        public IActionResult Edit(int id)
        {
            Role role = RoleRepo.GetById(id);
            if (role == null)
                return NotFound();

            return View("Edit", role);
        }

        [HttpPost]
        public IActionResult SaveEdit(int id, Role roleFromReq)
        {
            if (id != roleFromReq.RoleID)
                return BadRequest();

            if (ModelState.IsValid)
            {
                RoleRepo.Update(roleFromReq);
                RoleRepo.Save();
                return RedirectToAction("Index");
            }
            return View("Edit", roleFromReq);
        }
       

        public IActionResult Delete(int id)
        {
            Role role = RoleRepo.GetById(id);
            if (role == null)
                return NotFound();

            return View("Delete", role);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            RoleRepo.Delete(id);
            RoleRepo.Save();
            return RedirectToAction("Index");
        }
      
    }
}