using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Condemned.CondemnedCode.Cards.Ancient;
using Condemned.CondemnedCode.Cards.Basic;
using Condemned.CondemnedCode.Cards.Common;
using Condemned.CondemnedCode.Cards.Curse;
using Condemned.CondemnedCode.Cards.Rare;
using Condemned.CondemnedCode.Cards.Uncommon;
using Condemned.CondemnedCode.Extensions;
using Condemned.CondemnedCode.Relics;
using Condemned.CondemnedCode.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Condemned.CondemnedCode.Character;

public class Condemned : PlaceholderCharacterModel
{
    public const string CharacterId = "Condemned";

    public static readonly Color Color = new("#666666");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 75;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeCondemned>(),
        ModelDb.Card<StrikeCondemned>(),
        ModelDb.Card<StrikeCondemned>(),
        ModelDb.Card<StrikeCondemned>(),
        ModelDb.Card<DefendCondemned>(),
        ModelDb.Card<DefendCondemned>(),
        ModelDb.Card<DefendCondemned>(),
        ModelDb.Card<DefendCondemned>(),
        ModelDb.Card<Disparage>(),
        ModelDb.Card<Atone>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<CursedEye>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<CondemnedCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<CondemnedRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<CondemnedPotionPool>();


    public override string CustomIconTexturePath => "character_icon_condemned.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_condemned.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_condemned_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_condemned.png".CharacterUiPath();
    
    public override string CustomArmPointingTexturePath => "multiplayer_hand_condemned_point.png".CharacterUiPath();
    public override string CustomArmRockTexturePath => "multiplayer_hand_condemned_rock.png".CharacterUiPath();
    public override string CustomArmPaperTexturePath => "multiplayer_hand_condemned_paper.png".CharacterUiPath();
    public override string CustomArmScissorsTexturePath => "multiplayer_hand_condemned_scissors.png".CharacterUiPath();

    public override string CustomCharacterSelectBg => CondemnedResources.NCharSelectBgCondemnedPath;
    public override string CharacterSelectSfx => CondemnedResources.CharacterSelectSfx;
    public override string CharacterTransitionSfx => CondemnedResources.CharacterTransitionSfx;
}