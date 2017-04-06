using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerSurveyWeb.Models
{
    public class StatisticsViewModel
    {
        public int SurveyCount { get; set; }
        public int SubmittedCount { get; set; }
        public int UserCount { get; set; }
    }
}