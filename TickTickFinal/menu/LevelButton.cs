using Microsoft.Xna.Framework;

class LevelButton : GameObjectList
{
    protected TextGameObject text;
    protected SpriteGameObject levelsSolved, levelsUnsolved, sprLock;
    protected bool pressed;
    protected int levelIndex;
    protected Level level;

    public LevelButton(int levelIndex, Level level, int layer = 0, string id = "")
        : base(layer, id)
    {
        this.levelIndex = levelIndex;
        this.level = level;

        levelsSolved = new SpriteGameObject("Sprites/spr_level_solved", Camera.UILayer, "", levelIndex - 1);
        levelsUnsolved = new SpriteGameObject("Sprites/spr_level_unsolved", Camera.UILayer + 1);
        sprLock = new SpriteGameObject("Sprites/spr_level_locked", Camera.UILayer + 2);
        Add(levelsSolved);
        Add(levelsUnsolved);
        Add(sprLock);

        text = new TextGameObject("Fonts/Hud", Camera.UILayer + 1);
        text.Text = levelIndex.ToString();
        text.Position = new Vector2(sprLock.Width - text.Size.X - 10, 5);
        Add(text);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        pressed = (IsButtonPressedAndWithinBoundaries(inputHelper) && !level.Locked);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        sprLock.Visible = level.Locked;
        levelsSolved.Visible = level.Solved;
        levelsUnsolved.Visible = !level.Solved;
    }

    protected bool IsButtonPressedAndWithinBoundaries(InputHelper inputHelper)
    {
        if (layer >= Camera.UILayer)
        {
            float x = inputHelper.MousePosition.X;
            float y = inputHelper.MousePosition.Y;

            return inputHelper.MouseLeftButtonPressed() && levelsSolved.BoundingBox.Contains(x, y);
        }
        else
        {
            float x = inputHelper.MousePosition.X - GameEnvironment.Camera.Position.X;
            float y = inputHelper.MousePosition.Y - GameEnvironment.Camera.Position.Y;

            return inputHelper.MouseLeftButtonPressed() && levelsSolved.BoundingBox.Contains(x, y);
        }
    }

    public int LevelIndex
    {
        get { return levelIndex; }
    }

    public bool Pressed
    {
        get { return pressed; }
    }

    public int Width
    {
        get { return sprLock.Width; }
    }

    public int Height
    {
        get { return sprLock.Height; }
    }
}
