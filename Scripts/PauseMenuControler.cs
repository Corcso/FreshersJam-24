using Godot;
using System;

public partial class PauseMenuControler : Control
{
	GameManager gameManager;
    BaseButton resumeButton;
    BaseButton menuButton;

	[Export] AudioStreamPlayer ButtonSound;
	[Export] AudioStreamPlayer menuSound;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameManager = GetTree().Root.GetNode<GameManager>("./GameManager");
		gameManager.activePauseMenu = this;

		resumeButton = GetNode<BaseButton>("./Resume Button");
		menuButton = GetNode<BaseButton>("./Menu Button");

		resumeButton.Pressed += () => ResumeGame();
		menuButton.Pressed += () => ReturnToMenu();

		VisibilityChanged += () => MyVisibilityChanged();

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void ResumeGame() { 
		ButtonSound.Play();
		gameManager.UnpauseGame();
	}

    private void ReturnToMenu()
    {
		ButtonSound.Play();
        gameManager.LoadMenuScene();
    }

	private void MyVisibilityChanged() {
		menuSound.Play();
		if (Visible) { }
		else { }
	}
}
