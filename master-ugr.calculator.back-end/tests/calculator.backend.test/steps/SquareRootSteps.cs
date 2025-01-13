using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace calculator.lib.test.steps
{
    [Binding]
    public class SquareRootSteps
    {
        private readonly ScenarioContext _scenarioContext;
        public SquareRootSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        private void ApiCall(string operation)
        {
            using (var client = new HttpClient())
            {
                var urlBase = _scenarioContext.Get<string>("urlBase");
                var number = _scenarioContext.Get<double>("number");
                var url = $"{urlBase}api/Calculator/";
                var api_call = $"{url}{operation}?number={number}";
                var response = client.GetAsync(api_call).Result;
                response.EnsureSuccessStatusCode();
                var responseBody = response.Content.ReadAsStringAsync().Result;
                var jsonDocument = JsonDocument.Parse(responseBody);
                var result = jsonDocument.RootElement.GetProperty("square_root");

                if (result.ValueKind == JsonValueKind.Number)
                    _scenarioContext.Add("square_root", result.GetDouble());
                else if (result.ValueKind == JsonValueKind.String){
                    if (result.GetString() == "NaN")
                        _scenarioContext.Add("square_root", double.NaN);
                }
            }
        }

        [Given(@"the number is (.*)")]
        public void GivenTheNumberIs(double number)
        {
            _scenarioContext.Add("number", number);
        }

        [When(@"the square root is calculated")]
        public void WhenTheSquareRootIsCalculated()
        {
            ApiCall("number_attribute");
        }

        [Then(@"the square root result should be (.*)")]
        public void ThenTheSquareRootResultShouldBe(double expectedResult)
        {
            var result = _scenarioContext.Get<double>("square_root");
            Assert.Equal(expectedResult, result);
        }
    }
}