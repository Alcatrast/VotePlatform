using System;
using System.Collections.Generic;

using VotePlatform.Models.SystemServices.Serializable;

namespace VotePlatform.Models.Organizations.Serializable
{
    [Serializable]
    public class SOrganizationAttributesV1
    {
        public bool IsValidationPassed = false;
        public OrganizationTypeOfJoin TypeOfJoin = OrganizationTypeOfJoin.Uncontrolled;
        public SOrganizationAttributesV1(OrganizationAttributes groupAttributes)
        {
            IsValidationPassed = groupAttributes.IsValidationPassed;
            TypeOfJoin = groupAttributes.TypeOfJoin;
        }
        public SOrganizationAttributesV1() { }
    }
    [Serializable]
    public class SOrganizationMembersV1
    {
        public string Owner = string.Empty;
        public List<string> Admins = new List<string>();
        public List<string> Audience = new List<string>();
        public List<string> Queue = new List<string>();
        public SOrganizationMembersV1(OrganizationMembers groupMembers)
        {
            Owner = groupMembers.Owner;
            Admins = groupMembers.Admins;
            Audience = groupMembers.Audience;
            Queue = groupMembers.Queue;
        }
        public SOrganizationMembersV1() { }
    }
    public class SOrganizationV1
    {
        public bool IsDeleted = true;
        public string Id = string.Empty;
        public string Nickname = string.Empty;
        public SOrganizationAttributesV1 Attributes = new SOrganizationAttributesV1();
        public SMetaV1 Meta = new SMetaV1();
        public SOrganizationMembersV1 Members = new SOrganizationMembersV1();
        public List<string> VoteIds = new List<string>();

        public SOrganizationV1(Organization group)
        {
            IsDeleted = group.IsDeleted;
            Id = group.Id;
            Nickname = group.Nickname;
            Attributes = new SOrganizationAttributesV1(group.Attributes);
            Meta = new SMetaV1(group.Meta);
            Members = new SOrganizationMembersV1(group.Members);
            VoteIds = group.VoteIds;
        }
        public SOrganizationV1() { }
    }


}
