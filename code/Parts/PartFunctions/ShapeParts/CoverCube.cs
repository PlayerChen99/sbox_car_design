
public class CoverCube : CarPart
{
	[Property, RequireComponent]
	BoxPlacer Placer { get; set; }

	[Property]
	Covering[] Covers = new Covering[8];

	public int group = 0;

	public override bool Placing( Ray ray )
	{
		if ( Placer.Placing( ray, "shapepart" ) )
		{
			UpdateAllCover();
			return true;
		}
		return false;
	}

	public void UpdateCover( int index )
	{
		Vector3 direction = GetDirection( index );
		SceneTraceResult result = Scene.Trace.Ray( WorldPosition, WorldPosition + direction * DesignController.GridSize ).IgnoreGameObjectHierarchy( GameObject ).WithTag( "shapepart" ).WithoutTags( "covering", "shapeignore" ).Run();
		if ( result.Hit )
		{
			CoverCube nearbyCube = result.GameObject.Components.Get<CoverCube>();
			if ( nearbyCube.IsValid() )
			{
				SetCoverVisible( index, nearbyCube.group < group );
				nearbyCube.SetCoverVisible( -direction, nearbyCube.group > group );
			}
			else
			{
				SetCoverVisible( index, true );
			}
		}
		else
		{
			SetCoverVisible( index, true );
		}
	}

	public void UpdateAllCover()
	{
		for ( int index = 0; index < 6; index++ )
		{
			UpdateCover( index );
		}
	}

	public void SetCoverVisible( int index, bool visible )
	{
		if ( Covers[index].IsValid() )
		{
			Covers[index].Visible = visible;
		}
	}

	public void SetCoverVisible( Vector3 direction, bool visible )
	{
		SetCoverVisible( GetIndex( direction ), visible );
	}

	Vector3 GetDirection( int index )
	{
		return index switch
		{
			0 => WorldRotation.Backward,
			1 => WorldRotation.Forward,
			2 => WorldRotation.Right,
			3 => WorldRotation.Left,
			4 => WorldRotation.Down,
			5 => WorldRotation.Up,
			_ => WorldRotation.Backward,
		};
	}

	int GetIndex( Vector3 direction )
	{
		if ( direction.x < -0.5f )
		{
			return 0;
		}
		else if ( direction.x > 0.5f )
		{
			return 1;
		}
		else if ( direction.y < -0.5f )
		{
			return 2;
		}
		else if ( direction.y > 0.5f )
		{
			return 3;
		}
		else if ( direction.z < -0.5f )
		{
			return 4;
		}
		else if ( direction.z > 0.5f )
		{
			return 5;
		}
		else
		{
			return 0;
		}
	}

	public override void Remove()
	{
		foreach ( var cover in Covers )
		{
			cover?.GameObject?.Destroy();
		}
		for ( int index = 0; index < 6; index++ )
		{
			Vector3 direction = GetDirection( index );
			SceneTraceResult result = Scene.Trace.Ray( WorldPosition, WorldPosition + direction * DesignController.GridSize ).WithTag( "shapepart" ).WithoutTags( "covering" ).Run();
			if ( result.Hit )
			{
				result.GameObject.Components.Get<CoverCube>()?.SetCoverVisible( -direction, true );
			}
		}
		base.Remove();
	}

	public void RemoveNoCheck()
	{
		foreach ( var cover in Covers )
		{
			cover?.GameObject?.Destroy();
		}
		base.Remove();
	}
}
