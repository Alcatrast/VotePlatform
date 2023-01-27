using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using VotePlatform.Models.Votes;
using VotePlatform.Models.SystemServices;
using VotePlatform.Models.Organizations.Serializable;
using VotePlatform.Models.Organizations;
using VotePlatform.Models.Votes.Serializable;
using VotePlatform.Models.Users.Serializable;
using VotePlatform.Models.Users;
using VotePlatform.Models.Votes.DB;
using VotePlatform.Models.Organizations.DB;
using VotePlatform.Models.Users.DB;
using VotePlatform.Models.DataBaseAPI;
using VotePlatform.Models.DataBaseAPI.SystemServices;
using Microsoft.Data.SqlClient;
using VotePlatform.Models.SystemServices.Serializable;

namespace VotePlatform
{
    internal static class Zalupa
    {
        private static void StartFilling()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand cu = new SqlCommand(@"CREATE TABLE[dbo].[Users] ( [Id] NVARCHAR (50)  NOT NULL PRIMARY KEY, [Nickname] NVARCHAR(50) NOT NULL, [Email] NVARCHAR(50) NOT NULL, [Password] NVARCHAR(50) NOT NULL, [SerializedUser] NVARCHAR(MAX) NOT NULL );", connection);
            SqlCommand co = new SqlCommand(@"CREATE TABLE[dbo].[Organizations] ( [Id] NVARCHAR (50)  NOT NULL PRIMARY KEY, [Nickname] NVARCHAR(50) NOT NULL, [SerializedOrganization] NVARCHAR(MAX) NOT NULL );", connection);
            SqlCommand cv = new SqlCommand(@"CREATE TABLE[dbo].[Votes] ( [OrganizationId] NVARCHAR (50)  NOT NULL, [IndexIn] NVARCHAR(50) NOT NULL, [SerializedVote] NVARCHAR(MAX) NOT NULL );", connection);
            try { cu.ExecuteNonQuery(); } catch { }
            try { co.ExecuteNonQuery(); } catch { }
            try { cv.ExecuteNonQuery(); } catch { }

            connection.Close();
            Console.WriteLine("Tables created.");

            UsersDataBaseAPI.Create("@owner", "owner@gmail.com", "Owner", "OWNER");
            UsersDataBaseAPI.FindByNick("@owner", out User owner);
            SUserV1 sOwnerV1 = new SUserV1(owner)
            {
                Role = RoleInPlatform.Owner
            };
            UsersDataBaseAPI.Update(new User(sOwnerV1));

            UsersDataBaseAPI.Create("@admin", "admin@gmail.com", "Admin", "ADMIN");
            UsersDataBaseAPI.FindByNick("@admin", out User admin);
            admin.ChangeRole(sOwnerV1.Id, RoleInPlatform.Admin);
            UsersDataBaseAPI.Update(admin);

            UsersDataBaseAPI.Create("@validator", "validator@gmail.com", "Validator", "VALIDATOR");
            UsersDataBaseAPI.FindByNick("@admin", out User validator);
            validator.ChangeRole(admin.Id, RoleInPlatform.Validator);
            UsersDataBaseAPI.Update(validator);

            UsersDataBaseAPI.Create("@user", "user@gmail.com", "User", "USER");
            Console.WriteLine("Users added.");

            OrganizationsDataBaseAPI.Create("#organization", "u4");
            OrganizationsDataBaseAPI.FindByNick("#organization", out Organization organization);
            organization.ChangeMeta(new Meta(new SMetaV1() { Name = "Organization" }), "u4");
            OrganizationsDataBaseAPI.Update(organization);
            Console.WriteLine("Organizations added.");

        }
        public static void InitializateDB()
        {
            UsersDataBaseAPI.Initialize(ConnectionString);
            OrganizationsDataBaseAPI.Initialize(ConnectionString);
            VotesDataBaseAPI.Initialize(ConnectionString);
        }
        public static void SAV()
        {
            VotesDataBaseAPI.Create("u4", "o1", VoteType.AloneAswer,
               new VoteAttributes(new TimeSpan(), false, true, false, true, RoleInOrganization.Passerby),
               new VoteResultAttributes(false, RoleInOrganization.Passerby, RoleInOrganization.Passerby),
               new VoteMeta("Vote", "This vote exist for debug."),
               new List<VoteMeta>(){ new VoteMeta("Answer 1", "Answer 1 exist for debug."), new VoteMeta("Answer 2", "Answer 2 exist for debug."),
                new VoteMeta("Answer 3", "Answer 3 exist for debug."), new VoteMeta("Answer 4","Answer 4 exist for debug.") }
               );
            Console.WriteLine("Votes added");
        }
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
        public static string ConnectionString = CFG.N;
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        public static void MainZalupa()
        {
            Console.WriteLine("Hello, World!");
            
            InitializateDB();
            //StartFilling();
            //SAV();

            Console.WriteLine("Startup completed");

            //VotesDataBaseAPI.FindById(new VoteId("o1", "v1"), out Vote vote1);
            //vote1.Voiting("u1", new List<int>() { 2 });
            //VotesDataBaseAPI.Update(vote1);
        }

    }
}