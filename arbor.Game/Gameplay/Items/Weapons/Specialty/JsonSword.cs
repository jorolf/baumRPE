using arbor.Game.Gameplay.Items.Weapons.BaseWeapons;
using Newtonsoft.Json;

namespace arbor.Game.Gameplay.Items.Weapons.Specialty
{
    [JsonObject]
    public class JsonSword : Sword
    {
        //private JsonStore jsonStore;

        public JsonSword(Character character)
            : base(character)
        {
        }

        public override float Damage { get; }
    }
}
