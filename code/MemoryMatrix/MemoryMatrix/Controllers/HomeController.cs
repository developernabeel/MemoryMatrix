using MemoryMatrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemoryMatrix.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            GameSession gameSession;
            if (Session["GameSession"] != null)
            {
                gameSession = (GameSession)Session["GameSession"];
            }
            else
            {
                gameSession = new GameSession();
                gameSession.Level = 1;
                Session["GameSession"] = gameSession;
            }
            ViewBag.gameSession = gameSession;
            ViewBag.matrix = CalculateMatrix(gameSession);
            return View();
        }

        private Matrix CalculateMatrix(GameSession gameSession)
        {
            var matrix = new Matrix();
            matrix.Height = 600; // Hardcoded for now.
            matrix.Width = 600; // Harcoded for now.
            matrix.NoOfTiles = (int)Math.Pow(2 + gameSession.Level, 2);
            return matrix;
        }
    }
}
