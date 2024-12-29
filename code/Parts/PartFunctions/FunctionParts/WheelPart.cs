
using Sandbox.UI;
using System.Diagnostics.Metrics;

public class WheelPart : TransmissionPart
{
	[Property, RequireComponent]
	Wheel WheelCollider { get; set; }

	[Property]
	public float SteerAngle { get; set; } = 0;

	public override void SwitchDriveMode()
	{
		base.SwitchDriveMode();
		Placer.PartCollider.Enabled = false;
		WheelCollider.Enabled = true;
	}

	public override void SwitchDesignMode()
	{
		base.SwitchDesignMode();
		Placer.PartCollider.Enabled = true;
		WheelCollider.Enabled = false;
	}

	public override Panel CreateEditPanel()
	{
		return new WheelEditor( this );
	}

	public override void TransmissionUpdate()
	{
		TransmissionInputs[0].SetSpeed( GetSpeed() );
		WheelCollider.ApplyMotorTorque( TransmissionInputs[0].GetTorque() );
	}

	float GetSpeed()
	{
		if ( Car.IsValid() && Car.CarRigidbody.IsValid() )
		{
			return Car.CarRigidbody.GetVelocityAtPoint( WorldPosition ).Dot( WorldRotation.Forward ) / WheelCollider.WheelRadius;
		}
		else
		{
			return 0;
		}
	}
}
