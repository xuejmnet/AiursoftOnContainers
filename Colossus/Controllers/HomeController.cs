﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aiursoft.Colossus.Models;
using Aiursoft.Pylon.Attributes;
using System.IO;
using Aiursoft.Pylon;
using Microsoft.Extensions.Configuration;

namespace Aiursoft.Colossus.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AiurForceAuth("", "", justTry: true)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [FileChecker]
        public async Task<IActionResult> Upload()
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/");
            }
            var file = Request.Form.Files.First();
            var path = await Pylon.Services.StorageService.SaveToOSS(file, Convert.ToInt32(_configuration["ColossusPublicBucketId"]), 30);
            return Json(new
            {
                message = "Uploaded!",
                value = path
            });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
