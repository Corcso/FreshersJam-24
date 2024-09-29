using Godot;
using System;

public partial class GameManager : Node
{
	public enum GameState { START_MENU, PLAYING, PAUSED, DEAD, WON};
	public GameState currentGameState = GameState.PLAYING;

	PackedScene gameScene;

	Node2D activeGameScene;
	Node2D activeMenuScene;

	public Control endScreen;

	// Set by the pause menu itself when it awakes
	public Control activePauseMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameScene = ResourceLoader.Load<PackedScene>("res://Scene Nodes/Game Scenes/GameScene.tscn");
		//activeMenuScene = GetTree().Root.GetNode<Node2D>("./MainMenu");

		ProcessMode = ProcessModeEnum.Always;

		endScreen.Hide();
	}

	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventKey keyEvent && keyEvent.Pressed)
		{

			if (keyEvent.Keycode == Key.Escape && currentGameState == GameState.PLAYING)
			{
				PauseGame();
			}
			else if (keyEvent.Keycode == Key.Escape && currentGameState == GameState.PAUSED)
			{
				UnpauseGame();
			}

		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void LoadGameScene() {
		activeGameScene = gameScene.Instantiate<Node2D>();
		GetTree().Root.CallDeferred(Node.MethodName.AddChild, activeGameScene);
		activeMenuScene.Hide();
	}

	public void LoadMenuScene()
	{
		activeGameScene.QueueFree();
		activeMenuScene.Show();
	}

	public void PauseGame()
	{
		currentGameState = GameState.PAUSED;
		GetTree().Paused = true;
		activePauseMenu.Show();
	}

	public void UnpauseGame()
	{
		currentGameState = GameState.PLAYING;
		GetTree().Paused = false;
		activePauseMenu.Hide();
	}

	public void WonGame()
	{
		currentGameState = GameState.WON;
		GetTree().Paused = true;
		endScreen.Show();

	}

}
