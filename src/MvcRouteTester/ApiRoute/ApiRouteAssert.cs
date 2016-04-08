﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using MvcRouteTester.Assertions;
using MvcRouteTester.Common;

namespace MvcRouteTester.ApiRoute
{
    internal static class ApiRouteAssert
    {
        static ApiRouteAssert()
        {
            ControllerSelectorType = typeof(DefaultHttpControllerSelector);
        }

        internal static Type ControllerSelectorType;

        internal static void HasRoute(HttpConfiguration config, string url, HttpMethod httpMethod, Dictionary<string, string> headers)
        {
            var absoluteUrl = UrlHelpers.MakeAbsolute(url);
            ReadRequestProperties(config, absoluteUrl, httpMethod, headers, string.Empty, BodyFormat.None);
        }

        internal static void HasRoute(HttpConfiguration config, string url, HttpMethod httpMethod, Dictionary<string, string> headers, string body, BodyFormat bodyFormat, RouteValues expectedProps)
        {
            var absoluteUrl = UrlHelpers.MakeAbsolute(url);
            var actualProps = ReadRequestProperties(config, absoluteUrl, httpMethod, headers, body, bodyFormat);

            var verifier = new Verifier(expectedProps, actualProps, url);
            verifier.VerifyExpectations();
        }

        internal static void NoRoute(HttpConfiguration config, string url)
        {
            var absoluteUrl = UrlHelpers.MakeAbsolute(url);
            var request = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
            var routeGenerator = new Generator(config, request);

            if (routeGenerator.IsControllerRouteFound())
            {
                var hasRouteMessage = string.Format("Found a route for url '{0}'", url);
                Asserts.Fail(hasRouteMessage);
            }
        }

        internal static void RouteDoesNotHaveMethod(HttpConfiguration config, string url, HttpMethod httpMethod)
        {
            var absoluteUrl = UrlHelpers.MakeAbsolute(url);
            var request = new HttpRequestMessage(httpMethod, absoluteUrl);
            var routeGenerator = new Generator(config, request);

            routeGenerator.CheckNoMethod(url, httpMethod);
        }

        public static void RouteDoesNotHaveMethod(HttpConfiguration config, string url, Type controllerType, HttpMethod httpMethod)
        {
            var absoluteUrl = UrlHelpers.MakeAbsolute(url);
            var request = new HttpRequestMessage(httpMethod, absoluteUrl);
            var routeGenerator = new Generator(config, request);

            routeGenerator.CheckControllerHasNoMethod(url, httpMethod, controllerType);
        }

        internal static void RouteMatches(HttpConfiguration config, string url)
        {
            var absoluteUrl = UrlHelpers.MakeAbsolute(url);
            var request = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
            var routeGenerator = new Generator(config, request);

            if (!routeGenerator.HasMatchedRoute)
            {
                var hasRouteMessage = string.Format("Did not match a route for url '{0}'", url);
                Asserts.Fail(hasRouteMessage);
            }
        }

        internal static void NoRouteMatches(HttpConfiguration config, string url)
        {
            var absoluteUrl = UrlHelpers.MakeAbsolute(url);
            var request = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
            var routeGenerator = new Generator(config, request);

            if (routeGenerator.HasMatchedRoute)
            {
                var hasRouteMessage = string.Format("Matched a route for url '{0}'", url);
                Asserts.Fail(hasRouteMessage);
            }
        }

        internal static void HasHandler<THandler>(HttpConfiguration config, string url) where THandler : HttpMessageHandler
        {
            var absoluteUrl = UrlHelpers.MakeAbsolute(url);
            var request = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
            var routeGenerator = new Generator(config, request);

            if (!routeGenerator.HasHandlerOfType<THandler>())
            {
                var actualHandlerType = routeGenerator.HandlerType();
                var failureMessage = string.Format("Did not match handler type '{0}' for url '{1}', found ", typeof(THandler).Name, absoluteUrl);
                if (actualHandlerType != null)
                {
                    failureMessage += string.Format("a handler of type '{0}'.", actualHandlerType.Name);
                }
                else
                {
                    failureMessage += "no handler.";
                }
                Asserts.Fail(failureMessage);
            }
        }

        internal static void HasNoHandler(HttpConfiguration config, string url)
        {
            var absoluteUrl = UrlHelpers.MakeAbsolute(url);
            var request = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
            var routeGenerator = new Generator(config, request);

            if (routeGenerator.HasHandler())
            {
                var hasHandlerMessage = string.Format("Matching handler of type '{0}' found for url '{1}'.",
                    routeGenerator.HandlerType().Name, absoluteUrl);
                Asserts.Fail(hasHandlerMessage);
            }
        }

        internal static void HasNoHandlerofType<THandler>(HttpConfiguration config, string url) where THandler : HttpMessageHandler
        {
            var absoluteUrl = UrlHelpers.MakeAbsolute(url);
            var request = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
            var routeGenerator = new Generator(config, request);

            if (routeGenerator.HasHandlerOfType<THandler>())
            {
                var hasHandlerMessage = string.Format("Matching handler of type '{0}' found for url '{1}'.",
                    routeGenerator.HandlerType().Name, absoluteUrl);
                Asserts.Fail(hasHandlerMessage);
            }
        }

        private static RouteValues ReadRequestProperties(HttpConfiguration config, string url, HttpMethod httpMethod, Dictionary<string, string> headers, string body, BodyFormat bodyFormat)
        {
            var request = new HttpRequestMessage(httpMethod, url);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            request.Content = new StringContent(body);

            var routeGenerator = new Generator(config, request);
            return routeGenerator.ReadRequestProperties(url, httpMethod, bodyFormat);
        }
    }
}
