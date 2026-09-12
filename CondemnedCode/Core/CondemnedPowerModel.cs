using BaseLib.Abstracts;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Character;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Runs;

namespace Condemned.CondemnedCode.Core;

public class CondemnedPowerModel(): CustomSingletonModel(HookType.Run)
{
    /*
    #region Dispossess

        private static List<Player> targetDispossessPlayers = new List<Player>();

        public static void AddTargetDispossessPlayer(Player player)
        {
            targetDispossessPlayers.Add(player);
        }
    
        public override bool TryModifyCardRewardOptions(Player player, List<CardCreationResult> cardRewardOptions, CardCreationOptions creationOptions)
        {
            GD.Print("CondemnedPowerModel");
            if (creationOptions.Source != CardCreationSource.Encounter ||
                !targetDispossessPlayers.Contains(player)) return false;
            
            var cursePool = ModelDb.CardPool<CurseCardPool>();
            var condemnedPool = ModelDb.CardPool<CondemnedCardPool>();

            var options = new CardCreationOptions(
                new CardPoolModel[]
                {
                    cursePool,
                    condemnedPool
                },
                CardCreationSource.Other,
                CardRarityOddsType.Uniform,
                c => c.Type == CardType.Curse
            ).WithFlags(
                CardCreationFlags.NoCardPoolModifications |
                CardCreationFlags.NoModifyHooks);

            CardModel curse = CardFactory.CreateForReward(player, 1,
                options).FirstOrDefault()?.Card;

            if (curse == null) return false;
            
            targetDispossessPlayers.Remove(player);

            cardRewardOptions.Add(new CardCreationResult(curse));

            return true;
        }
    #endregion
    */
}