using Godot;
using System;
using System.Globalization;

public partial class FinalRoomMusicTrigger : Area2D
{
	[Export] AudioStreamPlayer MainMusic;
	[Export] AudioStreamPlayer FinalMusic;
    Tween tween = new Tween();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		AreaEntered += (Area2D body) => Collided(body);
	}

	private void Collided(Area2D body) {
		if (body is WaterController) {
            // Add switch here
		
            FinalMusic.Playing = true;
            MainMusic.Playing = false;
			
		}
	}
}
