using System;
using creditPreCheck.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace creditPreCheck.Tests.Helpers
{
    public static class Stubs
    {
        public static string GetCacheKey()
        {
            return "TestFirstNameTestLastName20000101";
        }

        public static User GetInputUser()
        {
            return new User
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                DoB = DateTime.Parse("01-01-2000"),
                Income = 31000
            };
        }

        public static User GetIncorrectInputUser()
        {
            return new User
            {
                FirstName = "TestFirstName",
                DoB = DateTime.Parse("01-01-2000"),
                Income = 31000
            };
        }

        public static User GetFullUser()
        {
            return new User
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                DoB = DateTime.Parse("01-01-2000"),
                Income = 31000,
                UserId = 1,
                Eligibility = new Eligibility
                {
                    Card = new Card
                    {
                        Id = 1,
                        APR = 0.5M,
                        Message = "Card Message",
                        MinAge = 18,
                        IncomeThreshold = 30000,
                        Name = "Barclays Card",
                    },
                    CardId = 1,
                    isEligible = true,
                    UserId = 1,
                    Id = 1,
                },
            };
        }

        public static ControllerContext GetControllerContext(string referrer = "Home")
        {
            var request = new Mock<HttpRequest>();
            var headers = new HeaderDictionary { { "Referer", referrer } };
            request.Setup(x => x.Headers).Returns(headers);

            var httpContext = Mock.Of<HttpContext>(_ => _.Request == request.Object);

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            return controllerContext;
        }
    }
}
