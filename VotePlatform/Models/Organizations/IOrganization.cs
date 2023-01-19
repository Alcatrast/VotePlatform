using VotePlatform.Models.SystemServices;

namespace VotePlatform.Models.Organizations
{
    interface IOrganization
    {
        public bool ChangeMeta(Meta newMeta, string adminId);
        public bool ChangeType(OrganizationTypeOfJoin newType, string adminId);

        public bool Delete(string validatorOrGroupOwnerId);
        public bool Restore(string validatorId);
        public bool AssertValidity(string validatorId);
        public bool RemoveValidity(string validatorId);

        public bool ApplicationForMembership(string toAddUserId);
        public bool CancelApplicationForMembersip(string toCancelUserId);
        public bool RejectApplicationForMembersip(string adminId, string toRejectUserId);

        public bool AcceptPerson(string adminId, string toAcceptUserId);
        public bool ExcludePerson(string adminId, string toExcludeUserId);

        public bool AddAdmin(string ownerId, string toAddUserId);
        public bool ExcludeAdmin(string ownerId, string toExcludeAdminId);

        public RoleInOrganization GetRoleInOrganization(string userId);
        public RoleInPlatform GetRoleInPlatform(string userId);
    }
}
