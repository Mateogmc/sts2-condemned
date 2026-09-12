using Godot;
using MegaCrit.Sts2.Core.Assets;

namespace Condemned.CondemnedCode.Utils;

public class CondemnedResources
{
    public const string NCharSelectBgCondemnedPath = "res://Condemned/scenes/screens/char_select_bg_condemned.tscn";
    public const string CharacterSelectSfx = "res://Condemned/audio/character_select_sfx.ogg";
    public const string CharacterTransitionSfx = "res://Condemned/audio/character_transition_sfx.ogg";
    

    public static readonly IEnumerable<string> AssetPaths =
    [
        NCharSelectBgCondemnedPath,
        CharacterSelectSfx,
        CharacterTransitionSfx,
    ];
}