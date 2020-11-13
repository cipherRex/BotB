using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotB.Server.Models.UoW
{
    public class PlayerUoW
    {
        DAL.IDAL _iDAL = null;

        public PlayerUoW(DAL.SqlDAL iDAL)
        {
            _iDAL = iDAL;
        }

        public void updatePlayerBalance(string PlayerID, int amt)
        {
            _iDAL.UpdateBalance(PlayerID, amt);
        }

        //public int getPlayerBalance (string PlayerID)
        //{
        //    int bal = Convert.ToInt32(_iDAL.GetPlayerBalance(PlayerID).Rows[0].ItemArray[0]);
        //    return bal;
        //}

        public string CreateFighter(string fighterName, string OwnerId, string pictureFilename)
        {
            string newGuid = Guid.NewGuid().ToString();
            _iDAL.InsertFighter(newGuid, fighterName, OwnerId, pictureFilename);
            updatePlayerBalance(OwnerId, -40);
            return newGuid;
        }

        public void DeleteFighter(string FighterId)
        {
            _iDAL.DeleteFighter(FighterId);
        }
    }
}
