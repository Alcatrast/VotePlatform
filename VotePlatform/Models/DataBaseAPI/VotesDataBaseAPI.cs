using System.Collections.Generic;

using VoteM.Models.Organizations;
using VoteM.Models.SystemServices;
using VoteM.Models.Votes;
using VoteM.Models.DataBaseAPI.SystemServices;
using VoteM.Models.Votes.Serializable;
using VoteM.Models.Votes.DB;

namespace VoteM.Models.DataBaseAPI
{
    internal static class VotesDataBaseAPI
    {
        private static VoteDataBase dataBase = new(string.Empty);

        public static void Initialize(string connectionString) => dataBase = new(connectionString);

        public static bool Create(string adminId,string organizationId, VoteType type,VoteAttributes attributes, VoteResultAttributes resultAttributes,VoteMeta meta,List<VoteMeta> answersMetas)
        {
            OrganizationsDataBaseAPI.FindById(organizationId, out Organization organization);
            if (((sbyte)organization.GetRoleInOrganization(adminId) >= (sbyte)RoleInOrganization.Admin) == false) { return false; }
            VoteId id = new(organization.Id, GetIndex(organization.VoteIds.Count));
            VoteInDB voteInDB = new(new ( id, type, attributes, resultAttributes, meta, answersMetas));
           if (dataBase.Add(voteInDB)) 
            {
                organization.VoteIds.Add(id.IndexIn);
                OrganizationsDataBaseAPI.Update(organization);
                return true; 
            }
            else { return false; }
        }
        public static bool Update(Vote vote)
        {
            if (dataBase.GetById(vote.Id).Count == 0) { return false; }
            return dataBase.Update(new(vote));
        }
        public static bool FindById(VoteId id, out Vote vote)
        {
            vote = new(new SVoteV1());
            var res = dataBase.GetById(id);
            if(res.Count== 0) { return false; }
            else { vote = res[0].Construct(); return true; }
        }
        public static List<Vote> FindAllBelongingTo(Organization organization, RoleInOrganization ro, RoleInPlatform rp)
        {
            var res = new List<Vote>();
            if (organization.IsDeleted) { return res; }

            var bufl=new List<Vote>();

            var buf=dataBase.GetById(organization.Id);
            foreach(var it in buf)
            {
                bufl.Add(it.Construct());
            }
            if ((sbyte)rp < (sbyte)RoleInPlatform.Validator)
            {
                foreach(Vote vote in bufl)
                {
                    if ((((sbyte)ro >= (sbyte)vote.MinRoleToView) && vote.IsAvailable) || (sbyte)ro>=(sbyte)RoleInOrganization.Admin) { res.Add(vote); }
                }
            }
            else { res = bufl; }
            return res;
        }
        private static string GetIndex(int prev)
        {
            return $"v{Convert.ToString(prev + 1)}";
        }
    }
}
