﻿using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;
using MvcRouteTester.Test.Controllers;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
    [TestFixture]
    public class FluentExtensionsAliasRouteTests
    {
        [SetUp]
        public void Setup()
        {
            RouteAssert.UseAssertEngine(new NunitAssertEngine());
        }

        [Test]
        public void FluentRouteWithCaps()
        {
            var routesWithCaps = new RouteCollection();
            routesWithCaps.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // note that "Controller" and "Action" are capitalised
            routesWithCaps.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { Controller = "Home", Action = "Index", id = 32 });

            routesWithCaps.ShouldMap("/home/index/32").To<HomeController>(x => x.Index(32));
        }

        [Test]
        public void FluentRouteShouldHandleAliasRoute()
        {
            var routesWithAlias = new RouteCollection();
            routesWithAlias.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routesWithAlias.MapRoute(
                name: "Alias Route",
                url: "aliasroute",
                defaults: new { controller = "Home", action = "Index", id = 32 });

            routesWithAlias.ShouldMap("/aliasroute").To<HomeController>(x => x.Index(32));
        }

        [Test]
        public void FluentRouteShouldHandleAliasRouteWithCaps()
        {
            var routesWithAlias = new RouteCollection();
            routesWithAlias.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // note that "Controller" and "Action" are capitalised
            routesWithAlias.MapRoute(
                name: "Alias Route",
                url: "aliasroute",
                defaults: new { Controller = "Home", Action = "Index", id = 32 });

            routesWithAlias.ShouldMap("/aliasroute").To<HomeController>(x => x.Index(32));
        }
    }
}
