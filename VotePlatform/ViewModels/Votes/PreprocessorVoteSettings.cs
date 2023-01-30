namespace VotePlatform.ViewModels.Votes
{
    public class PreprocessorVoteSettings
    {
        public string OwnerOrganization { get; }
        public int CountAnswers { get; }
        public PreprocessorVoteSettings(string ownerOrganization, int countAnswers)
        {
            OwnerOrganization = ownerOrganization;
            CountAnswers = countAnswers;
        }
    }
}
