using Godot;

public class PlayerCollision : RigidBody
{
	// How much user inputs should be multiplied before being added as forces.
    const float MOVEMENT_MULT = 300;

    // Maximum lift coefficient. Characterization of wing efficiency, size, drag, ect.
	// See ../assets/lift-coefficient-suggestions.pdf
    const float MAX_LIFT_COEFFICIENT = 1.4f;

    // Wing area in meters squared.
    const float WING_AREA = 1f;

	// Density of air in kg/m^3.
	// https://en.m.wikipedia.org/wiki/Density_of_air
    const float AIR_DENSITY = 1.204f;

    // Amount angle of attack is changed while in the air and angle of attack is indicated to be changed.
    const float ANGLE_OF_ATTACK_CHANGE_AMOUNT = Mathf.Pi / 8f;

	// Number which takes into account factors which aggregate into how much drag an object has.
	// Number is for a Cessna 172: http://www.temporal.com.au/c172.pdf (pg 8)
    const float DRAG_COEFFICIENT = 0.0341f;

    // The collision normal of the player on the ground. Or null if the player is not touching the ground.
    public Vector3? floorCollisionNormal = null;

    // The ball 3D model.
    private Ball ball;

    public override void _Ready()
    {
        this.ball = GetNode<Ball>("ball");
    }

    public override void _Process(float delta)
    {
		// Open ball
		if (Input.IsActionPressed("open_ball") && this.floorCollisionNormal == null) {
			this.ball.Open();
            this.Rotation = Vector3.Zero;
            this.AngularVelocity = Vector3.Zero;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        this.Sleeping = false;

		var forwardStrength = Input.GetActionStrength("player_forward") - Input.GetActionStrength("player_backward");
		var leftStrength = Input.GetActionStrength("player_left") - Input.GetActionStrength("player_right");

        // Move player
        if (this.ball.isOpen)
        {
            // Air movement
            //this.Mode = ModeEnum.Kinematic;
            var liftCoefficient = ((this.Rotation.x * (180 / Mathf.Pi)) * 0.06f) + 0.4f;

			// TODO: Should liftMagnitude and dragMagnitude use all velocity components?

			// Note linear velocity represents "true air speed", change to account for wind when added to game
            var liftMagnitude = liftCoefficient * WING_AREA * 0.5f * AIR_DENSITY * Mathf.Pow(this.LinearVelocity.z, 2);
            // Lift is perpedicular to the flight path (https://www.grc.nasa.gov/www/k-12/airplane/glidvec.html)
			var liftDir = Vector3.Up.Rotated(new Vector3(1, 0, 0), this.Rotation.x);
            var lift = liftDir * liftMagnitude;

            var dragMagnitude = DRAG_COEFFICIENT * ((AIR_DENSITY * Mathf.Pow(this.LinearVelocity.z, 2)) / 2f) * WING_AREA;
            var dragDir = Vector3.Back.Rotated(new Vector3(1, 0, 0), this.Rotation.x);
            var drag = dragDir * dragMagnitude;

            GD.Print(lift + " - " + drag);

            this.AddCentralForce(lift - drag);

            if (forwardStrength < 0)
            {
                this.RotateX(-1 * ANGLE_OF_ATTACK_CHANGE_AMOUNT * delta);
            } else if (forwardStrength > 0)
            {
				this.RotateX(ANGLE_OF_ATTACK_CHANGE_AMOUNT * delta);
            }
        } else
        {
            // Ground movement
            //this.Mode = ModeEnum.Rigid;
			
            forwardStrength *= delta;
            leftStrength *= delta;

            forwardStrength *= MOVEMENT_MULT * this.Mass;
            leftStrength *= MOVEMENT_MULT * this.Mass;

            this.AddCentralForce(new Vector3(leftStrength, 0, forwardStrength));
        }
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
