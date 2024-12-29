using Sandbox;
using System.Xml;

public sealed class CarController : Component
{
	[RequireComponent]
	public Rigidbody CarRigidbody { get; set; }

	[Property]
	CarCameraController CarCamera { get; set; }

	List<CarPart> CarParts = new();
	List<TransmissionPart> TransmissionParts = new();

	public float accelerate;
	public float steer;
	public bool brake;
	public bool shiftup;
	public bool shiftdown;
	public float[] doorState = new float[10];

	protected override void OnUpdate()
	{
		accelerate = Input.AnalogMove.x;
		steer = Input.AnalogMove.y;
		brake = Input.Down( "brake" );
		shiftup = Input.Pressed( "shiftup" );
		shiftdown = Input.Pressed( "shiftdown" );

		foreach ( var part in CarParts )
		{
			part.DriveUpdate();
		}
	}

	protected override void OnFixedUpdate()
	{
		foreach ( var part in TransmissionParts )
		{
			part.TransmissionUpdate();
		}
	}

	public void SwitchDriveMode()
	{
		CarParts = Components.GetAll<CarPart>( FindMode.InChildren ).ToList();
		TransmissionParts = Components.GetAll<TransmissionPart>( FindMode.InChildren ).ToList();

		foreach ( var part in CarParts )
		{
			part.Car = this;
			part.SwitchDriveMode();
		}

		CarRigidbody.Enabled = true;
		Enabled = true;
	}


	public void SwitchDesignMode()
	{
		foreach ( var part in CarParts )
		{
			part.SwitchDesignMode();
		}

		CarRigidbody.Enabled = false;
		Enabled = false;
		WorldPosition = Vector3.Zero;
		WorldRotation  = Rotation.Identity;
	}

	void RemoveAllParts()
	{
		foreach ( CarPart part in Components.GetAll<CarPart>( FindMode.InChildren ) )
		{
			part.GameObject.Destroy();
		}
	}
}
