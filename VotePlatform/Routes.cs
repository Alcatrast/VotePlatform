namespace VotePlatform
{
    public static class VRoutes
    {
        public const string Host = @"https://localhost:44313";
        public const string Controller = @"/Votes";
        public const string AVoting = @"/Voting";
        public const string ADynamicView = @"/Dynamic";
        public const string AVoters = @"/Voters";

        public const string PAnswerIndex = @"answer";
        public const string PVCancel = @"CANCEL";

        public const string ACreate = @"/Create";
        public const string ACreating = @"/Creating";
        public const string AChangePin = @"/ChangePin";

        public const string PPinPrefState = @"pinPrefState";
        public const string PVPin = @"pin";
        public const string PVUnpin = @"unpin";
    }
    public static class ORoutes
    {
        public const string Host = @"https://localhost:44313";
        public const string Controller = @"/Organizations";
        public const string AMain = @"/Main";
        public const string AAudience = @"/Audience";
        public const string AAdmins = @"/Admins";
        public const string AQueue = @"/Queue";
        public const string ACreating = @"/Creating";
        public const string ACreate = @"/Create";
    }
    public static class URoutes
    {
        public const string Host = @"https://localhost:44313";
        public const string Controller = @"/Users";
        public const string AMain = @"/Main";
        public const string ACreating = @"/Creating";
        public const string ACreate = @"/Create";
        public const string ASingingIn = @"/SingingIn";
        public const string ASingIn = @"/SingIn";
    }
}
