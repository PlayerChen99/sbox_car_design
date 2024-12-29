
using System.Diagnostics;

public class TransmissionPart : CarPart
{
	[Property]
	public InputTorquePort[] TransmissionInputs { get; set; }
	[Property]
	public OutputTorquePort[] TransmissionOutputs { get; set; }

	[Property, RequireComponent]
	public BoxPlacer Placer { get; set; }

	public override bool Placing( Ray ray )
	{
		return Placer.Placing( ray, "functionpart" );
	}

	public override void Remove()
	{
		foreach ( var item in TransmissionInputs )
		{
			if ( item.Shaft.IsValid() )
			{
				item.Shaft.Remove();
			}
		}
		foreach ( var item in TransmissionOutputs )
		{
			if ( item.Shaft.IsValid() )
			{
				item.Shaft.Remove();
			}
		}
		base.Remove();
	}


	public override void OnMove()
	{
		foreach ( var item in TransmissionInputs )
		{
			if ( item.Shaft.IsValid() )
			{
				item.Shaft.UpdatePosition();
			}
		}
		foreach ( var item in TransmissionOutputs )
		{
			if ( item.Shaft.IsValid() )
			{
				item.Shaft.UpdatePosition();
			}
		}
	}

	public override void AfterMove()
	{
		foreach ( var item in TransmissionInputs )
		{
			if ( item.Shaft.IsValid() && !item.Shaft.CheckPositionValid() )
			{
				item.Shaft.Remove();
			}
		}
		foreach ( var item in TransmissionInputs )
		{
			if ( item.Shaft.IsValid() && !item.Shaft.CheckPositionValid() )
			{
				item.Shaft.Remove();
			}
		}
	}

	public override bool CheckPositionValid()
	{
		SceneTrace trace = Scene.Trace.Box( Placer.PartCollider.Scale * WorldRotation / 2 - BoxPlacer.CheckOffset, WorldPosition, WorldPosition ).WithTag( "functionpart" ).IgnoreGameObjectHierarchy( GameObject );
		foreach ( var item in TransmissionInputs )
		{
			if ( item.Shaft.IsValid() )
			{
				trace = trace.IgnoreGameObjectHierarchy( item.Shaft.GameObject );
			}
		}
		foreach ( var item in TransmissionOutputs )
		{
			if ( item.Shaft.IsValid() )
			{
				trace = trace.IgnoreGameObjectHierarchy( item.Shaft.GameObject );
			}
		}
		return !trace.Run().StartedSolid;
	}

	public virtual void TransmissionUpdate()
	{

	}
}
