

public class DoorSetter : PartFunction
{
	public DoorPart door;

	bool isAdding;

	public override bool Placing( Ray ray )
	{
		if ( Input.Down( "attack1" ) )
		{
			Covering cover = Scene.Trace.Ray( ray, DesignController.MaxDistance ).WithTag( "covering" ).Run().GameObject.Components.Get<Covering>();
			if ( Input.Pressed( "attack1" ) )
			{
				if ( cover.GameObject.Parent == door.DoorRoot )
				{
					isAdding = false;
				}
				else
				{
					isAdding = true;
				}
			}
			if ( isAdding )
			{
				cover.SetDoor( door );
			}
			else
			{
				cover.SetDoor( null );
			}
		}
		return false;
	}

	public override void Remove()
	{
		door.DoorOutline.Enabled = false;
		base.Remove();
	}
}
