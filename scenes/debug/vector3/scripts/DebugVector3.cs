using Godot;
using System;

public class DebugVector3 : Spatial
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

    // The parent of the meshes used to draw the vector.
    private Spatial renderArrow;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.renderArrow = GetNode<Spatial>("Arrow");
    }

	// Update 3D model to point in correct direction and have correct magnitude.
    // The magnitude is shown by setting the arrow's y scale. 
    // X angle (rotates for vector.?) = tan^-1(y/z)
    // Y angle (rotates for vector.z) = tan^-1(x/z)
    // Z angle (rotates for vector.y)= tan^-1(y/x)
	public override void _Process(float delta)
	{
        var xAngle = this.SafeAtan(this.vector.y, this.vector.z);// * Mathf.Pi * 2 * this.NormalizedSign(this.vector.z);
        var yAngle = this.SafeAtan(this.vector.x, this.vector.z) - Mathf.Pi * 2 * this.NormalizedSign(this.vector.z);
        var zAngle = this.SafeAtan(this.vector.y, this.vector.x) + (Mathf.Pi / 2) * this.NormalizedSign(this.vector.x);
        
        var xQuat = new Quat(new Vector3(1, 0, 0), xAngle);
        var yQuat = new Quat(new Vector3(0, 1, 0), yAngle);
        var zQuat = new Quat(new Vector3(0, 0, 1), zAngle);
        
        var quat = yQuat * zQuat;//xQuat * yQuat * zQuat;

       GD.Print("vector=" + this.vector + ", angle=(" + xAngle + ", " + yAngle + ", " + zAngle + ")");

        var transform = new Transform(quat, Vector3.Zero);
        this.renderArrow.Transform = transform;

        // Set magnitude
        this.renderArrow.Scale = new Vector3(0.05f, this.vector.Length(), 0.05f);
    }

    // Performs an inverse tangent operation = tan^-1(a/b). However if b is 0 simply returns 0, in order to avoid a divide by zero.
    private float SafeAtan(float a, float b) {
        if (b == 0) {
            return 0;
        }

        return Mathf.Atan(a / b);
    }

    // Get a 1 or -1 based on the sign of the number. Returns 1 if value is 0.
    private float NormalizedSign(float value)
    {
        if (value < 0) {
            return -1;
        }
            
        return 1;
    }
}
