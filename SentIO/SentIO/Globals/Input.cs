using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SentIO.Globals
{
    public static class Input
    {
        static KeyboardState keyboardState;
        static KeyboardState lastKeyboardState;

        static KeyboardState defaultState = new KeyboardState();

        public static bool IsAnyKeyPressed()
        {
            return keyboardState != defaultState;
        }

        public static void Update(GameTime gt)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }
    }
}
