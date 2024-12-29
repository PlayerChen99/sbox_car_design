
public class BoxPlacer : Component
{

	[Property, RequireComponent]
	public BoxCollider PartCollider { get; set; }

	public const float CheckOffset = 2.0f;


	public bool Placing( Ray ray, string tag )
	{
		if ( Input.Pressed( "rotate" ) )
		{
			WorldRotation *= Rotation.FromYaw( 90 );
		}
		SceneTraceResult result = Scene.Trace.Box( PartCollider.Scale * WorldRotation - CheckOffset, ray, DesignController.MaxDistance ).WithAnyTags( "designplane", tag ).IgnoreGameObjectHierarchy( GameObject ).Run();
		if ( result.Hit )
		{
			GameObject.Enabled = true;
			WorldPosition = SnapToGrid( result.HitPosition ); ;
			if ( Input.Pressed( "attack1" ) && CheckPositionValid( tag ) )
			{
				return true;
			}
		}
		else
		{
			GameObject.Enabled = false;
		}
		return false;
	}

	public Vector3 SnapToGrid( Vector3 position )
	{
		return (position - PartCollider.Scale / 2).SnapToGrid( DesignController.GridSize ) + PartCollider.Scale / 2;
	}

	public bool CheckPositionValid( string tag )
	{
		return !Scene.Trace.Box( PartCollider.Scale * WorldRotation / 2 - CheckOffset, WorldPosition, WorldPosition ).WithTag( tag ).IgnoreGameObjectHierarchy( GameObject ).Run().StartedSolid;
	}
}
