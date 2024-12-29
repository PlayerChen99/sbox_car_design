
using Sandbox.UI;

[GameResource( "Part", "part", "", Icon = "track_changes" )]
public class PartResource : GameResource
{
	public static HashSet<PartResource> All { get; set; } = new();
	public static Dictionary<string, PartResource> PartDictionary { get; set; } = new();

	public string Name { get; set; } = "PlaceHolder";

	public string Description { get; set; } = "";

	public TypeOfPart PartType { get; set; }

	public GameObject PartPrefab { get; set; }

	public bool RepeatPlace { get; set; }

	public GameObject MoverPrefab { get; set; }

	protected override void PostLoad()
	{
		PartDictionary[Name] = this;
		if ( All.Contains( this ) )
		{
			return;
		}
		All.Add( this );
	}

	public enum TypeOfPart
	{
		Function = 0,
		Shape = 1,
		Tool = 2,
	}

	public string GetPartTag()
	{
		if ( PartType == TypeOfPart.Function )
		{
			return "functionpart";
		}
		else if ( PartType == TypeOfPart.Shape )
		{
			return "shapepart";
		}
		else
		{
			return "";
		}
	}
}


