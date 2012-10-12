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

    public ActionResult Sample(int id)
    {
      return View();
    }

    public ActionResult Article(DateTime dt)
    {
      return View(dt);
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

    public ActionResult OldAction(int id)
    {
      return View();
    }

    public ActionResult NewAction(int id)
    {
      return View();
    }

    public ActionResult Banned(int id)
    {
      return View();
    }
  }
}
