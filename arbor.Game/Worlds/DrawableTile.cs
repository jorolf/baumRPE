using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace arbor.Game.Worlds
{
    public class DrawableTile : CompositeDrawable
    {
        private readonly Tile tile;
        private readonly Sprite tileSprite;

        public DrawableTile(Tile tile)
        {
            this.tile = tile;
            AddInternal(tileSprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both
            });
        }

        protected override void Update()
        {
            base.Update();
            tileSprite.Texture = tile.GetTexture(Time.Current);
        }
    }
}
