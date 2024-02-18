using DefectsMVC.Models;
using ImageRecognition;
using ImageRecognitionMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using System.Diagnostics;
using Tensorflow.Keras.Engine;

namespace ImageRecognitionMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PredictionEnginePool<MLImageModel.ModelInput, MLImageModel.ModelOutput> _predictionImage;

        public HomeController(ILogger<HomeController> logger,
            PredictionEnginePool<MLImageModel.ModelInput, MLImageModel.ModelOutput> predictionImage)
        {
            _logger = logger;
            _predictionImage = predictionImage;
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Файл не выбран.");
            }

            using MemoryStream ms = new MemoryStream();
            file.OpenReadStream().CopyTo(ms);
            var bytes = ms.ToArray();

            MLImageModel.ModelInput sampleImage = new MLImageModel.ModelInput()
            {
                ImageSource = bytes
            };

            var predictImage = _predictionImage.Predict(sampleImage);
            var labels = ModelLabel.GetLabels();

            for (int i = 0; i < predictImage.Score.Length; i++)
            {
                var label = labels.FirstOrDefault(x => x.Id == i);

                if (label != null) label.Score = (int)Math.Round(predictImage.Score[i] * 100);
            }

            labels = labels.OrderByDescending(x => x.Score).Take(2).ToList();

            return Json(new { labels });
        }

        [HttpPost]
        public IActionResult Index(IFormFile file)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(null);
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}