using Sandbox;

public sealed class DifferentialPart : TransmissionPart
{
	public override void TransmissionUpdate()
	{
		TransmissionInputs[0].SetSpeed( (TransmissionOutputs[0].GetSpeed() + TransmissionOutputs[1].GetSpeed()) / 2 );
		TransmissionOutputs[0].SetTorque( TransmissionInputs[0].GetTorque() / 2 );
		TransmissionOutputs[1].SetTorque( TransmissionInputs[0].GetTorque() / 2 );
	}
}
