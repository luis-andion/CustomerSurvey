using CustomerSurvey.Common.DTOs;
using CustomerSurvey.Data;
using System;
using System.Collections.Generic;

namespace CustomerSurvey.Repository.Interfaces
{
    public interface ICustomerSurveyRepository : IGenericRepository<Survey>
    {
        List<Survey> GetCustomerSurveyCollection(int? surveyId);

        bool SaveCustomerSurveyAnswers(List<Answer> customerSurveyAnswers);
        List<DateTime> GetSurveySubmissionsDates(int surveyId);
        List<SurveySubmissionDto> GetSurveySubmissions(int surveyId, DateTime date);
    }
}
