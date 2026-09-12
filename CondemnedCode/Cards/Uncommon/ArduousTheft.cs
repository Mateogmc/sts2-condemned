using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Odds;

namespace Condemned.CondemnedCode.Cards.Uncommon;

public class ArduousTheft : CondemnedCard
{
    public ArduousTheft() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(3, 1);
        WithVar("InjuryCount", 1, 1);
        WithTip(typeof(Injury));
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Draw(this, choiceContext);

        for (int i = 0; i < DynamicVars["InjuryCount"].BaseValue; i++)
        {
            CardModel injury = CombatState.CreateCard<Injury>(Owner);
            await CardPileCmd.AddGeneratedCardToCombat(injury, PileType.Hand, Owner);
        }
    }
}