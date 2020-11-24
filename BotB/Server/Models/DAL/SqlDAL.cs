using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;

namespace BotB.Server.Models.DAL
{
    public class SqlDAL: IDAL
    {
        IDbConnection _IDbConnection;

        public SqlDAL(IDbConnection dbConnection) 
        {
            _IDbConnection = dbConnection;
        }

        public DataTable GetPlayerFighters(string playerId) 
        {

            DataTable table = new DataTable();
 
            using (var cmd = new SqlCommand("dbo.brawlGetPlayerFighters", (SqlConnection)_IDbConnection))
            using (var da = new SqlDataAdapter(cmd))

            {
                cmd.Parameters.Add("@PlayerID", SqlDbType.VarChar);
                cmd.Parameters["@PlayerID"].Value = playerId;
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }

            return table;
        }

        public DataTable GetPlayerInfo(string playerId)
        {

            DataTable table = new DataTable();
  
            using (var cmd = new SqlCommand("dbo.brawlGetPlayer", (SqlConnection)_IDbConnection))
            using (var da = new SqlDataAdapter(cmd))

            {
                cmd.Parameters.Add("@PlayerID", SqlDbType.VarChar);
                cmd.Parameters["@PlayerID"].Value = playerId;
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }

            return table;
        }

        public async void DeleteFighterAsync(string fighterId)
        {

            DataTable table = new DataTable();

            using (var cmd = new SqlCommand("dbo.brawlDeleteFighter", (SqlConnection)_IDbConnection))
            {
                cmd.Parameters.Add("@FighterID", SqlDbType.VarChar);
                cmd.Parameters["@FighterID"].Value = fighterId;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQueryAsync();
            }

        }

        public void UpdateBalance(string playerId, int Amt)
        {

            DataTable table = new DataTable();

            using (var cmd = new SqlCommand("dbo.brawlUpdateBalance", (SqlConnection)_IDbConnection))
            {
                cmd.Parameters.Add("@PlayerID", SqlDbType.VarChar);
                cmd.Parameters["@PlayerID"].Value = playerId;
                cmd.Parameters.Add("@Amt", SqlDbType.SmallInt);
                cmd.Parameters["@Amt"].Value = Amt;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }

        }

        public void InsertFighter(string fighterId, string fighterName, string ownerId, string pictureFilename)
        {

            DataTable table = new DataTable();
            
            using (var cmd = new SqlCommand("dbo.brawlInsertFighter", (SqlConnection)_IDbConnection))
            {
                cmd.Parameters.Add("@FighterID", SqlDbType.VarChar);
                cmd.Parameters["@FighterID"].Value = fighterId;

                cmd.Parameters.Add("@OwnerID", SqlDbType.VarChar);
                cmd.Parameters["@OwnerID"].Value = ownerId;

                cmd.Parameters.Add("@FighterName", SqlDbType.VarChar);
                cmd.Parameters["@FighterName"].Value = fighterName;

                cmd.Parameters.Add("@PictureFilename", SqlDbType.VarChar);
                cmd.Parameters["@PictureFilename"].Value = pictureFilename;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }

        }

    }
}
