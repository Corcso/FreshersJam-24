using Godot;
using System;
using System.Globalization;

public partial class FinalRoomMusicTrigger : Area2D
{
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += (Node2D body) => Collided(body);
	}

	private void Collided(Node2D body) {
		if (body is WaterController) { 
			// Add switch here
		}
	}
}
