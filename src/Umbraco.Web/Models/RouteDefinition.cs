﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Umbraco.Web.Models
{
    public class RouteDefinition
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Area { get; set; }
    }
}
