using Sandbox;

public abstract class TorquePort : Component
{
	[RequireComponent]
	BoxCollider PortCollider { get; set; }

	public TransmissionShaft Shaft;

	public GameObject PartGameObject => Transform.Parent;

	public float GetSpeed()
	{
		if ( Shaft.IsValid() )
		{
			return Shaft.Speed;
		}
		else {
			return 0;
		}
	}

	public float GetTorque()
	{
		if ( Shaft.IsValid() )
		{
			return Shaft.Torque;
		}
		else
		{
			return 0;
		}
	}
}
