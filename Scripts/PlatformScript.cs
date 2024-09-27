using Godot;
using System;



public partial class PlatformScript : StaticBody2D
{

    enum PlatformMode { UNDECIDED, BOUNCE, CRUMBLE };
    
	//public CharacterBody2D Player;
    [Export] float crumbleTime = 3.5f;
	double deltaTime = 0.0f;
	double bounceVelocity = -0.0f;

	RandomNumberGenerator rng;

    [Export] PlatformMode currentMode = PlatformMode.UNDECIDED;

	[Export] bool isAlive = true;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rng = new RandomNumberGenerator();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//collis
		deltaTime += delta;

		if (deltaTime > crumbleTime)
		{
			isAlive = false;
		}
	}

	public void On_Platform_Body_Enter_Player(CharacterBody2D player)
	{
		if (currentMode == PlatformMode.UNDECIDED)
		{
			int random = rng.RandiRange(1, 2);

			if (random == 1)
			{
				currentMode = PlatformMode.BOUNCE;
			}
			else
			{
				currentMode = PlatformMode.CRUMBLE;

			}
		}

		else if (currentMode == PlatformMode.BOUNCE)
		{
			//Make the player go bouncy!
			bounceVelocity = -500.0f;
			
		}
	//	else
	//	{
	//		if (deltaTime > crumbleTime)
	//		{
				//Kill platform
	//			isAlive = false;
	//			deltaTime = 0.0f;
	//		}
			//Crumble that shit
	//	}
		GD.Print("Collision has happened! \n");
	}
}
