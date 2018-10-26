class Button : SpriteGameObject
{
    protected bool pressed;

    public Button(string imageAsset, int layer = 0, string id = "")
        : base(imageAsset, layer, id)
    {
        pressed = false;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        pressed = IsButtonPressedAndWithinBoundaries(inputHelper);
    }

    protected bool IsButtonPressedAndWithinBoundaries(InputHelper inputHelper)
    {
        if (layer >= Camera.UILayer)
        {
            float x = inputHelper.MousePosition.X;
            float y = inputHelper.MousePosition.Y;

            return inputHelper.MouseLeftButtonPressed() && BoundingBox.Contains(x, y);
        }
        else
        {
            float x = inputHelper.MousePosition.X + GameEnvironment.Camera.Position.X;
            float y = inputHelper.MousePosition.Y + GameEnvironment.Camera.Position.Y;

            return inputHelper.MouseLeftButtonPressed() && BoundingBox.Contains(x, y);
        }
    }

    public override void Reset()
    {
        base.Reset();
        pressed = false;
    }

    public bool Pressed
    {
        get { return pressed; }
    }
}
