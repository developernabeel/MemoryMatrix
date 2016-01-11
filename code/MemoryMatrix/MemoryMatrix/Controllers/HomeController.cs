using MemoryMatrix.Constants;
using MemoryMatrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MemoryMatrix.Helpers;

namespace MemoryMatrix.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            Session.Abandon();
            return View();
        }

        [HttpGet]
        public ActionResult Matrix()
        {
            GameSession gameSession;

            string mode = "c";
            if (Request.QueryString["mode"].IsStringInSet(new[] { "m" }))
            {
                mode = Request.QueryString["mode"];
            }

            if (Session["GameSession"] != null)
            {
                gameSession = (GameSession)Session["GameSession"];

                // If mode changed in between game, restart session
                if (gameSession.Mode != mode)
                    gameSession.Level = 1;
            }
            else
            {
                gameSession = new GameSession();
                gameSession.Level = 1;
            }

            gameSession.Mode = mode;
            Session["GameSession"] = gameSession;

            ViewBag.gameSession = gameSession;
            ViewBag.matrix = CalculateMatrix(gameSession);
            return View();
        }

        [HttpPost]
        public ActionResult ChangeLevel(int level)
        {
            var gameSession = (GameSession)Session["GameSession"];
            gameSession.Level = level;
            Session["GameSession"] = gameSession;
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ClearSession()
        {
            Session.Abandon();
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private Matrix CalculateMatrix(GameSession gameSession)
        {
            var matrix = new Matrix();
            int unitIncrement = (int)Math.Ceiling((gameSession.Level / 2.0));
            int tilesPerRow = 2 + unitIncrement;

            // Matrix height is calculated as Initial Height + 1px border and 1px margin on both side horizontally of each cell.
            matrix.Height = Dimensions.INITIAL_MATRIX_HEIGHT + (tilesPerRow * 4) - (4 + (3 * (unitIncrement - 1)));
            // Matrix height is calculated as Initial Width + 1px border and 1px margin on both size vertically of each cell.
            matrix.Width = Dimensions.INITIAL_MATRIX_WIDTH + (tilesPerRow * 4) - (4 + (2 * (unitIncrement - 1)));
            matrix.NoOfTiles = (int)Math.Pow(tilesPerRow, 2);

            matrix.CellHeight = Dimensions.INITIAL_MATRIX_HEIGHT / (((int)Math.Ceiling((gameSession.Level / 2.0)) - 1) + 3);
            matrix.CellWidth = Dimensions.INITIAL_MATRIX_WIDTH / (((int)Math.Ceiling((gameSession.Level / 2.0)) - 1) + 3);
            return matrix;
        }
    }
}
