using Godot;

public class PlayerCollision : RigidBody
{
	public override void _IntegrateForces(PhysicsDirectBodyState state)
	{
		this.Sleeping = false;
	   
		var forwardStrength = Input.GetActionStrength("player_forward") - Input.GetActionStrength("player_backward");
		var leftStrength = Input.GetActionStrength("player_left") - Input.GetActionStrength("player_right");
	   
		state.AddCentralForce(new Vector3(leftStrength, 0, forwardStrength));
	}
}
