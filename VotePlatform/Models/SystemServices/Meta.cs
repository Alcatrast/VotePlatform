using VotePlatform.Models.SystemServices.Serializable;

namespace VotePlatform.Models.SystemServices
{
    public class Meta
    {
        public string Name { get; }
        public string ShortDescription { get; }
        public string Description { get; }

        public Meta(string name = "", string shortDescription = "", string description = "")
        {
            Name = name;
            ShortDescription = shortDescription;
            Description = description;
        }
        public Meta(SMetaV1 sMeta)
        {
            Name = sMeta.Name;
            ShortDescription = sMeta.ShortDescription;
            Description = sMeta.Description;
        }
    }
}
