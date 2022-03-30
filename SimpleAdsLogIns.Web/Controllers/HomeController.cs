using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleAdsLogIns.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SimpleAdsLogIns.Data;

namespace SimpleAdsLogIns.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Ads;Integrated Security=true;";


        public IActionResult Index()
        {
            var repo = new AdsRepository(_connectionString);
            List<Ad>ads= repo.GetAds();
           
            if (User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name;
                int userid = repo.GetIdByEmail(email);
                foreach (Ad a in ads)
                {
                    if(a.UserId==userid)
                    {
                        a.Delete = true;
                    }
                }    
            }
            return View(new IndexViewModel
            {
                Ads = ads
            });
        }
        [Authorize]
        public IActionResult NewAd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewAd(Ad newad)
        {
            var repo = new AdsRepository(_connectionString);
            var email = User.Identity.Name;
            var user = repo.GetByEmail(email);
            repo.Add(newad, user.Id, user.Name);
            return Redirect("/");

        }


        public IActionResult Delete(int id)
        {
            var repo= new AdsRepository (_connectionString);

            repo.DeleteAd(id);
            return Redirect("/");
        }

        public IActionResult MyAccount()
        {
            var repo = new AdsRepository(_connectionString);
            List<Ad> ads = repo.GetAds();
            List<Ad> acctads = new List<Ad>();

            if (User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name;
                int userid = repo.GetIdByEmail(email);
                foreach (Ad a in ads)
                {
                    if (a.UserId == userid)
                    {
                        acctads.Add(a);
                    }
                }
            }
            return View(new IndexViewModel
            {
                Ads = acctads
            });
        }

    }
}
