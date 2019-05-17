using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JiraDemo
{
    public class IssuesInEachProject
    {
        public string ProjectName { get; set; }
        public int NoOfIssues { get; set; }
        public JToken Issues { get; set; }
    }
}