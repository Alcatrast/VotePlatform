using System;
using System.Collections.Generic;
using System.Linq;

using VotePlatform.Models.Organizations;
using VotePlatform.Models.SystemServices;
using VotePlatform.Models.Votes.Serializable;
using VotePlatform.Models.DataBaseAPI;
using VotePlatform.Models.Users;

namespace VotePlatform.Models.Votes
{
    public class Vote : IVote
    {
        public bool IsAvailable { get; set; }
        public RoleInOrganization MinRoleToView { get { return (RoleInOrganization)(new List<sbyte>() { (sbyte)Attibutes.MinRoleToVoting, (sbyte)ResultAttributes.MinRoleToActual }).Max(); } }
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
            if (IsVotingAccessible(userId)==false) { return false; }
            bool voiceAlreadyExist = IsVoiceAlreadyExist(userId);

            List<int> voice = new List<int>();
            for(int i = 0; i < AnswersMetas.Count; i++) { voice.Add(0); }

            if (voiceAlreadyExist)
            {
                if (Attibutes.IsVoiceCancellationPossible == false) { return false; }
                if (choice.Count != 1) { return false; }
                if (choice[0] != -1) { return false; }
            }
            else
            {
                if (choice.Count != AnswersMetas.Count) {  return false; }
                if (Type == VoteType.AloneAswer)
                {
                    for(int i = 0; i < choice.Count; i++) { if (choice[i] != 0) { voice[i] = 1; break; } }
                }
                else if (Type == VoteType.SomeAnswers)
                {
                    for (int i = 0; i < choice.Count; i++) { if (choice[i] != 0) { voice[i] = 1; } }
                }
                else if (Type == VoteType.PreferVote)
                {
                    var checker = new List<int>();
                    for(int i = 0; i < choice.Count; i++) { if (checker.Contains(choice[i])) { return false; }; }
                    Dictionary<int,int> keyValuePairs= new Dictionary<int,int>();
                    for(int i = 0; i < choice.Count; i++) { keyValuePairs.Add(choice[i], i); }
                    choice.Sort();
                    for(int i = 0; i < choice.Count; i++) { voice[keyValuePairs[choice[i]]] = i; }
                }
            }
            AddVoice(userId, voice);
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
            Id = new VoteId(sVote.Id);
            Type = sVote.Type;
            Attibutes = new VoteAttributes(sVote.Attibutes);
            ResultAttributes = new VoteResultAttributes(sVote.ResultAttributes);
            Meta = new VoteMeta(sVote.Meta);
            CountVoters = sVote.CountVoters;
            IsAvailable = sVote.IsAvailable;

            AnswersMetas = new List<VoteMeta>();
            foreach (var it in sVote.AnswersMetas) { AnswersMetas.Add(new VoteMeta(it)); }
            Voices = new List<Voice>();
            foreach (var it in sVote.Voices) { Voices.Add(new Voice(it)); }
        }
        public bool IsVotingAccessible(string userId)
        {
            if (IsAvailable == false) { return false; }
            if (IsAvaliableTimeToVote() == false) { return false; }

            if (IsAccessAllowed(userId, Attibutes.MinRoleToVoting) == false || UsersDataBaseAPI.FindById(userId, out User user) == false) { return false; }
            if ((sbyte)user.Role <= (sbyte)RoleInPlatform.Passerby) { return false; }
            if(Attibutes.IsVoiceCancellationPossible== false) { if (IsVoiceAlreadyExist(userId)) { return false; }; }
            return true;
        }
        public bool IsCancellationPossible(string userId)
        {
            if (Attibutes.IsVoiceCancellationPossible == false) return false;
            if (IsVoiceAlreadyExist(userId)==false) { return false; }
            return true;
        }
    }
}
