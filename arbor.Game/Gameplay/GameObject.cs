using System;
using arbor.Game.Worlds;
using Newtonsoft.Json;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace arbor.Game.Gameplay
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GameObject : Container
    {
        /// <summary>
        /// whether we want to do hit detection
        /// </summary>
        [JsonProperty("solid")]
        public bool Solid { get; set; }

        [JsonProperty("position")]
        private Vector2 worldPositionJson
        {
            get => roundToPlace(WorldPosition);
            set => WorldPosition = value;
        }

        /// <summary>
        /// The position of this object based on the tile system
        /// </summary>
        public Vector2 WorldPosition
        {
            get => Position / Chunk.TILE_SIZE;
            set => Position = value * Chunk.TILE_SIZE;
        }

        [JsonProperty("size")]
        private Vector2 tileSizeJson
        {
            get => roundToPlace(TileSize);
            set => TileSize = value;
        }

        public Vector2 TileSize
        {
            get => Size / Chunk.TILE_SIZE;
            set => Size = value * Chunk.TILE_SIZE;
        }

        private static Vector2 roundToPlace(Vector2 vec, float place = 0.0001f)
        {
            var mult = vec / place;
            var rounded = new Vector2((float)Math.Round(mult.X), (float)Math.Round(mult.Y));
            return rounded * place;
        }

        public GameObject()
        {
            Child = new Circle
            {
                RelativeSizeAxes = Axes.Both
            };
        }

        /*
        /// <summary>
        /// the shape of this object (used for hit detection)
        /// </summary>
        public Shape Shape { get; set; }
        */

        /*
        public bool HitDetect(ArborGameplayObject object1, ArborGameplayObject object2)
        {
            if (object1.Solid && object2.Solid)
            {
                if (object1.Shape == Shape.Circle && object2.Shape == Shape.Circle)
                {
                    Vector2 object2Pos = object2.ToSpaceOfOtherDrawable(Vector2.Zero, object1);
                    float distance = (float) Math.Sqrt(Math.Pow(object2Pos.X, 2) + Math.Pow(object2Pos.Y, 2));
                    float minDist = object1.Width + object2.Width;
                    if (distance < minDist)
                        return true;
                }
                else if (object1.Shape == Shape.Circle && object2.Shape == Shape.Rectangle ||
                         object1.Shape == Shape.Rectangle && object2.Shape == Shape.Circle)
                {
                }
                else if (object1.Shape == Shape.Rectangle && object2.Shape == Shape.Rectangle)
                {
                }
            }

            return false;
        }
        */
    }

    /*
    public enum Shape
    {
        Circle,
        Rectangle
    }
    */
}
