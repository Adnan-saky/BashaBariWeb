using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TBashaBari.Models;
using TBashaBari.Controllers;
using System.Dynamic;
using TBashaBari.Models.ViewModel;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;

namespace TBashaBari.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        List<OwnerNotice> owner_ownernoticelist = new List<OwnerNotice>();
        List<TenantRequest> owner_tenantrequestlist = new List<TenantRequest>();
        List<TenantConnectsOwner> owner_tenantconnectsownerlist = new List<TenantConnectsOwner>();
        List<BillInformation> owner_billinformationlist = new List<BillInformation>();
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public ContactUsVM ContactUsVM
        {
            get;
            set;
        }

        List<OwnerNotice> tenant_ownernoticelist = new List<OwnerNotice>();
        List<TenantRequest> tenant_tenantrequestlist = new List<TenantRequest>();
        List<TenantConnectsOwner> tenant_tenantconnectsownerlist = new List<TenantConnectsOwner>();
        List<BillInformation> tenant_billinformationlist = new List<BillInformation>();

        public HomeController(ILogger<HomeController> logger, 
            IWebHostEnvironment webHostEnvironment, 
            IEmailSender emailSender)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            ownerFetchOwnerNotice();
            ownerFetchTenantRequest();
            ownerFetchPendingTenants();
            ownerFetchBillInfo();
            tenantFetchBillInfo();
            tenantFetchOwnerNotice();
            tenantFetchTenantConnectsOwner();
            tenantFetchTenantRequest();
            dynamic homeViewModel = new ExpandoObject();
            homeViewModel.Notice_Owner = owner_ownernoticelist;
            homeViewModel.Request_Owner = owner_tenantrequestlist;
            homeViewModel.Connect_Owner = owner_tenantconnectsownerlist;
            homeViewModel.Bill_Owner = owner_billinformationlist;
            homeViewModel.Notice_Tenant = tenant_ownernoticelist;
            homeViewModel.Request_Tenant = tenant_tenantrequestlist;
            homeViewModel.Connect_Tenant = tenant_tenantconnectsownerlist;
            homeViewModel.Bill_Tenant = tenant_billinformationlist;
            return View(homeViewModel);
        }
        public IActionResult OwnerHome()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::://
        //:::::::::::::::::::::::::::::         custom methods        ::::::::::::::::::::::::::::::::://
        private void ownerFetchOwnerNotice()
        {
            //User.Identity.Name returns current logged in user's email
            string queryString = "SELECT TOP 10 [NoticeId],[OwnerEmail],[NoticeText],[NoticeTime] " +
                                        "FROM [BashaBariWeb].[dbo].[OwnerNotice] " +
                                        "WHERE [OwnerEmail] = '" + User.Identity.Name + "' " +
                                        "ORDER BY [NoticeTime] DESC";
            //to clear the list initially
            if (owner_ownernoticelist.Count > 0)
            {
                owner_ownernoticelist.Clear();
            }

            //database operation
            DatabaseConnection obj = new DatabaseConnection();
            obj.DbConnect();
            while (obj.ExeQuery(queryString).Read())
            {
                owner_ownernoticelist.Add(new OwnerNotice
                {
                    NoticeId = int.Parse(obj.ExeQuery(queryString)["NoticeId"].ToString()),
                    OwnerEmail = obj.ExeQuery(queryString)["OwnerEmail"].ToString(),
                    NoticeText = obj.ExeQuery(queryString)["NoticeText"].ToString(),
                    NoticeTime = obj.ExeQuery(queryString)["NoticeTime"].ToString(),
                });
            }
            obj.CloseDbConnect();
        }

        private void ownerFetchTenantRequest()
        {
            //User.Identity.Name returns current logged in user's email
            string queryString = "SELECT TOP 10 T.[RequestId], C.[TenantEmail], T.[RequestText], T.[RequestTime], T.[CommentOnRequestText], T.[CommentOnRequestTime] " +
                                        "FROM [BashaBariWeb].[dbo].[TenantConnectsOwner] C JOIN [BashaBariWeb].[dbo].[TenantRequest] T " +
                                        "ON C.[TenantEmail] = T.[TenantEmail] " +
                                        "WHERE C.[IsConfirmed] ='Yes' AND C.[OwnerEmail] = '" + User.Identity.Name + "' " +
                                        "ORDER BY T.[RequestTime] DESC";
            //to clear the list initially
            if (owner_tenantrequestlist.Count > 0)
            {
                owner_tenantrequestlist.Clear();
            }

            //database operation
            DatabaseConnection obj = new DatabaseConnection();
            obj.DbConnect();
            while (obj.ExeQuery(queryString).Read())
            {
                owner_tenantrequestlist.Add(new TenantRequest
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

        private void ownerFetchPendingTenants()
        {
            //User.Identity.Name returns current logged in user's email
            string queryString = "SELECT TOP 10 [ConnectionId],[TenantEmail],[OwnerEmail],[IsConfirmed] " +
                                        "FROM [BashaBariWeb].[dbo].[TenantConnectsOwner] " +
                                        "WHERE [OwnerEmail] = '" + User.Identity.Name + "' AND [IsConfirmed] = 'No'" +
                                        "ORDER BY [ConnectionId] DESC";
            //to clear the list initially
            if (owner_tenantconnectsownerlist.Count > 0)
            {
                owner_tenantconnectsownerlist.Clear();
            }

            //database operation
            DatabaseConnection obj = new DatabaseConnection();
            obj.DbConnect();
            while (obj.ExeQuery(queryString).Read())
            {
                owner_tenantconnectsownerlist.Add(new TenantConnectsOwner
                {
                    ConnectionId = int.Parse(obj.ExeQuery(queryString)["ConnectionId"].ToString()),
                    TenantEmail = obj.ExeQuery(queryString)["TenantEmail"].ToString(),
                    OwnerEmail = obj.ExeQuery(queryString)["OwnerEmail"].ToString(),
                    IsConfirmed = obj.ExeQuery(queryString)["IsConfirmed"].ToString(),
                });
            }
            obj.CloseDbConnect();
        }

        private void ownerFetchBillInfo()
        {
            //User.Identity.Name returns current logged in user's email
            string queryString = "SELECT TOP 1000 [BillId],[OwnerEmail],[TenantEmail],[BillTime],[WaterAmount],[WaterPaid],[WaterVerified],[ElectricAmount]," +
                                        "[ElectricPaid],[ElectricVerified],[RentAmount],[RentPaid],[RentVerified],[GasAmount],[GasPaid],[GasVerified] " +
                                        "FROM [BashaBariWeb].[dbo].[BillInformation] " +
                                        "WHERE [OwnerEmail] = '" + User.Identity.Name + "' " +
                                        "ORDER BY [BillTime] DESC";
            //to clear the list initially
            if (owner_billinformationlist.Count > 0)
            {
                owner_billinformationlist.Clear();
            }

            //database operation
            DatabaseConnection obj = new DatabaseConnection();
            obj.DbConnect();
            while (obj.ExeQuery(queryString).Read())
            {
                owner_billinformationlist.Add(new BillInformation
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

        private void tenantFetchTenantRequest()
        {
            //User.Identity.Name returns current logged in user's email
            string queryString = "SELECT TOP 10 T.[RequestId],T.[TenantEmail],T.[RequestText],T.[RequestTime],T.[CommentOnRequestText],T.[CommentOnRequestTime],C.[TenantEmail],C.[IsConfirmed]" +
                                        "FROM [BashaBariWeb].[dbo].[TenantConnectsOwner] C JOIN [BashaBariWeb].[dbo].[TenantRequest] T " +
                                        "ON C.[TenantEmail] = T.[TenantEmail] " +
                                        "WHERE C.[IsConfirmed] = 'Yes' AND C.[TenantEmail] = '" + User.Identity.Name + "' " +
                                        "ORDER BY [RequestTime] DESC";
            //to clear the list initially
            if (tenant_tenantrequestlist.Count > 0)
            {
                tenant_tenantrequestlist.Clear();
            }

            //database operation
            DatabaseConnection obj = new DatabaseConnection();
            obj.DbConnect();
            while (obj.ExeQuery(queryString).Read())
            {
                tenant_tenantrequestlist.Add(new TenantRequest
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
        private void tenantFetchBillInfo()
        {
            //User.Identity.Name returns current logged in user's email
            string queryString = "SELECT TOP 10 [BillId],[OwnerEmail],[TenantEmail],[BillTime],[WaterAmount],[WaterPaid],[WaterVerified],[ElectricAmount]," +
                                        "[ElectricPaid],[ElectricVerified],[RentAmount],[RentPaid],[RentVerified],[GasAmount],[GasPaid],[GasVerified] " +
                                        "FROM [BashaBariWeb].[dbo].[BillInformation] " +
                                        "WHERE [TenantEmail] = '" + User.Identity.Name + "' " +
                                        "ORDER BY [BillTime] DESC";
            //to clear the list initially
            if (tenant_billinformationlist.Count > 0)
            {
                tenant_billinformationlist.Clear();
            }

            //database operation
            DatabaseConnection obj = new DatabaseConnection();
            obj.DbConnect();
            while (obj.ExeQuery(queryString).Read())
            {
                tenant_billinformationlist.Add(new BillInformation
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
        private void tenantFetchTenantConnectsOwner()
        {
            //User.Identity.Name returns current logged in user's email
            string queryString = "SELECT TOP 10 [ConnectionId],[TenantEmail],[OwnerEmail],[IsConfirmed] " +
                                        "FROM [BashaBariWeb].[dbo].[TenantConnectsOwner] " +
                                        "WHERE [TenantEmail] = '" + User.Identity.Name + "' " +
                                        "ORDER BY [ConnectionId] DESC";
            //to clear the list initially
            if (tenant_tenantconnectsownerlist.Count > 0)
            {
                tenant_tenantconnectsownerlist.Clear();
            }

            //database operation
            DatabaseConnection obj = new DatabaseConnection();
            obj.DbConnect();
            while (obj.ExeQuery(queryString).Read())
            {
                tenant_tenantconnectsownerlist.Add(new TenantConnectsOwner
                {
                    ConnectionId = int.Parse(obj.ExeQuery(queryString)["ConnectionId"].ToString()),
                    TenantEmail = obj.ExeQuery(queryString)["TenantEmail"].ToString(),
                    OwnerEmail = obj.ExeQuery(queryString)["OwnerEmail"].ToString(),
                    IsConfirmed = obj.ExeQuery(queryString)["IsConfirmed"].ToString(),
                });
            }
            obj.CloseDbConnect();
        }
        private void tenantFetchOwnerNotice()
        {
            //User.Identity.Name returns current logged in user's email
            string queryString = "SELECT TOP 10 O.[NoticeId],O.[NoticeText],O.[NoticeTime], C.[OwnerEmail], C.[IsConfirmed] " +
                "FROM [BashaBariWeb].[dbo].[TenantConnectsOwner] C JOIN  [BashaBariWeb].[dbo].[OwnerNotice] O " +
                "ON C.[OwnerEmail] = O.[OwnerEmail] " +
                "WHERE C.[IsConfirmed] = 'Yes' AND C.[TenantEmail] = '" + User.Identity.Name + "'" +
                "ORDER BY [NoticeTime] DESC";
            //to clear the list initially
            if (tenant_ownernoticelist.Count > 0)
            {
                tenant_ownernoticelist.Clear();
            }

            //database operation
            DatabaseConnection obj = new DatabaseConnection();
            obj.DbConnect();
            while (obj.ExeQuery(queryString).Read())
            {
                tenant_ownernoticelist.Add(new OwnerNotice
                {
                    NoticeId = int.Parse(obj.ExeQuery(queryString)["NoticeId"].ToString()),
                    OwnerEmail = obj.ExeQuery(queryString)["OwnerEmail"].ToString(),
                    NoticeText = obj.ExeQuery(queryString)["NoticeText"].ToString(),
                    NoticeTime = obj.ExeQuery(queryString)["NoticeTime"].ToString(),
                });
            }
            obj.CloseDbConnect();
        }
        public IActionResult ContactUs()
        {
            //var userId = User.FindFirstValue(ClaimTypes.Name);
            ContactUsVM = new ContactUsVM();
            return View(ContactUsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ContactUs")]
        public async Task<IActionResult> ContactUsPost(ContactUsVM ContactUsVM)
        {
            var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "templates" + Path.DirectorySeparatorChar.ToString() +
                "ContactMail.html";

            var subject = ContactUsVM.Subject;
            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }
          

            StringBuilder message = new StringBuilder();

            message.Append(ContactUsVM.Message);


            string messageBody = string.Format(HtmlBody,
                ContactUsVM.ApplicationUser.FullName,
                ContactUsVM.ApplicationUser.Email,
                ContactUsVM.ApplicationUser.PhoneNumber,
                message.ToString());

            await _emailSender.SendEmailAsync(WebConstant.EmailAdmin, subject, messageBody);

            return RedirectToAction(nameof(Index));
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }


    
}
