using Microsoft.Xna.Framework;
using System;

class Mountains : GameObjectList
{
    public Mountains(Vector2 worldSize, int layer = 0, string id = "") : base(layer, id)
    {
        // add a few random mountains
        for (int i = 0; i < 5; i++)
        {
            SpriteGameObject mountain = new SpriteGameObject("Backgrounds/spr_mountain_" + (GameEnvironment.Random.Next(2) + 1), layer + i);
            mountain.Position = new Vector2((float)GameEnvironment.Random.NextDouble() * worldSize.X - mountain.Width / 2,
                GameEnvironment.Screen.Y - mountain.Height);
            Add(mountain);
        }
    }

    public override void Update(GameTime gameTime)
    {
        //This line is pure to make the code easier to read
        Camera camera = GameEnvironment.Camera;

        //If the camera hasn't moved, there is nothing to update
        if (camera.Position == camera.PreviousPosition)
        {
           return;
        }
        
        //What is the delta position
        float moveDistance = camera.PreviousPosition.X - camera.Position.X;

        base.Update(gameTime);
        foreach (GameObject obj in children)
        {
            SpriteGameObject mountain = obj as SpriteGameObject;

            //Move all the mountains
            Vector2 newPos = mountain.Position;
            newPos.X += (moveDistance / 15) * mountain.Layer;
            mountain.Position = newPos;
        }
    }
}

