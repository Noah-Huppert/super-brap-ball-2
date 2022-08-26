using Godot;
using System;

public class Ball : Spatial
{
    private AnimationPlayer animationPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void Open() {
        this.animationPlayer.Play("Open");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
