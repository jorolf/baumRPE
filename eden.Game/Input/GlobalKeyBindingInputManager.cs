using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Framework.Platform;

namespace eden.Game.Input
{
    public class GlobalKeyBindingInputManager : KeyBindingContainer<EdenKeyBindings>, IHandleGlobalInput
    {
        private readonly Storage storage;
        private const string keybinding_filename = "keybindings.txt";

        public override IEnumerable<KeyBinding> DefaultKeyBindings
        {
            get
            {
                if (!storage.Exists(keybinding_filename))
                    return new[]
                    {
                        new KeyBinding(InputKey.Tilde, EdenKeyBindings.Console)
                    };

                using (var reader = new StreamReader(storage.GetStream(keybinding_filename)))
                {
                    var list = new List<KeyBinding>();
                    while (!reader.EndOfStream)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        var binding = reader.ReadLine().Split('=');
                        list.Add(new KeyBinding((InputKey) Enum.Parse(typeof(InputKey), binding[1]), Enum.Parse(typeof(EdenKeyBindings), binding[0])));
                    }

                    return list;
                }
            }
        }

        public void SaveKeyBindings()
        {
            using (var reader = new StreamWriter(storage.GetStream(keybinding_filename, FileAccess.Write)))
                foreach (var binding in KeyBindings)
                    reader.WriteLine(binding.Action + "=" + binding.KeyCombination.Keys.First());
        }

        public GlobalKeyBindingInputManager(Storage storage) : base(SimultaneousBindingMode.All)
        {
            this.storage = storage;
        }
    }

    public enum EdenKeyBindings
    {
        Console
    }
}
