using VoteM.Models.Votes.Serializable;

namespace VoteM.Models.Votes
{
    public class VoteMeta
    {
        public string Header { get; }
        public bool IsPinned { get; set; }
        public string Description { get; }
        public VoteMeta(string header, string description)
        {
            Header = header;
            IsPinned = false;
            Description = description;
        }
        public VoteMeta(SVoteMetaV1 sVoteMeta)
        {
            Header = sVoteMeta.Header;
            IsPinned = sVoteMeta.IsPinned;
            Description = sVoteMeta.Description;
        }
    }
}
