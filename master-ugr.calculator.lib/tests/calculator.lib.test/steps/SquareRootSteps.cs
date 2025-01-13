using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [Given(@"the number is (.*)")]
        public void GivenTheNumberIs(double number)
        {
            _scenarioContext.Add("number", number);
        }

        [When(@"the square root is calculated")]
        public void WhenTheSquareRootIsCalculated()
        {
            var number = _scenarioContext.Get<double>("number");
            var result = NumberAttributter.SquareRoot(number);
            _scenarioContext.Add("result", result);
        }

        [Then(@"the square root result should be (.*)")]
        public void ThenTheSquareRootResultShouldBe(double expectedResult)
        {
            var result = _scenarioContext.Get<double>("result");
            Assert.Equal(expectedResult, result);
        }
    }
}