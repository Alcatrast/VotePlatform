using VotePlatform.Models.Votes.Serializable;

namespace VotePlatform.Models.Votes
{
    public class VoteId
    {
        public string OwnerGroupId { get; }
        public string IndexIn { get; }
        public string FullId { get { return $"{OwnerGroupId}-{IndexIn}"; } }

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
        public VoteId(string? id)
        {
            OwnerGroupId = "o0";
            IndexIn = "v0";
            if(id == null) { return; }
            try
            {
                if (id.Length > 0)
                {
                    string[] ss = id.Split('-');
                    OwnerGroupId = ss[0];
                    IndexIn = ss[1];
                }
            }
            catch
            {
                OwnerGroupId = "o0";
                IndexIn = "v0";
            }
        }
    }
}
