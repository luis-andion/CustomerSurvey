using System;
using System.Collections.Generic;

namespace CustomerSurvey.Common.DTOs
{
    public class SurveyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalSubmissions { get; set; }
        public ICollection<DateTime> SubmissionDates { get; set; }

        public ICollection<QuestionDto> Questions { get; set; }
    }
}
