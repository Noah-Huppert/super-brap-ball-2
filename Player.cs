using Godot;
using System;

public class Player : Spatial
{
	private RigidBody collision;
	private Camera camera;
	private Vector3 initialCameraOffset;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.collision = GetNode<RigidBody>("Collision");
		this.camera = GetNode<Camera>("Camera");

		this.initialCameraOffset = this.camera.Transform.origin;
	}

	 // Called every frame. 'delta' is the elapsed time since the previous frame.
	 public override void _Process(float delta)
	 {
		var cameraTransform = this.camera.Transform;
		cameraTransform.origin = this.collision.Transform.origin + this.initialCameraOffset;
		this.camera.Transform = cameraTransform;
	}
}
