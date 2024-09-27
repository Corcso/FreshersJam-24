using Godot;
using System;

public partial class player : CharacterBody2D
{

	[Export]
	public float moveVelocity = 100.0f;
	public float jumpVelocity= -400.0f;
	int jumpCount=0;
	[Export]
	bool jumpDouble = true;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		velocity.Y += gravity * (float)delta;


		//direction.X = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		if(IsOnFloor()&&jumpDouble==true)
		{
			jumpCount = 1;
		}
		
		if (Input.IsActionJustPressed("Jump")&&IsOnFloor())
		{
			velocity.Y = jumpVelocity;
			
		}
		else if (Input.IsActionJustPressed("Jump")&&jumpCount>0)
		{
			velocity.Y = jumpVelocity;
			jumpCount--;
		}
		//x axis movement
		float directionX = Input.GetAxis("Left", "Right");
	   
		if(Input.IsActionPressed("Sprint"))
		{
			velocity.X = directionX * moveVelocity*2;
		}
		else
		{
			velocity.X = directionX * moveVelocity;
		}

		Velocity = velocity;
		MoveAndSlide();


		//Vector2 velocity = Velocity;

		//// Add the gravity.
		//if (!IsOnFloor())
		//	velocity.Y += gravity * (float)delta;

		//// Handle Jump.
		//if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		//	velocity.Y = JumpVelocity;

		//// Get the input direction and handle the movement/deceleration.
		//// As good practice, you should replace UI actions with custom gameplay actions.
		//Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		//if (direction != Vector2.Zero)
		//{
		//	velocity.X = direction.X * Speed;
		//}
		//else
		//{
		//	velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		//}

		//Velocity = velocity;
		//MoveAndSlide();
	}
}
