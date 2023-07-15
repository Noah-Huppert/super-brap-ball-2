using Godot;

public partial class PlayerCollision : CharacterBody3D
{
    // The collision normal of the player on the ground. Or null if the player is not touching the ground.
    public Vector3? floorCollisionNormal = null;

    // The ball 3D model.
    private Ball ball;

    //private KinematicCollision3D lastColl;

    private PrintEvery printer;

    public override void _Ready()
    {
        this.ball = GetNode<Ball>("ball");
        this.printer = new PrintEvery(10);
    }

    public override void _Process(double delta)
    {
		// Open ball
		if (Input.IsActionPressed("open_ball") && this.floorCollisionNormal == null) {
			this.ball.Open();
            this.Rotation = Vector3.Zero;
            //this.AngularVelocity = Vector3.Zero;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        //this.Sleeping = false;

		var forwardStrength = Input.GetActionStrength("player_forward") - Input.GetActionStrength("player_backward");
		var leftStrength = Input.GetActionStrength("player_left") - Input.GetActionStrength("player_right");

        // Movement equations
        var vPrev = Velocity;
        var accel = new Vector3();

        // ... Gravity
        accel += new Vector3(0, -9.8f, 0);

        // ... Contact based movement
        var lastColl = GetLastSlideCollision();
        
        if (lastColl != null && lastColl.GetCollisionCount() > 0) {
            // When contacting
            var collisionNormal = lastColl.GetNormal();

            // ... Slide on surfaces
            accel += new Vector3(collisionNormal.X, collisionNormal.Y, collisionNormal.Z) * 100;
        }

        // ... Scale acceleration for delta
        accel *= 10;

        // Update new velocity
        var vNext = vPrev + (accel * (float)delta);
        Velocity = vNext;

        MoveAndSlide();

        printer.Print("vNext=" + vNext);
    }

   /*  public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        // Detect collision with floor
        if (state.GetContactCount() > 0)
        {
            this.floorCollisionNormal = state.GetContactLocalNormal(0);
        } else {
			this.floorCollisionNormal = null;
		}
    } */
}
