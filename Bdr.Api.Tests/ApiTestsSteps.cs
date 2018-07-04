using Flurl.Http;
using System;
using System.Linq;
using System.Net.Http;
using TechTalk.SpecFlow;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bdr.Api.Tests
{
    [Binding]
    public class ApiTestsSteps
    {
        [Given(@"path ""(.*)""")]
        public void GivenPath(string path)
        {
            CurrentRequest = new FlurlRequest($"http://localhost:5000/{path}");
        }

        [Given(@"query string ""(.*)"" = ""(.*)""")]
        public void GivenQueryString(string key, int value)
        {
            CurrentRequest.SetQueryParam(key, value);
        }

        [When(@"I ""(.*)""")]
        public void WhenI(string httpVerb)
        {
            HttpContent content = null;
            var method = GetHttpMethod(httpVerb);
            
            if (DataToSend != null)
                content = new StringContent(JsonConvert.SerializeObject(DataToSend), System.Text.Encoding.UTF8, "application/json");

            CurrentResponse = CurrentRequest.SendAsync(method, content).Result;
        }

        [Then(@"the response body should be")]
        public void ThenTheResponseBodyShouldBe(Table table)
        {
            var actualJson = CurrentResponse.Content.ReadAsStringAsync().Result;
            var tableData = table.Rows.Select(RowToJson);
            var expectedJson = JsonConvert.SerializeObject(tableData);
            Assert.AreEqual(expectedJson, actualJson);
        }

        [Given(@"body")]
        public void GivenBody(Table table)
        {
            DataToSend = table.Rows.Select(RowToJson).First();
        }

        [Then(@"the response code is ""(.*)""")]
        public void ThenTheResponseCodeIs(int statusCode)
        {
            Assert.AreEqual(statusCode, (int)CurrentResponse.StatusCode);
        }


        private JObject RowToJson(TableRow row)
        {
            var data = row.ToDictionary(r => r.Key, r => r.Value);
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            return JObject.Parse(json);
        }

        private HttpMethod GetHttpMethod(string method)
        {
            switch(method.ToUpperInvariant())
            {
                case "GET":
                    return HttpMethod.Get;
                case "POST":
                    return HttpMethod.Post;
                default:
                    throw new NotSupportedException();
            }
        }

        private FlurlRequest CurrentRequest
        {
            set
            {
                ScenarioContext.Current.Add("Request", value);
            }
            get
            {
                return ScenarioContext.Current.Get<FlurlRequest>("Request");
            }
        }

        private HttpResponseMessage CurrentResponse
        {
            set
            {
                ScenarioContext.Current.Add("Response", value);
            }
            get
            {
                return ScenarioContext.Current.Get<HttpResponseMessage>("Response");
            }
        }

        private object DataToSend
        {
            set
            {
                ScenarioContext.Current.Add("Data", value);
            }
            get
            {
                if(ScenarioContext.Current.ContainsKey("Data"))
                    return ScenarioContext.Current.Get<object>("Data");
                return null;
            }
        }
    }
}
