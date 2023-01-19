using System;
using System.IO;
using System.Xml.Serialization;

using VotePlatform.Models.Votes.Serializable;

namespace VotePlatform.Models.Votes.DB
{
    public class VoteInDB
    {
        public string OrganizationId = string.Empty;
        public string IndexIn = string.Empty;
        public string SerializedVote = string.Empty;
        public VoteInDB() { }
        public VoteInDB(Vote vote)
        {
            OrganizationId = vote.Id.OwnerGroupId;
            IndexIn = vote.Id.IndexIn;

            XmlSerializer serializer = new XmlSerializer(typeof(SVoteV1));
            StringWriter stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, new SVoteV1(vote));
            SerializedVote = stringWriter.ToString();
            stringWriter.Close();
        }
        public Vote Construct()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SVoteV1));
            StringReader stringReader = new StringReader(SerializedVote);
            Vote vote = new Vote(new SVoteV1());
            try
            {
                var ro = serializer.Deserialize(stringReader);
                if (ro is SVoteV1 sOrganization) { vote = new Vote(sOrganization); }
            }
            catch { Console.WriteLine($"{OrganizationId}-{IndexIn} Vote Deserialize Error"); }
            finally { stringReader.Close(); }
            return vote; ;
        }
    }
}
