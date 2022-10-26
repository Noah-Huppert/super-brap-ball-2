using Godot;
using System;

public partial class Ball : Node3D
{
    // Animations on ball.
    private AnimationPlayer animationPlayer;

    // If the ball is open.
    public bool isOpen;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void Open() {
        this.animationPlayer.Play("Open");
        this.isOpen = true;
    }
}
