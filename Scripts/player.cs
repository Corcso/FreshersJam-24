using Godot;
using System;
using System.Diagnostics;

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
	Area2D water;
	//Control deathScreen;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	// Animator
	AnimatedSprite2D spriteAnimator;
	string animationState = "Idle";
	bool animationFlipped = false;
	int framesFalling = 0; //hacky workaround

	// list of all animation states + a variable to keep track of which ones are applicable, to be sorted through later
    [Flags]
    enum AnimationStates
	{
		ANIM_IDLE = 1,
		ANIM_WALK = 2,
		ANIM_RUN = 4,
		ANIM_JUMP = 8,
		ANIM_FALL = 16,
		ANIM_DASH = 32,
		ANIM_JUMP_DOUBLE = 64
	}
	AnimationStates validStates = new AnimationStates();

    public override void _Ready()
    {
        gameManager = GetTree().Root.GetNode<GameManager>("./GameManager");
        floorChecker = GetNode<ShapeCast2D>("./FloorCaster");
		floorChecker.ExcludeParent = true;

		water = GetNode<Area2D>("../Water");
		//deathScreen = GetNode<Control>("../Death Screen");

		spriteAnimator = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		spriteAnimator.Play("Idle");
		//this.Position = 
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		string thisFramesAnimationState = "Idle";
		bool thisFramesAnimationFlipped = animationFlipped;
		int thisFrameAnimationFrameLock = -1;


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
            if (velocity.Y <= 0)
            {

                velocity.Y = 0;
				
            }
			state.LinearVelocity = velocity;
            return;

		}

		// Default animation to idle
		validStates = AnimationStates.ANIM_IDLE;

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
			if (floorChecker.IsColliding()) validStates |= AnimationStates.ANIM_WALK;
            thisFramesAnimationFlipped = directionX < 0;
		}

		// While not on ground and falling use frame 3 of jump 
		if (!floorChecker.IsColliding() && velocity.Y > 0) {
			framesFalling++;
            if (framesFalling > 2) validStates |= AnimationStates.ANIM_FALL;
            //thisFrameAnimationFrameLock = 3;

        }
		else { framesFalling = 0; }

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
            validStates |= AnimationStates.ANIM_JUMP;

        }
        else if (Input.IsActionJustPressed("Jump")&&jumpCount>0)
		{
			velocity.Y = jumpVelocity;
			jumpCount--;
			doubleJumpSound.Play();
            validStates |= AnimationStates.ANIM_JUMP_DOUBLE;
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
			velocity.X = directionX * moveVelocity* 2;
            validStates |= AnimationStates.ANIM_RUN;
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
        }
		if (dash==true)
		{
			if (Time.GetTicksMsec() < timeDash + 200)
			{
				velocity.X = directionX * dashVelocity;
				dashSound.Play();
				thisFrameAnimationFrameLock = 1;
                validStates |= AnimationStates.ANIM_DASH;
            }
			else
			{
				dash = false;
			}
		}
		state.LinearVelocity = velocity;

		thisFramesAnimationState = EvaluateAnimation(validStates, thisFramesAnimationState);

        if (thisFramesAnimationState != animationState || thisFramesAnimationFlipped != animationFlipped)
        {
			Debug.Print("swapping state to " + thisFramesAnimationState);
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

	//just the fucking worst
	private string EvaluateAnimation(AnimationStates validStates, string thisFramesAnimationState)
	{
		//let some animations play out, reset jump anim only for double jump
		if (animationState == "Jump" && spriteAnimator.Frame < 4)
		{
			if (LinearVelocity.Y > 0 && !validStates.HasFlag(AnimationStates.ANIM_JUMP_DOUBLE)) return "Fall";
			if (validStates.HasFlag(AnimationStates.ANIM_JUMP_DOUBLE)) { spriteAnimator.Frame = 0; }
			return "Jump";
		}
		if (animationState == "Dash" && spriteAnimator.Frame < 2)
		{
			return "Dash";
		}

		//decide on animation to play
		if (validStates.HasFlag(AnimationStates.ANIM_DASH)) return "Dash";
		else
		{
			if (validStates.HasFlag(AnimationStates.ANIM_FALL)) { thisFramesAnimationState = "Fall"; }
			else if (validStates.HasFlag(AnimationStates.ANIM_JUMP) || validStates.HasFlag(AnimationStates.ANIM_JUMP_DOUBLE)) { thisFramesAnimationState = "Jump"; }
			else if (validStates.HasFlag(AnimationStates.ANIM_WALK))
			{
				if (validStates.HasFlag(AnimationStates.ANIM_RUN)) { thisFramesAnimationState = "Run"; }
				else { thisFramesAnimationState = "Walk"; }
			}
			else if (floorChecker.IsColliding()) thisFramesAnimationState = "Idle";
			else thisFramesAnimationState = "Fall";
		}
		return thisFramesAnimationState;
	}


	public override void _Process(double delta)
    {
		if (gameManager.currentGameState == GameManager.GameState.DEAD)
		{



			Vector2 pos = Position;

			Vector2 vel = LinearVelocity;

			GravityScale -= 22.5f * (float)delta;
			//GD.Print(GravityScale);

			//vel.Y -= gravity * (float)delta;



			if (Position.X > water.Position.Y)
			{

				pos.Y = water.Position.Y;

			}

			Position = pos;

			LinearVelocity = vel;

		}
    }
}
