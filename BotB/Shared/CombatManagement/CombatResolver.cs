using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BotB.Shared.CombatManagement
{
    public interface ICombatResolver
    {
        CombatResult ResolveRound(List<CombatMove> CombatMoves, CombatSession Session);
        int NumberPreviousFalseBlocks(CombatSession Session, string FighterId);
        int NumberPreviousSuccessfulHeals(CombatSession Session, string FighterId);
        int NumberPreviousSuccessfulStrikes(CombatSession Session, string FighterId);
        int NumberPreviousSuccessfulBlocks(CombatSession Session, string FighterId);
    }

    public class CombatResolver : ICombatResolver
    {
        const int WHITE_KNIGHT = 0;
        const int BLACK_KNIGHT = 1;

        #region Counters
        public int NumberPreviousFalseBlocks(CombatSession Session, string FighterId)
        {
            int ret = 0;

            if (Session.CombatRounds.Count == 0) { return 0; }

            string thisFighterId = FighterId;
            string otherFighterId = OtherFighterId(Session, FighterId);

            for (int i = Session.CombatRounds.Count - 1; i > -1; i--)
            {
                CombatRound combatRound = Session.CombatRounds[i];

                if (combatRound.Result != null)
                {
                    if (combatRound.Moves.Where(x => x.FighterId == thisFighterId).FirstOrDefault().Action == CombatEnums.BLOCK &&
                        combatRound.Moves.Where(x => x.FighterId == otherFighterId).FirstOrDefault().Action != CombatEnums.SWING
                    )
                    {
                        ret++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return ret;
        }

        public int NumberPreviousSuccessfulBlocks(CombatSession Session, string FighterId)
        {
            int ret = 0;

            if (Session.CombatRounds.Count == 0) { return 0; }

            string thisFighterId = FighterId;
            string otherFighterId = OtherFighterId(Session, FighterId);

            for (int i = Session.CombatRounds.Count - 1; i > -1; i--)
            {
                CombatRound combatRound = Session.CombatRounds[i];

                if (combatRound.Result != null)
                {
                    if (combatRound.Moves.Where(x => x.FighterId == thisFighterId).FirstOrDefault().Action == CombatEnums.BLOCK &&
                        combatRound.Moves.Where(x => x.FighterId == otherFighterId).FirstOrDefault().Action == CombatEnums.SWING
                    )
                    {
                        ret++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return ret;
        }

        public int NumberPreviousSuccessfulHeals(CombatSession Session, string FighterId)
        {
            int ret = 0;

            if (Session.CombatRounds.Count == 0) { return 0; }

            string thisFighterId = FighterId;
            string otherFighterId = OtherFighterId(Session, FighterId);

            for (int i = Session.CombatRounds.Count - 1; i > -1; i--)
            {
                CombatRound combatRound = Session.CombatRounds[i];

                if (combatRound.Result != null)
                {
                    if (combatRound.Moves.Where(x => x.FighterId == thisFighterId).FirstOrDefault().Action == CombatEnums.REST &&
                        combatRound.Moves.Where(x => x.FighterId == otherFighterId).FirstOrDefault().Action != CombatEnums.SWING
                    )
                    {
                        ret++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return ret;
        }

        public int NumberPreviousSuccessfulStrikes(CombatSession Session, string FighterId)
        {
            int ret = 0;

            if (Session.CombatRounds.Count == 0) { return 0; }

            string thisFighterId = FighterId;
            string otherFighterId = OtherFighterId(Session, FighterId);

            for (int i = Session.CombatRounds.Count - 1; i > -1; i--)
            {
                CombatRound combatRound = Session.CombatRounds[i];

                if (combatRound.Result != null)
                {
                    if (combatRound.Moves.Where(x => x.FighterId == thisFighterId).FirstOrDefault().Action == CombatEnums.SWING &&
                        combatRound.Moves.Where(x => x.FighterId == otherFighterId).FirstOrDefault().Action == CombatEnums.REST
                    )
                    {
                        ret++;
                    }
                    else 
                    {
                        break;
                    }
                }
            }

            return ret;
        }
        #endregion

        public CombatResult ResolveRound(List<CombatMove> CombatMoves, CombatSession Session)
        {

            string whiteKnightId = CombatMoves[WHITE_KNIGHT].FighterId;
            string blackKnightId = CombatMoves[BLACK_KNIGHT].FighterId;

            CombatResult combatResult = new CombatResult();

            switch (CombatMoves[WHITE_KNIGHT].Action) 
            {
                case CombatEnums.SWING:
                    switch (CombatMoves[BLACK_KNIGHT].Action) 
                    {
                        #region WHITE swings BLACK swings
                        case CombatEnums.SWING:
                            //WHITE swings BLACK swings

                            //Random rng = new Random();
                            bool randomBool = new Random().Next(0, 2) > 0;
                            string randomWinnerFighterId = randomBool ? whiteKnightId : blackKnightId;
                            string randomLoserFighterId = randomBool ? blackKnightId: whiteKnightId;

                            combatResult.CombatAnimationInstructions[randomWinnerFighterId].AnimCommand = AnimationCommand.AC_COUNTERPARRY;
                            combatResult.CombatAnimationInstructions[randomLoserFighterId].AnimCommand = AnimationCommand.AC_PARRY;
                            combatResult.HPAdjustment[randomLoserFighterId] = -1;

                            combatResult.Comments = "Both knights swing. Random damage.";
                             
                            break;
                        #endregion

                        #region WHITE swings BLACK blocks
                        case CombatEnums.BLOCK:
                            //WHITE swings BLACK blocks

                            int numberPreviousSuccessfulBlocks = NumberPreviousSuccessfulBlocks(Session, blackKnightId);
                            combatResult.CombatAnimationInstructions[whiteKnightId].AnimCommand = AnimationCommand.AC_SWING;
                            combatResult.CombatAnimationInstructions[blackKnightId].AnimCommand = AnimationCommand.AC_SWING;

                            if (numberPreviousSuccessfulBlocks < 2) 
                            {
                                
                            }

                            break;
                        #endregion

                        #region WHITE swings BLACK rests
                        case CombatEnums.REST:
                            //WHITE swings BLACK rests

                            int numberPreviousSuccessfulStrikes = NumberPreviousSuccessfulStrikes(Session, whiteKnightId);

                            if (numberPreviousSuccessfulStrikes == 0) 
                            {
                                combatResult.CombatAnimationInstructions[whiteKnightId].AnimCommand = AnimationCommand.AC_KICK;
                                combatResult.CombatAnimationInstructions[blackKnightId].AnimCommand = AnimationCommand.AC_GROINED;
                            }
                            else 
                            {
                                combatResult.CombatAnimationInstructions[whiteKnightId].AnimCommand = AnimationCommand.AC_CLEAVE;
                                combatResult.CombatAnimationInstructions[blackKnightId].AnimCommand = AnimationCommand.AC_CLEAVED;
                            }

                            combatResult.HPAdjustment[blackKnightId] = -2 - numberPreviousSuccessfulStrikes;
                            break;
                        #endregion
                    }
                    break;

                case CombatEnums.BLOCK:
                    break;

                case CombatEnums.REST:
                    break;

            }

            throw new NotImplementedException();
        }

        private string OtherFighterId(CombatSession Session, string thisFighterId)
        {
            string otherFighterId = thisFighterId == Session.CombatRounds[0].Moves[WHITE_KNIGHT].FighterId ?
                                    Session.CombatRounds[0].Moves[BLACK_KNIGHT].FighterId :
                                    Session.CombatRounds[0].Moves[WHITE_KNIGHT].FighterId;
            return otherFighterId;
        }
    }
}
