using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JiraDemo;
using System.Text;
using System.Web.Http.Cors;

namespace JiraDemo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "POST")]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        [ActionName("list")]
        public string GetDemo()
        {
            return "lakshay";
        }

        [HttpPost]
        [ActionName("list1")]
        public JObject PostAllProducts(Filter filter)
        {
            
            var client = new WebClient();
            //client.Headers.Add("Access-Control-Allow-Origin", "*");
            //client.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            //client.Headers.Add("Access-Control-Allow-Methods", "POST");
            //client.Headers.Add("Access-Control-Request-Headers","POST");
            client.Credentials = new NetworkCredential("thinksysuser", "thinksys@123");
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("thinksysuser:thinksys@123"));
            client.Headers[HttpRequestHeader.Authorization] = string.Format("Basic ", credentials);
            var response = client.DownloadString("http://10.101.21.116:8080/rest/api/2/search?jql="+filter.getFilter);
            var jresult = JObject.Parse(response);
            // var j = jresult["issues"][0]["fields"]["project"]["name"];

            //var jarray = JArray.Parse(outputOfproduct);

            List<String> projectNames = new List<String>();

            List<String> uniqueProjectNames = new List<String>();
            var Count = new Dictionary<string, int>();

            for (var i = 0; i < jresult["issues"].Count(); i++)
            {
                projectNames.Add((string)jresult["issues"][i]["fields"]["project"]["name"]);
                if (uniqueProjectNames.Contains(projectNames[i]))
                {

                }
                else
                {
                    uniqueProjectNames.Add(projectNames[i]);
                    Console.WriteLine(uniqueProjectNames);
                }
            }

            List<Issue> Issue1 = new List<Issue>();
            var Issue = new Dictionary<string, List<Issue>>();

            for (var j = 0; j < uniqueProjectNames.Count(); j++)
            {
                var initialCount = 0;
                for (var k = 0; k < projectNames.Count(); k++)
                {
                    if (uniqueProjectNames[j] == projectNames[k])
                    {

                        initialCount++;
                        Count[uniqueProjectNames[j]] = initialCount;
                        Console.WriteLine(Count[uniqueProjectNames[j]]);
                    }
                    else
                    {

                    }
                }

            }

            //JObject issueList = new JObject();
            //issueList["issueList"] = JToken.FromObject(Issue);

            List<IssuesInEachProject> issuesInEachProject = new List<IssuesInEachProject>();
            for (int l = 0; l < uniqueProjectNames.Count(); l++)
            {
                Issue1.Clear();
                Issue.Clear();
                for (var k = 0; k < projectNames.Count(); k++)
                {
                    if (uniqueProjectNames[l] == projectNames[k])
                    {
                        Issue1.Add(new Issue { Name = (string)jresult["issues"][k]["fields"]["summary"], Id = (int)jresult["issues"][k]["id"] });
                    }
                }

                Issue[uniqueProjectNames[l]] = Issue1;
                issuesInEachProject.Add(new IssuesInEachProject { ProjectName = uniqueProjectNames[l], NoOfIssues = Count[uniqueProjectNames[l]], Issues = JToken.FromObject(Issue) });
            }

            //var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //string outputofproduct = serializer.Serialize(issuesInEachProject);

            JObject outputofproduct = new JObject();
            outputofproduct["issue"] = JToken.FromObject(issuesInEachProject);

            return outputofproduct;
        }

        public class Hello
        {
            public string Hello123 { get; set; }
        }
    }
}
