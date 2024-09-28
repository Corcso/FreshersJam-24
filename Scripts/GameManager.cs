using Godot;
using System;

public partial class GameManager : Node
{
	public enum GameState { START_MENU, PLAYING, PAUSED, DEAD, WON};
	public GameState currentGameState = GameState.START_MENU;

	PackedScene gameScene;

	Node2D activeGameScene;
	Node2D activeMenuScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameScene = ResourceLoader.Load<PackedScene>("res://Game Scenes/GameScene.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void LoadGameScene() {
		activeGameScene = gameScene.Instantiate<Node2D>();
		activeMenuScene.Hide();
	}

    public void LoadMenuScene()
    {
		activeGameScene.QueueFree();
        activeMenuScene.Show();
    }


}
