using CustomerSurvey.Common.DTOs;
using CustomerSurvey.Repository.Interfaces;
using CustomerSurvey.Service.Services;
using CustomerSurveyWeb.Helpers;
using CustomerSurveyWeb.Models;
using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CustomerSurveyWeb.Controllers
{
    public class CustomerSurveyController : Controller
    {
        // Data Repository interface
        private readonly ICustomerSurveyRepository customerSurveyRepository;

        // Service class
        private CustomerSurveyService customerSurveyService;

        /// <summary>
        /// CustomerSurveyController constructor
        /// Following rules apply to Repositories 
        /// - Repositories are mandatory and read only 
        /// - Repositories need to implement interface IGenericRepository
        /// - Repositories need to be injected via Constructor Parameters
        /// - Use of dependency Injection is required
        /// - Ninject is used in this project as the dependency injector
        /// </summary>
        /// <param name="custServRepo">The Customer Survey Repository</param>
        public CustomerSurveyController(ICustomerSurveyRepository custServRepo)
        {
            // Validate if Repositories are valid
            if (custServRepo == null)
            {
                throw new ArgumentNullException("The Customer Service Repository cannot be null");
            }

            // Init Repositories from injected parameters
            customerSurveyRepository = custServRepo;

            // Init Services and inject Repositories as constructor parameters
            customerSurveyService = new CustomerSurveyService(custServRepo);
        }

        // GET: CustomerSurvey
        public ActionResult Index()
        {
            var surveys = customerSurveyService.GetCustomerSurveyCollection(null, true);
            return View(DtoViewModelMapper.ToViewModel(surveys));
        }

        [Authorize]
        public ActionResult Survey(int? id)
        {
            List<SurveyDto> surveyList = id == null ? new List<SurveyDto>() : customerSurveyService.GetCustomerSurveyCollection(id, false);
            return View(DtoViewModelMapper.ToViewModel(surveyList.FirstOrDefault()));
        }

        [Authorize]
        public ActionResult Submission(int surveyId, DateTime submissionDate)
        {
            var surveySubmissionList = customerSurveyService.GetSurveySubmissions(surveyId, submissionDate);
            return View(surveySubmissionList);
        }

        /// <summary>
        /// Method in charge of saving the Survey Submissions to the Database
        /// Important points to notice
        /// - Use of [Authorize] attribute to force user to be register
        /// - Use of ValidateAntiForgeryToken to prevent Cross Site Request Forgery attacks
        /// https://docs.microsoft.com/en-us/aspnet/mvc/overview/security/xsrfcsrf-prevention-in-aspnet-mvc-and-web-pages
        /// - Use of AntiXssLibrary and HtmlSantizationLibrary to prevent Cross Site Scripting attacks
        /// http://www.dotnet-programming.com/post/2015/04/12/How-to-Handle-Cross-Site-Scripting-in-ASPNET-MVC-Application.aspx
        /// - Use of [ValidateInput(false)] attribute to prevent ASP.NET validation and avoid System.Web.HttpRequestValidationException
        /// But I'm sanitizing the input and removing any malicius script inserted in it: 
        /// Sanitizer.GetSafeHtmlFragment(freeTextAnswer)
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost, Authorize, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Submit(FormCollection form) //public ActionResult Submit(SurveyViewModel surveyViewModel)
        {
            // Due to lack of time I will be using FormCollection
            // TODO: Use MVC Model Binding for a more ellegant solution 
            // For that purpose we need to use Html helpers and naming convention required by the binding engine
            // @Html.TextAreaFor(model => model.Value);

            if (!ModelState.IsValid)
            {
                ViewBag.SurveySubmitResult = "Invalid Mode State";
                return View("SurveySubmitResult");
            }

            int surveyId;
            if (form.HasKeys() && form["Id"] != null && int.TryParse(form["Id"], out surveyId))
            {
                var customerSurveyAnswers = new List<AnswerDto>();
                var currentDate = DateTime.Now;
                var userName = User.Identity.Name;

                foreach (var key in form.AllKeys)
                {
                    int questionId;
                    var question = key.Split('.');
                    if (question.Length != 2 || !int.TryParse(question[1], out questionId))
                    {
                        continue;
                    }

                    int optionId;
                    switch (question[0])
                    {
                        case "SingleChoice":
                        case "NumericScale":
                            if (!int.TryParse(form[key], out optionId))
                            {
                                continue;
                            }

                            customerSurveyAnswers.Add(new AnswerDto
                            {
                                UserName = userName,
                                QuestionId = questionId,
                                OptionId = optionId,
                                Date = currentDate
                            });

                            break;
                        case "MultipleChoice":
                            var answers = form[key].Split(',');
                            foreach (var answer in answers)
                            {
                                if (!int.TryParse(answer, out optionId))
                                {
                                    continue;
                                }

                                customerSurveyAnswers.Add(new AnswerDto
                                {
                                    UserName = userName,
                                    QuestionId = questionId,
                                    OptionId = optionId,
                                    Date = currentDate
                                });
                            }

                            break;
                        case "FreeText":
                            var freeTextAnswer = form[key].Trim();
                            if (string.IsNullOrEmpty(freeTextAnswer))
                            {
                                continue;
                            }

                            customerSurveyAnswers.Add(new AnswerDto
                            {
                                UserName = userName,
                                QuestionId = questionId,
                                OptionId = null,
                                Date = currentDate,
                                Answer = Sanitizer.GetSafeHtmlFragment(freeTextAnswer) // Sanitizing the answer by removing any script inserted in it
                            });

                            break;
                    }
                }

                ViewBag.SurveySubmitResult = "No answers have been submitted. Please try again and answer the survey";
                if (customerSurveyAnswers.Any())
                {
                    if (!customerSurveyService.SaveCustomerSurveyAnswers(customerSurveyAnswers))
                    {
                        ViewBag.SurveySubmitResult = "An error have occurred while submitting the survey. Please contact Admin for more details";
                    }
                    else
                    {
                        ViewBag.SurveySubmitResult = "Thank you for your cooperation. The survey have been Successfully submitted!";
                    }
                }
            }

            return View("SurveySubmitResult");
        }
    }
}