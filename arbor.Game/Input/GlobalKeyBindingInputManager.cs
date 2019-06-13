using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Framework.Platform;

namespace arbor.Game.Input
{
    public class GlobalKeyBindingInputManager : KeyBindingContainer<ArborKeyBindings>, IHandleGlobalInput
    {
        [Resolved]
        private Storage storage { get; set; }

        [Resolved]
        private ArborBaseGame baseGame { get; set; }

        private const string keybinding_filename = "keybindings.txt";

        private static readonly Dictionary<ArborKeyBindings, InputKey> default_key_bindings = new Dictionary<ArborKeyBindings, InputKey>
        {
            { ArborKeyBindings.Console, InputKey.F3 }
        };

        public GlobalKeyBindingInputManager()
            : base(SimultaneousBindingMode.All)
        {
        }

        public override IEnumerable<KeyBinding> DefaultKeyBindings
        {
            get
            {
                var bindings = new Dictionary<ArborKeyBindings, InputKey>(default_key_bindings);

                using (var reader = new StreamReader(storage.GetStream(keybinding_filename)))
                {
                    string[] binding;
                    while ((binding = reader.ReadLine()?.Split('=')) != null)
                    {
                        if (Enum.TryParse(binding[0], out ArborKeyBindings keyBindings) && Enum.TryParse(binding[1], out InputKey inputKey))
                            bindings[keyBindings] = inputKey;
                    }
                }

                return bindings.Select(pair => new KeyBinding(pair.Value, pair.Key));
            }
        }

        protected override IEnumerable<Drawable> KeyBindingInputQueue => base.KeyBindingInputQueue.Prepend(baseGame);

        public void SaveKeyBindings()
        {
            using (var reader = new StreamWriter(storage.GetStream(keybinding_filename, FileAccess.Write)))
                foreach (var binding in KeyBindings)
                    reader.WriteLine(binding.Action + "=" + binding.KeyCombination.Keys.First());
        }
    }

    public enum ArborKeyBindings
    {
        Console
    }
}
