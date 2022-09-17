using Godot;
using System;

public class DebugVector3 : ImmediateGeometry
{
	// The vector which will be represented by the render.
    [Export]
    public Vector3 vector = Vector3.Zero;

    // Gets and sets the vector.x property.
	[Export]
	public float vectorX
    {
        get
        {
            return this.vector.x;
        }
        set
        {
            this.vector.x = value;
        }
    }

    // Gets and sets the vector.y property.
    [Export]
    public float vectorY
    {
        get
        {
            return this.vector.y;
        }
        set
        {
            this.vector.y = value;
        }
    }

    // Gets and sets the vector.z property.
	[Export]
	public float vectorZ
    {
        get
        {
            return this.vector.z;
        }
        set
        {
            this.vector.z = value;
        }
    }

    // Color in which vector body will be drawn.
	[Export]
    private Color bodyColor = new Color(1, 0, 0, 1);

    // Color in which vector end "arrow" will be drawn.
	[Export]
    private Color arrowColor = new Color(0, 1, 0, 1);

    // If true then the parent's rotation will be ignored.
	[Export]
    private bool ignoreParentRotation = false;

    // Used to help ignore parent rotation if ignoreParentRoation is enabled.
    private Vector3 initialGlobalRotation;

    public override void _Ready()
    {
        this.initialGlobalRotation = this.GlobalRotation;
    }

    public override void _Process(float delta)
    {
        // Maybe ignore rotation
        if (this.ignoreParentRotation)
        {
            this.GlobalRotation = this.initialGlobalRotation;
        }

        // Draw vector
        this.Clear();
        this.Begin(Mesh.PrimitiveType.Lines);

        // Draw body
        this.SetColor(this.bodyColor);

        this.AddVertex(new Vector3(0, 0, 0));

        var body = this.vector * 0.9f;
        this.AddVertex(body);

        // Draw arrow.
        this.SetColor(this.arrowColor);

        var arrow = this.vector * 0.1f;
        this.AddVertex(body);
        this.AddVertex(body + arrow);

        this.End();
    }
}
