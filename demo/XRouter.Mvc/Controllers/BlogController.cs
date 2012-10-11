using System;
using System.Web.Mvc;

namespace XRouter.Mvc.Controllers
{
  public class BlogController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Article(string dt)
    {
      return View((object)dt);
    }

    public ActionResult Parameters(int a, int b, int c)
    {
      ViewBag.a = a;
      ViewBag.b = b;
      ViewBag.c = c;

      return View();
    }

    public ActionResult OnlyPost()
    {
      return View();
    }

    public ActionResult OldAction(string arg)
    {
      return View((object)arg);
    }

    public ActionResult NewAction(string arg)
    {
      return View((object)arg);
    }
  }
}
