using System.Collections.Generic;
using VoteM.Models.Organizations.Serializable;

namespace VoteM.Models.Organizations
{
    public class OrganizationMembers
    {
        public string Owner { get; }
        public List<string> Admins { get; }
        public List<string> Audience { get; }
        public List<string> Queue { get; }
        public List<string> AllMembers
        {
            get
            {
                var res = new List<string>() { Owner };
                foreach (string it in Admins)
                {
                    res.Add(it);
                }
                foreach (string it in Audience)
                {
                    res.Add(it);
                }
                return res;
            }
        }
        public OrganizationMembers(string owner)
        {
            Owner = owner;
            Admins = new List<string>();
            Audience = new List<string>();
            Queue = new List<string>();
        }
        public OrganizationMembers(SOrganizationMembersV1 sGroupMembers)
        {
            Owner = sGroupMembers.Owner;
            Admins = sGroupMembers.Admins;
            Audience = sGroupMembers.Audience;
            Queue = sGroupMembers.Queue;
        }
    }
}
