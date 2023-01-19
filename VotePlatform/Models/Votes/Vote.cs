using System.Collections.Generic;

using VoteM.Models.Organizations;
using VoteM.Models.SystemServices;
using VoteM.Models.Votes.Serializable;
using VoteM.Models.DataBaseAPI;


namespace VoteM.Models.Votes
{
    public class Vote : IVote
    {
        public bool IsAvailable { get; set; }
        public RoleInOrganization MinRoleToView { get { return (RoleInOrganization)new List<sbyte>() { (sbyte)Attibutes.MinRoleToVoting, (sbyte)ResultAttributes.MinRoleToActual }.Max(); } }
        public DateTime CreatingDateTime { get; }
        public VoteId Id { get; }
        public VoteType Type { get; }
        public VoteAttributes Attibutes { get; }
        public VoteResultAttributes ResultAttributes { get; }
        public VoteMeta Meta { get; }
        public List<VoteMeta> AnswersMetas { get; }
        public int CountVoters { get; private set; }
        public List<Voice> Voices { get; }
        public Vote(VoteId id, VoteType type, VoteAttributes attibutes, VoteResultAttributes resultAttributes, VoteMeta meta, List<VoteMeta> answersMetas)
        {
            CreatingDateTime = CurrentDateTime();
            Id = id;
            Type = type;
            Attibutes = attibutes;
            ResultAttributes = resultAttributes;
            Meta = meta;
            AnswersMetas = answersMetas;
            CountVoters = 0;
            Voices = new List<Voice>();
            IsAvailable = true;
        }

        public bool Voiting(string userId, List<int> choice)
        {
            if (IsAvailable == false) { return false; }
            if (IsAvaliableTimeToVote() == false) { return false; }

            if (IsAccessAllowed(userId, Attibutes.MinRoleToVoting) == false || UsersDataBaseAPI.FindById(userId, out _) == false) { return false; }
            bool voiceAlreadyExist = IsVoiceAlreadyExist(userId);
            if (voiceAlreadyExist)
            {
                if(Attibutes.IsVoiceCancellationPossible==false) { return false; }
                if (choice.Count != 1) { return false; }
                if (choice[0] != -1) { return false; }
            }
            else
            {
                if (Type == VoteType.AloneAswer)
                {
                    if (choice.Count != 1) return false;
                }
                else if (Type == VoteType.SomeAnswers)
                {
                    if (choice.Count > AnswersMetas.Count) return false;
                }
                else if (Type == VoteType.PreferVote)
                {
                    if (choice.Count != AnswersMetas.Count - 1) return false;
                }

                foreach (int index in choice)
                {
                    if ((index >= 0 && index < AnswersMetas.Count) == false) return false;
                }
            }
            AddVoice(userId, choice);
            if (voiceAlreadyExist == false) { CountVoters++; } else { CountVoters--; }
            return true;
        }
        public bool Pin(string adminId)
        {
            if (IsAccessAllowed(adminId, RoleInOrganization.Admin))
            {
                Meta.IsPinned = true; return true;
            }
            return false;
        }
        public bool Unpin(string adminId)
        {
            if (IsAccessAllowed(adminId, RoleInOrganization.Admin))
            {
                Meta.IsPinned = false; return true;
            }
            return false;
        }

        private void AddVoice(string userId, List<int> choice)
        {
            Voices.Add(new Voice(userId, choice, CurrentDateTime()));
        }
        private bool IsVoiceAlreadyExist(string userId)
        {
            int countRequests = 0;
            foreach (Voice voice in Voices)
            {
                if (voice.UserId == userId) { countRequests++; };
            }
            return countRequests % 2 == 1;
        }

        public bool IsAccessAllowed(string userId, RoleInOrganization minRole)
        {
            if (Attibutes.MinRoleToVoting == RoleInOrganization.Passerby) { return true; }
            if (OrganizationsDataBaseAPI.FindById(Id.OwnerGroupId, out Organization owner) == false) { return false; }
            if (owner.IsDeleted) { return false; }
            return owner.GetRoleInOrganization(userId) >= minRole;
        }
        private bool IsAvaliableTimeToVote()
        {
            if (Attibutes.IsAlwaysActiveToVote) return true;
            else if (CreatingDateTime + Attibutes.TimeActiveToVote > CurrentDateTime()) return true; else return false;
        }
        private static DateTime CurrentDateTime() { return DateTime.Now; }
        public bool Extend(string userId, DateTime newDateTime)
        {
            var ts = newDateTime - CreatingDateTime;
            if (ts < Attibutes.TimeActiveToVote)
            {
                Attibutes.TimeActiveToVote = ts;
                return true;
            }
            return false;
        }

        public Vote(SVoteV1 sVote)
        {
            CreatingDateTime = sVote.CreatingDateTime;
            Id = new(sVote.Id);
            Type = sVote.Type;
            Attibutes = new(sVote.Attibutes);
            ResultAttributes = new(sVote.ResultAttributes);
            Meta = new(sVote.Meta);
            CountVoters = sVote.CountVoters;
            IsAvailable = sVote.IsAvailable;

            AnswersMetas = new();
            foreach (var it in sVote.AnswersMetas) { AnswersMetas.Add(new(it)); }
            Voices = new();
            foreach (var it in sVote.Voices) { Voices.Add(new(it)); }
        }
    }
}
