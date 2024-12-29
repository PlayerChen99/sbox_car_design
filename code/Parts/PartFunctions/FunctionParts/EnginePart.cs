
public class EnginePart : TransmissionPart
{
	public override void TransmissionUpdate()
	{
		//TransmissionOutputs[0].GetSpeed();
		//TransmissionOutputs[0].SetTorque( 100000 );

		TransmissionOutputs[0].SetTorque( Car.accelerate * 100000 );
	}
}
