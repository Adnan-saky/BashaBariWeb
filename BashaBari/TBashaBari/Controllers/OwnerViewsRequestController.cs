using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TBashaBari.Data;
using TBashaBari.Models;
using System.Data.SqlClient;
using TBashaBari.Controllers;

namespace BashaBari.Controllers
{
    public class OwnerViewsRequest : Controller
    {
        List<TenantRequest> _tenantrequestlist = new List<TenantRequest>();

        private readonly ApplicationDbContext _db;

        public OwnerViewsRequest(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            FetchTenantRequest();
            return View(_tenantrequestlist);
        }

        //:::::::::::::::::::::::::::::::::::::::::GET_CREATE
        public IActionResult Create()
        {
            return View();
        }
        //::::::::::::::::::::::::::::::::::::::::POST_CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TenantRequest obj)
        {
            if (ModelState.IsValid)
            {
                _db.TenantRequest.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
            
        }

        //:::::::::::::::::::::::::::::::::::::::GET_EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.TenantRequest.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //::::::::::::::::::::::::::::::::::::::POST_EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TenantRequest obj)
        {
            if (ModelState.IsValid)
            {
                _db.TenantRequest.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //:::::::::::::::::::::::::::::::::::::::GET_DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.TenantRequest.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //::::::::::::::::::::::::::::::::::::::POST_DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(TenantRequest obj)
        {
            if (ModelState.IsValid)
            {
                _db.TenantRequest.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::://
        //:::::::::::::::::::::::::::::         custom methods        ::::::::::::::::::::::::::::::::://
        private void FetchTenantRequest()
        {
            //User.Identity.Name returns current logged in user's email
            string queryString = "SELECT TOP 1000 T.[RequestId], C.[TenantEmail], T.[RequestText], T.[RequestTime], T.[CommentOnRequestText], T.[CommentOnRequestTime] " +
                                        "FROM [BashaBariWeb].[dbo].[TenantConnectsOwner] C JOIN [BashaBariWeb].[dbo].[TenantRequest] T " +
                                        "ON C.[TenantEmail] = T.[TenantEmail] " +
                                        "WHERE C.[IsConfirmed] ='Yes' AND C.[OwnerEmail] = '" + User.Identity.Name + "' " +
                                        "ORDER BY T.[RequestTime] DESC";
            //to clear the list initially
            if (_tenantrequestlist.Count > 0)
            {
                _tenantrequestlist.Clear();
            }

            //database operation
            DatabaseConnection obj = new DatabaseConnection();
            obj.DbConnect();
            while (obj.ExeQuery(queryString).Read())
            {
                _tenantrequestlist.Add(new TenantRequest
                {
                    RequestId = int.Parse(obj.ExeQuery(queryString)["RequestId"].ToString()),
                    TenantEmail = obj.ExeQuery(queryString)["TenantEmail"].ToString(),
                    RequestText = obj.ExeQuery(queryString)["RequestText"].ToString(),
                    RequestTime = obj.ExeQuery(queryString)["RequestTime"].ToString(),
                    CommentOnRequestText = obj.ExeQuery(queryString)["CommentOnRequestText"].ToString(),
                    CommentOnRequestTime = obj.ExeQuery(queryString)["CommentOnRequestTime"].ToString(),
                });
            }
            obj.CloseDbConnect();
        }


        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::://

    }
}
