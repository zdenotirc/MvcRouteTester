﻿using System;
using System.Globalization;

using MvcRouteTester.Assertions;

namespace MvcRouteTester.Common
{
    internal class Verifier
    {
        private readonly RouteValues expected;
        private readonly RouteValues actual;
        private readonly string url;
        private int expectationsDone;

        public Verifier(RouteValues expected, RouteValues actual, string url)
        {
            this.expected = expected;
            this.actual = actual;
            this.url = url;
        }

        public void VerifyExpectations()
        {
            VerifyStringValue(expected.Controller, actual.Controller, "controller");
            VerifyStringValue(expected.Action, actual.Action, "action");
            VerifyStringValue(expected.Area, actual.Area, "area");

            foreach (var expectedValue in expected.Values)
            {
                var actualValue = actual.GetRouteValue(expectedValue.Name, expectedValue.Origin);
                if ((actualValue == null) && (expectedValue.Value != null))
                {
                    var notFoundErrorMessage = string.Format("Expected '{0}', got missing value for '{1}' at url '{2}'.",
                        expectedValue.Value, expectedValue.Name, url);
                    Asserts.Fail(notFoundErrorMessage);
                    return;
                }

                VerifyValue(expectedValue, actualValue);

                expectationsDone++;
            }

            if (expectationsDone == 0)
            {
                var message = string.Format("No expectations were found for url '{0}'", url);
                Asserts.Fail(message);
            }
        }

        private void VerifyValue(RouteValue expectedValue, RouteValue actualValue)
        {
            if (expectedValue.Value is DateTime)
            {
                VerifyDateTimeValue(expectedValue, actualValue);
            }
            else
            {
                var actualValueString = (actualValue == null) ? string.Empty : actualValue.ValueAsString;
                VerifyStringValue(expectedValue.ValueAsString, actualValueString, expectedValue.Name);
            }
        }

        private void VerifyStringValue(string expectedValue, string actualValue, string name)
        {
            if (string.IsNullOrEmpty(expectedValue))
            {
                return;
            }

            if (string.IsNullOrEmpty(actualValue))
            {
                var notFoundErrorMessage = string.Format("Expected '{0}', got no value for '{1}' at url '{2}'.",
                    expectedValue, name, url);
                Asserts.Fail(notFoundErrorMessage);
                return;
            }

            var mismatchErrorMessage = string.Format("Expected '{0}', not '{1}' for '{2}' at url '{3}'.",
                expectedValue, actualValue, name, url);
            Asserts.StringsEqualIgnoringCase(expectedValue, actualValue, mismatchErrorMessage);

            expectationsDone++;
        }

        private void VerifyDateTimeValue(RouteValue expectedValue, RouteValue actualValue)
        {
            DateTime expectedDateTime = (DateTime)(expectedValue.Value);
            string expectedDateTimeString = expectedDateTime.ToString("s", CultureInfo.InvariantCulture);

            DateTime actualDateTime;
            string actualDateTimeString;
            bool actualValueParsed;

            if (actualValue.Value is DateTime)
            {
                actualDateTime = (DateTime)(actualValue.Value);
                actualDateTimeString = actualDateTime.ToString("s", CultureInfo.InvariantCulture);
                actualValueParsed = true;
            }
            else
            {
                actualDateTimeString = actualValue.ValueAsString;
                actualValueParsed = DateTime.TryParse(actualDateTimeString, out actualDateTime);
            }

            if (actualValueParsed)
            {
                if (expectedDateTime != actualDateTime)
                {
                    var mismatchErrorMessage = string.Format("Expected '{0}', not '{1}' for '{2}' at url '{3}'.",
                        expectedDateTimeString, actualDateTimeString, expectedValue.Name, url);
                    Asserts.Fail(mismatchErrorMessage);
                }
            }
            else
            {
                var parseFailErrorMessage = string.Format("Actual value '{0}' could not be parsed as a DateTime at url '{1}'.",
                    actualDateTimeString, url);
                Asserts.Fail(parseFailErrorMessage);
            }

            expectationsDone++;
        }
    }
}
