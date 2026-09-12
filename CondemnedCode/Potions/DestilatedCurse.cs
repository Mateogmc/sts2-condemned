using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.Keywords;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace Condemned.CondemnedCode.Potions;

public class DestilatedCurse: CondemnedPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
        NCombatRoom.Instance?.PlaySplashVfx(target, new Color(0.3f, 0.1f, 0.3f));
        foreach (CardModel card in target.Player.PlayerCombatState.Hand.Cards.Where(c => !c.Keywords.Contains(CondemnedKeywords.Cursed)))
        {
            card.AddKeyword(CondemnedKeywords.Cursed);
            CondemnedKeywordModel.TriggerCardKeywordsModified(card);
        }
        
        return Task.CompletedTask;
    }
}