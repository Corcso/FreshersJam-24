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

    RayCast2D deathArea;


    AnimatedSprite2D animatedSprite;
    bool dead;
    float timeDead;

    public override void _Ready()
    {
        BodyEntered += (Node body) => Collision(body);

        animatedSprite = GetNode<AnimatedSprite2D>("./AnimatedSprite2D");

        deathArea = GetNode<RayCast2D>("deathArea");
        deathArea.ExcludeParent = true;
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
        if (dead) { state.LinearVelocity = Vector2.Zero; }
		//MoveAndSlide();
		//for (int i = 0; i < GetSlideCollisionCount(); i++)
		//{
		//	GetSlideCollision(i);
		//	Debug.Print(GetSlideCollisionCount().ToString());
		//}

	}

    private void Collision(Node body) {
        Debug.Print("collision with " + body.Name.ToString());
        player possiblePlayer = body as player;
        if (possiblePlayer != null && !dead)
        {
            Debug.Print("yep thats the player");
            Debug.Print(possiblePlayer.GlobalPosition.Y.ToString() + ", " + GlobalPosition.Y.ToString());
            Debug.Print(possiblePlayer.LinearVelocity.Y.ToString() + ", " + LinearVelocity.Y.ToString());
            //check if 
            if (deathArea.IsColliding() && deathArea.GetCollider() is player/*possiblePlayer.GlobalPosition.Y < GlobalPosition.Y && (possiblePlayer.LinearVelocity.Y > LinearVelocity.Y || (possiblePlayer.LinearVelocity.Y >= LinearVelocity.Y && LinearVelocity.Y != 0))*/)
            {
                // Kill player
                dead = true;
                animatedSprite.Animation = "Dead";
                CollisionLayer = 0;
            }
            else
            {
                // Bounce back player
                float launchDirectionX = (possiblePlayer.GlobalPosition.X - GlobalPosition.X) < 0 ? -1 : 1;
                GD.Print(launchDirectionX.ToString());
                possiblePlayer.ApplyCentralImpulse(new Vector2(launchDirectionX, -1f) * 400);
            }
        }
    }

    public override void _Process(double dt) {
        if (dead) {
            timeDead += (float)dt;

            if (timeDead > 1.0f) {
                QueueFree();
            }
        }
    }
}
