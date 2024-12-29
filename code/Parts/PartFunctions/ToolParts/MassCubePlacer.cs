
using Sandbox;
using System.Numerics;
using System;

public class MassCubePlacer : PartFunction
{
	bool firstPoint = true;
	Vector3 firstPointPosition;
	Vector3 GridVector => Vector3.One * DesignController.GridSize;
	public int Group;

	[Property]
	GameObject CoverCubePrefab { get; set; }


	public override bool Placing( Ray ray )
	{
		Vector3 targetPosition;

		SceneTraceResult result = Scene.Trace.Box( GridVector, ray, DesignController.MaxDistance ).WithAnyTags( "designplane", "shapepart" ).WithoutTags( "covering" ).Run();
		if ( result.Hit )
		{
			targetPosition = (result.HitPosition - GridVector / 2).SnapToGrid( DesignController.GridSize ) + GridVector / 2;
			GameObject.Enabled = true;
		}
		else
		{
			GameObject.Enabled = false;
			return false;
		}

		if ( firstPoint )
		{
			WorldPosition = targetPosition;
			if ( Input.Pressed( "attack1" ) )
			{
				firstPoint = false;
				firstPointPosition = targetPosition;
			}
		}
		else
		{
			WorldPosition = (targetPosition + firstPointPosition) / 2;
			Vector3 scaleVector = (targetPosition - firstPointPosition).Abs() + GridVector;
			LocalScale = scaleVector / DesignController.GridSize + Vector3.One * 0.2f;

			if ( Input.Pressed( "attack1" ) )
			{

				foreach ( var hitResult in Scene.Trace.Box( scaleVector - GridVector, WorldPosition, WorldPosition ).WithTag( "shapepart" ).WithoutTags( "covering" ).RunAll() )
				{
					hitResult.GameObject.Components.Get<CoverCube>().RemoveNoCheck();
				}

				Vector3 minPosition = Vector3.Min( firstPointPosition, targetPosition );
				Vector3 maxPosition = Vector3.Max( firstPointPosition, targetPosition );

				int lengthX = (int)MathF.Round( (maxPosition.x - minPosition.x) / DesignController.GridSize );
				int lengthY = (int)MathF.Round( (maxPosition.y - minPosition.y) / DesignController.GridSize );
				int lengthZ = (int)MathF.Round( (maxPosition.z - minPosition.z) / DesignController.GridSize );

				for ( int x = 0; x <= lengthX; x++ )
				{
					for ( int y = 0; y <= lengthY; y++ )
					{
						for ( int z = 0; z <= lengthZ; z++ )
						{
							CoverCube cube = CoverCubePrefab.Clone( Car.GameObject, minPosition + GridVector * new Vector3( x, y, z ), Rotation.Identity, Vector3.One ).Components.Get<CoverCube>();
							if ( x == 0 )
							{
								cube.UpdateCover( 0 );
							}
							else
							{
								cube.SetCoverVisible( 0, false );
							}
							if ( x == lengthX )
							{
								cube.UpdateCover( 1 );
							}
							else
							{
								cube.SetCoverVisible( 1, false );
							}
							if ( y == 0 )
							{
								cube.UpdateCover( 2 );
							}
							else
							{
								cube.SetCoverVisible( 2, false );
							}
							if ( y == lengthY )
							{
								cube.UpdateCover( 3 );
							}
							else
							{
								cube.SetCoverVisible( 3, false );
							}
							if ( z == 0 )
							{
								cube.UpdateCover( 4 );
							}
							else
							{
								cube.SetCoverVisible( 4, false );
							}
							if ( z == lengthZ )
							{
								cube.UpdateCover( 5 );
							}
							else
							{
								cube.SetCoverVisible( 5, false );
							}
						}
					}
				}
				return true;
			}
		}
		return false;
	}
}
