using Godot;
using System;

public class Ball : Spatial
{
    // Animations on ball.
    private AnimationPlayer animationPlayer;

    // If the ball is open.
    public bool isOpen;

    // The rotation the ball started in.
    private Vector3 startingGlobalRotation;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        this.startingGlobalRotation = this.GlobalRotation;
    }

    public void Open() {
        this.animationPlayer.Play("Open");
        this.isOpen = true;
    }

    
    public override void _Process(float delta)
    {
        if (this.isOpen) {
            this.GlobalRotation = this.startingGlobalRotation;
        }
    }
}
