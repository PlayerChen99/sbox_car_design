

public class CoverHider : PartFunction
{
	bool visible;

	public override bool Placing( Ray ray )
	{
		if ( Input.Down( "attack1" ) )
		{
			Covering cover = Scene.Trace.Ray( ray, DesignController.MaxDistance ).WithTag( "covering" ).Run().GameObject.Components.Get<Covering>();
			if ( Input.Pressed( "attack1" ) )
			{
				visible = !cover.Visible;
			}
			cover.Visible = visible;
		}
		return false;
	}
}
