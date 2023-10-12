using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RoadNode
{
	private bool useForwardLanes;
    private float roadsideOffset;



	public RoadNode(Vector3 position, float heading)
    {
        Position = position;
        Heading = heading;
    }

    public Vector3 Position { get; set; }
    public float Heading { get; set; }

    public Vector3 RoadPosition { get; private set; }
    public int ForwardLanes { get; private set; }
    public int BackwardsLanes { get; private set; }
    public float Width { get; private set; }
    public bool HasRoad { get; private set; }
	public bool IsOneWay { get; private set; }
	public int TotalLanes => ForwardLanes + BackwardsLanes;
	public bool MajorRoadsOnly { get; set; } = false;

	//Mostly from Fish from the Codewalker Discord 
	public void GetRodeNodeProperties()
	{
		Vector3 startingPos;
		Vector3 finalPos;
		int forwardLanes;
		int backwardsLanes;
		float width;
		HasRoad = NativeFunction.Natives.GET_CLOSEST_ROAD<bool>(Position.X, Position.Y, Position.Z, 1.0f, 1, out startingPos, out finalPos, out forwardLanes, out backwardsLanes, out width, MajorRoadsOnly);
		if (HasRoad)
		{
			RoadPosition = finalPos;
			ForwardLanes = forwardLanes;
			BackwardsLanes = backwardsLanes;
			Width = width;
		}
		if (Heading < 90f || Heading >= 270f)
		{
			useForwardLanes = true;
		}
		else
		{
			useForwardLanes = false;
		}
		IsOneWay = false;
		if (useForwardLanes)
		{
			if (TotalLanes == ForwardLanes)
			{
				IsOneWay = true;
			}
		}
		else if (TotalLanes == BackwardsLanes)
		{
			IsOneWay = true;
		}
		if (Width < 0f)
		{
			roadsideOffset = 0f;
		}
		else
		{
			if (useForwardLanes)
			{
				if (IsOneWay)
				{
					roadsideOffset = (4.5f * ((float)(ForwardLanes) * 0.5f));
				}
				else
				{
					roadsideOffset = (4.5f * (float)(ForwardLanes));
				}
				if (IsOneWay)
				{
					if (ForwardLanes > 2)
					{
						roadsideOffset = (roadsideOffset + ((float)((ForwardLanes - 2)) * 1f));
					}
				}
				else if (ForwardLanes > 1)
				{
					roadsideOffset = (roadsideOffset + ((float)((ForwardLanes - 1)) * 1f));
				}
			}
			else
			{
				if (IsOneWay)
				{
					roadsideOffset = (4.5f * ((float)(BackwardsLanes) * 0.5f));
				}
				else
				{
					roadsideOffset = (4.5f * (float)(BackwardsLanes));
				}
				if (IsOneWay)
				{
					if (BackwardsLanes > 2)
					{
						roadsideOffset = (roadsideOffset + ((float)((BackwardsLanes - 2)) * 1f));
					}
				}
				else if (BackwardsLanes > 1)
				{
					roadsideOffset = (roadsideOffset + ((float)((BackwardsLanes - 1)) * 1f));
				}
			}
		}
	}
}

