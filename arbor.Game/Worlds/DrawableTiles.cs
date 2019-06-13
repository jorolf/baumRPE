using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics.ES11;

namespace arbor.Game.Worlds
{
    public class DrawableTiles : Drawable
    {
        private TileAtlas atlas;

        public TileAtlas Atlas
        {
            get => atlas;
            set
            {
                atlas = value;
                Invalidate(Invalidation.DrawNode);
            }
        }

        public int?[,] Tiles
        {
            get
            {
                Invalidate(Invalidation.DrawNode);
                return tiles;
            }
            set => Array.Copy(value, tiles, tiles.Length);
        }

        private readonly int?[,] tiles = new int?[Chunk.TILES_PER_CHUNK, Chunk.TILES_PER_CHUNK];

        internal IShader Shader;

        protected override DrawNode CreateDrawNode() => new DrawableTilesDrawNode(this);

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders)
        {
            Shader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
        }
    }

    public class DrawableTilesDrawNode : DrawNode
    {
        private readonly Tile[,] tiles = new Tile[Chunk.TILES_PER_CHUNK, Chunk.TILES_PER_CHUNK];

        private IShader shader;
        private IClock clock;
        private readonly Vector2[,] tilePositions = new Vector2[Chunk.TILES_PER_CHUNK + 1, Chunk.TILES_PER_CHUNK + 1];

        protected new DrawableTiles Source => (DrawableTiles)base.Source;

        public DrawableTilesDrawNode(DrawableTiles source)
            : base(source)
        {
        }

        public override void ApplyState()
        {
            base.ApplyState();

            const float tile_size = 1f / Chunk.TILES_PER_CHUNK;

            for (int i = 0; i < tiles.GetLength(0) + 1; i++)
            for (int j = 0; j < tiles.GetLength(1) + 1; j++)
            {
                if (i < tiles.GetLength(0) && j < tiles.GetLength(1))
                    tiles[i, j] = Source.Tiles[i, j] == null ? new BlankTile() : Source.Atlas[Source.Tiles[i, j].Value];
                tilePositions[i, j] = mapQuad(base.Source.ScreenSpaceDrawQuad, new Vector2(i * tile_size, j * tile_size));
            }

            shader = Source.Shader;
            clock = base.Source.Clock;
        }

        public override void Draw(Action<TexturedVertex2D> vertexAction)
        {
            base.Draw(vertexAction);

            shader.Bind();
            GL.Disable(EnableCap.AlphaTest);

            for (int i = 0; i < tiles.GetLength(0); i++)
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                var texture = tiles[i, j].GetTexture(clock.CurrentTime);
                if (texture != null)
                    DrawQuad(texture, new Quad(tilePositions[i, j], tilePositions[i + 1, j], tilePositions[i, j + 1], tilePositions[i + 1, j + 1]), DrawColourInfo.Colour, vertexAction: vertexAction);
            }

            GL.Enable(EnableCap.AlphaTest);
            shader.Unbind();
        }

        private static Vector2 mapQuad(Quad quad, Vector2 vec)
        {
            var top = quad.TopLeft + (quad.TopRight - quad.TopLeft) * vec.X;
            var bottom = quad.BottomLeft + (quad.BottomRight - quad.BottomLeft) * vec.X;

            return top + (bottom - top) * vec.Y;
        }
    }
}
