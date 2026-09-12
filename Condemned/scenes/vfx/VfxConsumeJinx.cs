using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using System.Threading.Tasks;

namespace Condemned.Condemned.Scenes.Vfx;

// Asegúrate de que la clase sea pública
public partial class VfxConsumeJinx : Node2D
{
	private float _progress;
	private ShaderMaterial? _material;
	private AnimationPlayer? _animPlayer;

	// Constructor público sin parámetros
	public VfxConsumeJinx()
	{
		// Inicialización básica (sin cargar recursos)
	}

	public override void _Ready()
	{
		// Carga segura del material (solo si existe)
		if (ResourceLoader.Exists("res://Condemned/scenes/vfx/ConsumeJinx.tres"))
		{
			_material = GD.Load<ShaderMaterial>("res://Condemned/scenes/vfx/ConsumeJinx.tres");
			if (_material != null)
			{
				// Si tienes un TextureRect, asígnale el material
				var texRect = GetNodeOrNull<TextureRect>("CloudTexture");
				if (texRect != null)
					texRect.Material = _material;
			}
		}

		// Buscar AnimationPlayer si existe
		_animPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
		_animPlayer?.Play("play"); // Si hay una animación llamada "play"
	}

	public void Initialize(Creature creature)
	{
		if (creature != null)
		{
			GlobalPosition = creature.GetCreatureNode().GlobalPosition;
			// Ajusta desplazamiento si quieres
			Position += new Vector2(0, -40);
		}
	}

	public override void _Process(double delta)
	{
		_progress += (float)delta / 0.7f;
		_progress = Mathf.Min(_progress, 1.0f);

		if (_material != null)
			_material.SetShaderParameter("progress", _progress);

		if (_progress >= 1.0f)
			QueueFree();
	}
}
