using System;
using System.Collections.Generic;

using Microsoft.Data.SqlClient;
using VotePlatform.Models.Organizations.DB;

namespace VotePlatform.Models.DataBaseAPI.SystemServices
{
    /*
    CREATE TABLE[dbo].[Organizations]
    (
    [Id] NVARCHAR (50)  NOT NULL PRIMARY KEY,
    [Nickname] NVARCHAR(50) NOT NULL,
    [SerializedOrganization] NVARCHAR(MAX) NOT NULL
    );
    */
    public class OrganizationDataBase
    {
        private string ConnectionString { get; }
        public OrganizationDataBase(string connectionString) => ConnectionString = connectionString;
        public List<OrganizationInDB> GetById(string id) { return GetBy("Id", id); }
        public List<OrganizationInDB> GetByNick(string nick) { return GetBy("Nickname", nick); }
        public List<OrganizationInDB> GetBy(string key, string value)
        {
            List<OrganizationInDB> res = new List<OrganizationInDB>();
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand($@"SELECT * FROM Organizations WHERE {key}=N'{value}'", connection);
            SqlDataReader dataReader = command.ExecuteReader();
            try
            {
                while (dataReader.Read())
                {
                    var id = Convert.ToString(dataReader["Id"]);
                    var nickname = Convert.ToString(dataReader["Nickname"]);
                    var serializedOrganization = Convert.ToString(dataReader["SerializedOrganization"]);
                    res.Add(new OrganizationInDB()
                    {
                        Id = id ?? string.Empty,
                        Nickname = nickname ?? string.Empty,
                        SerializedOrganization = serializedOrganization ?? string.Empty
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
        public bool Add(OrganizationInDB OrganizationInDB)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(@$"INSERT INTO [Organizations] (Id, Nickname, SerializedOrganization) VALUES (N'{OrganizationInDB.Id}', N'{OrganizationInDB.Nickname}', N'{OrganizationInDB.SerializedOrganization}')", connection);
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public bool Update(OrganizationInDB OrganizationInDB)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(@$"UPDATE Organizations SET Id=N'{OrganizationInDB.Id}',Nickname=N'{OrganizationInDB.Nickname}', SerializedOrganization=N'{OrganizationInDB.SerializedOrganization}'  WHERE Id=N'{OrganizationInDB.Id}'", connection);
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public int CountUsers()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(@$"SELECT count(*) FROM Organizations", connection);
            var resp = Convert.ToString(command.ExecuteScalar());
            if (resp != null) { return int.Parse(resp); }
            else { return 0; }
        }
    }
}
