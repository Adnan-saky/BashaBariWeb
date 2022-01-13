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
    public class OwnerViewsBillController : Controller
    {
        List<BillInformation> _billinformationlist = new List<BillInformation>();
        List<TenantConnectsOwner> _tenantconnectsownerlist = new List<TenantConnectsOwner>();

        private readonly ApplicationDbContext _db;

        public OwnerViewsBillController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            FetchBillInfo();
            return View(_billinformationlist);
        }
        //:::::::::::::::::::::::::::::::::::::::::GET_Select
        public IActionResult Select()
        {
            FetchTenantConnectsOwner();
            return View(_tenantconnectsownerlist);
        }

        //:::::::::::::::::::::::::::::::::::::::::GET_BillExists
        public IActionResult BillExists()
        {
            FetchBillInfo();
            return View(_billinformationlist);
        }

        //:::::::::::::::::::::::::::::::::::::::::GET_CREATE
        public IActionResult Create(string TenantEmail)
        {
            ViewBag.SelectedTenantEmail = TenantEmail;
            return View();
        }
        //::::::::::::::::::::::::::::::::::::::::POST_CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BillInformation obj)
        {
            if (ModelState.IsValid)
            {
                DatabaseConnection ob = new DatabaseConnection();
                if (ob.isBillInformationExist(obj.TenantEmail, obj.BillTime) == true) {
                    return RedirectToAction("BillExists");
                }
                _db.BillInformation.Add(obj);
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

            var obj = _db.BillInformation.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //::::::::::::::::::::::::::::::::::::::POST_EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BillInformation obj)
        {
            if (ModelState.IsValid)
            {
                _db.BillInformation.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //:::::::::::::::::::::::::::::::::::::::GET_Verify
        public IActionResult Verify(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.BillInformation.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //::::::::::::::::::::::::::::::::::::::POST_Verify
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Verify(BillInformation obj)
        {
            if (ModelState.IsValid)
            {
                _db.BillInformation.Update(obj);
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

            var obj = _db.BillInformation.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //::::::::::::::::::::::::::::::::::::::POST_DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(BillInformation obj)
        {
            if (ModelState.IsValid)
            {
                _db.BillInformation.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::://
        //:::::::::::::::::::::::::::::         custom methods        ::::::::::::::::::::::::::::::::://
        private void FetchBillInfo()
        {
            //User.Identity.Name returns current logged in user's email
            string queryString = "SELECT TOP 1000 [BillId],[OwnerEmail],[TenantEmail],[BillTime],[WaterAmount],[WaterPaid],[WaterVerified],[ElectricAmount]," +
                                        "[ElectricPaid],[ElectricVerified],[RentAmount],[RentPaid],[RentVerified],[GasAmount],[GasPaid],[GasVerified] " +
                                        "FROM [BashaBariWeb].[dbo].[BillInformation] " +
                                        "WHERE [OwnerEmail] = '" + User.Identity.Name + "' " +
                                        "ORDER BY [BillTime] DESC";
            //to clear the list initially
            if (_billinformationlist.Count > 0)
            {
                _billinformationlist.Clear();
            }

            //database operation
            DatabaseConnection obj = new DatabaseConnection();
            obj.DbConnect();
            while (obj.ExeQuery(queryString).Read())
            {
                _billinformationlist.Add(new BillInformation
                {
                    BillId = int.Parse(obj.ExeQuery(queryString)["BillId"].ToString()),
                    OwnerEmail = obj.ExeQuery(queryString)["OwnerEmail"].ToString(),
                    TenantEmail = obj.ExeQuery(queryString)["TenantEmail"].ToString(),
                    BillTime = obj.ExeQuery(queryString)["BillTime"].ToString(),
                    
                    WaterAmount = obj.ExeQuery(queryString)["WaterAmount"].ToString(),
                    WaterPaid = obj.ExeQuery(queryString)["WaterPaid"].ToString(),
                    WaterVerified = obj.ExeQuery(queryString)["WaterVerified"].ToString(),

                    ElectricAmount = obj.ExeQuery(queryString)["ElectricAmount"].ToString(),
                    ElectricPaid = obj.ExeQuery(queryString)["ElectricPaid"].ToString(),
                    ElectricVerified = obj.ExeQuery(queryString)["ElectricVerified"].ToString(),

                    RentAmount = obj.ExeQuery(queryString)["RentAmount"].ToString(),
                    RentPaid = obj.ExeQuery(queryString)["RentPaid"].ToString(),
                    RentVerified = obj.ExeQuery(queryString)["RentVerified"].ToString(),

                    GasAmount = obj.ExeQuery(queryString)["GasAmount"].ToString(),
                    GasPaid = obj.ExeQuery(queryString)["GasPaid"].ToString(),
                    GasVerified = obj.ExeQuery(queryString)["GasVerified"].ToString(),
                });
            }
            obj.CloseDbConnect();
        }

        private void FetchTenantConnectsOwner()
        {
            //User.Identity.Name returns current logged in user's email
            string queryString = "SELECT TOP 1000 [ConnectionId],[TenantEmail],[OwnerEmail],[IsConfirmed] " +
                                        "FROM [BashaBariWeb].[dbo].[TenantConnectsOwner] " +
                                        "WHERE [OwnerEmail] = '" + User.Identity.Name + "' " +
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
