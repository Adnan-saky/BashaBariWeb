using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TBashaBari.Controllers;
using TBashaBari.Data;
using TBashaBari.Models;

namespace BashaBari.Controllers
{
    public class TenantViewsConnectedListController : Controller
    {
        List<TenantConnectsOwner> _tenantconnectsownerlist = new List<TenantConnectsOwner>();
        private readonly ApplicationDbContext _db;

        public TenantViewsConnectedListController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            FetchTenantConnectsOwner();
            return View(_tenantconnectsownerlist);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TenantConnectsOwner obj)
        {
            if (ModelState.IsValid)
            {
                _db.TenantConnectsOwner.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
            
        }

        //GET_EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.TenantConnectsOwner.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST_EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TenantConnectsOwner obj)
        {
            if (ModelState.IsValid)
            {
                _db.TenantConnectsOwner.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET_DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.TenantConnectsOwner.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST_DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(TenantConnectsOwner obj)
        {
            if (ModelState.IsValid)
            {
                _db.TenantConnectsOwner.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::://
        //:::::::::::::::::::::::::::::         custom methods        ::::::::::::::::::::::::::::::::://
        private void FetchTenantConnectsOwner()
        {
            //User.Identity.Name returns current logged in user's email
            string queryString = "SELECT TOP 1000 [ConnectionId],[TenantEmail],[OwnerEmail],[IsConfirmed] " +
                                        "FROM [BashaBariWeb].[dbo].[TenantConnectsOwner] " +
                                        "WHERE [TenantEmail] = '" + User.Identity.Name + "' " +
                                        "ORDER BY [ConnectionId] DESC";
            //to clear the list initially
            if (_tenantconnectsownerlist.Count > 0)
            {
                _tenantconnectsownerlist.Clear();
            }

            //database operation
            DatabaseConnection obj = new DatabaseConnection();
            obj.DbConnect();
            while (obj.ExeQuery(queryString).Read())
            {
                _tenantconnectsownerlist.Add(new TenantConnectsOwner
                {
                    ConnectionId = int.Parse(obj.ExeQuery(queryString)["ConnectionId"].ToString()),
                    TenantEmail = obj.ExeQuery(queryString)["TenantEmail"].ToString(),
                    OwnerEmail = obj.ExeQuery(queryString)["OwnerEmail"].ToString(),
                    IsConfirmed = obj.ExeQuery(queryString)["IsConfirmed"].ToString(),
                });
            }
            obj.CloseDbConnect();
        }


        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::://

    }
}
