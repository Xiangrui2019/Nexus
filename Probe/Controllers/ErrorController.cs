﻿using Aiursoft.Pylon;
using Aiursoft.Pylon.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aiursoft.Probe.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Code404()
        {
            return View();
        }

        public IActionResult ServerException()
        {
            return this.Protocol(ErrorType.UnknownError, "Probe server crashed! Please tell us!");
        }
    }
}
