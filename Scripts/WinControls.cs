using Godot;
using System;

public partial class WinControls : Control
{
    GameManager gameManager;
    BaseButton againButton;
    BaseButton menuButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        gameManager = GetTree().Root.GetNode<GameManager>("./GameManager");
        againButton = GetNode<BaseButton>("./Again Button");
        menuButton = GetNode<BaseButton>("./Menu Button");

        againButton.Pressed += () => PlayAgain();
        menuButton.Pressed += () => ReturnToMenu();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void PlayAgain()
    {
        gameManager.LoadMenuScene();
        gameManager.LoadGameScene();
    }

    private void ReturnToMenu()
    {
        gameManager.LoadMenuScene();
    }
}
