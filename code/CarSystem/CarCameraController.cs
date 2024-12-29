using Sandbox;

public sealed class CarCameraController : Component
{
	[RequireComponent]
	public CameraComponent CarCamera { get; set; }

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
		angles += Input.AnalogLook;
		angles.pitch = angles.pitch.Clamp( -90, 90 );
		angles.roll = 0;

		distance += -Input.MouseWheel.y * ZoomSpeed;
		distance = distance.Clamp( MinDistance, MaxDistance );

		LocalPosition = WorldRotation.Backward * distance;
	}
}
