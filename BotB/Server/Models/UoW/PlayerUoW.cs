using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotB.Server.Models.Repositories.PlayerRepos;
using BotB.Server.Models.DAL;

namespace BotB.Server.Models.UoW
{
    public class PlayerUoW
    {
        IDAL _iDAL = null;
        IPlayerRepo _playerRepo = null;

        public PlayerUoW(SqlDAL iDAL, IPlayerRepo PlayerRepo)
        {
            _iDAL = iDAL;
            _playerRepo = PlayerRepo;
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

        public async void DeleteFighter(string FighterId)
        {
            _iDAL.DeleteFighterAsync(FighterId);
        }

        public void EndGame(string WinningFighterId, string LosingFighterId) 
        {
            _iDAL.DeleteFighterAsync(LosingFighterId);

            

            Player winner = _playerRepo.GetPlayer(WinningFighterId);


            int winnersCurrentBalance = winner.Balance;
            _iDAL.UpdateBalance(WinningFighterId, winnersCurrentBalance + 5);

        }

    }
}
