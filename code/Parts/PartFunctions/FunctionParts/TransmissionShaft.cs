
public class TransmissionShaft : CarPart
{
	[Property]
	public GameObject ShaftBody;
	[Property]
	public GameObject InputBody;
	[Property]
	public GameObject OutputBody;

	const float maxAngle = 100;

	InputTorquePort InputPort;
	OutputTorquePort OutputPort;

	public float Torque;
	public float Speed;


	public override bool Placing( Ray ray )
	{
		var result = Scene.Trace.Ray( ray, DesignController.MaxDistance ).WithTag( "torqueport" ).Run();
		if ( result.Hit )
		{
			var port = result.GameObject.Components.Get<TorquePort>();
			if ( !port.Shaft.IsValid() )
			{
				if ( port is InputTorquePort inputTorquePort )
				{
					if ( !InputPort.IsValid() )
					{
						InputBody.WorldPosition = port.WorldPosition;
						InputBody.WorldRotation = port.WorldRotation;
						InputBody.Enabled = true;
						if ( OutputPort.IsValid() )
						{
							SetShaftPosition();
							ShaftBody.Enabled = true;
						}
						if ( Input.Pressed( "attack1" ) )
						{
							InputPort = inputTorquePort;
							if ( OutputPort.IsValid() )
							{
								if ( CheckPositionValid() )
								{
									InputPort.Shaft = this;
									OutputPort.Shaft = this;
									return true;
								}
								else
								{
									InputPort = null;
								}
							}
						}
					}
				}
				else if ( port is OutputTorquePort outputTorquePort )
				{
					if ( !OutputPort.IsValid() )
					{
						OutputBody.WorldPosition = port.WorldPosition;
						OutputBody.WorldRotation = port.WorldRotation;
						OutputBody.Enabled = true;
						if ( InputPort.IsValid() )
						{
							SetShaftPosition();
							ShaftBody.Enabled = true;
						}
						if ( Input.Pressed( "attack1" ) )
						{
							OutputPort = outputTorquePort;
							if ( InputPort.IsValid() )
							{
								if ( CheckPositionValid() )
								{
									InputPort.Shaft = this;
									OutputPort.Shaft = this;
									return true;
								}
								else
								{
									OutputPort = null;
								}
							}
						}
					}
				}
				return false;
			}
		}
		if ( !InputPort.IsValid() )
		{
			InputBody.Enabled = false;
			ShaftBody.Enabled = false;
		}
		if ( !OutputPort.IsValid() )
		{
			OutputBody.Enabled = false;
			ShaftBody.Enabled = false;
		}
		return false;
	}

	public override bool CheckPositionValid()
	{
		if ( InputPort.WorldRotation.Forward.Angle( ShaftBody.WorldRotation.Forward ) < 180 - maxAngle )
		{
			return false;
		}
		if ( OutputPort.WorldRotation.Forward.Angle( ShaftBody.WorldRotation.Forward ) > maxAngle )
		{
			return false;
		}
		return !Scene.Trace.Sphere( 2, InputPort.WorldPosition, OutputPort.WorldPosition ).WithTag( "functionpart" ).IgnoreGameObjectHierarchy( GameObject ).IgnoreGameObjectHierarchy( InputPort.PartGameObject ).IgnoreGameObjectHierarchy( OutputPort.PartGameObject ).Run().Hit;
	}

	void SetShaftPosition()
	{
		ShaftBody.WorldPosition = (InputBody.WorldPosition + OutputBody.WorldPosition) / 2;
		ShaftBody.WorldRotation = (InputBody.WorldPosition - OutputBody.WorldPosition).EulerAngles;
		ShaftBody.LocalScale = ShaftBody.LocalScale.WithX( (InputBody.WorldPosition - OutputBody.WorldPosition).Length );
	}

	public void UpdatePosition()
	{
		Log.Info( "here" );
		InputBody.WorldPosition = InputPort.WorldPosition;
		InputBody.WorldRotation = InputPort.WorldRotation;
		OutputBody.WorldPosition = OutputPort.WorldPosition;
		OutputBody.WorldRotation = OutputPort.WorldRotation;
		SetShaftPosition();
	}
}
