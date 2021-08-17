using Ch3_PartyRSVP.Properties;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;


namespace Ch3_PartyRSVP.Services
{
    public class DataService
    {
        private readonly IConfiguration _configuration;

        public DataService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ManageData()
        {
            DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
            builder.ConnectionString = _configuration.GetConnectionString("Default");
            builder.TryGetValue("Data Source", out object databaseFile);

            if (!File.Exists(databaseFile.ToString()))
            {
                using (var connection = new SqliteConnection(_configuration.GetConnectionString("Default")))
                {
                    connection.Open();
                    connection.Execute(Resources.SQLiteDB_txt);
                }
            }
        }
    }
}
