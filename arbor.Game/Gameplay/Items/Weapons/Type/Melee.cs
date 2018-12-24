namespace arbor.Game.Gameplay.Items.Weapons.Type
{
    public abstract class Melee : Weapon
    {
        protected Melee(Character character) : base(character)
        {
        }

        /*protected override void Update()
        {
            base.Update();

            List<Drawable> worldChildren = new List<Drawable>();

            foreach (Drawable draw in World.Children)
                worldChildren.Add(draw);

            foreach (Drawable draw in worldChildren)
                if (draw is Character)
                {
                    Character character = draw as Character;
                    if (character.Team != ParentCharacter.Team && Hitbox.HitDetect(Hitbox, character.Hitbox))
                    {
                        Hit(character);
                        character.Hit(Damage);
                        break;
                    }
                }
        }*/
    }
}
