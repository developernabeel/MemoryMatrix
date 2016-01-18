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
            /*
             * JS code to find letter positions 
             var arr =[];$('.red-tile').each(function(i,v){  if($(this).css('z-index') == 1){ var id = $(this).parent().attr('id').split('-')[1]; var idnum = parseInt(id);idnum++; arr.push(idnum );  }return arr;   });
             */

            List<int> letterPositions = new List<int>
            {
                93, 97, 99, 100, 101, 103, 107, 110, 111, 114, 
                115, 116, 119, 121, 136, 137, 139, 140, 142, 
                146, 147, 149, 150, 152, 155, 157, 160, 162, 
                164, 179, 181, 183, 185, 186, 187, 189, 191, 
                193, 195, 198, 200, 201, 202, 203, 205, 206, 
                207, 222, 226, 228, 232, 236, 238, 241, 243, 
                245, 249, 265, 269, 271, 272, 273, 275, 279, 
                282, 283, 286, 289, 292, 350, 354, 357, 358, 
                361, 362, 363, 364, 365, 367, 368, 369, 372, 
                373, 374, 376, 380, 393, 394, 396, 397, 399, 
                402, 406, 410, 413, 416, 420, 422, 436, 438,
                440, 442, 443, 444, 445, 449, 453, 454, 455, 
                456, 459, 464, 479, 483, 485, 488, 492, 496, 
                498, 502, 506, 508, 522, 526, 528, 531, 535, 
                539, 542, 544, 545, 546, 548, 552
            };

            ViewBag.LetterPositions = letterPositions;
            return View();
        }

        [HttpGet]
        public ActionResult Matrix()
        {
            GameSession gameSession;

            string mode = "c";
            if (Request.QueryString["mode"].IsStringInSet(new[] { "p" }))
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
