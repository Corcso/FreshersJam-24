using Godot;
using System;

public partial class dude : AnimatedSprite2D
{

	


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Vector2 pos = Position;

		if (Input.IsKeyPressed(Key.W))
		{

			pos.Y -= 5;

		}

		Position = pos;

	}
}
