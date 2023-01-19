using System.Xml.Serialization;
using VoteM.Models.Organizations.Serializable;
using VoteM.Models.Users.Serializable;
using VoteM.Models.Users;

namespace VoteM.Models.Organizations.DB
{
    public class OrganizationInDB
    {
        public string Id =string.Empty;
        public string Nickname = string.Empty;
        public string SerializedOrganization = string.Empty;
        public OrganizationInDB() { }   
        public OrganizationInDB(Organization organization)
        {       
            Id = organization.Id;
            Nickname= organization.Nickname;

            XmlSerializer serializer = new(typeof(SOrganizationV1));
            StringWriter stringWriter = new();
            serializer.Serialize(stringWriter, new SOrganizationV1(organization));
            SerializedOrganization=stringWriter.ToString();
            stringWriter.Close();
        }
        public Organization Construct()
        {
            XmlSerializer serializer = new(typeof(SOrganizationV1));
            StringReader stringReader = new(SerializedOrganization);
            Organization organization = new(new());
            try
            {
                var ro = serializer.Deserialize(stringReader);
                if (ro is SOrganizationV1 sOrganization) { organization = new(sOrganization); }
            }
            catch { Console.WriteLine($"{Id} Organization Deserialize Error"); }
            finally { stringReader.Close(); }
            return organization;
        }
    }
}
