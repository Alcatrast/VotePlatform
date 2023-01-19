using VoteM.Models.Votes.Serializable;

namespace VoteM.Models.Votes
{
    public class VoteId
    {
        public string OwnerGroupId { get; }
        public string IndexIn { get; }
        public string Id { get { return $"{OwnerGroupId}-{IndexIn}"; } }

        public VoteId(string ownerGroupId, string index)
        {
            OwnerGroupId = ownerGroupId;
            IndexIn = index;
        }
        public VoteId(SVoteIdV1 sVoteId)
        {
            OwnerGroupId = sVoteId.OwnerGroupId;
            IndexIn = sVoteId.Index;
        }
    }
}
