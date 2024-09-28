using Godot;
using System;
using System.Net.NetworkInformation;

public partial class PlayerCamera : Camera2D
{
	
	Node2D dude;

	const int ROOM_HEIGHT = 500;

	float camSpeed;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Vector2 pos = Position;
		
		dude = GetNode<Node2D>("../dude");

		pos = Position;

		pos.X = dude.Position.X;

		//pos.Y = ROOM_HEIGHT / 2;

		Position = pos;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 pos = Position;

		camSpeed = 5 * (float)delta;

		float targetY = -((float)Math.Floor(-dude.Position.Y / ROOM_HEIGHT) * ROOM_HEIGHT + (ROOM_HEIGHT / 2));


		pos.Y += (targetY - pos.Y) * camSpeed;


		GD.Print(targetY);
		

		Position = pos;

	}
}
