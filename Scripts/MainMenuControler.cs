using Godot;
using System;

public partial class MainMenuControler : Control
{
	GameManager gameManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameManager = GetTree().Root.GetNode<GameManager>("./GameManager");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void _on_begin_button_pressed() {
		gameManager.LoadGameScene();
		gameManager.currentGameState = GameManager.GameState.PLAYING;
	}

	public void _on_quit_button_pressed() {
		GetTree().Quit();
	}
}
