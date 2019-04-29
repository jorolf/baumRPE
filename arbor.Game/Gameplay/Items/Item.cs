using osu.Framework.Graphics.Textures;

namespace arbor.Game.Gameplay.Items
{
    public class Item : GameObject
    {
        //Maybe this should be public for easier set?
        //TODO: allow this to be animated for special items of high rarity or something. "SymcolSprite"?
        public Texture ItemTexture;

        /// <summary>
        /// Max amount of this item that can be stacked in inventory
        /// </summary>
        public virtual int MaxStackSize => 100;

        /// <summary>
        /// How many of us in an inventory
        /// </summary>
        public int StackSize = 1;

        /// <summary>
        /// the inventory this item is in if in one
        /// </summary>
        public Inventory CurrentInventory;

        /// <summary>
        /// Item specific id. (ex: arbor.weapon.seasonal_shift.shaft)
        /// </summary>
        //TODO: set this automatically
        public virtual string ItemID => "arbor.base.null";

        protected readonly ItemType ItemType;

        public Item(ItemType itemType)
        {
            ItemType = itemType;
        }

        /// <summary>
        /// removes this item from it's inventory and places it in the world
        /// </summary>
        public void Drop()
        {
            if (StackSize == 1)
            {
                StackSize--;
                CurrentInventory.Remove(this);
                CurrentInventory = null;
            }
            else
                StackSize--;

            //TODO: Add to world here
        }

        /// <summary>
        /// adds this item to an inventory and removes it from the world if possible
        /// </summary>
        /// <param name="inventory"></param>
        public bool Pickup(Inventory inventory)
        {
            foreach (Item item in inventory.Items)
                if (item.ItemID == ItemID)
                    if (item.StackSize < item.MaxStackSize)
                    {
                        item.StackSize++;
                        StackSize = item.StackSize;
                        CurrentInventory = inventory;
                        //World.Remove(this);
                        return true;
                    }
                    else
                        return false;

            inventory.Add(this);
            //TODO: dont repeat so much logic
            CurrentInventory = inventory;
            //World.Remove(this);
            StackSize = 1;
            return true;
        }
    }

    public enum ItemType
    {
        Weapon,

        //Currently will be unused until weapon system re-write
        WeaponPart,

        //Heal
        Food,

        //used for dank ass spells
        Potions,
    }
}
