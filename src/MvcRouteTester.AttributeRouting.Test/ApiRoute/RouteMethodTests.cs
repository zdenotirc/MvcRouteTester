﻿using System.Net.Http;
using System.Web.Http;

using MvcRouteTester.AttributeRouting.Test.ApiControllers;

using NUnit.Framework;

namespace MvcRouteTester.AttributeRouting.Test.ApiRoute
{
    [TestFixture]
    public class RouteMethodTests
    {
        private HttpConfiguration config;

        [SetUp]
        public void MakeRouteTable()
        {
            config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();
        }

        [Test]
        public void CustomerControllerHasGetMethod()
        {
            RouteAssert.HasApiRoute(config, "/api/customerattr/1", HttpMethod.Get);
        }

        [Test]
        public void PostOnlyControllerHasPostMethod()
        {
            RouteAssert.HasApiRoute(config, "/api/postonlyattr/1", HttpMethod.Post);
        }

        [Test]
        public void HasFluentMethod()
        {
            config.ShouldMap("/api/postonlyattr/1").To<PostOnlyAttrController>(HttpMethod.Post, x => x.Post(1));
        }
    }
}
