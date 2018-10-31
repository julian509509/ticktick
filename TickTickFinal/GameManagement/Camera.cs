using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Camera
{
    /// <summary>
    /// Every game object with this layer or higher will not move with the camera.
    /// </summary>
    public const int UILayer = 100;

    private Vector2 position;
    public Vector2 Position { get { return position; } }

    public Vector2 PreviousPosition { get; private set; }

    private float maxYOffset;
    private float maxXOffset;

    public Camera(Vector2 position)
    {
        this.position = position;
    }

    //Used to make sure the camera can't view outside of the map
    public void SetLevelBoundaries(Vector2 worldSize)
    {
        maxXOffset = worldSize.X - GameEnvironment.Screen.X;
        maxYOffset = worldSize.Y - GameEnvironment.Screen.Y;
    }

    public void Reset()
    {
        position = new Vector2(0, 0);
    }

    public void UpdatePosition(Vector2 playerPos)
    {
        //Store previous position
        PreviousPosition = position;

        //Calculate the new position
        position.X = playerPos.X - GameEnvironment.Screen.X / 2;
        position.Y = playerPos.Y - GameEnvironment.Screen.Y / 2;

        //Make sure the camera is within boundaries
        if(position.X > maxXOffset)
        {
            position.X = maxXOffset;
        }else if(position.X < 0)
        {
            position.X = 0;
        }

        if (position.Y > maxYOffset)
        {
            position.Y = maxYOffset;
        }
        else if (position.Y < 0)
        {
            position.Y = 0;
        }
    }
}
