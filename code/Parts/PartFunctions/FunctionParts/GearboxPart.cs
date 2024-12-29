
public class GearboxPart : TransmissionPart
{
	[Property] int MaxGear = 6;

	[Property] float[] GearRatio = new float[7];

	int currentGear;

	public override void DriveUpdate()
	{
		if ( Car.shiftup )
		{
			currentGear = (currentGear + 1).Clamp( -1, MaxGear );
		}
		else if ( Car.shiftdown )
		{
			currentGear = (currentGear - 1).Clamp( -1, MaxGear );
		}
	}

	public override void TransmissionUpdate()
	{
		//if ( currentGear == 0 )
		//{
		//	TransmissionInputs[0].SetSpeed( 0 );
		//	TransmissionOutputs[0].SetTorque( 0 );
		//}
		//else if ( currentGear == -1 )
		//{
		//	TransmissionInputs[0].SetSpeed( TransmissionOutputs[0].GetSpeed() * GearRatio[0] );
		//	TransmissionOutputs[0].SetTorque( TransmissionInputs[0].GetTorque() * GearRatio[0] );
		//}
		//else
		//{
		//	TransmissionInputs[0].SetSpeed( TransmissionOutputs[0].GetSpeed() * GearRatio[currentGear] );
		//	TransmissionOutputs[0].SetTorque( TransmissionInputs[0].GetTorque() * GearRatio[currentGear] );
		//}

		TransmissionOutputs[0].SetTorque( TransmissionInputs[0].GetTorque() );
	}
}
