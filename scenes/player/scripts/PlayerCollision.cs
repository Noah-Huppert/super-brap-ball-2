using Godot;

public partial class PlayerCollision : CharacterBody3D
{
    // The collision normal of the player on the ground. Or null if the player is not touching the ground.
    public Vector3? floorCollisionNormal = null;

    // The ball 3D model.
    private Ball ball;

    private float rotateLerpT = 0;
    private Vector3 prevInput = Vector3.Zero;

    private PrintEvery printer;

    public override void _Ready()
    {
        this.ball = GetNode<Ball>("ball");
        this.printer = new PrintEvery(10);
    }

    public override void _Process(double delta)
    {
		// Open ball
		if (Input.IsActionPressed("open_ball") && this.floorCollisionNormal == null && !this.ball.isOpen) {
			this.ball.Open();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        //this.Sleeping = false;

		var forwardStrength = Input.GetActionStrength("player_forward") - Input.GetActionStrength("player_backward");
		var leftStrength = Input.GetActionStrength("player_left") - Input.GetActionStrength("player_right");

        var currentInput = new Vector3(leftStrength, 0, forwardStrength);

        // Movement equations
        var vPrev = Velocity;
        var accel = new Vector3();

        // ... Gravity
        accel += new Vector3(0, -9.8f, 0);

        // ... Contact based movement
        var lastColl = GetLastSlideCollision();
        
        if (lastColl != null && lastColl.GetCollisionCount() > 0) {
            // When contacting
            if (!this.ball.isOpen) {
                // Rolling

                var impulseAccel = Vector3.Zero;
                impulseAccel.Z = forwardStrength * 4;
                impulseAccel.X = leftStrength;

                accel += impulseAccel;
            }
        } else {
            // Not contacting
            floorCollisionNormal = null;

            if (this.ball.isOpen) {
                // Flying

            } else {
                // In free fall mode  
            }


        }

        // Update new velocity
        var vNext = vPrev + (accel * (float)delta);
        Velocity = vNext;

        MoveAndSlide();

        printer.Print("accel=" + accel);

        prevInput = currentInput;
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
