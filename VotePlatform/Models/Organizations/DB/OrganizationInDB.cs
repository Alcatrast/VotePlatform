using System;
using System.IO;
using System.Xml.Serialization;

using VotePlatform.Models.Organizations.Serializable;

namespace VotePlatform.Models.Organizations.DB
{
    public class OrganizationInDB
    {
        public string Id = string.Empty;
        public string Nickname = string.Empty;
        public string SerializedOrganization = string.Empty;
        public OrganizationInDB() { }
        public OrganizationInDB(Organization organization)
        {
            Id = organization.Id;
            Nickname = organization.Nickname;

            XmlSerializer serializer = new XmlSerializer(typeof(SOrganizationV1));
            StringWriter stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, new SOrganizationV1(organization));
            SerializedOrganization = stringWriter.ToString();
            stringWriter.Close();
        }
        public Organization Construct()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SOrganizationV1));
            StringReader stringReader = new StringReader(SerializedOrganization);
            Organization organization = new Organization(new SOrganizationV1());
            try
            {
                var ro = serializer.Deserialize(stringReader);
                if (ro is SOrganizationV1 sOrganization) { organization = new Organization(sOrganization); }
            }
            catch { Console.WriteLine($"{Id} Organization Deserialize Error"); }
            finally { stringReader.Close(); }
            return organization;
        }
    }
}
