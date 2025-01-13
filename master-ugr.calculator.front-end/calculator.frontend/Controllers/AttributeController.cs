using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace calculator.frontend.Controllers
{
    public class AttributeController : Controller
    {
        private string base_url =
            Environment.GetEnvironmentVariable("CALCULATOR_BACKEND_URL") ??
            "https://nachobc82-calculator-backend.azurewebsites.net";
        const string api = "api/Calculator";

        public IActionResult Index()
        {
            return View();
        }

        public string GetIsPrime(bool? raw_data)
        {
            var ret_isPrime = "unknown";
            if (raw_data != null && raw_data.Value)
            {
                ret_isPrime = "Yes";
            }
            else if (raw_data != null && !raw_data.Value)
            {
                ret_isPrime = "No";
            }

            return ret_isPrime;
        }

        public string GetIsOdd(bool? raw_data)
        {            
            var ret_isOdd = "unknown";
            if (raw_data != null && raw_data.Value)
            {
                ret_isOdd = "Yes";
            }
            else if (raw_data != null && !raw_data.Value)
            {
                ret_isOdd = "No";
            }
            return ret_isOdd;
        }

        private string GetSqrt(double? raw_data)
        {
            var ret_sqrt = "Unknow";
            if( raw_data != null)
            {
                ret_sqrt = raw_data.ToString();
            }
            return ret_sqrt;
        }
        
        private Dictionary<string,string> ExecuteOperation(string number)
        {
            // List with result
            Dictionary<string,string> result = new Dictionary<string, string>();

            //Raw data for properties to check
            bool? raw_prime = null;
            bool? raw_odd = null;
            double? raw_sqrt = null;

            // HTTP Client
            var clientHandler = new HttpClientHandler();
            var client = new HttpClient(clientHandler);
            var url = $"{base_url}/api/Calculator/number_attribute?number={number}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
            };
            using (var response = client.Send(request))
            {
                response.EnsureSuccessStatusCode();
                var body = response.Content.ReadAsStringAsync().Result;
                var json = JObject.Parse(body);

                Console.WriteLine(json);
                var prime = json["prime"];
                var odd = json["odd"];
                var sqrt = json["square_root"];

                if (prime != null)
                {
                    raw_prime = prime.Value<bool>();
                }
                if (odd != null)
                {
                    raw_odd = odd.Value<bool>();
                }
                if(sqrt != null)
                {
                    raw_sqrt = sqrt.Value<double>();
                }
            }

            // Fill with value from backend
            var is_prime = GetIsPrime(raw_prime);
            result.Add("Prime",is_prime);
            var is_odd = GetIsOdd(raw_odd);
            result.Add("Odd",is_odd);
            var sqrt_result = GetSqrt(raw_sqrt);
            result.Add("Sqrt", sqrt_result);           

            return result;
        }
        
        [HttpPost]
        public ActionResult Index(string number)
        {
            var result = ExecuteOperation(number);
            ViewBag.IsPrime = result["Prime"];
            ViewBag.IsOdd = result["Odd"];
            ViewBag.Sqrt = result["Sqrt"];
            return View();
        }
    }
}
