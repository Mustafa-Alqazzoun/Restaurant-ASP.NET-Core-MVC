using Final_Project.Models;
using Final_Project.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class TableController : Controller
    {
        ITableRepository TableRepo;
        IBranchRepository BranchRepo;

        public TableController(ITableRepository tableRepo, IBranchRepository branchRepo)
        {
            TableRepo = tableRepo;
            BranchRepo = branchRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Table> tableList = TableRepo.GetAll();
            return View("Index", tableList);
        }


        public IActionResult New()
        {
            ViewBag.Branches = BranchRepo.GetAll();
            return View("New");
        }

        [HttpPost]
        public IActionResult SaveNew(Table tableFromReq)
        {
            if (ModelState.IsValid)
            {
                TableRepo.Add(tableFromReq);
                TableRepo.Save();
                return RedirectToAction("Index", "Table");
            }
            ViewBag.Branches = BranchRepo.GetAll();
            return View("New", tableFromReq);
        }



        public IActionResult Details(int id)
        {
            Table table = TableRepo.GetById(id);
            if (table == null)
                return NotFound();

            return View("Details", table);
        }



        public IActionResult Edit(int id)
        {
            Table table = TableRepo.GetById(id);
            if (table == null)
                return NotFound();

            ViewBag.Branches = BranchRepo.GetAll();
            return View("Edit", table);
        }

        [HttpPost]
        public IActionResult SaveEdit(int id, Table tableFromReq)
        {
            if (id != tableFromReq.TableID)
                return BadRequest();

            if (ModelState.IsValid)
            {
                TableRepo.Update(tableFromReq);
                TableRepo.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Branches = BranchRepo.GetAll();
            return View("Edit", tableFromReq);
        }


        public IActionResult Delete(int id)
        {
            Table table = TableRepo.GetById(id);
            if (table == null)
                return NotFound();

            return View("Delete", table);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            TableRepo.Delete(id);
            TableRepo.Save();
            return RedirectToAction("Index");
        }

    }
}