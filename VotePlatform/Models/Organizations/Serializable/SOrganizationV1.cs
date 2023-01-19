
using VoteM.Models.SystemServices.Serializable;

namespace VoteM.Models.Organizations.Serializable
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
        public List<string> Admins = new();
        public List<string> Audience = new();
        public List<string> Queue = new();
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
        public SOrganizationAttributesV1 Attributes = new();
        public SMetaV1 Meta = new();
        public SOrganizationMembersV1 Members = new();
        public List<string> VoteIds = new();

        public SOrganizationV1(Organization group)
        {
            IsDeleted = group.IsDeleted;
            Id = group.Id;
            Nickname = group.Nickname;
            Attributes = new(group.Attributes);
            Meta = new(group.Meta);
            Members = new(group.Members);
            VoteIds = group.VoteIds;
        }
        public SOrganizationV1() { }
    }


}
