using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;

public partial class enemyBasic : CharacterBody2D
{
    [Export]
    public bool moveLeft = false;
    [Export]
    public const float moveVelocity = 400.0f;
    [Export]
    public bool edgeDetect = true;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;
		else
		{
			if (edgeDetect)
			{
                // Check the "foot" approaching the edge of the platform, flip if there's no platform
                if (moveLeft)
				{
					RayCast2D lfoot = GetNode<RayCast2D>("leftFoot");
					if (!lfoot.IsColliding()) moveLeft = !moveLeft;
				}
				else
				{
					RayCast2D rfoot = GetNode<RayCast2D>("rightFoot");
					if (!rfoot.IsColliding()) moveLeft = !moveLeft;
				}
			}
		}
		// Move enemy
		velocity.X = (moveLeft ? -1 : 1) * moveVelocity;
        Velocity = velocity;
		MoveAndSlide();
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			GetSlideCollision(i);
			Debug.Print(GetSlideCollisionCount().ToString());
		}
	}
}
