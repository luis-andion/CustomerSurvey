using System;

namespace CustomerSurvey.Common.DTOs
{
    public class AnswerDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int QuestionId { get; set; }
        public int? OptionId { get; set; }
        public string Answer { get; set; }
        public DateTime Date { get; set; }
    }
}