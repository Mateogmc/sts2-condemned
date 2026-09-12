using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Cards.Curse;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class Bitterness : CondemnedCard
{
    public Bitterness() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithDamage(6, 1);
        WithTip(typeof(Injury));
        WithTip(CardKeyword.Exhaust);
    }

    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int count = ResolveEnergyXValue() + (IsUpgraded ? 1 : 0);
        
        await CommonActions.CardAttack(this, play)
            .WithHitCount(count)
            .WithHitFx("vfx/vfx_dramatic_stab")
            .Execute(choiceContext);

        for (int i = 0; i < count; i++)
        {
            var curse = CombatState.CreateCard<Injury>(Owner);
            curse.AddKeyword(CardKeyword.Exhaust);
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(curse, PileType.Discard, Owner));
        }
    }
}