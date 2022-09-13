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
    // 
    // The angle is a bit more complex. Think of 3 angles:
    //
    // - a: between axis x and y
    // - b: between axis y and z
    // - c: between axis x and z
    //
    // Each of these angles must be applied to the axis that is perpendicular to the axes it is between:
    //
    // - a: apply to z axis
    // - b: apply to x axis
    // - c: apply to y axis
    //
    // Inverse cosine is used to find these angles because the hypotenuse can be used a denominator, and it will never be zero if the vector should be shown. Thus ensuring no divide by zero scenarios occur. The angles are calculated as such:
    //
    // - a: cos^-1(y/h)
    // - b: cos^-1(z/h)
    // - c: cos^-1(x/h)
	public override void _Process(float delta)
	{
        var xAngle = this.SafeAcos(this.vector.z, this.vector.Length());
        var yAngle = this.SafeAcos(this.vector.x, this.vector.Length());
        var zAngle = this.SafeAcos(this.vector.y, this.vector.Length());
        
        var xQuat = new Quat(new Vector3(1, 0, 0), xAngle);
        var yQuat = new Quat(new Vector3(0, 1, 0), yAngle);
        var zQuat = new Quat(new Vector3(0, 0, 1), zAngle);
        
        var quat = xQuat * yQuat * zQuat;

       GD.Print("vector=" + this.vector + ", angle=(" + xAngle + ", " + yAngle + ", " + zAngle + "), (y/z, z/x, y/x)");

        var transform = new Transform(quat, Vector3.Zero);
        this.renderArrow.Transform = transform;

        // Set magnitude
        this.renderArrow.Scale = new Vector3(0.05f, this.vector.Length(), 0.05f);
    }

    // Performs an inverse cosine operation = cos^-1(a/b). However if b is 0 simply returns 0, in order to avoid a divide by zero.
    private float SafeAcos(float a, float b) {
        if (b == 0) {
            return 0;
        }

        return Mathf.Acos(a / b);
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
