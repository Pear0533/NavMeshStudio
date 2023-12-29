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
using Vector2 = Microsoft.Xna.Framework.Vector2;

// ReSharper disable CollectionNeverUpdated.Local

namespace NavMeshStudio;

public class Viewer : Game
{
    public readonly List<GeoElement> Facesets = new();
    public readonly List<GeoElement> Vertices = new();
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
    public bool IsFocused;
    public bool IsInitialized;
    private MouseState PreviousMouseState;
    private SpriteBatch SpriteBatch = null!;
    private VertexBuffer VertexBuffer = null!;
    private Rectangle ViewerBGArea;
    private Texture2D ViewerBGTexture = null!;
    private PictureBox ViewerPictureBox = null!;
    public Form ViewerWindow = null!;
    private readonly NavMeshStudio Studio = null!;

    public Viewer() { }

    public Viewer(NavMeshStudio studio)
    {
        Studio = studio;
        ConfigureViewerSettings();
        InitializeGraphicsManager();
        RegisterViewerEvents();
    }

    private void InitializeGraphicsManager()
    {
        GraphicsManager = new GraphicsDeviceManager(this);
    }

    private void RegisterViewerEvents()
    {
        GraphicsManager.PreparingDeviceSettings += (_, e) =>
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = DrawSurface;
            IsFocused = Studio.viewer.Invoke(() => Utils.IsMouseOverControl(Studio.viewer));
        };
        Studio.viewer.MouseEnter += (_, _) => IsFocused = true;
        Studio.viewer.MouseLeave += (_, _) => IsFocused = false;
        ViewerPictureBox = Studio.viewer;
        ViewerWindow = (Control.FromHandle(Window.Handle) as Form)!;
        ViewerWindow.Opacity = 0;
    }

    private void ConfigureViewerSettings()
    {
        Window.Title = "NavMesh Viewer";
        Window.AllowUserResizing = true;
        IsMouseVisible = true;
        Content.RootDirectory = "Content";
        Studio.viewer.Invoke(() => DrawSurface = Studio.viewer.Handle);
    }

    private void InitializeBasicEffect()
    {
        BasicEffect = new BasicEffect(GraphicsDevice);
        BasicEffect.VertexColorEnabled = true;
    }

    public void RefreshPrimitiveBuffers()
    {
        VertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, Vertices.Count, BufferUsage.None);
        VertexBuffer.SetData(Vertices.Select(i => i.Data).ToArray());
        IndexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, Indices.Count, BufferUsage.None);
        IndexBuffer.SetData(Indices.ToArray());
        if (Facesets.Count <= 0) return;
        FacesetBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, Facesets.Count, BufferUsage.None);
        FacesetBuffer.SetData(Facesets.Select(i => i.Data).ToArray());
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

    private Ray CreateRayFromMousePosition()
    {
        System.Drawing.Point mousePositionPoint = ViewerPictureBox.Invoke(() => ViewerPictureBox.PointToClient(Cursor.Position));
        Vector2 mousePosition = new(mousePositionPoint.X, mousePositionPoint.Y);
        Microsoft.Xna.Framework.Vector3 nearPoint =
            GraphicsDevice.Viewport.Unproject(new Microsoft.Xna.Framework.Vector3(mousePosition, 0), BasicEffect.Projection, BasicEffect.View, Matrix.Identity);
        Microsoft.Xna.Framework.Vector3 farPoint =
            GraphicsDevice.Viewport.Unproject(new Microsoft.Xna.Framework.Vector3(mousePosition, 1), BasicEffect.Projection, BasicEffect.View, Matrix.Identity);
        Microsoft.Xna.Framework.Vector3 direction = farPoint - nearPoint;
        direction.Normalize();
        Ray ray = new(nearPoint, direction);
        return ray;
    }

    private bool IsSingleLeftMouseButtonClick()
    {
        return PreviousMouseState.LeftButton == ButtonState.Released && CurrentMouseState.LeftButton == ButtonState.Pressed;
    }

    // TODO: Improve accuracy of raycasting

    private void UpdateLeftMouseButtonClick()
    {
        if (!IsSingleLeftMouseButtonClick()) return;
        List<CLNode> clNodes = Cache.SceneGraph.CLNodes.ToArray().Reverse().ToList();
        List<NVNode> nvNodes = Cache.SceneGraph.NVNodes.ToArray().Reverse().ToList();
        List<GeoNode> nodes = clNodes.Concat<GeoNode>(nvNodes).ToList();
        Ray ray = CreateRayFromMousePosition();
        List<(GeoNode node, float distance)> intersectedNodes = new();
        foreach (GeoNode node in nodes)
        {
            List<Microsoft.Xna.Framework.Vector3> vertices = node.GetVertexPositions();
            for (int i = 0; i < vertices.Count; i += 6)
            {
                List<Microsoft.Xna.Framework.Vector3> group = vertices.GetRange(i, 6);
                List<Microsoft.Xna.Framework.Vector3> tri = group.Distinct().ToList();
                if (tri.Count < 3) continue;
                if (!Utils3D.RayIntersectsTriangle(ray, tri, out Microsoft.Xna.Framework.Vector3 intersection)) continue;
                float distance = Microsoft.Xna.Framework.Vector3.Distance(ray.Position, intersection);
                intersectedNodes.Add((node, distance));
            }
        }
        if (intersectedNodes.Count > 0)
        {
            intersectedNodes.Sort((a, b) => a.distance.CompareTo(b.distance));
            GeoNode closestIntersectedNode = intersectedNodes[0].node;
            Cache.SceneGraph.Select(Studio, closestIntersectedNode);
        }
        else Cache.SceneGraph.DeselectAll();
    }

    private void UpdateRightMouseButtonClick()
    {
        Camera = Camera.RotatePoint(0, 0, -(CurrentMouseState.Position.X - PreviousMouseState.Position.X) * 0.01f);
        Vector3 direction = new(Camera.Y, -Camera.X, 0);
        float theta = (CurrentMouseState.Position.Y - PreviousMouseState.Position.Y) * 0.01f;
        Camera = Utils3D.RotateLine(Camera, new Vector3(0, 0, 0), direction, theta);
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

    private void UpdateMoveCameraUpDown(GameTime gameTime, bool up, int movementSpeed)
    {
        Vector3 upVector = new(0, 0, 1);
        CameraOffset -= movementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds * upVector * (up ? 1 : -1);
    }

    private void UpdateKeyboardInput(GameTime gameTime)
    {
        if (!Utils.IsMainWindowFocused()) return;
        CurrentKeyboardState = Keyboard.GetState();
        Key[] pressedKeys = CurrentKeyboardState.GetPressedKeys();
        bool shift = CurrentKeyboardState.IsKeyDown(Key.LeftShift) || CurrentKeyboardState.IsKeyDown(Key.RightShift);
        int movementSpeed = shift ? 200 : 50;
        foreach (Key key in pressedKeys)
        {
            switch (key)
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
                case Key.Q:
                    UpdateMoveCameraUpDown(gameTime, true, movementSpeed);
                    break;
                case Key.E:
                    UpdateMoveCameraUpDown(gameTime, false, movementSpeed);
                    break;
            }
        }
    }

    private void UpdatePreviousMouseState()
    {
        PreviousMouseState = CurrentMouseState;
    }

    // TODO: Suppress the Windows modal sound when pressing the Q or E keys

    private void UpdateMouseInput()
    {
        CurrentMouseState = Mouse.GetState();
        if (CurrentMouseState.LeftButton == ButtonState.Pressed)
        {
            UpdateLeftMouseButtonClick();
        }
        else if (CurrentMouseState.RightButton == ButtonState.Pressed)
        {
            UpdateRightMouseButtonClick();
        }
        UpdateMouseScrollWheel();
        UpdatePreviousMouseState();
    }

    protected override void Update(GameTime gameTime)
    {
        if (!IsFocused) return;
        UpdateKeyboardInput(gameTime);
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
        BasicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, viewerAspectRatio, 0.1f, 1000);
        BasicEffect.TextureEnabled = false;
        BasicEffect.CurrentTechnique.Passes.ToList().ForEach(i => i.Apply());
    }

    private void AddNodesGeometry<T>(List<T> nodes) where T : GeoNode
    {
        nodes.ForEach(i =>
        {
            Vertices.AddRange(i.DispVertices);
            Facesets.AddRange(i.DispFacesets);
        });
    }

    public void RefreshGeometry(bool hardRefresh = false)
    {
        if (hardRefresh)
        {
            Vertices.Clear();
            Facesets.Clear();
            Studio.Invoke(() => Studio.UpdateStatus("Rendering scene data..."));
            AddNodesGeometry(Cache.SceneGraph.NVNodes);
            AddNodesGeometry(Cache.SceneGraph.CLNodes);
            AddNodesGeometry(Cache.SceneGraph.MPNodes);
            Indices = Utils3D.GetIndices(Vertices);
            if (IsInitialized) RefreshPrimitiveBuffers();
            else IsInitialized = true;
            Studio.viewerOpenMapLabel.Invoke(() => Studio.viewerOpenMapLabel.Visible = false);
            Studio.Invoke(Studio.ResetStatus);
            Studio.Invoke(() => Studio.ToggleOpenFileMenuOption(true));
            Studio.Invoke(() => Studio.ToggleSaveAsFileMenuOption(true));
        }
        else
        {
            // TODO: Improve performance
            FacesetBuffer.SetData(Facesets.Select(i => i.Data).ToArray());
        }
    }
}