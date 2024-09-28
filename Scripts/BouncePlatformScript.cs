using Godot;
using System;



public partial class BouncePlatformScript : Area2D
{
	//Placeholder bounce velocity
	double bounceVelocity = 0.0f;

	[Export] float bounceEfficiency = 0.9f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{ 
		BodyEntered += (Node2D body) => On_Platform_Body_Enter(body);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void On_Platform_Body_Enter(Node2D collider)
	{
		player potentialPlayer = collider.GetParent().GetNodeOrNull<player>("./Player");
        if (potentialPlayer != null)
		{
			potentialPlayer.BouncePlayer(bounceEfficiency);
		}
	}
}
