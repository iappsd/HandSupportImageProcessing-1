using HandSupportLib.ImageProcessing.Models;
using HandSupportLib.ImageProcessing.Processing;
using HandSupportLib.ImageProcessing.WebDemo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using static HandSupportLib.ImageProcessing.Enums.ImageEnum;
using static System.Net.Mime.MediaTypeNames;

namespace HandSupportLib.ImageProcessing.WebDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;
        //private readonly IOptions<ReportSettings> _reportSettings;


        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment) // IOptions<ReportSettings> reportSettings
        {
            _logger = logger;
            _environment = environment;
            //  _reportSettings = reportSettings;
        }


        /// <summary>
        /// وضع نص على الصورة
        /// </summary>
        /// <returns></returns>
        [HttpGet("AddTextToImage")]
        public ActionResult AddTextToImage(CardViewModel card)
        {
            var generateImageCard = new GenerateImageCard($"{_environment.WebRootPath}\\assets\\images\\land.jpg",
                $"{_environment.WebRootPath}\\assets\\fonts");
            (int width, int height) cardSize = generateImageCard.GetCardSize();
            int centerX = cardSize.width / 2;
            //x 1243.6666870117188 : Y 1561.8021240234375
            double myFontSize = Math.Round(57 * 0.75 + 57, 0);
            var texts = new List<DrawTextOptions>()
            {
                 new DrawTextOptions(card.positionX, card.positionY, HorizontalAlignment.Center , card.text , card.fontSize, card.hexColor, FontStyle.Bold, "" , TextDirection.RTL , true ,  "AraHamahAlislam-Regular.otf"),
            };

            var images = new List<DrawImageOptions>() {
            new DrawImageOptions($"{_environment.WebRootPath}\\assets\\logos\\logo-demo-1.png", 100, cardSize.height - 200, 200, 200),
            new DrawImageOptions($"{_environment.WebRootPath}\\assets\\logos\\logo-demo-2.png", 400, cardSize.height / 3 , 200, 200),
            };

            var imageResult = generateImageCard.GetCardAsByteArray(texts);

            return File(imageResult.ToArray(), "application/image", "AddTextToImage-demo.jpg");
        }


        public IActionResult Index()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
