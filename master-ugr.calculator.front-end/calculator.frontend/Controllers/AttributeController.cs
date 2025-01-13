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
        
        private Dictionary<string,string> ExecuteOperation(string number)
        {
            // List with result
            Dictionary<string,string> result = new Dictionary<string, string>();

            //Raw data for properties to check
            bool? raw_prime = null;
            bool? raw_odd = null;

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
                var prime = json["prime"];
                var odd = json["odd"];
                if (prime != null)
                {
                    raw_prime = prime.Value<bool>();
                }
                if (odd != null)
                {
                    raw_odd = odd.Value<bool>();
                }
            }

            // Fill with value from backend
            var is_prime = GetIsPrime(raw_prime);
            result.Add("Prime",is_prime);
            var is_odd = GetIsOdd(raw_prime);
            result.Add("Odd",is_odd);

            return result;
        }
        
        [HttpPost]
        public ActionResult Index(string number)
        {
            var result = ExecuteOperation(number);
            ViewBag.IsPrime = result["Prime"];
            ViewBag.IsOdd = result["Odd"];
            return View();
        }
    }
}
