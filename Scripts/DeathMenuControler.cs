using Godot;
using System;

public partial class DeathMenuControler : Control
{

	GameManager gameManager;

	BaseButton retryButton;
	BaseButton menuButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameManager = GetTree().Root.GetNode<GameManager>("./GameManager");
		gameManager.activeDeathMenu = this;

		retryButton = GetNode<BaseButton>("./Retry Button");
		menuButton = GetNode<BaseButton>("./Menu Button");

		retryButton.Pressed += () => RetryButtonPressed();
		menuButton.Pressed += () => MenuButtonPressed();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void RetryButtonPressed() {
		gameManager.LoadMenuScene();
		gameManager.LoadGameScene();
		gameManager.currentGameState = GameManager.GameState.PLAYING;
	}
	public void MenuButtonPressed() {
        gameManager.LoadMenuScene();
        gameManager.currentGameState = GameManager.GameState.START_MENU;
    }
}
