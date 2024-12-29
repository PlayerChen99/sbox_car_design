using Sandbox;
using Sandbox.UI;
using Sandbox.Utility;
using System.Xml;
using System;
using System.IO;

public sealed class DesignController : Component
{
	[Property]
	public DesignCameraController CameraController { get; private set; }

	[Property]
	public CarController DesigningCar { get; private set; }

	public static DesignController Instance { get; private set; }

	public ModeOfCar CarMode;
	public ModeOfDesign DesignMode;

	PartFunction placingPart;

	CarPart selectedPart;
	HighlightOutline selectedPartOutline;

	PartMover selectPartMover;

	public const float GridSize = 16;
	public const float MaxDistance = 10000;

	public enum ModeOfDesign
	{
		Function = 0,
		Shape = 1,
	}

	public enum ModeOfCar
	{
		Design = 0,
		Drive = 1,
	}

	protected override void OnStart()
	{
		Instance = this;
	}

	protected override void OnUpdate()
	{
		if ( CarMode == ModeOfCar.Design )
		{
			DesignUpdate();
		}
		else if ( Input.Pressed( "jump" ) )
		{
			//Input.EscapePressed = false;
			SwitchDriveMode();
		}
	}

	void DesignUpdate()
	{
		if ( placingPart.IsValid() )
		{
			if ( placingPart.Placing( CameraController.DesignCamera.ScreenPixelToRay( Mouse.Position ) ) )
			{
				if ( placingPart is CarPart )
				{
					placingPart.GameObject.SetParent( DesigningCar.GameObject );
					if ( placingPart.Resource.RepeatPlace )
					{
						placingPart = placingPart.Resource.PartPrefab.Clone().Components.Get<PartFunction>();
					}
					else
					{
						SetSelectPart( placingPart as CarPart );
						placingPart = null;
					}
				}
				else
				{
					placingPart.Remove();
				}
			}
		}
		else if ( Input.Pressed( "attack1" ) )
		{
			SceneTraceResult result;
			result = Scene.Trace.Ray( CameraController.DesignCamera.ScreenPixelToRay( Mouse.Position ), MaxDistance ).WithTag( "partmover" ).Run();
			if ( result.Hit )
			{
				result.GameObject.Components.GetInAncestorsOrSelf<PartMover>().OnMouseDown( result );
			}
			else
			{
				result = Scene.Trace.Ray( CameraController.DesignCamera.ScreenPixelToRay( Mouse.Position ), MaxDistance ).WithTag( GetDesignTag() ).Run();
				if ( result.Hit )
				{
					SetSelectPart( result.GameObject.Components.GetInAncestorsOrSelf<CarPart>() );
					Panel panel = selectedPart.CreateEditPanel();
					DesignRoot.Instance.SetPartEditor( panel );
				}

			}
		}
	}

	public void SwitchDesignMode( ModeOfDesign mode )
	{
		if ( DesignMode == mode )
		{
			return;
		}
		DesignMode = mode;
		CameraController.SetMode( mode );
		ClearPlacingPart();
		ClearSelectPart();
	}

	public void SwitchDriveMode()
	{
		if ( CarMode == ModeOfCar.Design )
		{
			CarMode = ModeOfCar.Drive;
			ClearPlacingPart();
			ClearSelectPart();
			DesigningCar.SwitchDriveMode();
		}
		else if ( CarMode == ModeOfCar.Drive )
		{
			CarMode = ModeOfCar.Design;
			DesigningCar.SwitchDesignMode();
		}
	}

	public void AddPlacingPart( PartFunction part )
	{
		ClearPlacingPart();
		ClearSelectPart();
		if ( part.IsValid() )
		{
			placingPart = part;
			placingPart.Car = DesigningCar;
		}
	}

	public void SetSelectPart( CarPart part )
	{
		if ( part == selectedPart )
		{
			return;
		}
		ClearSelectPart();
		selectedPart = part;
		selectedPartOutline = selectedPart.GameObject.Components.Create<HighlightOutline>();
		selectedPartOutline.Color = Color.Orange;
		if ( part.Resource.MoverPrefab is not null )
		{
			selectPartMover = part.Resource.MoverPrefab.Clone( part.WorldPosition ).Components.Get<PartMover>();
			selectPartMover.SetPart( selectedPart );
		}
		Panel panel = selectedPart.CreateEditPanel();
		DesignRoot.Instance.SetPartEditor( panel );
	}

	public void RemoveSelectedPart()
	{
		selectedPart?.Remove();
		ClearPlacingPart();
		ClearSelectPart();
	}

	public void ClearPlacingPart()
	{
		if ( placingPart.IsValid() )
		{
			placingPart.Remove();
		}
	}

	public void ClearSelectPart()
	{
		selectPartMover?.GameObject?.Destroy();
		selectedPartOutline?.Destroy();
		selectedPart = null;
		DesignRoot.Instance.SetPartEditor( null );
	}

	string GetDesignTag()
	{
		if ( DesignMode == ModeOfDesign.Function )
		{
			return "functionpart";
		}
		else
		{
			return "shapepart";
		}
	}

	void Lab()
	{

	}
}
