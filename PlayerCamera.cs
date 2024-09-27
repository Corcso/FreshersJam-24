using Godot;
using System;
using System.Net.NetworkInformation;

public partial class PlayerCamera : Camera2D
{
	
	Node2D dude;

    const int ROOM_HEIGHT = 10;

	int totalHeight = 10;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Vector2 pos = Position;
        
		dude = GetNode<Node2D>("../dude");

		pos = Position;

		pos.X = dude.Position.X;

		pos.Y = ROOM_HEIGHT / 2;

		Position = pos;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Vector2 pos = Position;
        
		if (dude.Position.Y <= totalHeight)
		{
			pos.Y = pos.Y - ROOM_HEIGHT;

			totalHeight -= ROOM_HEIGHT;

			GD.Print(dude.Position.Y);
        }

		Position = pos;

	}
}
