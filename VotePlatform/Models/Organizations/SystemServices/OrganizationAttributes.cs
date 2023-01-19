using VoteM.Models.Organizations.Serializable;

namespace VoteM.Models.Organizations
{
    public class OrganizationAttributes
    {
        public bool IsValidationPassed { get; set; }
        public OrganizationTypeOfJoin TypeOfJoin { get; set; }
        public OrganizationAttributes(OrganizationTypeOfJoin type= OrganizationTypeOfJoin.Controlled)
        {
            IsValidationPassed = false;
            TypeOfJoin = type;
        }
        public OrganizationAttributes(SOrganizationAttributesV1 sGroupAttributes)
        {
            IsValidationPassed = sGroupAttributes.IsValidationPassed;
            TypeOfJoin = sGroupAttributes.TypeOfJoin;
        }
    }

}
