using System;
using System.Linq;
using System.Threading.Tasks;
using creditPreCheck.Models;
using creditPreCheck.Utils;
using Microsoft.EntityFrameworkCore;

namespace creditPreCheck.Data
{
    public interface IDataManager
    {
        User AddUser(User user, string cacheKey);
        User GetLastEntry();
        int GetApplicableCard(User user);
    }

    public class DataManager: IDataManager
    {
        private readonly CreditPreCheckContext _context;
        private readonly IUtil _utils;

        public DataManager(CreditPreCheckContext context, IUtil utils)
        {
            _context = context;
            _utils = utils;
        }

        public User AddUser(User user, string cacheKey)
        {
            try
            {
                var isEligible = _utils.IsEligibleForCard(user);
                var cardId = this.GetApplicableCard(user);
                var el = new Eligibility();

                if (isEligible && cardId != -1) el.CardId = cardId;

                el.isEligible = isEligible;
                user.CreatedOn = DateTime.Now;
                user.Eligibility = el;

                _context.Add(user);
                _context.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }

        public int GetApplicableCard(User user)
        {
            var age = _utils.GetAge(user);
            var eligibleCard = _context
                .Card
                .AsQueryable()
                .OrderByDescending(card => card.IncomeThreshold)
                .ToList()
                .Find(card => user.Income > card.IncomeThreshold && age >= card.MinAge);

            return eligibleCard != null ? eligibleCard.Id : -1;
        }

        public User GetLastEntry()
        {
            return _context
                .User
                .Include(user => user.Eligibility)
                .Include(user => user.Eligibility.Card)
                .OrderByDescending(user => user.CreatedOn)
                .FirstOrDefault();
        }
    }
}
