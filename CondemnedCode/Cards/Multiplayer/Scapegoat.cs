using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Cards.Token;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace Condemned.CondemnedCode.Cards.Multiplayer;

public class Scapegoat : CondemnedCard
{
    public Scapegoat() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies)
    {}
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        foreach (Creature teammate in CombatState.GetTeammatesOf(Owner.Creature))
        {
            if (teammate == Owner.Creature) continue;

            var cursesInHand = teammate.Player.PlayerCombatState.AllCards
                .Where(c => c.IsCurse() && c.Pile?.Type != PileType.Exhaust)
                .ToList();

            foreach (CardModel curse in cursesInHand)
            {
                CardModel clone = curse.CreateCloneForPlayer(Owner);
                await CardPileCmd.Add(clone, PileType.Discard);
                await CardPileCmd.RemoveFromCombat(curse);
            
                PileType.Discard.GetPile(Owner).InvokeCardAddFinished();
                
                CardCmd.Preview(clone, 0.4f);
            }
        }
    }
}