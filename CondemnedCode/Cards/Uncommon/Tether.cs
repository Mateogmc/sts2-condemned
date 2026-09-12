using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Tether : CondemnedCard
{
    public Tether() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(16, 4);
        WithTip(CardKeyword.Exhaust);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
        
        try {
        
            CardPile hand = new CardPile(PileType.Play);
            foreach (CardModel card in Owner.PlayerCombatState.Hand.Cards.ToList().FindAll(c => c.Keywords.Contains(CardKeyword.Exhaust)))
            {
                if (card == this) continue;
                hand.AddInternal(card);
            }
            var cardPileAddResultList = (await CardSelectCmd.FromCombatPile(choiceContext, hand, Owner,
                new CardSelectorPrefs(SelectionScreenPrompt, 1))).ToList();

            foreach (CardModel card in cardPileAddResultList)
            {
                card.RemoveKeyword(CardKeyword.Exhaust);
                CondemnedKeywordModel.TriggerCardKeywordsModified(this);
                CardCmd.Preview(card, 1f);
            }
        }
        catch (NullReferenceException e)
        {
            GD.Print($"[CONDEMNED] No target for Pyrric Strike");
        }
        
    }
}