using Godot;
using System;



public partial class PlatformScript : StaticBody2D
{

    enum PlatformMode { UNDECIDED, BOUNCE, CRUMBLE };
    
	//public CharacterBody2D Player;
    [Export] float crumbleTime = 3.5f;
	float deltaTime = 0.0f;

	RandomNumberGenerator rng;

    [Export] PlatformMode currentMode = PlatformMode.UNDECIDED;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rng = new RandomNumberGenerator();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//collis
	}

	public void On_Platform_Body_Enter_Player(CharacterBody2D player)
	{
		if (currentNode == PlatformMode.UNDECIDED)
		{
			int random = rng.RandiRange(1, 2);

			if (random == 1)
			{
				currentNode = PlatformMode.BOUNCE;
			}
			else
			{
				currentNode = PlatformMode.CRUMBLE;

			}
		}

		else if (currentNode == PlatformMode.BOUNCE)
		{
			//Make the player go bouncy!
		}
		else
		{
			//Crumble that shit
		}
		GD.Print("Collision has happened! \n");
	}
}
