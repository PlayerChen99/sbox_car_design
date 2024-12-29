
using Sandbox.UI;

public class DoorPart : CarPart
{
	public int group;

	public int angle;

	public int direction;

	[Property]
	public GameObject DoorRoot { get; set; }

	[Property]
	public HighlightOutline DoorOutline { get; set; }

	[Property]
	BoxCollider DoorCollider { get; set; }

	[Property]
	ModelRenderer DoorModel { get; set; }

	public override bool Placing( Ray ray )
	{
		SceneTraceResult result = Scene.Trace.Ray( ray, DesignController.MaxDistance ).WithTag( "covering" ).Run();
		if ( result.Hit )
		{
			Vector3[] nearPoints = new Vector3[7];
			nearPoints[0] = result.HitPosition.SnapToGrid( DesignController.GridSize );
			nearPoints[1] = (result.HitPosition + Vector3.VectorPlaneProject( Vector3.Backward, result.Normal ) * DesignController.GridSize / 2).SnapToGrid( DesignController.GridSize );
			nearPoints[2] = (result.HitPosition + Vector3.VectorPlaneProject( Vector3.Forward, result.Normal ) * DesignController.GridSize / 2).SnapToGrid( DesignController.GridSize );
			nearPoints[3] = (result.HitPosition + Vector3.VectorPlaneProject( Vector3.Right, result.Normal ) * DesignController.GridSize / 2).SnapToGrid( DesignController.GridSize );
			nearPoints[4] = (result.HitPosition + Vector3.VectorPlaneProject( Vector3.Left, result.Normal ) * DesignController.GridSize / 2).SnapToGrid( DesignController.GridSize );
			nearPoints[5] = (result.HitPosition + Vector3.VectorPlaneProject( Vector3.Down, result.Normal ) * DesignController.GridSize / 2).SnapToGrid( DesignController.GridSize );
			nearPoints[6] = (result.HitPosition + Vector3.VectorPlaneProject( Vector3.Up, result.Normal ) * DesignController.GridSize / 2).SnapToGrid( DesignController.GridSize );
			int min = 0;
			float distance = DesignController.GridSize * 2;
			for ( int i = 1; i < 7; i++ )
			{
				float newDistance = nearPoints[0].Distance( nearPoints[i] );
				if ( newDistance > DesignController.GridSize / 2 && newDistance < distance )
				{
					min = i;
					distance = newDistance;
				}
			}
			if ( min == 0 )
			{
				GameObject.Enabled = false;
				return false;
			}
			Vector3 firstPoint = nearPoints[0];
			Vector3 secondPoint = nearPoints[min];
			WorldPosition = (firstPoint + secondPoint) / 2;
			WorldRotation = Rotation.LookAt( secondPoint - firstPoint );
			float length = firstPoint.Distance( secondPoint );
			DoorCollider.Scale = DoorCollider.Scale.WithX( length );
			DoorModel.WorldScale = DoorModel.WorldScale.WithX( length );
			GameObject.Enabled = true;
			if ( Input.Pressed( "attack1" ) )
			{
				if ( !Scene.Trace.Ray( WorldPosition, WorldPosition ).Radius( 2 ).WithTag( "doorpart" ).IgnoreGameObjectHierarchy( GameObject ).Run().Hit )
				{
					return true;
				}
			}
		}
		else
		{
			GameObject.Enabled = false;
		}
		return false;
	}

	public override void Remove()
	{
		foreach ( GameObject cover in DoorRoot.Children.ToArray() )
		{
			cover.Components.Get<Covering>().SetDoor( null );
		}
		base.Remove();
	}


	public override Panel CreateEditPanel()
	{
		return new DoorEditor( this );
	}

	public override void SwitchDriveMode()
	{
		angle *= direction;
		DoorModel.Enabled = false;
	}

	public override void DriveUpdate()
	{
		DoorRoot.LocalRotation = Rotation.FromRoll( Car.doorState[group] * angle );
	}
}
