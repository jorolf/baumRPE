using arbor.Game.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace arbor.Game.Screens
{
    /// <summary>
    /// Seen on launch
    /// </summary>
    public class HomeScreen : ArborMenuScreen
    {
        private SettingsOverlay settings;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, ArborGame game, Story story)
        {
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Sprite
                        {
                            Depth = float.MaxValue,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Texture = textures.Get("Backgrounds/grass"),
                            Size = Vector2.One,
                            RelativeSizeAxes = Axes.Both,
                            FillMode = FillMode.Fill,
                        },
                        new Button
                        {
                            Position = new Vector2(10, -160),
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Text = "Load Test World",
                            CornerRadius = 8,
                            Size = new Vector2(200, 100),
                            Action = () => this.Push(new GameScreen()),
                            BackgroundColour = Color4.Gray,
                        },
                        settings = new SettingsOverlay
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            RelativeSizeAxes = Axes.Y,
                        },
                        new Button
                        {
                            Position = new Vector2(10, 160),
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Text = "Toggle Settings",
                            CornerRadius = 8,
                            Size = new Vector2(200, 100),
                            Action = settings.ToggleVisibility,
                            BackgroundColour = Color4.Gray,
                        },
                        new SpriteText
                        {
                            Colour = Color4.Wheat,
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Font = FontUsage.Default.With(size: 60),
                            Text = $"Welcome to the \"{story.Name}\" game!"
                        },
                    }
                }
            };

            if (game.StoryStorage != null)
                AddInternal(new Button
                {
                    Position = new Vector2(10, 0),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Text = "Load Editor",
                    CornerRadius = 8,
                    Size = new Vector2(200, 100),
                    Action = () => this.Push(new MapEditorScreen()),
                    BackgroundColour = Color4.Gray,
                });
        }
    }
}
