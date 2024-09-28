using Godot;
using System;
using System.Diagnostics;
using System.Drawing;

public partial class enemyBasic : CharacterBody2D
{
    [Export]
    public const float moveVelocity = 40.0f;
    [Export]
    public bool moveLeft = true;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		//Vector2 center = Position;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;
		else
		{
			// Prepare to raycast
			var spaceState = GetWorld2D().DirectSpaceState;
			var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, new Vector2(GlobalPosition.X + (moveLeft ? 1 : -1), GlobalPosition.Y - 1));
			query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };
			var result = spaceState.IntersectRay(query);

			// Flip moving direction if raycast fails
			if (result.Values.Count == 0)
			{
				moveLeft = !moveLeft;
				Debug.Print("flipping");
			}
		}
        velocity.X = (moveLeft ? 1 : -1) * moveVelocity;

		Velocity = velocity;
		MoveAndSlide();
	}
}
