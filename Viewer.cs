using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector3 = System.Numerics.Vector3;

// ReSharper disable CollectionNeverUpdated.Local

namespace NavMeshStudio;

public class Viewer : Game
{
    public readonly List<VertexPositionColor> Facesets = new();
    private readonly VertexPositionTexture[] GroundPlane = new VertexPositionTexture[6];
    public readonly List<VertexPositionColor> Vertices = new();
    private readonly string ViewerBGFilePath = $"{Utils.ResourcesPath}\\bg.png";
    private BasicEffect BasicEffect = null!;
    private Vector3 Camera = new(0, 4, 2);
    private Vector3 CameraOffset = new(0, 0, 0);
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
        if (Control.FromHandle(Window.Handle) is Form viewerDialog) viewerDialog.Opacity = 0;
    }

    private void ConfigureViewerSettings(NavMeshStudio studio)
    {
        Window.Title = "NavMesh Viewer";
        Window.AllowUserResizing = true;
        IsMouseVisible = true;
        IsInitialized = true;
        Content.RootDirectory = "Content";
        studio.viewer.Invoke(() => DrawSurface = studio.viewer.Handle);
    }

    private static VertexPositionColor[] GetGroundPlaneLines()
    {
        List<VertexPositionColor> groundPlaneLines = new();
        groundPlaneLines.AddRange(new[]
        {
            new VertexPositionColor(new Vector3(-1000, 0, 0), Color.Red),
            new VertexPositionColor(new Vector3(1000, 0, 0), Color.Red),
            new VertexPositionColor(new Vector3(0, -1000, 0), Color.Blue),
            new VertexPositionColor(new Vector3(0, 1000, 0), Color.Blue),
            new VertexPositionColor(new Vector3(0, 0, 0), Color.Yellow),
            new VertexPositionColor(new Vector3(0, 0, 1000), Color.Yellow)
        });
        return groundPlaneLines.ToArray();
    }

    private void DrawGroundPlaneLines()
    {
        VertexPositionColor[] groundPlaneLines = GetGroundPlaneLines();
        BasicEffect.TextureEnabled = false;
        BasicEffect.CurrentTechnique.Passes.ToList().ForEach(i =>
        {
            i.Apply();
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, groundPlaneLines, 0, 3);
        });
    }

    private void AddGroundPlaneVertices(IReadOnlyList<Microsoft.Xna.Framework.Vector3> vertices)
    {
        for (int i = 0; i < vertices.Count; ++i)
            GroundPlane[i].Position = vertices[i];
    }

    private void InitializeGroundPlane()
    {
        AddGroundPlaneVertices(new[]
        {
            new Microsoft.Xna.Framework.Vector3(-20, -20, 0),
            new Microsoft.Xna.Framework.Vector3(-20, 20, 0),
            new Microsoft.Xna.Framework.Vector3(20, -20, 0),
            new Microsoft.Xna.Framework.Vector3(-20, 20, 0),
            new Microsoft.Xna.Framework.Vector3(20, 20, 0),
            new Microsoft.Xna.Framework.Vector3(20, -20, 0)
        });
    }

    private void InitializeBasicEffect()
    {
        BasicEffect = new BasicEffect(GraphicsDevice);
        BasicEffect.VertexColorEnabled = true;
    }

    private void InitializePrimitiveBuffers()
    {
        VertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, Vertices.Count, BufferUsage.None);
        VertexBuffer.SetData(Vertices.ToArray());
        FacesetBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, Facesets.Count, BufferUsage.None);
        FacesetBuffer.SetData(Facesets.ToArray());
        IndexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, Indices.Count, BufferUsage.None);
        IndexBuffer.SetData(Indices.ToArray());
    }

    protected override void Initialize()
    {
        InitializeGroundPlane();
        InitializeBasicEffect();
        InitializePrimitiveBuffers();
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

    private void UpdatePreviousMousePosition()
    {
        PreviousMouseState = CurrentMouseState;
    }

    protected override void Update(GameTime gameTime)
    {
        if (!IsFocused) return;
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
        UpdatePreviousMousePosition();
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
        if (Facesets.Count <= 0) return;
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
        DrawGroundPlane();
        DrawGeometry();
        SpriteBatch.End();
        base.Draw(gameTime);
    }

    private void DrawGroundPlane()
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
        DrawGroundPlaneLines();
    }

    public void BuildGeometry()
    {
        Vertices.Clear();
        // TODO: Create a method for this...
        Cache.SceneGraph.NVNodes.ForEach(i =>
        {
            Vertices.AddRange(i.Vertices);
            Facesets.AddRange(i.Facesets);
        });
        Cache.SceneGraph.MPNodes.ForEach(i =>
        {
            Vertices.AddRange(i.Vertices);
            Facesets.AddRange(i.Facesets);
        });
        Indices = Utils3D.GetIndices(Vertices);
    }
}