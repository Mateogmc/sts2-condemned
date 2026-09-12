using BaseLib.Utils;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.Vfx;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Forms;

namespace Condemned.CondemnedCode.Powers;

public class RegressionPower() : CondemnedPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override bool ShouldReceiveCombatHooks => true;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        var allCards = new CardPile(PileType.Play);
        foreach (var card in Owner.Player.PlayerCombatState.ExhaustPile.Cards)
        {
            allCards.AddInternal(card);
        }

        var selectedCards = await CardSelectCmd.FromCombatPile(
            choiceContext,
            allCards,
            Owner.Player,
            new CardSelectorPrefs(SelectionScreenPrompt, Amount)
        );

        var cards = selectedCards.ToList();

        foreach (CardModel card in cards)
        {
            await CardPileCmd.Add(card, PileType.Hand.GetPile(Owner.Player));
            card.EnergyCost.SetThisCombat(card.EnergyCost.GetWithModifiers(CostModifiers.All) - 1);
        }
    }
}