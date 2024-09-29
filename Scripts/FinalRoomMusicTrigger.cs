using Godot;
using System;
using System.Globalization;

public partial class FinalRoomMusicTrigger : Area2D
{
	[Export] AudioStreamPlayer MainMusic;
	[Export] AudioStreamPlayer FinalMusic;

	float mainMusicStartDB;
	float finalMusicEndDB;

    [Export] float crossfadeTime = 2;
	[Export] float crossfadeFactor = 1;

	float timeInCrossfade;

	bool beginCrossfade;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		AreaEntered += (Area2D body) => Collided(body);

		timeInCrossfade = 0.0f;

		mainMusicStartDB = MainMusic.VolumeDb;
        finalMusicEndDB = FinalMusic.VolumeDb;
	}

	public override void _Process(double dt) {
		if (beginCrossfade)
		{
			timeInCrossfade += (float)dt;
			MainMusic.VolumeDb = Mathf.Lerp(mainMusicStartDB, -80, timeInCrossfade / crossfadeTime / crossfadeFactor);
            FinalMusic.VolumeDb = Mathf.Lerp(-80, finalMusicEndDB, timeInCrossfade / crossfadeTime / crossfadeFactor);

			if (timeInCrossfade >= crossfadeTime)
			{
                MainMusic.Playing = false;
            }
		}
	}

	private void Collided(Area2D body) {
		if (body is WaterController) {
            // Add switch here
		
            FinalMusic.Playing = true;
			FinalMusic.VolumeDb = -80;
            beginCrossfade = true;
			
		}
	}
}
