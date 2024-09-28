
using Godot;
using System;

public partial class WaterController : Area2D
{
	// Called when the node enters the scene tree for the first time.
	GameManager gameManager;
	player player;
	AudioStreamPlayer2D audioStreamPlayer;

    float waterSpeed;

	const int ROOM_HEIGHT = 512-32;

	public override void _Ready()
	{
		gameManager = GetTree().Root.GetNode<GameManager>("./GameManager");

		player = GetNode<player>("../Player");

		audioStreamPlayer = GetNode<AudioStreamPlayer2D>("./AudioStreamPlayer2D");

		BodyEntered += (Node2D body) => Collided(body);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 pos = Position;

		audioStreamPlayer.Position = new Vector2(player.Position.X, audioStreamPlayer.Position.Y);

		float playerRoom = ((float)Math.Floor(-player.Position.Y / ROOM_HEIGHT));
		float waterRoom = ((float)Math.Floor(-Position.Y / ROOM_HEIGHT));

		waterSpeed = -10 * (float)delta;

		if (playerRoom > waterRoom)
		{

			waterSpeed = -(playerRoom - waterRoom) * 10 * (float)delta;

		}

		pos.Y += waterSpeed;

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
