using Condemned.CondemnedCode.Cards.Curse;
using Condemned.CondemnedCode.Relics;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Relics;

public class CursedEye() : CondemnedRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner || Owner.PlayerCombatState is not { TurnNumber: 1 }) return;
        var curse = combatState.CreateCard<Obsession>(Owner);
        await CardPileCmd.AddGeneratedCardToCombat(curse, PileType.Hand, player);
        Flash();
    }

    public override RelicModel? GetUpgradeReplacement() => ModelDb.Relic<BlessedEye>();
}