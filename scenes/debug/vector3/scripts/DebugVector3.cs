using Godot;
using System;

public class DebugVector3 : Spatial
{
	// The vector which will be represented by the render.
    [Export]
    public Vector3 vector = Vector3.Zero;

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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
        var nVec = this.vector;

        // var rotX = nVec.z;
        // var rotY = nVec.y;
        // var rotZ = nVec.x;
        // var rotW = Mathf.Sqrt(1 - Mathf.Pow(rotX, 2) - Mathf.Pow(rotY, 2) - Mathf.Pow(rotZ, 2))
		// var rot = new Quat(rotW, rotX, rotY, rotZ);

        var yRot = new Quat(new Vector3(1, 0, 0), nVec.y * (Mathf.Pi / 2));
        var rot = Quat.Identity;

        var transform = new Transform(rot, Vector3.Zero);
        // transform.Scaled(new Vector3(0.05f, nVec.Length(), 0.05f));
        this.renderArrow.Transform = transform;

        GD.Print("vectorY=" + this.vectorY + ", nVec=" + nVec + ", quat=" + rot);

        // // Set angle
        // var dirVec = Vector3.Zero.DirectionTo(nVec);
        // var rightAngle = (Mathf.Pi / 2);

        // this.renderArrow.GlobalRotation = new Vector3(
        // 	dirVec.z != 0 ? rightAngle / dirVec.z : 0,
        // 	0,
        // 	dirVec.x != 0 ? rightAngle / dirVec.x : 0
        // );

        // // Set scale
        var vecSign = 1;
        if (this.vector.y < 0)
        {
            vecSign = -1;
        }
        this.renderArrow.Scale = new Vector3(0.05f, nVec.Length() * vecSign, 0.05f);
    }
}
