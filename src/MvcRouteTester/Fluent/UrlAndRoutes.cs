﻿using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.ApiRoute;
using MvcRouteTester.WebRoute;

namespace MvcRouteTester.Fluent
{
    public class UrlAndRoutes
    {
        private string requestBody = string.Empty;
        private BodyFormat bodyFormat = BodyFormat.None;

        private string requestAppPath = "/";

        public UrlAndRoutes(RouteCollection routes, HttpMethod httpMethod, string url)
        {
            Routes = routes;
            HttpMethod = httpMethod;
            Url = url;
        }

        public RouteCollection Routes { get; private set; }

        public HttpMethod HttpMethod { get; private set; }
        public string Url { get; private set; }

        public UrlAndRoutes WithFormUrlBody(string body)
        {
            requestBody = body;
            bodyFormat = BodyFormat.FormUrl;
            return this;
        }

        public UrlAndRoutes WithJsonBody(string body)
        {
            requestBody = body;
            bodyFormat = BodyFormat.Json;
            return this;
        }

        public UrlAndRoutes WithAppPath(string appPath)
        {
            requestAppPath = appPath;
            return this;
        }

        public void To<TController>(Expression<Func<TController, ActionResult>> action) where TController : Controller
        {
            To(HttpMethod, action);
        }

        public void To<TController>(HttpMethod httpMethod, Expression<Func<TController, ActionResult>> action) where TController : Controller
        {
            var expressionReader = new ExpressionReader();
            var expectedProps = expressionReader.Read(action);

            WebRouteAssert.HasRoute(Routes, httpMethod, Url, requestBody, bodyFormat, expectedProps);
        }

        public void To<TController>(Expression<Func<TController, Task<ActionResult>>> action) where TController : Controller
        {
            To(HttpMethod, action);
        }

        public void To<TController>(Expression<Func<TController, Task<JsonResult>>> action) where TController : Controller
        {
            To(HttpMethod, action);
        }

        public void To<TController>(HttpMethod httpMethod, Expression<Func<TController, Task<ActionResult>>> action) where TController : Controller
        {
            var expressionReader = new ExpressionReader();
            var expectedProps = expressionReader.Read(action);

            WebRouteAssert.HasRoute(Routes, httpMethod, Url, requestBody, bodyFormat, expectedProps);
        }

        public void To<TController>(HttpMethod httpMethod, Expression<Func<TController, Task<JsonResult>>> action) where TController : Controller
        {
            var expressionReader = new ExpressionReader();
            var expectedProps = expressionReader.Read(action);

            WebRouteAssert.HasRoute(Routes, httpMethod, Url, requestBody, bodyFormat, expectedProps);
        }

        public void ToNoRoute()
        {
            WebRouteAssert.NoRoute(Routes, Url);
        }

        public void ToIgnoredRoute()
        {
            WebRouteAssert.IsIgnoredRoute(Routes, Url);
        }

        public void ToNonIgnoredRoute()
        {
            WebRouteAssert.IsNotIgnoredRoute(Routes, Url);
        }

        public void From<TController>(Expression<Func<TController, ActionResult>> action) where TController : Controller
        {
            var expressionReader = new ExpressionReader();
            var fromProps = expressionReader.Read(action);

            WebRouteAssert.GeneratesActionUrl(Routes, HttpMethod.Get, Url, fromProps, appPath: requestAppPath, requestBody: requestBody);
        }
    }
}