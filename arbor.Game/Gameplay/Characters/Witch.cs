using arbor.Game.Gameplay.Characters.Friends;
using arbor.Game.Gameplay.Items.Weapons;

namespace arbor.Game.Gameplay.Characters
{
    /// <summary>
    /// A subset of Mages.
    /// </summary>
    public abstract class Witch : Mage
    {
        public abstract Weapon Weapon { get; }
    }
}
