using MemoryMatrix.Constants;
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
            int tilesPerRow = (2 + (int)Math.Ceiling((gameSession.Level / 2.0)));

            // Matrix height is calculated as Initial Height + 1px border and 1px margin on both side horizontally of each cell.
            matrix.Height = Dimensions.INITIAL_MATRIX_HEIGHT + (tilesPerRow * 4);
            // Matrix height is calculated as Initial Width + 1px border and 1px margin on both size vertically of each cell.
            matrix.Width = Dimensions.INITIAL_MATRIX_WIDTH + (tilesPerRow * 4);
            matrix.NoOfTiles = (int)Math.Pow(tilesPerRow, 2);

            matrix.CellHeight = Dimensions.INITIAL_MATRIX_HEIGHT / (((int)Math.Ceiling((gameSession.Level / 2.0)) - 1) + 3);
            matrix.CellWidth = Dimensions.INITIAL_MATRIX_WIDTH / (((int)Math.Ceiling((gameSession.Level / 2.0)) - 1) + 3);
            return matrix;
        }
    }
}
