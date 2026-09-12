using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Condemned.CondemnedCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class Sigil : CondemnedCard
{
    public Sigil() : base(0, CardType.Skill, CardRarity.Token, TargetType.None)
    {
        WithVar("SigilCards", 1);
        WithKeyword(CardKeyword.Retain);
        WithKeyword(CondemnedKeywords.Brittle);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
    }
}