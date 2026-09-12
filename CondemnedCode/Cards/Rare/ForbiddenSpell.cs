using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Condemned.CondemnedCode.Cards.Rare;

public class ForbiddenSpell : CondemnedCard
{
    public ForbiddenSpell() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(34, 6);
        WithVar("JinxRequirement", 10, -2);
        WithKeywords(CardKeyword.Unplayable, CardKeyword.Exhaust);
        WithTip(typeof(JinxPower));
        WithTip(StaticHoverTip.Stun);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        
        int targetJinx = play.Target.GetPowerAmount<JinxPower>();
                                                       
        if (targetJinx >= DynamicVars["JinxRequirement"].BaseValue)
        {
            await CreatureCmd.Stun(play.Target);
            CommonActions.Apply<JinxPower>(choiceContext, play.Target, this, -10m);
        }
        
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
    }
}