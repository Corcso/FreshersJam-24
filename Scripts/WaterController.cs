
using Godot;
using System;

public partial class WaterController : Area2D
{
	// Called when the node enters the scene tree for the first time.
	GameManager gameManager;
	CharacterBody2D player;

    float waterSpeed;

	public override void _Ready()
	{
		gameManager = GetTree().Root.GetNode<GameManager>("./GameManager");

		player = GetNode<CharacterBody2D>("./Player");

		BodyEntered += (Node2D body) => Collided(body);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 pos = Position;

		waterSpeed = -10 * (float)delta;

		pos.Y += waterSpeed;

		//GD.Print(pos.Y);

		Position = pos;	
	}

	private void Collided(Node2D body) {
		player potentialPlayer = body.GetParent().GetNodeOrNull<player>("./" + body.Name);
		if (potentialPlayer != null) {
			GD.Print("DIE");
            gameManager.currentGameState = GameManager.GameState.DEAD;

        }
	}
}
