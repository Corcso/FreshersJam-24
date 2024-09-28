using Godot;
using System;

public partial class CrumblyPlatform : StaticBody2D
{
	// Area2D used for collision detection as static body has none?
	Area2D detectionArea;

	// Sprite reference used for changing to the crumbled texture
	Sprite2D sprite;

	// Texture for crumble, export as will be different accross tiles. 
	[Export] Texture2D crumbledTexture;

	// Sound effects
	AudioStreamPlayer2D firstCrumbleSFX; bool firstCrumblePlayOnce = true;
	AudioStreamPlayer2D fullyCrumbleSFX; bool fullyCrumblePlayOnce = true;

    // Timing
    float crumbleTime = 1.5f;
	bool crumbling = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		detectionArea = GetNode<Area2D>("./Area2D");

		sprite = GetNode<Sprite2D>("./Sprite2D");

		firstCrumbleSFX = GetNode<AudioStreamPlayer2D>("./FirstCrumbleSFX");
        fullyCrumbleSFX = GetNode<AudioStreamPlayer2D>("./FullyCrumbleSFX");

		detectionArea.BodyEntered += (Node2D body) => Collision(body);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (crumbling) {
            if (firstCrumblePlayOnce) { firstCrumbleSFX.Play(); firstCrumblePlayOnce = false; }
            crumbleTime -= (float)delta;
			if (crumbleTime < 0) { 
				QueueFree();
			}
			else if (crumbleTime < 0.5f) { 
				sprite.Texture = crumbledTexture;
				if (fullyCrumblePlayOnce) { fullyCrumbleSFX.Play(); fullyCrumblePlayOnce = false; }
				CollisionLayer = 0;
			}
			
		}
	}

	public void Collision(Node2D body) {
		// 
		player potentialPlayer = body as player;
		if (potentialPlayer != null) {
			crumbling = true;
		}


	}
}
