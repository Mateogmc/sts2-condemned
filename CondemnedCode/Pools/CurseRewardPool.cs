using Condemned.CondemnedCode.Character;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Condemned.CondemnedCode.Pools;

public sealed class CurseRewardPool : CardPoolModel
{
    public override string Title => "dispossess_curse";

    public override string EnergyColorName => "colorless";

    public override string CardFrameMaterialPath => "card_frame_curse";

    public override Color DeckEntryCardColor => new Color("585B61FF");

    public override bool IsColorless => false;

    protected override CardModel[] GenerateAllCards()
    {
        var cursePool = ModelDb.CardPool<CurseCardPool>();
        var condemnedPool = ModelDb.CardPool<CondemnedCardPool>();

        return cursePool.AllCards
            .Concat(condemnedPool.AllCards.Where(c => c.Type == CardType.Curse))
            .DistinctBy(c => c.Id)
            .ToArray();
    }
}