using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Forms;
using MegaCrit.Sts2.Core.TestSupport;

namespace Condemned.CondemnedCode.Vfx;

public partial class NCursedFormVfx : NFormVfx
{
    private static readonly string _scenePath =
        CondemnedVfx.CursedFormRoute();

    public static NCursedFormVfx? Create(Creature target)
    {
        if (TestMode.IsOn)
            return null;

        NCreature creatureNode =
            NCombatRoom.Instance.GetCreatureNode(target);

        if (creatureNode == null)
            return null;

        var scene = PreloadManager.Cache.GetScene(_scenePath);

        var node = scene.Instantiate();

        GD.Print($"Cursed Form root: {node.GetType()}");
        GD.Print($"Cursed Form script: {node.GetScript()}");

        var formVfx = node as NCursedFormVfx;

        if (formVfx == null)
        {
            GD.PushError(
                $"vfx_cursed_form_vfx no tiene NCursedFormVfx como root. " +
                $"Root = {node.GetType()}, Script = {node.GetScript()}"
            );

            node.QueueFree();
            return null;
        }

        formVfx.Initialize(target.Player);

        creatureNode.Visuals.AddFormVfx(formVfx);

        return formVfx;
    }
}