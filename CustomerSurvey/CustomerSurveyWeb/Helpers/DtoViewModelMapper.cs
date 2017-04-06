using CustomerSurvey.Common.DTOs;
using CustomerSurveyWeb.Models;
using System.Collections.Generic;

namespace CustomerSurveyWeb.Helpers
{
    // TODO: Consider using AutoMapper to do the mapping
    // Mapping from Dto object to ModelView object

    public static class DtoViewModelMapper
    {
        public static List<SurveyViewModel> ToViewModel(ICollection<SurveyDto> surveys)
        {
            var surveyModels = new List<SurveyViewModel>();

            foreach (SurveyDto surveyDto in surveys)
            {
                surveyModels.Add(ToViewModel(surveyDto));
            }

            return surveyModels;
        }

        public static SurveyViewModel ToViewModel(SurveyDto survey)
        {
            if (survey == null)
            {
                return null;
            }

            return new SurveyViewModel
            {
                Id = survey.Id,
                Name = survey.Name,
                Description = survey.Description,
                TotalSubmissions = survey.TotalSubmissions,
                SubmissionDates = survey.SubmissionDates,
                Questions = ToViewModel(survey.Questions)
            };
        }

        public static List<QuestionVm> ToViewModel(ICollection<QuestionDto> questions)
        {
            if (questions == null)
            {
                return null;
            }

            var questionViewModels = new List<QuestionVm>();

            foreach (QuestionDto questionDto in questions)
            {
                questionViewModels.Add(ToViewModel(questionDto));
            }

            return questionViewModels;
        }

        public static QuestionVm ToViewModel(QuestionDto question)
        {
            if (question == null)
            {
                return null;
            }

            QuestionType questionType = QuestionType.FreeText;
            switch (question.QuestionType.Id)
            {
                case 1:
                    questionType = QuestionType.NumericScale;
                    break;
                case 2:
                    questionType = QuestionType.SingleChoice;
                    break;
                case 3:
                    questionType = QuestionType.MultipleChoice;
                    break;
                case 4:
                    questionType = QuestionType.FreeText;
                    break;
            }

            return new QuestionVm
            {
                Id = question.Id,
                Value = question.Value,
                QuestionOrder = question.QuestionOrder,
                QuestionType = questionType,
                Options = ToViewModel(question.Options)
            };
        }

        public static List<OptionVm> ToViewModel(ICollection<OptionDto> options)
        {
            if (options == null)
            {
                return null;
            }

            var optionViewModels = new List<OptionVm>();

            foreach (OptionDto optionDto in options)
            {
                optionViewModels.Add(ToViewModel(optionDto));
            }

            return optionViewModels;
        }

        public static OptionVm ToViewModel(OptionDto option)
        {
            if (option == null)
            {
                return null;
            }

            return new OptionVm
            {
                Id = option.Id,
                Value = option.Value,
                Description = option.Description,
                OptionOrder = option.OptionOrder
            };
        }
    }
}