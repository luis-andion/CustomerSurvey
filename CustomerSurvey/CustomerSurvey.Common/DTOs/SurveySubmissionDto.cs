using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerSurvey.Common.DTOs
{
    public class SurveySubmissionDto
    {
        public int QuestionId { get; set; }
        public string QuestionValue { get; set; }
        public int? OptionId { get; set; }
        public string OptionValue { get; set; }
        public string Answer { get; set; }
    }
}
