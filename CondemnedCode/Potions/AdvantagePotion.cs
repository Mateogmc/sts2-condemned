using Condemned.CondemnedCode.Cards.Curse;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.Keywords;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace Condemned.CondemnedCode.Potions;

public class AdvantagePotion: CondemnedPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
        NCombatRoom.Instance?.PlaySplashVfx(target, new Color(0.3f, 0.1f, 0.3f));

        if (target.Player != null)
        {
            CardModel curse1 = target.CombatState.CreateCard<Obsession>(target.Player);
            CardModel curse2 = Owner.Creature.CombatState.CreateCard<Obsession>(Owner);
            
            CardCmd.Upgrade(curse1);
            CardCmd.Upgrade(curse2);
            
            CardPileCmd.AddGeneratedCardToCombat(curse1, PileType.Hand, target.Player);
            CardPileCmd.AddGeneratedCardToCombat(curse2, PileType.Hand, target.Player);
        }
        
        
        return Task.CompletedTask;
    }
}