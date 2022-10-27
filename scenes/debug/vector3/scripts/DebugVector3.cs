using Godot;
using System;

public partial class DebugVector3 : Node3D
{
    // The label shown next to the vector.
    [Export]
    public String label = "";

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
    public Color bodyColor = new Color(1, 0, 0, 1);

    // Color in which vector end "arrow" will be drawn.
	[Export]
    public Color arrowColor = new Color(0, 1, 0, 1);

    // If true then the parent's rotation will be ignored.
	[Export]
    public bool ignoreParentRotation = true;

    // Used to help ignore parent rotation if ignoreParentRoation is enabled.
    private Vector3 initialGlobalRotation;

    // The label node in the world.
    private Label3D labelNode;

    // If true the vector's value will be shown in the label.
    [Export]
    public bool includeValueInLabel = true;

    // If greater than 0 the vector will be rounded to this number of decimal places when shown in the label.
    [Export]
    public int roundLabelValue = 2;

    // This number will be used to scale the vector so it can appear bigger or smaller on the screen. The vector value shown in the label will not be scaled by this. To indicate the vector is being scaled an astrix will be appended.
    [Export]
    public float vectorScale = 1;

    /// Used to draw triangles on the screen.
    private ImmediateMesh mesh;

    // Create a DebugVector3 scene and add it as a child of the parent node.
    static public DebugVector3 AddNode(Node parent, Vector3 initVec, String label, bool ignoreParentRotation = true, bool includeValueInLabel = true)
    {
        var scene = ResourceLoader.Load<PackedScene>("res://scenes/debug/vector3/DebugVector3.tscn");
		
        var debugVector = scene.Instantiate<DebugVector3>();
        debugVector.vector = initVec;
        debugVector.label = label;
        debugVector.ignoreParentRotation = ignoreParentRotation;
        debugVector.includeValueInLabel = includeValueInLabel;

        parent.AddChild(debugVector);

        return debugVector;
    }

    public override void _Ready()
    {
        this.initialGlobalRotation = this.GlobalRotation;
        this.labelNode = GetNode<Label3D>("Label3D");
        this.mesh = new ImmediateMesh();
    }

    public override void _Process(double delta)
    {
        // Maybe ignore rotation
        if (this.ignoreParentRotation)
        {
            this.GlobalRotation = this.initialGlobalRotation;
        }

        // Draw vector
        var scaledVector = this.vector * this.vectorScale;
		
        this.mesh.ClearSurfaces();
        this.mesh.SurfaceBegin(Mesh.PrimitiveType.Lines);

        // Draw body
        this.mesh.SurfaceSetColor(this.bodyColor);

        this.mesh.SurfaceAddVertex(new Vector3(0, 0, 0));

        var body = scaledVector * 0.9f;
        this.mesh.SurfaceAddVertex(body);

        // Draw arrow.
        this.mesh.SurfaceSetColor(this.arrowColor);

        var arrow = scaledVector * 0.1f;
        this.mesh.SurfaceAddVertex(body);
        this.mesh.SurfaceAddVertex(body + arrow);

        this.mesh.SurfaceEnd();

        // Draw label
        var labelText = this.label;
		var roundedVector = this.vector;

        if (this.roundLabelValue >= 0)
        {
            roundedVector = new Vector3(
				(float)Math.Round(this.vector.x, this.roundLabelValue),
				(float)Math.Round(this.vector.y, this.roundLabelValue),
				(float)Math.Round(this.vector.z, this.roundLabelValue)
			);
        }

        if (this.includeValueInLabel)
        {
            labelText += " " + roundedVector;
        }

        if (this.vectorScale != 1f)
        {
            labelText += "*";
        }		

		this.labelNode.Text = labelText;

		// Deal with label being off screen
        var cam = GetViewport().GetCamera3d();

        var labelPosition = scaledVector;
     /*    var labelUnprojected = cam.UnprojectPosition(labelPosition + this.GlobalTransform.origin);
        var viewportSize = cam.GetViewport().Size;

        if (labelUnprojected.x < 0 || labelUnprojected.x > viewportSize.x || labelUnprojected.y < 0 || labelUnprojected.y > viewportSize.y)
        {
			// If label would end up outside of camera viewport then reposition it to the edge of the viewport
            var onScreenUnprojected = this.limitVector2(labelUnprojected, Vector2.Zero, viewportSize);
            var zPosition = cam.GlobalTransform.origin.DistanceTo(this.GlobalTransform.origin);

            labelPosition = cam.ProjectPosition(onScreenUnprojected, zPosition) - this.GlobalTransform.origin;
        }
 */
		// Set label position (Label will appear backwards unless flipped on z axis)
        this.labelNode.Transform = new Transform3D(new Quaternion(new Vector3(0, 1, 0), Mathf.Pi), labelPosition);
    }

    private Vector2 limitVector2(Vector2 value, Vector2 min, Vector2 max)
    {
        var x = value.x;
        var y = value.y;

        if (x < min.x)
        {
            x = min.x;
        } else if (x > max.x)
        {
            x = max.x;
        }

        if (y < min.y)
        {
            y = min.y;
        } else if (y > max.y)
        {
            y = max.y;
        }

        return new Vector2(x, y);
    }
}
