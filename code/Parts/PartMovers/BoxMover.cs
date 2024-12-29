

internal class BoxMover : PartMover
{
	BoxPlacer boxPlacer;

	public bool isMoving;

	Vector3 origin;
	Vector3 direction;
	Vector3 offset;

	public override void SetPart( CarPart part )
	{
		MovingPart = part;
		boxPlacer = part.Components.Get<BoxPlacer>();
	}


	public override void OnMouseDown( SceneTraceResult result )
	{
		origin = MovingPart.WorldPosition;
		direction = result.GameObject.WorldRotation.Forward;
		offset = (result.HitPosition - origin).ProjectOnNormal( direction );
		isMoving = true;
	}

	protected override void OnUpdate()
	{
		if ( !isMoving )
		{
			return;
		}
		Ray ray = Scene.Camera.ScreenPixelToRay( Mouse.Position );
		Plane plane = new( origin, origin + direction, origin + direction.Cross( Scene.Camera.WorldRotation.Forward ) );
		if ( plane.TryTrace( ray, out Vector3 hitpoint ) )
		{
			MovingPart.WorldPosition = boxPlacer.SnapToGrid( origin + (hitpoint - origin).ProjectOnNormal( direction ) - offset );
			WorldPosition = MovingPart.WorldPosition;
		}

		MovingPart.OnMove();

		if ( Input.Released( "attack1" ) )
		{
			if ( MovingPart.CheckPositionValid() )
			{
				MovingPart.AfterMove();
			}
			else
			{
				MovingPart.WorldPosition = origin;
				WorldPosition = MovingPart.WorldPosition;
				MovingPart.OnMove();
			}
			isMoving = false;
		}
		
	}
}
