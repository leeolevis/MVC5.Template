﻿using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Security;
using NSubstitute;
using System;
using System.Web.Mvc;
using Xunit;
using Xunit.Extensions;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    public class AuthorizationExtensionsTests : IDisposable
    {
        private HtmlHelper html;

        public AuthorizationExtensionsTests()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            html = HtmlHelperFactory.CreateHtmlHelper();
        }
        public void Dispose()
        {
            Authorization.Provider = null;
        }

        #region Extension method: IsAuthorizedFor(this HtmlHelper html, String action)

        [Fact]
        public void IsAuthorizedFor_Action_NullProvider_ReturnsTrue()
        {
            Authorization.Provider = null;

            Assert.True(html.IsAuthorizedFor("Action"));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsAuthorizedFor_Action_ReturnsAuthorizationResult(Boolean isAuthorized)
        {
            String area = html.ViewContext.RouteData.Values["area"] as String;
            String accountId = html.ViewContext.HttpContext.User.Identity.Name;
            String controller = html.ViewContext.RouteData.Values["controller"] as String;

            Authorization.Provider.IsAuthorizedFor(accountId, area, controller, "Action").Returns(isAuthorized);

            Boolean actual = html.IsAuthorizedFor("Action");
            Boolean expected = isAuthorized;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: IsAuthorizedFor(this HtmlHelper html, String action, String controller)

        [Fact]
        public void IsAuthorizedFor_Controller_NullProvider_ReturnsTrue()
        {
            Authorization.Provider = null;

            Assert.True(html.IsAuthorizedFor("Action", "Controller"));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsAuthorizedFor_Controller_ReturnsProviderResult(Boolean isAuthorized)
        {
            String area = html.ViewContext.RouteData.Values["area"] as String;
            String accountId = html.ViewContext.HttpContext.User.Identity.Name;

            Authorization.Provider.IsAuthorizedFor(accountId, area, "Controller", "Action").Returns(isAuthorized);

            Boolean actual = html.IsAuthorizedFor("Action", "Controller");
            Boolean expected = isAuthorized;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: IsAuthorizedFor(this HtmlHelper html, String action, String controller, String area)

        [Fact]
        public void IsAuthorizedFor_Area_NullProvider_ReturnsTrue()
        {
            Authorization.Provider = null;

            Assert.True(html.IsAuthorizedFor("Action", "Controller", "Area"));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsAuthorizedFor_Area_ReturnsProviderResult(Boolean isAuthorized)
        {
            String accountId = html.ViewContext.HttpContext.User.Identity.Name;

            Authorization.Provider.IsAuthorizedFor(accountId, "Area", "Controller", "Action").Returns(isAuthorized);

            Boolean actual = html.IsAuthorizedFor("Action", "Controller", "Area");
            Boolean expected = isAuthorized;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
