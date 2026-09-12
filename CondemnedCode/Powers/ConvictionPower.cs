using BaseLib.Abstracts;
using Condemned.CondemnedCode.Cards.Uncommon;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Condemned.CondemnedCode.Powers;

public class ConvictionPower : TemporaryStrengthPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Conviction>();

    protected override bool IsPositive => false;
}