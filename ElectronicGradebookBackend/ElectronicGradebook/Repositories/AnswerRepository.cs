using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;
using ElectronicGradebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElectronicGradebook.Repositories
{
    public class AnswerRepository: IAnswerRepository
    {
        private readonly ElectronicGradebookDatabaseContext _dbContext;

        public AnswerRepository(ElectronicGradebookDatabaseContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task UpdateSelectionTimes(HashSet<int> selectedAnswersIds)
        {
            var answers = await _dbContext.Answers
                .Where(a => selectedAnswersIds.Contains(a.AnswerId))
                .ToListAsync();

            foreach(var answer in answers)
            {
                answer.TimesSelected++;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
