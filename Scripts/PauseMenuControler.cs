using Godot;
using System;

public partial class PauseMenuControler : Control
{
	GameManager gameManager;
    BaseButton resumeButton;
    BaseButton menuButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameManager = GetTree().Root.GetNode<GameManager>("./GameManager");
		gameManager.activePauseMenu = this;

		resumeButton = GetNode<BaseButton>("./Resume Button");
		menuButton = GetNode<BaseButton>("./Menu Button");

		resumeButton.Pressed += () => ResumeGame();
		menuButton.Pressed += () => ReturnToMenu();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void ResumeGame() { 
		gameManager.UnpauseGame();
	}

    private void ReturnToMenu()
    {
        gameManager.LoadMenuScene();
    }
}
