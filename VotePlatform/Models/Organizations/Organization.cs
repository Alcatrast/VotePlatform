using System.Collections.Generic;

using VotePlatform.Models.SystemServices;
using VotePlatform.Models.Users;
using VotePlatform.Models.Organizations.Serializable;
using VotePlatform.Models.DataBaseAPI;

namespace VotePlatform.Models.Organizations
{
    public class Organization : IOrganization
    {
        public bool IsDeleted { get; private set; }
        public string Id { get; }
        public string Nickname { get; }
        public OrganizationAttributes Attributes { get; }
        public Meta Meta { get; private set; }
        public OrganizationMembers Members { get; }
        public List<string> VoteIds { get; }

        public Organization(string id, string nickname, OrganizationAttributes attributes, Meta meta, OrganizationMembers members)
        {
            IsDeleted = false;
            Id = id;
            Nickname = nickname;
            Attributes = attributes;
            Meta = meta;
            Members = members;
            VoteIds = new List<string>();
        }

        public RoleInOrganization GetRoleInOrganization(string userId)
        {
            if (Members.Owner == userId) { return RoleInOrganization.Owner; }
            if (Members.Admins.Contains(userId)) { return RoleInOrganization.Admin; }
            if (Members.Audience.Contains(userId)) { return RoleInOrganization.Audience; }
            return RoleInOrganization.Passerby;
        }

        public RoleInPlatform GetRoleInPlatform(string userId)
        {
            UsersDataBaseAPI.FindById(userId, out User user);
            return user.Role;
        }

        public bool ChangeMeta(Meta newMeta, string adminId)
        {
            if ((sbyte)GetRoleInOrganization(adminId) >= (sbyte)RoleInOrganization.Admin)
            {
                Meta = newMeta;
                return true;
            }
            return false;
        }
        public bool ChangeType(OrganizationTypeOfJoin newType, string adminId)
        {
            if ((sbyte)GetRoleInOrganization(adminId) >= (sbyte)RoleInOrganization.Admin)
            {
                Attributes.TypeOfJoin = newType;
                return true;
            }
            return false;
        }

        public bool Delete(string validatorOrGroupOwnerId)
        {
            if ((sbyte)GetRoleInOrganization(validatorOrGroupOwnerId) >= (sbyte)RoleInOrganization.Owner || (sbyte)GetRoleInPlatform(validatorOrGroupOwnerId) >= (sbyte)RoleInPlatform.Validator)
            {
                IsDeleted = true;
                return true;
            }
            return false;
        }
        public bool Restore(string validatorId)
        {
            if ((sbyte)GetRoleInPlatform(validatorId) >= (sbyte)RoleInPlatform.Validator)
            {
                IsDeleted = false;
                return true;
            }
            return false;
        }
        public bool AssertValidity(string validatorId)
        {
            if ((sbyte)GetRoleInPlatform(validatorId) >= (sbyte)RoleInPlatform.Validator)
            {
                Attributes.IsValidationPassed = true;
                return true;
            }
            return false;
        }
        public bool RemoveValidity(string validatorId)
        {
            if ((sbyte)GetRoleInPlatform(validatorId) >= (sbyte)RoleInPlatform.Validator)
            {
                Attributes.IsValidationPassed = false;
                return true;
            }
            return false;
        }

        public bool ApplicationForMembership(string toAddUserId)
        {
            if ((sbyte)GetRoleInPlatform(toAddUserId) >= (sbyte)RoleInPlatform.User) { return false; }
            if (Members.AllMembers.Contains(toAddUserId)) { return false; }

            if (Attributes.TypeOfJoin == OrganizationTypeOfJoin.Controlled)
            {
                if (Members.Queue.Contains(toAddUserId) == false) { Members.Queue.Add(toAddUserId); }
                return true;
            }
            else if (Attributes.TypeOfJoin == OrganizationTypeOfJoin.Uncontrolled)
            {
                Members.Audience.Add(toAddUserId);

                UsersDataBaseAPI.FindById(toAddUserId, out User user);
                user.Membersip.AudienceInGtoups.Add(Id);
                UsersDataBaseAPI.Update(user);
                return true;
            }
            return false;
        }

        public bool CancelApplicationForMembersip(string toCancelUserId)
        {
            if ((sbyte)GetRoleInPlatform(toCancelUserId) >= (sbyte)RoleInPlatform.User)
                if (Members.Queue.Contains(toCancelUserId))
                {
                    Members.Queue.Remove(toCancelUserId);
                    return true;
                }
            return false;
        }
        public bool RejectApplicationForMembersip(string adminId, string toRejectUserId)
        {
            if ((sbyte)GetRoleInOrganization(toRejectUserId) >= (sbyte)RoleInOrganization.Admin == false) { return false; }
            Members.Queue.Remove(toRejectUserId);
            return true;
        }

        public bool AcceptPerson(string adminId, string toAcceptUserId)
        {
            if ((sbyte)GetRoleInOrganization(adminId) >= (sbyte)RoleInOrganization.Admin == false) { return false; }
            if (Members.AllMembers.Contains(toAcceptUserId)) { return false; }

            if (Members.Queue.Remove(toAcceptUserId))
            {
                Members.Audience.Add(toAcceptUserId);

                UsersDataBaseAPI.FindById(toAcceptUserId, out User user);
                user.Membersip.AudienceInGtoups.Add(Id);
                UsersDataBaseAPI.Update(user);

                return true;
            }
            return false;
        }
        public bool ExcludePerson(string adminId, string toExcludeUserId)
        {
            if ((sbyte)GetRoleInOrganization(adminId) >= (sbyte)RoleInOrganization.Admin == false) { return false; }
            Members.Audience.Remove(toExcludeUserId);

            UsersDataBaseAPI.FindById(toExcludeUserId, out User user);
            user.Membersip.AudienceInGtoups.Remove(Id);
            UsersDataBaseAPI.Update(user);

            return true;
        }

        public bool AddAdmin(string ownerId, string toAddUserId)
        {
            if ((sbyte)GetRoleInOrganization(ownerId) >= (sbyte)RoleInOrganization.Owner == false) { return false; }
            if (Members.Admins.Contains(toAddUserId) == false) { Members.Admins.Add(toAddUserId); Members.Audience.Remove(toAddUserId); Members.Queue.Remove(toAddUserId); }

            UsersDataBaseAPI.FindById(toAddUserId, out User user);
            user.Membersip.AdminInGroups.Add(Id);
            user.Membersip.AudienceInGtoups.Remove(Id);
            UsersDataBaseAPI.Update(user);

            return true;
        }
        public bool ExcludeAdmin(string ownerId, string toExcludeAdminId)
        {
            if (((sbyte)GetRoleInOrganization(ownerId) >= (sbyte)RoleInOrganization.Owner) == false) { return false; }
            Members.Admins.Remove(toExcludeAdminId);
            Members.Audience.Remove(toExcludeAdminId);

            UsersDataBaseAPI.FindById(toExcludeAdminId, out User user);
            user.Membersip.AdminInGroups.Remove(Id);
            user.Membersip.AudienceInGtoups.Add(Id);
            UsersDataBaseAPI.Update(user);

            return true;
        }

        public Organization(SOrganizationV1 sGroup)
        {
            IsDeleted = sGroup.IsDeleted;
            Id = sGroup.Id;
            Nickname = sGroup.Nickname;
            Attributes = new OrganizationAttributes(sGroup.Attributes);
            Meta = new Meta(sGroup.Meta);
            Members = new OrganizationMembers(sGroup.Members);
            VoteIds = sGroup.VoteIds;
        }
    }
}