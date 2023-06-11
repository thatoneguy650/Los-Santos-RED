using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HotelRoom
{
    public HotelRoom()
    {
    }

    public HotelRoom(string roomName, Vector3 cameraPosition, Vector3 cameraDirection, Rotator cameraRotation)
    {
        RoomName = roomName;
        CameraPosition = cameraPosition;
        CameraDirection = cameraDirection;
        CameraRotation = cameraRotation;
    }

    public string RoomName { get; set; } = "Room 1A";
    public Vector3 CameraPosition { get; set; }
    public Vector3 CameraDirection { get; set; }
    public Rotator CameraRotation { get; set; }

    public void AddDistanceOffset(Vector3 offsetToAdd)
    {
        CameraPosition += offsetToAdd;
        CameraDirection += offsetToAdd;
    }
}

