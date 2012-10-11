using System.Linq;
using NUnit.Framework;

namespace XRouter.Tests
{
  [TestFixture]
  public class XRouteSourceTests
  {
    private const string DirectoryPath = @"E:\2012\XRouter\XRouter.Tests\";

    [Test]
    public void Route_Test()
    {
      // arrange
      var routeSource = new XRouteSource("routes.config");
      routeSource.DirectoryPath = DirectoryPath;

      // act
      var routeItems = routeSource.GetRouteItems();

      // assert
      Assert.AreEqual(2, routeItems.Count());
      var ignoreRoute = routeItems.First();
      Assert.AreEqual(true, ignoreRoute.Ignore);
      var routeItemToTest = routeItems[1];
      Assert.AreEqual("default", routeItemToTest.Name);
      Assert.AreEqual("{controller}/{action}/{id}", routeItemToTest.Pattern);
      Assert.AreEqual("home", routeItemToTest.Controller);
      Assert.AreEqual("index", routeItemToTest.Action);
      Assert.AreEqual(2, routeItemToTest.HttpMethods.Length);
      Assert.AreEqual("GET", routeItemToTest.HttpMethods[0]);
      Assert.AreEqual("POST", routeItemToTest.HttpMethods[1]);
      Assert.AreEqual("SomeClass", routeItemToTest.Constraint);
    }

    [Test]
    public void Route_Parameters_Test()
    {
      // arrange
      var routeSource = new XRouteSource("routes_parameters.config");
      routeSource.DirectoryPath = DirectoryPath;

      // act
      var routeItems = routeSource.GetRouteItems();

      // assert
      var routeItem = routeItems[1];
      Assert.AreEqual(1, routeItem.Params.Length);
      Assert.AreEqual("id", routeItem.Params[0].Name);
      Assert.AreEqual("0", routeItem.Params[0].Value);
      Assert.AreEqual("int", routeItem.Params[0].Type);
      Assert.AreEqual(@"/d{2}", routeItem.Params[0].Constraint);
      Assert.AreEqual(true, routeItem.Params[0].Optional);
    }

    [Test]
    public void Route_Constraints_Test()
    {
      // arrange
      var routeSource = new XRouteSource("routes_constraints.config");
      routeSource.DirectoryPath = DirectoryPath;

      // act
      var routeItems = routeSource.GetRouteItems();

      // assert
      var routeItemToTest = routeItems[1];
      var constraints = routeItemToTest.Constraints;
      Assert.AreEqual(1, constraints.Count());
      Assert.AreEqual("c", constraints[0].Name);
      Assert.AreEqual("SomeClass", constraints[0].Value);
      Assert.AreEqual(true, constraints[0].Disabled);
    }

    [Test]
    public void Ignore_Route_Test()
    {
      // arrange
      var routeSource = new XRouteSource("routes_ignore.config");
      routeSource.DirectoryPath = DirectoryPath;

      // act
      var routeItems = routeSource.GetRouteItems();

      // assert
      Assert.AreEqual(1, routeItems.Count());
      var ignoreRoute = routeItems[0];
      Assert.AreEqual(true, ignoreRoute.Ignore);
      Assert.AreEqual("{resource}.axd/{*pathInfo}", ignoreRoute.Pattern);
      Assert.AreEqual(true, ignoreRoute.Disabled);
    }

    [Test]
    public void Route_Controller_Namespaces_And_DataTokens_Test()
    {
      // arrange
      var routeSource = new XRouteSource("routes_nsdatatokens.config");
      routeSource.DirectoryPath = DirectoryPath;

      // act
      var routeItems = routeSource.GetRouteItems();

      // assert
      var routeItemToTest = routeItems[0];
      Assert.AreEqual(2, routeItemToTest.DataTokens.Count());
      Assert.AreEqual("a", routeItemToTest.DataTokens.First().Key);
      Assert.AreEqual("23", routeItemToTest.DataTokens.First().Value);
      Assert.AreEqual(2, routeItemToTest.Namespaces.Length);
      Assert.AreEqual("Namespace1", routeItemToTest.Namespaces[0]);
      Assert.AreEqual("Namespace2", routeItemToTest.Namespaces[1]);
    }

    [Test]
    public void Route_Areas_Test()
    {
      // arrange
      var routeSource = new XRouteSource("routes_areas.config");
      routeSource.DirectoryPath = DirectoryPath;

      // act
      var routeItems = routeSource.GetRouteItems();

      // assert
      Assert.AreEqual(4, routeItems.Count());
      Assert.AreEqual("Help", routeItems[0].Area.Name);
      Assert.AreEqual(1, routeItems[0].Area.Namespaces.Count());
      Assert.AreEqual("Area1", routeItems[0].Area.Namespaces[0]);

      Assert.AreEqual("{resource}.axd/{*pathInfo}", routeItems[0].Pattern);
    }

    [Test]
    public void Route_Areas_Folder_Test()
    {
      // arrange
      var routeSource = new XRouteSource("routes.config");
      routeSource.DirectoryPath = @"E:\2012\XRouter\XRouter.Tests\Test\";

      // act
      var routeItems = routeSource.GetRouteItems();

      // assert 
      Assert.AreEqual(4, routeItems.Count());
      Assert.AreEqual("Help", routeItems[0].Area.Name);
      Assert.AreEqual(1, routeItems[0].Area.Namespaces.Count());
      Assert.AreEqual("Area1", routeItems[0].Area.Namespaces[0]);

      Assert.AreEqual("{resource}.axd/{*pathInfo}", routeItems[0].Pattern);
    }
  }
}
