using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Cards.Token;
using Condemned.CondemnedCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Cards.Multiplayer;

public class ForbiddenSigils : CondemnedCard
{
    public ForbiddenSigils() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
    {
        WithCards(1, 1);
        WithTip(typeof(Sigil));
        WithKeyword(CardKeyword.Exhaust);
    }

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {

        foreach (Creature teammate in CombatState.GetTeammatesOf(Owner.Creature))
        {
            for (int i = 0; i < DynamicVars.Cards.IntValue; i++)
            {
                int index = (int)Math.Floor(Owner.RunState.Rng.Niche.NextDouble() * UsableCardKeywords.Count);
                string keyword = UsableCardKeywords[index];
                CardModel card = SigilCardFactory.CreateSigilCard(keyword, teammate.Player, CombatState);
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, teammate.Player);
                await Cmd.Wait(0.1f);
            }
        }
    }
}