using eden.Game.Gameplay.Characters.Friends;
using eden.Game.Gameplay.Items.Weapons;

namespace eden.Game.Gameplay.Characters
{
    /// <summary>
    /// A subset of Mages.
    /// </summary>
    public abstract class Witch : Mage
    {
        public abstract Weapon Weapon { get; }
    }
}
