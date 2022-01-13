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
    public class TenantViewsBillController : Controller
    {
        List<BillInformation> _billinformationlist = new List<BillInformation>();

        private readonly ApplicationDbContext _db;

        public TenantViewsBillController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            FetchBillInfo();
            return View(_billinformationlist);
        }

        //:::::::::::::::::::::::::::::::::::::::::GET_CREATE
        public IActionResult Create()
        {
            return View();
        }
        //::::::::::::::::::::::::::::::::::::::::POST_CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BillInformation obj)
        {
            if (ModelState.IsValid)
            {
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
                                        "WHERE [TenantEmail] = '" + User.Identity.Name + "' " +
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

        
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::://
    }
}
