using Godot;
using System;

public partial class StartingArea : Node3D
{
	private float targetRotT = 0;
	private Vector3 targetRot;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
    {
		var forwardStrength = Input.GetActionStrength("player_forward") - Input.GetActionStrength("player_backward");
		var leftStrength = Input.GetActionStrength("player_left") - Input.GetActionStrength("player_right");

        // Move player by tilting ground
		if (this.targetRot == this.Rotation) {
			this.targetRotT = 0;
		} else {
			this.targetRotT += (float)delta * 0.05f;
		}

		this.targetRot = new Vector3(
			forwardStrength / 3,
			0,
			leftStrength
		);

		this.Rotation = this.Rotation.Lerp(this.targetRot, this.targetRotT);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
