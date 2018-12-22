using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Batches;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Timing;
using OpenTK;
using OpenTK.Graphics.ES11;
using PrimitiveType = OpenTK.Graphics.ES30.PrimitiveType;

namespace eden.Game.Worlds
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
        public readonly Vector2[,] TilePositions = new Vector2[Chunk.TILES_PER_CHUNK + 1, Chunk.TILES_PER_CHUNK + 1];
        private readonly DrawableTilesDrawNodeSharedData sharedData = new DrawableTilesDrawNodeSharedData();

        private Shader shader;

        protected override DrawNode CreateDrawNode() => new DrawableTilesDrawNode
        {
            TilePositions = TilePositions,
        };

        protected override void ApplyDrawNode(DrawNode node)
        {
            base.ApplyDrawNode(node);

            DrawableTilesDrawNode tilesNode = (DrawableTilesDrawNode) node;
            for (int i = 0; i < Tiles.GetLength(0) + 1; i++)
            for (int j = 0; j < Tiles.GetLength(1) + 1; j++)
            {
                if(i < Tiles.GetLength(0) && j < Tiles.GetLength(1))
                    tilesNode.Tiles[i, j] = Tiles[i, j] == null ? new BlankTile() : atlas[Tiles[i, j].Value];
                tilesNode.TilePositions[i, j] = mapQuad(ScreenSpaceDrawQuad, new Vector2(i * tile_size, j * tile_size));
            }

            tilesNode.Shader = shader;
            tilesNode.Clock = Clock;
            tilesNode.SharedData = sharedData;
        }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders)
        {
            shader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
        }

        private const float tile_size = 1f / Chunk.TILES_PER_CHUNK;

        private static Vector2 mapQuad(Quad quad, Vector2 vec)
        {
            var top = quad.TopLeft + (quad.TopRight - quad.TopLeft) * vec.X;
            var bottom = quad.BottomLeft + (quad.BottomRight - quad.BottomLeft) * vec.X;

            return top + (bottom - top) * vec.Y;
        }
    }

    public class DrawableTilesDrawNodeSharedData
    {
        public LinearBatch<TexturedVertex2D> TileBatch = new LinearBatch<TexturedVertex2D>(Chunk.TILES_PER_CHUNK * Chunk.TILES_PER_CHUNK, 10, PrimitiveType.Triangles);
    }

    public class DrawableTilesDrawNode : DrawNode
    {
        public readonly Tile[,] Tiles = new Tile[Chunk.TILES_PER_CHUNK, Chunk.TILES_PER_CHUNK];

        public Shader Shader;
        public IClock Clock;
        public Vector2[,] TilePositions;
        public DrawableTilesDrawNodeSharedData SharedData;

        public override void Draw(Action<TexturedVertex2D> vertexAction)
        {
            base.Draw(vertexAction);

            Shader.Bind();
            GL.Disable(EnableCap.AlphaTest);

            for (int i = 0; i < Tiles.GetLength(0); i++)
            for (int j = 0; j < Tiles.GetLength(1); j++)
            {
                Tiles[i, j].GetTexture(Clock.CurrentTime)?.DrawQuad(new Quad(TilePositions[i, j], TilePositions[i + 1, j], TilePositions[i, j + 1], TilePositions[i + 1, j + 1]), DrawColourInfo.Colour, vertexAction: vertexAction);
            }

            /*Texture.WhitePixel.TextureGL.Bind();

            for (int i = 0; i < Tiles.GetLength(0); i++)
            for (int j = 0; j < Tiles.GetLength(1); j++)
            {

                //Tiles[i, j].GetTexture(Clock.CurrentTime).TextureGL.WrapMode = TextureWrapMode.ClampToEdge;
                //Tiles[i, j].GetTexture(Clock.CurrentTime).TextureGL.Bind();

                SharedData.TileBatch.Add(new TexturedVertex2D
                {
                    Position = TilePositions[i, j],
                    TexturePosition = Vector2.Zero,
                });
                SharedData.TileBatch.Add(new TexturedVertex2D
                {
                    Position = TilePositions[i + 1, j],
                    TexturePosition = Vector2.UnitX
                });
                SharedData.TileBatch.Add(new TexturedVertex2D
                {
                    Position = TilePositions[i, j + 1],
                    TexturePosition = Vector2.UnitY
                });
                SharedData.TileBatch.Add(new TexturedVertex2D
                {
                    Position = TilePositions[i + 1, j + 1],
                    TexturePosition = Vector2.One
                });
                SharedData.TileBatch.Add(new TexturedVertex2D
                {
                    Position = TilePositions[i, j + 1],
                    TexturePosition = Vector2.UnitY
                });
                SharedData.TileBatch.Add(new TexturedVertex2D
                {
                    Position = TilePositions[i + 1, j],
                    TexturePosition = Vector2.UnitX
                });
            }*/

            GL.Enable(EnableCap.AlphaTest);
            Shader.Unbind();
        }
    }
}
