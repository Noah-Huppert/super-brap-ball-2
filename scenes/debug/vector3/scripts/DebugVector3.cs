using Godot;
using System;

public class DebugVector3 : Spatial
{
	// The vector which will be represented by the render.
    [Export]
    public Vector3 vector = Vector3.Zero;

    // The parent of the meshes used to draw the vector.
    private Spatial renderArrow;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.renderArrow = GetNode<Spatial>("Arrow");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var normalizedVector = this.vector.Normalized() + new Vector3(0.1f, 0.1f, 0.1f);

        //this.renderArrow.GlobalRotation = this.renderArrow.GlobalRotation.DirectionTo(normalizedVector);
        this.renderArrow.Scale = normalizedVector;

    }
}
