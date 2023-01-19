
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

using VotePlatform.Models.Votes;
using VotePlatform.Models.Votes.DB;

namespace VotePlatform.Models.DataBaseAPI.SystemServices
{
    /*
   CREATE TABLE[dbo].[Votes]
   (
   [OrganizationId] NVARCHAR (50)  NOT NULL,
   [IndexIn] NVARCHAR(50) NOT NULL,
   [SerializedVote] NVARCHAR(MAX) NOT NULL
   );
   */
    internal class VoteDataBase
    {
        private string ConnectionString { get; }
        public VoteDataBase(string connectionString) => ConnectionString = connectionString;
        public List<VoteInDB> GetById(VoteId id) { return Get($"OrganizationId=N'{id.OwnerGroupId}' AND IndexIn=N'{id.IndexIn}'"); }
        public List<VoteInDB> GetById(string organizationId) { return Get($"OrganizationId=N'{organizationId}'"); }
        private List<VoteInDB> Get(string selectFilter)
        {
            List<VoteInDB> res = new List<VoteInDB>();
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand($@"SELECT * FROM Votes WHERE {selectFilter}", connection);
            SqlDataReader dataReader = command.ExecuteReader();
            try
            {
                while (dataReader.Read())
                {
                    var organizationId = Convert.ToString(dataReader["OrganizationId"]);
                    var index = Convert.ToString(dataReader["IndexIn"]);
                    var serializedVote = Convert.ToString(dataReader["SerializedVote"]);
                    res.Add(new VoteInDB()
                    {
                        OrganizationId = organizationId ?? string.Empty,
                        IndexIn = index ?? string.Empty,
                        SerializedVote = serializedVote ?? string.Empty
                    });
                }
            }
            catch { }
            finally
            {
                if (dataReader != null && dataReader.IsClosed == false) { dataReader.Close(); }
            }
            return res;
        }
        public bool Add(VoteInDB voteInDB)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(@$"INSERT INTO [Votes] (OrganizationId, IndexIn, SerializedVote) VALUES (N'{voteInDB.OrganizationId}', N'{voteInDB.IndexIn}', N'{voteInDB.SerializedVote}')", connection);
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public bool Update(VoteInDB voteInDB)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(@$"UPDATE Votes SET OrganizationId=N'{voteInDB.OrganizationId}', IndexIn=N'{voteInDB.IndexIn}', SerializedVote=N'{voteInDB.SerializedVote}'  WHERE OrganizationId=N'{voteInDB.OrganizationId}' AND IndexIn=N'{voteInDB.IndexIn}'", connection);
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
    }
}
