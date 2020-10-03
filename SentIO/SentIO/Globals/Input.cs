using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public static bool WasAnyKeyPressedThisFrame()
        {
            Keys[] current = keyboardState.GetPressedKeys();
            Keys[] last = lastKeyboardState.GetPressedKeys();

            return current.Except(last).Any();
        }

        public static bool WasKeyPressedThisFrame(Keys key)
        {
            return keyboardState.IsKeyDown(key) && !lastKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyCurrentlyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public static Keys[] KeysPressedThisFrame()
        {
            Keys[] current = keyboardState.GetPressedKeys();
            Keys[] last = lastKeyboardState.GetPressedKeys();
           
            return current.Except(last).ToArray();
        }

        public static void Update(GameTime gt)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }
    }
}
