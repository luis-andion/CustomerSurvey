using CustomerSurvey.Common.DTOs;
using CustomerSurvey.Data;
using CustomerSurvey.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CustomerSurvey.Repository.Classes
{
    // Using EF Lazy Loading - This is not the best practice but I don't have time to use Eager loading
    // Since Lazy Loading is in use the Survey will get loaded with all the Questions and each Question 
    // will get loaded with all the Options
    // TODO: Switch to use Eager loading by removing virtual from methods on model classes and using Include
    // extension method. For more details check this article: 
    // https://msdn.microsoft.com/en-us/library/jj574232(v=vs.113).aspx 

    public class CustomerSurveyRepository :
        GenericRepository<CustomerSurveyDBEntities, Survey>, ICustomerSurveyRepository
    {

        public List<Survey> GetCustomerSurveyCollection(int? surveyId)
        {
            return Context.Surveys.Where(s => (s.id == surveyId || surveyId == null)).ToList();
        }

        public bool SaveCustomerSurveyAnswers(List<Answer> customerSurveyAnswers)
        {
            try
            {
                foreach (Answer answer in customerSurveyAnswers)
                {
                    Context.Answers.Add(answer);
                }

                var count = Context.SaveChanges();
                return count == customerSurveyAnswers.Count;
            }
            catch(Exception ex)
            {
                // TODO: log error 
                return false;
            }
        }

        public List<DateTime> GetSurveySubmissionsDates(int surveyId)
        {
            var submissionDates = (from a in Context.Answers
                                    join q in Context.Questions on a.qid equals q.id
                                    join s in Context.Surveys on q.sid equals s.id
                                    where s.id == surveyId
                                    select a.date).Distinct().ToList();

            return submissionDates;
        }

        public List<SurveySubmissionDto> GetSurveySubmissions(int surveyId, DateTime date)
        {
            // TODO: implement left join with Answers to include FreeText
            /*
            select q.id, q.value, o.id, o.value, a.answer
            from Answer a
            left join [Option] o on a.oid = o.id
            inner join Question q on a.qid = q.id
            inner join Survey s on q.sid = s.id
            where s.id = 1 and
            [date] = '2017-04-05 10:47:40.730'
            */

            var submissions = (from a in Context.Answers.Where(x => DbFunctions.DiffMinutes(date, x.date) == 0)
                               join o in Context.Options on a.oid equals o.id
                               join q in Context.Questions on a.qid equals q.id
                               join s in Context.Surveys on q.sid equals s.id
                               where s.id == surveyId
                               orderby q.qorder, o.oporder
                               select new SurveySubmissionDto
                               {
                                   OptionId = o.id,
                                   OptionValue = o.value,
                                   QuestionId = q.id,
                                   QuestionValue = q.value,
                                   Answer = a.answer1
                               }).ToList();

            return submissions;
        }
    }
}
