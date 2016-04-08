﻿using System.Web.Mvc;

namespace MvcRouteTester.Test.Controllers
{
    public class InputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class InputModelWithNullable
    {
        public int? Id { get; set; }
    }


    public class WithObjectController : Controller
    {
        public ActionResult Index(InputModel data)
        {
            return new EmptyResult();
        }

        public ActionResult Action2(InputModelWithNullable data)
        {
            return new EmptyResult();
        }

    }
}
