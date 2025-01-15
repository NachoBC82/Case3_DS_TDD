using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace calculator.frontend.tests.steps
{
    [Binding]
    public class SquareRootSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public SquareRootSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the number is (.*)")]
        public async Task GivenTheNumberIs(int number) 
        {
            _scenarioContext.Add("number", number);            
        }

        [When(@"the square root is calculated")]
        public async Task SquareRootIsCalculated()
        {
            IPage page = _scenarioContext.Get<IPage>("page");
            var base_url = _scenarioContext.Get<string>("urlBase");
            var number = _scenarioContext.Get<int>("number");
            await page.GotoAsync($"{base_url}/Attribute");
            await page.FillAsync("#number", number.ToString());
            await page.ClickAsync("#attribute");
        }

        [Then(@"the square root result should be (.*)")]
        public async Task ThenTheSquareRootResultShouldBe(string expectedResult)
        {
            var page = (IPage)_scenarioContext["page"];
            var resultText = await page.InnerTextAsync("#sqrt");
            var americanDouble = expectedResult.Replace(",", ".");
            var latinDouble = expectedResult.Replace(".", ",");
            var ok = expectedResult.Equals(americanDouble) ||
                expectedResult.Equals(latinDouble);
            Assert.True(ok, $"expected {expectedResult} but actual {resultText}");
        }
    }
}
