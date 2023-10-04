using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Color = Microsoft.Xna.Framework.Color;
using Key = Microsoft.Xna.Framework.Input.Keys;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector3 = System.Numerics.Vector3;

// ReSharper disable CollectionNeverUpdated.Local

namespace NavMeshStudio;

public class Viewer : Game
{
    public readonly List<VertexPositionColor> Facesets = new();
    public readonly List<VertexPositionColor> Vertices = new();
    private readonly string ViewerBGFilePath = $"{Utils.ResourcesPath}\\bg.png";
    private BasicEffect BasicEffect = null!;
    private Vector3 Camera = new(0, 4, 2);
    private Vector3 CameraOffset = new(0, 0, 0);
    private KeyboardState CurrentKeyboardState;
    private MouseState CurrentMouseState;
    private IntPtr DrawSurface;
    private VertexBuffer FacesetBuffer = null!;
    public GraphicsDeviceManager GraphicsManager = null!;
    private IndexBuffer IndexBuffer = null!;
    public List<int> Indices = new();
    private bool IsFocused;
    public bool IsInitialized;
    private MouseState PreviousMouseState;
    private SpriteBatch SpriteBatch = null!;
    private VertexBuffer VertexBuffer = null!;
    private Rectangle ViewerBGArea;
    private Texture2D ViewerBGTexture = null!;
    public Form ViewerWindow = null!;

    public Viewer() { }

    public Viewer(NavMeshStudio studio)
    {
        ConfigureViewerSettings(studio);
        InitializeGraphicsManager();
        RegisterViewerEvents(studio);
    }

    private void InitializeGraphicsManager()
    {
        GraphicsManager = new GraphicsDeviceManager(this);
    }

    private void RegisterViewerEvents(NavMeshStudio studio)
    {
        GraphicsManager.PreparingDeviceSettings += (_, e) =>
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = DrawSurface;
            IsFocused = studio.viewer.Invoke(() => Utils.IsMouseOverControl(studio.viewer));
        };
        studio.viewer.MouseEnter += (_, _) => IsFocused = true;
        studio.viewer.MouseLeave += (_, _) => IsFocused = false;
        ViewerWindow = (Control.FromHandle(Window.Handle) as Form)!;
        ViewerWindow.Opacity = 0;
    }

    private void ConfigureViewerSettings(NavMeshStudio studio)
    {
        Window.Title = "NavMesh Viewer";
        Window.AllowUserResizing = true;
        IsMouseVisible = true;
        Content.RootDirectory = "Content";
        studio.viewer.Invoke(() => DrawSurface = studio.viewer.Handle);
    }

    private void InitializeBasicEffect()
    {
        BasicEffect = new BasicEffect(GraphicsDevice);
        BasicEffect.VertexColorEnabled = true;
    }

    private void RefreshPrimitiveBuffers()
    {
        VertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, Vertices.Count, BufferUsage.None);
        VertexBuffer.SetData(Vertices.ToArray());
        IndexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, Indices.Count, BufferUsage.None);
        IndexBuffer.SetData(Indices.ToArray());
        if (Facesets.Count <= 0) return;
        FacesetBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, Facesets.Count, BufferUsage.None);
        FacesetBuffer.SetData(Facesets.ToArray());
    }

    protected override void Initialize()
    {
        InitializeBasicEffect();
        RefreshPrimitiveBuffers();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        ViewerBGTexture = Texture2D.FromFile(GraphicsDevice, ViewerBGFilePath);
    }

    private void UpdateLeftMouseButtonClick()
    {
        Camera = Camera.RotatePoint(0, 0, -(CurrentMouseState.Position.X - PreviousMouseState.Position.X) * 0.01f);
        Vector3 direction = new(Camera.Y, -Camera.X, 0);
        float theta = (CurrentMouseState.Position.Y - PreviousMouseState.Position.Y) * 0.01f;
        Camera = Utils3D.RotateLine(Camera, new Vector3(0, 0, 0), direction, theta);
    }

    private void UpdateMiddleMouseButtonClick()
    {
        Vector3 upVector = new(0, 0, 1);
        Vector3 rightVector = Utils3D.CrossProduct(upVector, Camera).NormalizeNumericsVector3();
        Microsoft.Xna.Framework.Vector3 cameraUpVector = Utils3D.CrossProduct(Camera, rightVector).NormalizeNumericsVector3();
        float mouseX = CurrentMouseState.Position.X - PreviousMouseState.Position.X;
        float mouseY = CurrentMouseState.Position.Y - PreviousMouseState.Position.Y;
        CameraOffset -= new Vector3(rightVector.X * mouseX * 0.01f, rightVector.Y * mouseX * 0.01f, rightVector.Z * mouseX * 0.01f);
        CameraOffset += new Vector3(cameraUpVector.X * mouseY * 0.01f, cameraUpVector.Y * mouseY * 0.01f, cameraUpVector.Z * mouseY * 0.01f);
    }

    private void UpdateMouseScrollWheel()
    {
        switch (CurrentMouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue)
        {
            case > 0:
                Camera.X -= 0.5f * (Camera.X / Camera.Length());
                Camera.Y -= 0.5f * (Camera.Y / Camera.Length());
                Camera.Z -= 0.5f * (Camera.Z / Camera.Length());
                break;
            case < 0:
                Camera.X += 0.5f * (Camera.X / Camera.Length());
                Camera.Y += 0.5f * (Camera.Y / Camera.Length());
                Camera.Z += 0.5f * (Camera.Z / Camera.Length());
                break;
        }
    }

    private void UpdateMoveCameraForwardBackward(GameTime gameTime, bool forwards, int movementSpeed)
    {
        Vector3 forwardVector = Camera.NormalizeNumericsVector3();
        CameraOffset -= movementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds * forwardVector * (forwards ? 1 : -1);
    }

    private void UpdateMoveCameraLeftRight(GameTime gameTime, bool left, int movementSpeed)
    {
        Vector3 upVector = new(0, 0, 1);
        Vector3 rightVector = Utils3D.CrossProduct(upVector, Camera).NormalizeNumericsVector3();
        CameraOffset -= movementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds * rightVector * (left ? 1 : -1);
    }

    private void UpdateKeyboardInput(GameTime gameTime)
    {
        CurrentKeyboardState = Keyboard.GetState();
        Key pressedKey = CurrentKeyboardState.GetPressedKeys().ElementAtOrDefault(0);
        bool shift = CurrentKeyboardState.IsKeyDown(Key.LeftShift) || CurrentKeyboardState.IsKeyDown(Key.RightShift);
        int movementSpeed = shift ? 200 : 50;
        switch (pressedKey)
        {
            case Key.W:
                UpdateMoveCameraForwardBackward(gameTime, true, movementSpeed);
                break;
            case Key.A:
                UpdateMoveCameraLeftRight(gameTime, true, movementSpeed);
                break;
            case Key.S:
                UpdateMoveCameraForwardBackward(gameTime, false, movementSpeed);
                break;
            case Key.D:
                UpdateMoveCameraLeftRight(gameTime, false, movementSpeed);
                break;
        }
    }

    private void UpdatePreviousMouseState()
    {
        PreviousMouseState = CurrentMouseState;
    }

    private void UpdateMouseInput()
    {
        CurrentMouseState = Mouse.GetState();
        if (CurrentMouseState.LeftButton == ButtonState.Pressed)
        {
            UpdateLeftMouseButtonClick();
        }
        else if (CurrentMouseState.MiddleButton == ButtonState.Pressed)
        {
            UpdateMiddleMouseButtonClick();
        }
        UpdateMouseScrollWheel();
        UpdatePreviousMouseState();
    }

    protected override void Update(GameTime gameTime)
    {
        UpdateKeyboardInput(gameTime);
        if (!IsFocused) return;
        UpdateMouseInput();
        base.Update(gameTime);
    }

    private void DrawGeometry()
    {
        if (Vertices.Count > 0)
        {
            GraphicsDevice.Indices = IndexBuffer;
            GraphicsDevice.SetVertexBuffer(VertexBuffer);
            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, Vertices.Count / 2);
        }
        if (Facesets.Count == 0) return;
        GraphicsDevice.SetVertexBuffer(FacesetBuffer);
        GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Facesets.Count / 3);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        SpriteBatch.Begin();
        ViewerBGArea = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        SpriteBatch.Draw(ViewerBGTexture, ViewerBGArea, Color.White);
        SpriteBatch.End();
        SpriteBatch.Begin();
        UpdateView();
        DrawGeometry();
        SpriteBatch.End();
        base.Draw(gameTime);
    }

    private void UpdateView()
    {
        Vector3 cameraPosition = new(Camera.X + CameraOffset.X, Camera.Y + CameraOffset.Y, Camera.Z + CameraOffset.Z);
        DepthStencilState depthStencilState = new();
        depthStencilState.DepthBufferEnable = true;
        depthStencilState.DepthBufferFunction = CompareFunction.LessEqual;
        GraphicsDevice.DepthStencilState = depthStencilState;
        BasicEffect.View = Matrix.CreateLookAt(cameraPosition, CameraOffset, Vector3.UnitZ);
        BasicEffect.VertexColorEnabled = true;
        float viewerAspectRatio = GraphicsManager.PreferredBackBufferWidth / (float)GraphicsManager.PreferredBackBufferHeight;
        BasicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, viewerAspectRatio, 0.1f, 500);
        BasicEffect.TextureEnabled = false;
        BasicEffect.CurrentTechnique.Passes.ToList().ForEach(i => i.Apply());
    }

    private void AddNodesGeometry<T>(List<T> nodes) where T : GeoNode
    {
        nodes.ForEach(i =>
        {
            Vertices.AddRange(i.Vertices);
            Facesets.AddRange(i.Facesets);
        });
    }

    public void BuildGeometry(NavMeshStudio studio)
    {
        Vertices.Clear();
        Facesets.Clear();
        studio.Invoke(() => studio.UpdateStatus("Rendering scene data..."));
        AddNodesGeometry(Cache.SceneGraph.NVNodes);
        AddNodesGeometry(Cache.SceneGraph.CLNodes);
        AddNodesGeometry(Cache.SceneGraph.MPNodes);
        Indices = Utils3D.GetIndices(Vertices);
        if (IsInitialized) RefreshPrimitiveBuffers();
        else IsInitialized = true;
        studio.viewerOpenMapLabel.Invoke(() => studio.viewerOpenMapLabel.Visible = false);
        studio.Invoke(studio.ResetStatus);
    }
}