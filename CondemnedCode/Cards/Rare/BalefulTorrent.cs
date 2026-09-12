using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.StaticHoverTips;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Condemned.CondemnedCode.Cards.Rare;

public class BalefulTorrent : CondemnedCard
{

    public BalefulTorrent() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithDamage(7, 3);
        WithVar("HitCountBase", 0);
        WithVar("HitCountExtra", 1);
        WithVar(
            new CustomCalculatedVar("HitCount")
                .WithMultiplier((card, target) => ((CardModel)card).Owner.PlayerCombatState.AllCards.Count(c => c.IsCurse()))
        );
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        List<CardModel> curses = Owner.PlayerCombatState.AllCards.Where(c => c.IsCurse()).ToList();

        foreach (CardModel card in curses)
        {
            await CardCmd.Exhaust(choiceContext, card);
        }
        
        for (int i = 0; i < DynamicVars["HitCount"].PreviewValue; i++)
        {
            await CommonActions.CardAttack(this, play).WithHitFx("vfx/vfx_fire_burst").Execute(choiceContext);
        } 
    }
}