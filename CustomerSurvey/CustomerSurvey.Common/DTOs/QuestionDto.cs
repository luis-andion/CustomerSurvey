using System.Collections.Generic;

namespace CustomerSurvey.Common.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int QuestionOrder { get; set; }

        public ICollection<OptionDto> Options { get; set; }
        public QuestionTypeDto QuestionType { get; set; }
    }
}
