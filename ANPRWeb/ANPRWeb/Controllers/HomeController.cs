using ANPRWeb.Helpers;
using IronOcr;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleLPR2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ANPRWeb.Controllers
{
    public class Vechicle
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public string Alerts { get; set; }
        public string imgPath { get; set; }
        public string imgName { get; set; }
        public List<PreviousVechicle> previous = new List<PreviousVechicle>();

    }
    public class PreviousVechicle
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public string Alerts { get; set; }
        public string imgPath { get; set; }
        public string imgName { get; set; }

    }
    public class HomeController : Controller
    {
        public static Vechicle objectVechicle = new Vechicle();
        string currentFile = string.Empty;
        public ActionResult Index()
        {
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

            fileSystemWatcher.Path = @"C:\Users\ajanthan.thangarajah\source\repos\ANPRWeb\ANPRWeb\CarImages";

            fileSystemWatcher.Created += FileSystemWatcher_Created;

            //fileSystemWatcher.Renamed += FileSystemWatcher_Renamed; ;

            //fileSystemWatcher.Deleted += FileSystemWatcher_Deleted; ;

            fileSystemWatcher.EnableRaisingEvents = true;

            return View(objectVechicle);
        }
        public JsonResult getVechile()
        {
            return Json(objectVechicle, JsonRequestBehavior.AllowGet);
        }
        public bool sendEmail()
        {
            return EmailUtility.SendMail("abirami@gmail.com", "Vehicle detected", "Following vechicle found with alert");
        }
        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File created: {0}", e.Name);
        }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine("File created: {0}", e.Name);
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (!e.Name.EndsWith("jpg") || currentFile == e.Name)
                return;

            currentFile = e.Name;
            try
            {
                Task<string> recognizeTask = Task.Run(() => ProcessImage($@"C:\Users\ajanthan.thangarajah\source\repos\ANPRWeb\ANPRWeb\CarImages\{e.Name}"));
                recognizeTask.Wait();
                string task_result = recognizeTask.Result;
                dynamic jsonResult = JsonConvert.DeserializeObject(task_result);
                

                if (!string.IsNullOrWhiteSpace(objectVechicle.Number))
                {
                    objectVechicle.previous.Add(new PreviousVechicle {
                        Number = objectVechicle.Number,
                        Make = objectVechicle.Make,
                        Model = objectVechicle.Model,
                        Color = objectVechicle.Color,
                        Year = objectVechicle.Year,
                        Alerts = objectVechicle.Alerts,
                        imgPath = objectVechicle.imgPath,
                        imgName= objectVechicle.imgName,
                    });
                }
                foreach (JProperty app in jsonResult)
                {
                    if (app.Name == "results")
                    {
                        objectVechicle.Number = app.Value[0]["plate"].ToString();
                    }
                }
                
                objectVechicle.Make = "BMW";
                objectVechicle.Model = "Z4";
                objectVechicle.Color = "Gray";
                objectVechicle.Year = "2018";
                objectVechicle.Alerts = "New Car";
                objectVechicle.imgPath = $"/CarImages/{e.Name}";
                objectVechicle.imgName = e.Name;
                System.Console.WriteLine(task_result);
            }
            catch (Exception ex){ var aa = ex; };

            /*
            ISimpleLPR lpr = SimpleLPR.Setup();
            // Set the product key
            //lpr.set_productKey("productkey.xml");
            // Enable Germany, disable Spain and United Kingdom
            lpr.set_countryWeight("Germany", 1.0f);
            lpr.set_countryWeight("UK-GreatBritain", 1.0f);
            // Apply changes
            lpr.realizeCountryWeights();
            // Create Processor
            IProcessor proc = lpr.createProcessor();
            // Process source file
            List<Candidate> cds = proc.analyze($@"C:\Temp\{e.Name}",100);
            if (cds.Count > 0)
            {
                // Iterate over all candidates
                foreach (Candidate cd in cds)
                    Console.Write(" [{0}, {1}]", cd.text, cd.confidence);
                Console.WriteLine();
            }
            else
                Console.WriteLine("Nothing detected");
            Console.WriteLine("File created: {0}", e.Name);
            */
        }
        private static readonly HttpClient client = new HttpClient();
        public static async Task<string> ProcessImage(string image_path)
        {
            string SECRET_KEY = "sk_f9a2f3823b876e7b75a4ca6f";

            Byte[] bytes = System.IO.File.ReadAllBytes(image_path);
            string imagebase64 = Convert.ToBase64String(bytes);

            var content = new StringContent(imagebase64);

            var response = await client.PostAsync("https://api.openalpr.com/v2/recognize_bytes?recognize_vehicle=1&country=us&secret_key=" + SECRET_KEY, content).ConfigureAwait(false);

            var buffer = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            var byteArray = buffer.ToArray();
            var responseString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

            return responseString;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}