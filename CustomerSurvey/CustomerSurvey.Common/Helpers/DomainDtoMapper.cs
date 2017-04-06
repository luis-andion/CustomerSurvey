using CustomerSurvey.Common.DTOs;
using CustomerSurvey.Data;
using System.Collections.Generic;

namespace CustomerSurvey.Common.Helpers
{
    // TODO: Consider using AutoMapper to do the mapping
    // Mapping from Database object to Dto object

    public static class DomainDtoMapper
    {
        public static List<SurveyDto> ToDto(ICollection<Survey> surveys)
        {
            var surveyList = new List<SurveyDto>();

            foreach (Survey survey in surveys)
            {
                surveyList.Add(ToDto(survey));
            }

            return surveyList;
        }

        public static SurveyDto ToDto(Survey survey)
        {
            if (survey == null)
            {
                return null;
            }

            return new SurveyDto
            {
                Id = survey.id,
                Name = survey.name,
                Description = survey.description,
                TotalSubmissions = 0, // To be filled out later on demand by controller
                Questions = ToDto(survey.Questions)
            };
        }

        public static List<QuestionDto> ToDto(ICollection<Question> questions)
        {
            if (questions == null)
            {
                return null;
            }

            var questionList = new List<QuestionDto>();

            foreach (Question question in questions)
            {
                questionList.Add(ToDto(question));
            }

            return questionList;
        }

        public static QuestionDto ToDto(Question question)
        {
            if (question == null)
            {
                return null;
            }

            return new QuestionDto
            {
                Id = question.id,
                Value = question.value,
                QuestionOrder = question.qorder,
                QuestionType = new QuestionTypeDto { Id = question.QuestionType.id, Value = question.QuestionType.value },
                Options = ToDto(question.Options)
            };
        }

        public static List<OptionDto> ToDto(ICollection<Option> options)
        {
            if (options == null)
            {
                return null;
            }

            var optionList = new List<OptionDto>();

            foreach (Option option in options)
            {
                optionList.Add(ToDto(option));
            }

            return optionList;
        }

        public static OptionDto ToDto(Option option)
        {
            if (option == null)
            {
                return null;
            }

            return new OptionDto
            {
                Id = option.id,
                Value = option.value,
                Description = option.description,
                OptionOrder = option.oporder
            };
        }

        public static List<Answer> ToDomain(ICollection<AnswerDto> answers)
        {
            if (answers == null)
            {
                return null;
            }

            var answerList = new List<Answer>();

            foreach (AnswerDto answerDto in answers)
            {
                answerList.Add(ToDomain(answerDto));
            }

            return answerList;
        }

        public static Answer ToDomain(AnswerDto answer)
        {
            if (answer == null)
            {
                return null;
            }

            return new Answer
            {
                uname = answer.UserName,
                qid = answer.QuestionId,
                oid = answer.OptionId,
                answer1 = answer.Answer,
                date = answer.Date                
            };
        }
    }
}