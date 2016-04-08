﻿using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
    [TestFixture]
    public class RouteParamsTests
    {
        private RouteCollection routes;

        [SetUp]
        public void MakeRouteTable()
        {
            RouteAssert.UseAssertEngine(new NunitAssertEngine());

            routes = new RouteCollection();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        [Test]
        public void HasRouteWithParams()
        {
            RouteAssert.HasRoute(routes, "/test/index?foo=1&bar=2");
        }

        [Test]
        public void HasRouteWithParamsCapturesValues()
        {
            var expectedRoute = new { controller = "Test", action = "Index", foo = "1", bar = "2" };
            RouteAssert.HasRoute(routes, "/test/index?foo=1&bar=2", expectedRoute);
        }
    }
}
