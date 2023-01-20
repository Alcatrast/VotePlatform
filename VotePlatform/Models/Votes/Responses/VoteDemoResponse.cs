
using System;
using System.Collections.Generic;

namespace VotePlatform.Models.Votes
{
    public class VoteDemoResponse : IComparable
    {
        public bool IsAvailable { get; }
        public DateTime CreatingDateTime { get; }
        public VoteId Id { get; }
        public VoteType Type { get; }
        public VoteAttributes Attributes { get; }
        public VoteResultAttributes ResultAttributes { get; }
        public VoteMeta Meta { get; }
       
        public VoteDemoResponse(Vote vote, string userId)
        {
            IsAvailable = vote.IsAvailable;
            Id = vote.Id;
            CreatingDateTime = vote.CreatingDateTime;
            Type = vote.Type;
            Attributes = vote.Attibutes;
            ResultAttributes = vote.ResultAttributes;
            Meta = vote.Meta;
        }

        int IComparable.CompareTo(object? obj)
        {
            if (obj != null)
            {
                var other = (VoteMainResponse)obj;
                return DateTime.Compare(CreatingDateTime, other.CreatingDateTime);
            }
            else { return DateTime.Compare(new DateTime(), new DateTime()); }
        }
    }
}
