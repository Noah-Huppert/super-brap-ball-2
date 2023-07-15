using Godot;

public partial class PlayerCollision : CharacterBody3D
{
    // The collision normal of the player on the ground. Or null if the player is not touching the ground.
    public Vector3? floorCollisionNormal = null;

    // The ball 3D model.
    private Ball ball;

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

        // Move player
        if (this.ball.isOpen)
        {
            // Air movement
        } else
        {
            // Ground movement
            var newVel = new Vector3(0, -9.8f, 0);
            var lastColl = GetLastSlideCollision();

            if (lastColl != null) {
                var collNorm = lastColl.GetNormal();

                newVel.X = collNorm.X;
                newVel.Z = collNorm.Z;
            }
            
            Velocity = newVel;
            MoveAndSlide();
        }
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
