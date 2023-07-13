using HKLib.hk2018;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector3 = System.Numerics.Vector3;
using Vector4 = System.Numerics.Vector4;

// ReSharper disable CollectionNeverUpdated.Local

namespace NavMeshStudio;

public class StudioViewer : Game
{
    private static GraphicsDeviceManager? GraphicsManager;
    public readonly List<VertexPositionColor> Facesets = new();
    private readonly VertexPositionTexture[] GroundPlane = new VertexPositionTexture[6];
    public readonly List<VertexPositionColor> Vertices = new();
    private readonly string ViewerBGFilePath = $"{Utils.ResourcesPath}\\bg.png";
    private BasicEffect? BasicEffect;
    private Vector3 Camera = new(0, 4, 2);
    private Vector3 CameraOffset = new(0, 0, 0);
    private MouseState CurrentMouseState;
    private IntPtr DrawSurface;
    private MouseState PreviousMouseState;
    private SpriteBatch? SpriteBatch;
    private Rectangle ViewerBGArea;
    private Texture2D? ViewerBGTexture;

    public StudioViewer(NavMeshStudio studio)
    {
        ConfigureViewerSettings(studio);
        InitializeGraphicsManager();
        RegisterViewerEvents();
    }

    [DllImport("NavGen.dll")]
    public static extern int GetMeshVertCount();

    [DllImport("NavGen.dll")]
    public static extern int GetMeshTriCount();

    [DllImport("NavGen.dll")]
    public static extern void GetMeshVerts([In] [Out] ushort[] buffer);

    [DllImport("NavGen.dll")]
    public static extern void GetMeshTris([In] [Out] ushort[] buffer);

    private void InitializeGraphicsManager()
    {
        GraphicsManager = new GraphicsDeviceManager(this);
    }

    private void RegisterViewerEvents()
    {
        if (GraphicsManager != null)
        {
            GraphicsManager.PreparingDeviceSettings += (_, e) =>
                e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = DrawSurface;
            if (Control.FromHandle(Window.Handle) is Form viewerDialog) viewerDialog.Opacity = 0;
        }
    }

    private void ConfigureViewerSettings(NavMeshStudio studio)
    {
        Window.Title = "NavMesh Viewer";
        Window.AllowUserResizing = true;
        IsMouseVisible = true;
        Content.RootDirectory = "Content";
        studio.viewer.Invoke(() => DrawSurface = studio.viewer.Handle);
    }

    private void AddVertices(IReadOnlyList<Vector4> vertices)
    {
        Color vertexColor = Color.Pink;
        VertexPositionColor firstVertex = new(vertices[0].ToVector3(), vertexColor);
        VertexPositionColor secondVertex = new(vertices[1].ToVector3(), vertexColor);
        VertexPositionColor thirdVertex = new(vertices[2].ToVector3(), vertexColor);
        Vertices.AddRange(new[] { firstVertex, secondVertex, firstVertex, thirdVertex, secondVertex, thirdVertex });
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
        BasicEffect!.TextureEnabled = false;
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

    protected override void Initialize()
    {
        InitializeGroundPlane();
        InitializeBasicEffect();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        ViewerBGTexture = Texture2D.FromFile(GraphicsDevice, ViewerBGFilePath);
    }

    private void UpdateLeftMouseButtonClick()
    {
        Camera = Utils3D.RotatePoint(Camera, 0, 0, -(CurrentMouseState.Position.X - PreviousMouseState.Position.X) * 0.01f);
        Vector3 direction = new(Camera.Y, -Camera.X, 0);
        float theta = (CurrentMouseState.Position.Y - PreviousMouseState.Position.Y) * 0.01f;
        Camera = Utils3D.RotateLine(Camera, new Vector3(0, 0, 0), direction, theta);
    }

    private void UpdateMiddleMouseButtonClick()
    {
        Vector3 upVector = new(0, 0, 1);
        Vector3 rightVector = Utils3D.Normalize(Utils3D.CrossProduct(upVector, Camera));
        Microsoft.Xna.Framework.Vector3 cameraUpVector = Utils3D.Normalize(Utils3D.CrossProduct(Camera, rightVector));
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
        if (Vertices.Count <= 0 && Facesets.Count <= 0) return;
        GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, Vertices.ToArray(), 0, Vertices.Count / 2);
        // GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Facesets.ToArray(), 0, Facesets.Count / 3);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        SpriteBatch?.Begin();
        ViewerBGArea = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        SpriteBatch?.Draw(ViewerBGTexture, ViewerBGArea, Color.White);
        SpriteBatch?.End();
        SpriteBatch?.Begin();
        DrawGroundPlane();
        DrawGeometry();
        SpriteBatch?.End();
        base.Draw(gameTime);
    }

    private void DrawGroundPlane()
    {
        Vector3 cameraPosition = new(Camera.X + CameraOffset.X, Camera.Y + CameraOffset.Y, Camera.Z + CameraOffset.Z);
        DepthStencilState depthStencilState = new();
        depthStencilState.DepthBufferEnable = true;
        depthStencilState.DepthBufferFunction = CompareFunction.LessEqual;
        GraphicsDevice.DepthStencilState = depthStencilState;
        BasicEffect!.View = Matrix.CreateLookAt(cameraPosition, CameraOffset, Vector3.UnitZ);
        BasicEffect.VertexColorEnabled = true;
        float viewerAspectRatio = GraphicsManager!.PreferredBackBufferWidth / (float)GraphicsManager.PreferredBackBufferHeight;
        BasicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, viewerAspectRatio, 0.1f, 200);
        DrawGroundPlaneLines();
    }

    public void ConfigureGeometry()
    {
        foreach (hkaiNavMesh? navMesh in Cache.NavMeshes.Where(i => i?.m_vertices != null))
        {
            List<List<Vector4>>? vertexGroups = navMesh?.m_vertices.ChunkBy(3);
            if (vertexGroups == null) continue;
            // TODO: Account for the W component in the vertex groups
            foreach (List<Vector4> group in vertexGroups.Where(i => i.Count == 3)) AddVertices(group);
        }
    }
}