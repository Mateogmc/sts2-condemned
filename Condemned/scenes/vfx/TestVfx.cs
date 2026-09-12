using Godot;

public partial class TestVfx : Node2D
{
	public override void _Ready()
	{
		var anim = GetNode<AnimationPlayer>("Play");
		anim.Play("play");

		anim.AnimationFinished += _ => QueueFree();
	}
}
