
using VoteM.Models.SystemServices;
using VoteM.Models.Votes.Serializable;

namespace VoteM.Models.Votes
{
    public class VoteResultAttributes
    {
        public bool ResultsOnlyAfterCompletion { get; }
        public RoleInOrganization MinRoleToActual { get; }
        public RoleInOrganization MinRoleToDynamic { get; }

        public VoteResultAttributes(bool resultsOnlyAfterCompletion, RoleInOrganization minRoleToActual, RoleInOrganization minRoleToDynamic)
        {
            ResultsOnlyAfterCompletion = resultsOnlyAfterCompletion;
            MinRoleToActual = minRoleToActual;
            MinRoleToDynamic = minRoleToDynamic >= minRoleToActual ? minRoleToDynamic : minRoleToActual;
        }
        public VoteResultAttributes(SVoteResultAttributesV1 sVoteResultAttributes)
        {
            ResultsOnlyAfterCompletion = sVoteResultAttributes.ResultsOnlyAfterCompletion;
            MinRoleToActual = sVoteResultAttributes.MinRoleToActual;
            MinRoleToDynamic = sVoteResultAttributes.MinRoleToDynamic;
        }
    }
}
