using Godot;
using System;
using System.Net.NetworkInformation;

public partial class PlayerCamera : Camera2D
{
	
	[Export] Node2D dude;

	const int ROOM_HEIGHT = 512-32;

	float camSpeed;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Vector2 pos = Position;

		pos = Position;

		pos.X = dude.Position.X;

		Position = pos;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 pos = Position;

		camSpeed = 5 * (float)delta;

		float targetY = -((float)Math.Floor(-dude.Position.Y / ROOM_HEIGHT) * ROOM_HEIGHT + (ROOM_HEIGHT / 2));

		pos.Y += (targetY - pos.Y) * camSpeed;
		
		Position = pos;

	}
}
