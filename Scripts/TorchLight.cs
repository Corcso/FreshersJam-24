using Godot;
using System;
using System.Diagnostics;

public partial class TorchLight : Node2D
{
	double desiredScale = 1.0f;
	double originalScale = 1.0f;
	double currentScale = 1.0f;
	double lerpTime = 0.0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//var water = GetParent<Node2D>().GetNode<Node2D>("Water");
		var water = GetNode<Node2D>("../../Water");
		if (water != null)
		{
            PointLight2D torchLight = GetNode<PointLight2D>("TorchPointLight");
            AnimatedSprite2D torchAnim = GetNode<AnimatedSprite2D>("TorchAnimate");
            if (water.GlobalPosition.Y < GlobalPosition.Y)
			{
				torchAnim.Play("extinguish");
				torchLight.Enabled = false;
			}
			else
			{
                torchAnim.Play("default");
                torchLight.Enabled = true;
				if (Math.Abs(currentScale - desiredScale) <= 0.1)
				{
					lerpTime = 0.0f;
                    var randomNum = new RandomNumberGenerator();
                    originalScale = currentScale;
                    desiredScale = 1.0f + (0.25f * Math.Sin(randomNum.RandfRange(0, 6.283f)));
					//Debug.Print("scale reached, new scale = " + desiredScale.ToString());
					//Debug.Print(currentScale.ToString());
                }
				//lerp
				currentScale = originalScale + (desiredScale - originalScale) * lerpTime;
				lerpTime += delta * 1.5;
				torchLight.TextureScale = (float)currentScale;
            }
		}
	}
}
