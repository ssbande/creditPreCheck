using System;
using System.Linq;
using creditPreCheck.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace creditPreCheck.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new CreditPreCheckContext(serviceProvider
                .GetRequiredService<DbContextOptions<CreditPreCheckContext>>());

            if (context.Card.Any()) return;

            context.Card.AddRange(
              new Card
              {
                  Name = "Barclays Card",
                  Message = "Some promotional message for Barclays card ...",
                  APR = 5.0M,
                  MinAge = 18,
                  IncomeThreshold = 3000
              },
              new Card
              {
                  Name = "Vanquis Card",
                  Message = "Some promotional message for Vanquis card ...",
                  APR = 6.3M,
                  MinAge = 18,
                  IncomeThreshold = 0
              }
            );

            context.SaveChanges();
        }
    }
}
