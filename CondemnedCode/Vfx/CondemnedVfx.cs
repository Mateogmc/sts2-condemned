using Condemned.Condemned.Scenes.Vfx;
using Condemned.CondemnedCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;

namespace Condemned.CondemnedCode.Vfx;

public class CondemnedVfx
{
    private const string Root = "res://Condemned/scenes/vfx/";

    public static Node2D? ConsumeJinx(Creature target, Creature owner)
    {
        if (!target.HasPower<JinxPower>()) return null;
        var vfx = Hit(target, owner, "consume_jinx");
        ApplyVfx(target, vfx);
        return vfx;
    }
    
    public static Node2D? ApplyJinx(Creature target)
    {
        if (!target.HasPower<JinxPower>()) return null;
        var vfx = Hit(target, "apply_jinx");
        ApplyVfx(target, vfx);
        return vfx;
    }

    public static Node2D? DarkWave(Creature target)
    {
        var vfx = Hit(target, "dark_wave");
        ApplyVfx(target, vfx);
        return vfx;
    }

    public static string CursedFormRoute()
    {
        return Root + "vfx_cursed_form_vfx.tscn";
    }

    private static Node2D? Hit(Creature target, string name)
    {
        string path = $"{Root}{name}.tscn";

        var scene = GD.Load<PackedScene>(path);
        if (scene == null)
        {
            GD.PushError($"[CondemnedVfxLib] Failed to load Vfx: {path}");
            return null;
        }
        
        var node = scene.Instantiate<Node2D>();
        var targetNode = target.GetCreatureNode();
        if (targetNode != null)
            node.GlobalPosition = targetNode.VfxSpawnPosition;
        
        var creatureNode = target.GetCreatureNode();
        if (creatureNode != null)
        {
            node.GlobalPosition = creatureNode.VfxSpawnPosition;
        }

        return node;
    }

    private static Node2D? Hit(Creature target, Creature owner, string name)
    {
        string path = $"{Root}{name}.tscn";

        var scene = GD.Load<PackedScene>(path);
        if (scene == null)
        {
            GD.PushError($"[CondemnedVfxLib] Failed to load Vfx: {path}");
            return null;
        }

        var node = scene.Instantiate<Node2D>();

        // Fallback: solo posicionar en el target
        var targetNode = target.GetCreatureNode();
        if (targetNode != null)
            node.GlobalPosition = targetNode.VfxSpawnPosition;

        return node;
    }

    private static void ApplyVfx(Creature target, Node2D node)
    {
        Control vfxContainer = target.GetVfxContainer();
        if (vfxContainer != null)
        {
            vfxContainer.AddChildSafely(node);
        }
    }
}