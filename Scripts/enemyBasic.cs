using Godot;
using Microsoft.VisualBasic.FileIO;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;

public partial class enemyBasic : RigidBody2D
{
    [Export]
    public bool moveLeft = false;
    [Export]
    public const float moveVelocity = 100.0f;
    [Export]
    public bool edgeDetect = true;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        BodyEntered += (Node body) => Collision(body);
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		Vector2 velocity = state.LinearVelocity;
		state.AngularVelocity = 0;
        // Add the gravity.
        //if (!IsOnFloor())
        //	velocity.Y += gravity * (float)delta;
        //else

        RayCast2D lfoot = GetNode<RayCast2D>("leftFoot");
        RayCast2D rfoot = GetNode<RayCast2D>("rightFoot");
        //{
        if (edgeDetect)
		{
            // Check the "foot" approaching the edge of the platform, flip if there's no platform
            
            if (moveLeft)
			{
				
				if (!lfoot.IsColliding() && rfoot.IsColliding()) moveLeft = !moveLeft;
			}
			else
			{
				
				if (!rfoot.IsColliding() && lfoot.IsColliding()) moveLeft = !moveLeft;
			}
				
		}
		if(rfoot.IsColliding() || lfoot.IsColliding()) velocity.X = (moveLeft ? -1 : 1) * moveVelocity;
        //}
        // Move enemy

        state.LinearVelocity = velocity;
		//MoveAndSlide();
		//for (int i = 0; i < GetSlideCollisionCount(); i++)
		//{
		//	GetSlideCollision(i);
		//	Debug.Print(GetSlideCollisionCount().ToString());
		//}

	}

    private void Collision(Node body) {
        Debug.Print("collision with " + body.Name.ToString());
        player possiblePlayer = body.GetParent().GetNodeOrNull<player>("./" + body.Name);
        if (possiblePlayer != null)
        {
            if (possiblePlayer.GlobalPosition.Y > GlobalPosition.Y && possiblePlayer.Velocity.Y < LinearVelocity.Y)
            {
                Debug.Print("enemy should die");
            }
        }
    }
}
