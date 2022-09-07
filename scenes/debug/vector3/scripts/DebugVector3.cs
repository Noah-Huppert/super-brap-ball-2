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
        var nVec = this.vector.Normalized();

		// Set angle
		var dirVec = Vector3.Zero.DirectionTo(nVec);
        var rightAngle = (Mathf.Pi / 2);
		
        this.renderArrow.GlobalRotation = new Vector3(
			dirVec.z != 0 ? rightAngle / dirVec.z : 0,
			0,
			dirVec.x != 0 ? rightAngle / dirVec.x : 0
		);

		// Set scale
        this.renderArrow.Scale = new Vector3(0.05f, nVec.Length(), 0.05f);
    }
}
