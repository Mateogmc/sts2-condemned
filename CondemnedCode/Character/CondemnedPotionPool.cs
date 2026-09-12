using BaseLib.Abstracts;
using Condemned.CondemnedCode.Extensions;
using Godot;

namespace Condemned.CondemnedCode.Character;

public class CondemnedPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => Condemned.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}