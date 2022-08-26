using Godot;

public class PlayerCollision : RigidBody
{
	// The collision normal of the player on the ground. Or null if the player is not touching the ground.
    public Vector3? floorCollisionNormal = null;
    
	// How much user inputs should be multiplied before being added as forces.
    const float MOVEMENT_MULT = 300;

    public override void _PhysicsProcess(float delta)
    {
        this.Sleeping = false;

        // Move player
        var forwardStrength = Input.GetActionStrength("player_forward") - Input.GetActionStrength("player_backward");
        var leftStrength = Input.GetActionStrength("player_left") - Input.GetActionStrength("player_right");

        forwardStrength *= delta;
        leftStrength *= delta;

		forwardStrength *= MOVEMENT_MULT;
		leftStrength *= MOVEMENT_MULT;

        this.AddCentralForce(new Vector3(leftStrength, 0, forwardStrength));
    }

    public override void _IntegrateForces(PhysicsDirectBodyState state)
    {
        // Detect collision with floor
        if (state.GetContactCount() > 0)
        {
            this.floorCollisionNormal = state.GetContactLocalNormal(0);
        } else {
			this.floorCollisionNormal = null;
		}
    }
}
