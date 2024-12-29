using Sandbox;

public sealed class InputTorquePort : TorquePort
{
	public void SetSpeed( float speed )
	{
		if ( Shaft.IsValid() )
		{
			Shaft.Speed = speed;
		}
	}
}
