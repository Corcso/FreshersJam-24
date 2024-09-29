using Godot;
using System;

public partial class player : RigidBody2D
{

	[Export]
	public float moveVelocity = 100.0f;
	public float dashVelocity = 1000.0f;
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
	Area2D water;
	//Control deathScreen;

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

		water = GetNode<Area2D>("../Water2");
		//deathScreen = GetNode<Control>("../Death Screen");

		spriteAnimator = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		spriteAnimator.Play("Idle");
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		string thisFramesAnimationState = "Idle";
		bool thisFramesAnimationFlipped = animationFlipped;
		int thisFrameAnimationFrameLock = -1;


		Vector2 velocity = state.LinearVelocity;
		//velocity.Y += gravity * (float)delta;

		if (gameManager.currentGameState == GameManager.GameState.DEAD)
		{
            if (velocity.Y <= 0)
            {

                velocity.Y = 0;
				
            }
			state.LinearVelocity = velocity;
            return;

		}

        // Default animation to idle
       //spriteAnimator.Animation = "Idle";


        float directionX = Input.GetAxis("Left", "Right");
		// First set us walking if we are this is the lowest priority animation
		if (directionX != 0) {
			thisFramesAnimationState = "Walk";
			thisFramesAnimationFlipped = directionX < 0;
		}

		// While not on ground and falling use frame 3 of jump 
		if (!floorChecker.IsColliding() && velocity.Y > 0) {
            //thisFramesAnimationState = "Jump";
            //thisFrameAnimationFrameLock = 3;

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
			//thisFramesAnimationState = "Jump";
			
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
		else if (directionX != 0)
		{
			velocity.X = directionX * moveVelocity;
		}
		if (Input.IsActionJustPressed("Dash") && dashAvailable  && !floorChecker.IsColliding() && directionX != 0)
		{
			dash = true;
			timeDash = Time.GetTicksMsec();
			dashAvailable = false;
            thisFramesAnimationState = "Dash";
        }
		if (dash==true)
		{
			if (Time.GetTicksMsec() < timeDash + 200)
			{
				velocity.X = directionX * dashVelocity;
				dashSound.Play();
				thisFrameAnimationFrameLock = 1;
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
		if (thisFrameAnimationFrameLock != -1) {
			spriteAnimator.Frame = 3;
			spriteAnimator.Pause();	
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

    public override void _Process(double delta)
    {
		if (gameManager.currentGameState == GameManager.GameState.DEAD)
		{

			

			//Vector2 pos = Position;

			GravityScale -= 22.5f * (float)delta;
			GD.Print(GravityScale);

			//vel.Y -= gravity * (float)delta;

			
			
   //         if (Position.X <= water.Position.Y)
   //         {

   //             pos.Y = water.Position.Y;

   //         }

			//Position = pos;

		}
    }
}
