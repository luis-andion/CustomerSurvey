using System;
using System.Collections.Generic;

namespace CustomerSurveyWeb.Models
{
    public enum QuestionType
    {
        NumericScale,
        SingleChoice,
        MultipleChoice,
        FreeText
    }

    public class SurveyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalSubmissions { get; set; }

        public ICollection<DateTime> SubmissionDates { get; set; }
        public List<QuestionVm> Questions { get; set; }
    }

    public class QuestionVm
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int QuestionOrder { get; set; }

        public List<OptionVm> Options { get; set; }
        public QuestionType QuestionType { get; set; }
    }

    public class OptionVm
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int OptionOrder { get; set; }
    }
}