using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Character;
using Condemned.CondemnedCode.Extensions;

namespace Condemned.CondemnedCode.Potions;

[Pool(typeof(CondemnedPotionPool))]
public abstract class CondemnedPotion : CustomPotionModel
{
    public override string CustomPackedImagePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
    public override string CustomPackedOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".PotionImagePath();
}