using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using RewardTypeExtensions = Condemned.CondemnedCode.Extensions.RewardTypeExtensions;

namespace Condemned.CondemnedCode.Vfx;

public class CurseCardReward(
    CardCreationOptions options,
    int cardCount,
    Player player,
    PlayerChoiceSynchronizer? synchronizer = null)
    : CardReward(options, cardCount, player, synchronizer)
{
    private const string Icon = "res://Condemned/images/icons/reward_icon_curse.png";

    protected override string IconPath => Icon;

    protected override RewardType RewardType => RewardTypeExtensions.Curse;
}