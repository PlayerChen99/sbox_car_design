
using Sandbox;
using System.Numerics;
using System;

public class MassCubeRemover : PartFunction
{
	bool firstPoint = true;
	Vector3 firstPointPosition;

	static Vector3 GridVector => Vector3.One * DesignController.GridSize;

	public override bool Placing( Ray ray )
	{
		Vector3 targetPosition;

		SceneTraceResult result;
		result = Scene.Trace.Ray( ray, DesignController.MaxDistance ).WithTag( "shapepart" ).WithoutTags( "covering" ).Run();
		if ( result.Hit )
		{
			targetPosition = result.GameObject.WorldPosition;
		}
		else
		{
			result = Scene.Trace.Box( GridVector, ray, DesignController.MaxDistance ).WithAnyTags( "designplane", "shapepart" ).WithoutTags( "covering" ).Run();
			if ( result.Hit )
			{
				targetPosition = (result.HitPosition - GridVector / 2).SnapToGrid( DesignController.GridSize ) + GridVector / 2;
			}
			else
			{
				GameObject.Enabled = false;
				return false;
			}
		}
		if ( firstPoint )
		{
			WorldPosition = targetPosition;
			GameObject.Enabled = true;
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
			GameObject.Enabled = true;

			Gizmo.Draw.LineBBox( new( WorldPosition - (scaleVector + GridVector) / 2, WorldPosition + (scaleVector + GridVector) / 2 ) );

			if ( Input.Pressed( "attack1" ) )
			{
				Vector3 minVector = WorldPosition - scaleVector / 2;
				Vector3 maxVector = WorldPosition + scaleVector / 2;
				foreach ( var hitResult in Scene.Trace.Box( scaleVector + GridVector, WorldPosition, WorldPosition ).WithTag( "shapepart" ).WithoutTags( "covering" ).RunAll() )
				{
					CoverCube cube = hitResult.GameObject.Components.Get<CoverCube>();
					int deltaX = 0, deltaY = 0, deltaZ = 0;
					if ( hitResult.GameObject.WorldPosition.x > maxVector.x )
					{
						deltaX = -1;
					}
					else if ( hitResult.GameObject.WorldPosition.x < minVector.x )
					{
						deltaX = 1;
					}
					if ( hitResult.GameObject.WorldPosition.y > maxVector.y )
					{
						deltaY = -1;
					}
					else if ( hitResult.GameObject.WorldPosition.y < minVector.y )
					{
						deltaY = 1;
					}
					if ( hitResult.GameObject.WorldPosition.z > maxVector.z )
					{
						deltaZ = -1;
					}
					else if ( hitResult.GameObject.WorldPosition.z < minVector.z )
					{
						deltaZ = 1;
					}

					if ( deltaX == 0 && deltaY == 0 && deltaZ == 0 )
					{
						cube.RemoveNoCheck();
					}
					else if ( deltaX == 0 && deltaY == 0 || deltaX == 0 && deltaZ == 0 || deltaY == 0 && deltaZ == 0 )
					{
						cube.SetCoverVisible( new Vector3( deltaX, deltaY, deltaZ ), true );
					}
				}
				return true;
			}
		}
		return false;
	}
}
