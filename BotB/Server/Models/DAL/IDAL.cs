﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BotB.Server.Models.DAL
{
    public interface IDAL
    {
        DataTable GetPlayerFighters(string playerId);
        DataTable GetPlayerInfo(string playerId);
        void DeleteFighterAsync(string fighterId);
        public void UpdateBalance(string playerId, int Amt);
        void InsertFighter(string fighterId, string fighterName, string ownerId, string pictureFilename);

    }
}
