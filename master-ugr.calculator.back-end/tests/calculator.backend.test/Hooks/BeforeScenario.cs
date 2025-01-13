using TechTalk.SpecFlow;
using static System.Net.WebRequestMethods;

namespace calculator.backend.test.Hooks
{
    [Binding]
    public class BeforeScenario
    {
        private readonly ScenarioContext _scenarioContext;
        public BeforeScenario(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        [BeforeScenario]
        public void ScenarioPreparation(ScenarioContext scenarioContext)
        {
            // Getting url from environment variable
            // When not present, default to https://localhost:7012/
            var urlBase =
                Environment.GetEnvironmentVariable("CALCULATOR_BACKEND_URL") ?? "https://localhost:7012/";

            if (!urlBase.EndsWith("/"))
                urlBase += "/";

            _scenarioContext.Add("urlBase", urlBase);
        }
    }
}
