using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PonySimpleRuleAuthenticationDemo.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PonySimpleRuleAuthenticationDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        [Authorize]
        public async Task<IActionResult> jwttest()
        {

            return Content(DateTime.Now.ToString());

        }

        public async Task<IActionResult> MakeJwtRequest()
        {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", PonySimpleRuleAuthentication.PonySimpleRuleController.LastTokenGenerated);
            using (var response1 = await client.GetAsync("https://localhost:44300/Home/jwttest"))
            //using (var response1 = await client.GetAsync("https://localhost:5000/PonySimpleRuleController/LoginJWT?uid=a;pwd=a"))
            {
                response1.EnsureSuccessStatusCode();
                ViewBag.MakeJwtRequest = await response1.Content.ReadAsStringAsync();

                //Debug.WriteLine(PonySimpleRuleAuthentication.PonySimpleRuleController.LastTokenGenerated);
                //var companies = await JsonSerializer.DeserializeAsync<List<CompanyDto>>(stream, _options);
            }


            //return Content(response);

            return View("Index");
        }




        public async Task<IActionResult> GetJwtToken ()
        {

            HttpClient client = new HttpClient();
            using (var response1 = await client.GetAsync("http://localhost:5000/PonySimpleRuleController/LoginJWT?uid=a;pwd=a"))
            {
                response1.EnsureSuccessStatusCode();
                Debug.WriteLine(PonySimpleRuleAuthentication.PonySimpleRuleController.LastTokenGenerated);
                //var companies = await JsonSerializer.DeserializeAsync<List<CompanyDto>>(stream, _options);
            }


            //return Content(response);

            return View("Index");
        }




        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
