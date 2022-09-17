using Godot;
using System;

/// Player controller.
/// Main responsibilities: moving camera with player
public class Player : Spatial
{
	/// The player collision node
	private PlayerCollision collision;

	/// The spring arm which the camera is mounted onto
	private SpringArm camera;

	/// The position the camera was positioned via the 3D editor when the scene started, used to position the camera in a good position above the player.
	private Vector3 initialCameraOffset;
	
	private Ball ball;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		this.collision = GetNode<PlayerCollision>("Collision");
        this.camera = GetNode<SpringArm>("SpringArm");
		
		this.initialCameraOffset = this.camera.Transform.origin;

		this.ball = GetNode<Ball>("Collision/ball");
	}

	 // Called every frame. 'delta' is the elapsed time since the previous frame.
	 public override void _Process(float delta)
	 {
		// Move camera with player
		var cameraTransform = this.camera.Transform;
		cameraTransform.origin = this.collision.Transform.origin + this.initialCameraOffset;

		// Angle camera based on ground
		if (this.collision.floorCollisionNormal != null)
		{
			var rotDiff = cameraTransform.basis.GetEuler().x - (Mathf.Abs(this.collision.floorCollisionNormal.Value.z) * -1);
			var eased = (1 - Mathf.Pow(1 - rotDiff, 5)); // parabola easing
			cameraTransform = cameraTransform.Rotated(new Vector3(1, 0, 0), eased * delta);
		}
		
		this.camera.Transform = cameraTransform;
    }
}
