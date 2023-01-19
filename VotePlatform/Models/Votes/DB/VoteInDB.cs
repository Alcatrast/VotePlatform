using System.Xml.Serialization;
using VoteM.Models.Organizations;
using VoteM.Models.Organizations.Serializable;
using VoteM.Models.Votes.Serializable;

namespace VoteM.Models.Votes.DB
{
    public class VoteInDB
    {
        public string OrganizationId= string.Empty;
        public string IndexIn= string.Empty;
        public string SerializedVote= string.Empty;
        public VoteInDB() { }
        public VoteInDB(Vote vote) 
        {
            OrganizationId = vote.Id.OwnerGroupId;
            IndexIn = vote.Id.IndexIn;

            XmlSerializer serializer = new(typeof(SVoteV1));
            StringWriter stringWriter= new ();
            serializer.Serialize(stringWriter, new SVoteV1(vote));
            SerializedVote= stringWriter.ToString();
            stringWriter.Close();
        }
        public Vote Construct()
        {
            XmlSerializer serializer = new(typeof(SVoteV1));
            StringReader stringReader = new(SerializedVote);
            Vote vote = new(new());
            try
            {
                var ro = serializer.Deserialize(stringReader);
                if (ro is SVoteV1 sOrganization) { vote = new(sOrganization); }
            }
            catch { Console.WriteLine($"{OrganizationId}-{IndexIn} Vote Deserialize Error"); }
            finally { stringReader.Close(); }
            return vote; ;
        }
    }
}
