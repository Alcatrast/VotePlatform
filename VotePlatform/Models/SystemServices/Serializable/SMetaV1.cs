

namespace VoteM.Models.SystemServices.Serializable
{
    [Serializable]
    public class SMetaV1
    {
        public string Name = string.Empty;
        public string ShortDescription = string.Empty;
        public string Description = string.Empty;

        public SMetaV1(Meta meta)
        {
            Name = meta.Name;
            ShortDescription = meta.ShortDescription;
            Description = meta.Description;
        }
        public SMetaV1() { }
    }
}
