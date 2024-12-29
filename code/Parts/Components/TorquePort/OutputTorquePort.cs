using Sandbox;

public sealed class OutputTorquePort : TorquePort
{

	public void SetTorque( float torque )
	{
		if ( Shaft.IsValid() )
		{
			Shaft.Torque = torque;
		}
	}
}
