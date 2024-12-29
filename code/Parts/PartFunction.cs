[Category("PartFunction")]
public class PartFunction : Component
{

	[Property]
	public PartResource Resource {  get; set; }

	public CarController Car;

	public virtual bool Placing( Ray ray )
	{
		return false;
	}

	public virtual void Remove()
	{
		GameObject.Destroy();
	}
}
