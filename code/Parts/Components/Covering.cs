

public class Covering : Component
{
	[Property]
	ModelRenderer CoverModel { get; set; }

	GameObject cube;

	public bool Visible
	{
		get
		{
			return CoverModel.Enabled;
		}
		set
		{
			CoverModel.Enabled = value;
		}
	}

	protected override void OnAwake()
	{
		cube = GameObject.Parent;
	}

	public void SetDoor( DoorPart door )
	{
		if ( door.IsValid() )
		{
			GameObject.SetParent( door.DoorRoot );
		}
		else
		{
			GameObject.SetParent( cube );
		}
	}
}
