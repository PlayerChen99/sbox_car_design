using Sandbox.UI;
using System.Xml;


public class CarPart : PartFunction
{
	public int SavePartIndex;

	public virtual bool CheckPositionValid()
	{
		return false;
	}

	public virtual Panel CreateEditPanel()
	{
		return null;
	}

	public virtual void OnMove()
	{

	}

	public virtual void AfterMove()
	{
		
	}

	public virtual void SwitchDriveMode()
	{

	}

	public virtual void SwitchDesignMode()
	{

	}

	public virtual void DriveUpdate()
	{

	}

}
