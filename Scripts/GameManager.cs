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
		gameScene = ResourceLoader.Load<PackedScene>("res://Scene Nodes/Game Scenes/GameScene.tscn");
		activeMenuScene = GetTree().Root.GetNode<Node2D>("./MainMenu");

		LoadGameScene();
		LoadMenuScene();

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


}
