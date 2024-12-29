using Sandbox;

public sealed class DesignCameraController : Component
{
	[RequireComponent]
	public CameraComponent DesignCamera { get; set; }

	[Property]
	float Sensitivity { get; set; } = 0.4f;

	[Property]
	float ZoomSpeed { get; set; } = 100f;

	[Property]
	float MinDistance { get; set; } = 200;
	[Property]
	float MaxDistance { get; set; } = 1000;

	Angles angles;
	float distance;


	protected override void OnStart()
	{
		angles = WorldRotation.Angles();
		distance = WorldPosition.Length;
	}


	protected override void OnUpdate()
	{
		if ( Input.Down( "attack2" ) )
		{
			angles.yaw -= Mouse.Delta.x * Sensitivity;
			angles.pitch = (angles.pitch + Mouse.Delta.y * Sensitivity).Clamp( -90, 90 );
			angles.roll = 0.0f;

			//angles += Input.AnalogLook;
			//angles.pitch = angles.pitch.Clamp( -90, 90 );
			//angles.roll = 0.0f;

			WorldRotation = angles;
		}
		distance += -Input.MouseWheel.y * ZoomSpeed;
		distance = distance.Clamp( MinDistance, MaxDistance );

		WorldPosition = WorldRotation.Backward * distance;
	}

	public void SetMode( DesignController.ModeOfDesign designMode )
	{
		if ( designMode == DesignController.ModeOfDesign.Function )
		{
			DesignCamera.RenderExcludeTags.Remove( "functionpart" );
			DesignCamera.RenderExcludeTags.Add( "shapepart" );
		}
		else if ( designMode == DesignController.ModeOfDesign.Shape )
		{
			DesignCamera.RenderExcludeTags.Remove( "shapepart" );
			DesignCamera.RenderExcludeTags.Add( "functionpart" );
		}
	}
}
