using Godot;

public class PlayerCollision : RigidBody
{
	public Vector3? floorCollisionNormal = null;

	public override void _IntegrateForces(PhysicsDirectBodyState state)
	{
		this.Sleeping = false;
	   
	   // Move player
		var forwardStrength = Input.GetActionStrength("player_forward") - Input.GetActionStrength("player_backward");
		var leftStrength = Input.GetActionStrength("player_left") - Input.GetActionStrength("player_right");
	   
		state.AddCentralForce(new Vector3(leftStrength, 0, forwardStrength));

		// Detect collision with floor
		if (state.GetContactCount() > 0) {
			this.floorCollisionNormal = state.GetContactLocalNormal(0);
		}
	}
}
