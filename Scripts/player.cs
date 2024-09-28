using Godot;
using System;

public partial class player : CharacterBody2D
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
	public float dashCooldown = 2000.0f;

	[Export] AudioStreamPlayer jumpSound;
	[Export] AudioStreamPlayer dashSound;
	[Export] AudioStreamPlayer doubleJumpSound;


	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		velocity.Y += gravity * (float)delta;


		float directionX = Input.GetAxis("Left", "Right");
		//direction.X = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		if (IsOnFloor()&&jumpDouble==true)
		{
			jumpCount = 1;
		}
		
		
		if (Input.IsActionJustPressed("Jump")&&IsOnFloor())
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
		//x axis movement
		
	   
		if(Input.IsActionPressed("Sprint"))
		{
			velocity.X = directionX * moveVelocity*2;
		}
		else
		{
			velocity.X = directionX * moveVelocity;
		}
		if (Input.IsActionJustPressed("Dash") && timeDash + dashCooldown < Time.GetTicksMsec() && !IsOnFloor() && directionX != 0)
		{
			dash = true;
			timeDash = Time.GetTicksMsec();
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
		Velocity = velocity;
		MoveAndSlide();


		}

}
