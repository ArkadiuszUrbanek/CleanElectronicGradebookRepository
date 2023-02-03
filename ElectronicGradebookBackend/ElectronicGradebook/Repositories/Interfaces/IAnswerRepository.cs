namespace ElectronicGradebook.Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        Task UpdateSelectionTimes(HashSet<int> selectedAnswersIds);
    }
}
