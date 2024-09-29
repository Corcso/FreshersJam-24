using Godot;
using System;

public partial class player : RigidBody2D
{

	[Export]
	public float moveVelocity = 100.0f;
	public float dashVelocity = 700.0f;
	public float jumpVelocity= -400.0f;

	float timeDash = 0;
	int jumpCount=0;
	bool dashStat = false;
	[Export]
	bool jumpDouble = true;
	[Export]
	bool dash = false;

	//in ms
	[Export]
	bool dashAvailable;

	[Export] AudioStreamPlayer jumpSound;
	[Export] AudioStreamPlayer dashSound;
	[Export] AudioStreamPlayer doubleJumpSound;

	bool bounceMe;
	float bounceEfficiency;

	ShapeCast2D floorChecker;
	GameManager gameManager;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	// Animator
	AnimatedSprite2D spriteAnimator;
	string animationState = "Idle";
	bool animationFlipped = false;

    public override void _Ready()
    {
        gameManager = GetTree().Root.GetNode<GameManager>("./GameManager");
        floorChecker = GetNode<ShapeCast2D>("./FloorCaster");
		floorChecker.ExcludeParent = true;

		spriteAnimator = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		spriteAnimator.Play("Idle");
		//this.Position = 
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		string thisFramesAnimationState = "Idle";
		bool thisFramesAnimationFlipped = animationFlipped;


		Vector2 velocity = state.LinearVelocity;
        //velocity.Y += gravity * (float)delta;

        if (this.Position.Y < -4855 && (this.Position.X > 71 || this.Position.X < -72) && floorChecker.IsColliding() && gameManager.currentGameState != GameManager.GameState.WON)
        {
            //Player completing level
            GD.Print("WIN!");
            gameManager.currentGameState = GameManager.GameState.WON;

        }

        if (gameManager.currentGameState == GameManager.GameState.DEAD)
		{

			return;

		}

		if (gameManager.currentGameState == GameManager.GameState.WON)
		{
			moveVelocity = 0.0f;
			dashVelocity = 0.0f;
			GD.Print("WINNER!");
		}

        // Default animation to idle
       //spriteAnimator.Animation = "Idle";


        float directionX = Input.GetAxis("Left", "Right");
		// First set us walking if we are this is the lowest priority animation
		if (directionX != 0) {
			thisFramesAnimationState = "Walk";
			thisFramesAnimationFlipped = directionX < 0;
		}


		//direction.X = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		if (floorChecker.IsColliding()&&jumpDouble==true)
		{
			jumpCount = 1;
			dashAvailable = true;
		}
		
		
		if (Input.IsActionJustPressed("Jump")&& floorChecker.IsColliding())
		{
			velocity.Y = jumpVelocity;
			jumpSound.Play();
			
		}
        else if (Input.IsActionJustPressed("Jump")&&jumpCount>0)
		{
			velocity.Y = jumpVelocity;
			jumpCount--;
			doubleJumpSound.Play();
		}

        // Bounce
        if (bounceMe)
        {
			// Bounce us
            velocity.Y = -velocity.Y * bounceEfficiency;
			// Set bounce to false
            bounceMe = false;
			// Also allow double jump again
			jumpCount = 1;
        }
        

        //x axis movement


        if (Input.IsActionPressed("Sprint"))
		{
			velocity.X = directionX * moveVelocity*2;
            thisFramesAnimationState = "Run";
        }
		else
		{
			velocity.X = directionX * moveVelocity;
		}
		if (Input.IsActionJustPressed("Dash") && dashAvailable  && !floorChecker.IsColliding() && directionX != 0)
		{
			dash = true;
			timeDash = Time.GetTicksMsec();
			dashAvailable = false;
		}
		if (dash==true)
		{
			if (Time.GetTicksMsec() < timeDash + 200)
			{
				velocity.X = directionX * dashVelocity;
				dashSound.Play();
			}
			else
			{
				dash = false;
			}
		}
		state.LinearVelocity = velocity;


        if (thisFramesAnimationState != animationState || thisFramesAnimationFlipped != animationFlipped)
        {
            spriteAnimator.Play(thisFramesAnimationState);
            animationState = thisFramesAnimationState;
            spriteAnimator.FlipH = thisFramesAnimationFlipped;
            animationFlipped = thisFramesAnimationFlipped;
        }

    }

	public void BouncePlayer(float bounceEfficiency) {
		// Set bounce me true, will be handled next update
		bounceMe = true;
		// Set the bounce efficiency
		this.bounceEfficiency = bounceEfficiency;
	}

	public void ChangeAnimationState(string animationName, bool flipped) {
		if (animationName != animationState || animationFlipped != flipped) { 
			spriteAnimator.Play(animationName);
			animationState = animationName;
			spriteAnimator.FlipH = flipped;
			animationFlipped = flipped;
		}
	}
}
