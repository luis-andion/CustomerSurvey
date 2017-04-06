using CustomerSurvey.Common.DTOs;
using CustomerSurvey.Common.Helpers;
using CustomerSurvey.Data;
using CustomerSurvey.Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace CustomerSurvey.Service.Services
{
    public class CustomerSurveyService : GenericService<Survey>
    {
        public CustomerSurveyService(IGenericRepository<Survey> repository) : base(repository)
        {
        }

        public List<SurveyDto> GetCustomerSurveyCollection(int? surveyId, bool includeSubmissionsDates)
        {
            var repository = dataRepository as ICustomerSurveyRepository;
            if (repository != null)
            {
                var surveyCollection = DomainDtoMapper.ToDto(repository.GetCustomerSurveyCollection(surveyId));

                if (includeSubmissionsDates)
                {
                    foreach (var survey in surveyCollection)
                    {
                        survey.SubmissionDates = repository.GetSurveySubmissionsDates(survey.Id);
                        survey.TotalSubmissions = survey.SubmissionDates.Count;
                    }
                }

                return surveyCollection;
            }

            throw new NullReferenceException("The Data repository hasn't been defined");
        }

        public bool SaveCustomerSurveyAnswers(List<AnswerDto> customerSurveyAnswers)
        {
            var repository = dataRepository as ICustomerSurveyRepository;
            if (repository != null)
            {
                return repository.SaveCustomerSurveyAnswers(DomainDtoMapper.ToDomain(customerSurveyAnswers));
            }

            throw new NullReferenceException("The Data repository hasn't been defined");
        }

        public List<SurveySubmissionDto> GetSurveySubmissions(int surveyId, DateTime date)
        {
            var repository = dataRepository as ICustomerSurveyRepository;
            if (repository != null)
            {
                return repository.GetSurveySubmissions(surveyId, date);
            }

            throw new NullReferenceException("The Data repository hasn't been defined");
        }
    }
}
