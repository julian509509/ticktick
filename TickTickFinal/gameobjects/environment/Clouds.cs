using Microsoft.Xna.Framework;

class Clouds : GameObjectList
{
    private Vector2 worldSize;

    public Clouds(Vector2 worldSize, int layer = 0, string id = "") : base(layer, id)
    {
        this.worldSize = worldSize;

        for (int i = 0; i < 3; i++)
        {
            SpriteGameObject cloud = new SpriteGameObject("Backgrounds/spr_cloud_" + (GameEnvironment.Random.Next(5) + 1), 2);
            cloud.Position = new Vector2((float)GameEnvironment.Random.NextDouble() * worldSize.X - cloud.Width / 2, 
                (float)GameEnvironment.Random.NextDouble() * GameEnvironment.Screen.Y - cloud.Height / 2);
            cloud.Velocity = new Vector2((float)((GameEnvironment.Random.NextDouble() * 2) - 1) * 20, 0);
            Add(cloud);
        }
    }
    
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        foreach (GameObject obj in children)
        {
            SpriteGameObject c = obj as SpriteGameObject;
            if ((c.Velocity.X < 0 && c.Position.X + c.Width < 0) || (c.Velocity.X > 0 && c.Position.X > worldSize.X))
            {
                Remove(c);
                SpriteGameObject cloud = new SpriteGameObject("Backgrounds/spr_cloud_" + (GameEnvironment.Random.Next(5) + 1));
                cloud.Velocity = new Vector2((float)((GameEnvironment.Random.NextDouble() * 2) - 1) * 20, 0);
                float cloudHeight = (float)GameEnvironment.Random.NextDouble() * GameEnvironment.Screen.Y - c.Height / 2;
                if (cloud.Velocity.X < 0)
                {
                    cloud.Position = new Vector2(worldSize.X, cloudHeight);
                }
                else
                {
                    cloud.Position = new Vector2(-cloud.Width, cloudHeight);
                }

                Add(cloud);
                return;
            }
        }
    }
}

