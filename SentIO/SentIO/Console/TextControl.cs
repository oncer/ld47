﻿using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SentIO.Globals;
using SentIO.Routines;
using SentIO.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SentIO.Console
{
    public class TextControl
    {
        private static char[] CONTROL_CHARS = new char[]{'<', '|'};
        private const char CURSOR_CHAR = '_';
        private const int CURSOR_BLINK_DELAY = 20;
        private const int AFTER_SHOW_WAIT_FRAMES = 30;

        private static TextControl instance;
        public static TextControl Instance
        {
            get
            {
                if (instance == null)
                    instance = new TextControl();
                return instance;
            }
        }

        public class Wait : ICoroutineYield
        {
            public Wait() { }        
            public void Execute() { }
            public bool IsDone() => Instance.IsDone;
        }

        public Color Background { get; set; } = Color.Black;
        public Color Foreground { get; set; } = Color.White;

        public bool IsDone => mode == Mode.Nothing;
        public Vector2 Position { get; set; } = Vector2.Zero;

        public enum Mode
        {
            Nothing,
            Output,
            Input,
            WaitForKeyPress,
            WaitForTime
        }
        private Mode mode;

        public enum Layout
        {
            TopLeft,
            Centered
        }
        private Layout layout;
        public string InputResult { get; private set; }


        private string[] lines = new string[2];
        private Vector2[] linePos = new Vector2[2];
        private string inputLine;
        private int cursorY;
        private string textOutput;
        private string textInput;
        private int index;
        private int nextCharDelay;
        private int blinkDelay;
        private int frameCountdown;

        private bool showBlink;
        
        public enum Speed
        {
            UltraSlow,
            VerySlow,
            Slow,
            Normal,
            Fast,
            VeryFast
        }
        public Speed CurrentSpeed { get; set; } = Speed.Normal;

        private TextControl()
        {
            mode = Mode.Nothing;
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = "";
            }
            textOutput = "";
            textInput = "";
            index = 0;
            nextCharDelay = 0;
            blinkDelay = 0;
            cursorY = -1;
            showBlink = false;
            frameCountdown = 0;
            SetLayout(Layout.TopLeft);
        }

        internal void Clear()
        {
            mode = Mode.Nothing;
            textOutput = "";            
        }

        public void SetLayout(Layout _layout)
        {
            layout = _layout;
        }

        public ICoroutineYield Show(string text)
        {
            mode = Mode.Output;
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = "";
            }
            inputLine = "";
            textOutput = text;
            index = 0;
            nextCharDelay = 0;
            blinkDelay = 0;
            cursorY = -1;
            showBlink = false;
            return new Wait();
        }

        public ICoroutineYield Input()
        {
            mode = Mode.Input;
            textInput = "";
            InputResult = "";
            cursorY = lines.Length;
            showBlink = false;
            return new Wait();
        }

        public ICoroutineYield WaitForKeyPress()
        {
            mode = Mode.WaitForKeyPress;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > 0) cursorY = i;
            }
            return new Wait();
        }

        public ICoroutineYield WaitForCountdown(int frames)
        {
            mode = Mode.WaitForTime;
            frameCountdown = frames;
            return new Wait();
        }

        private float StringWidth(string text)
        {
            string textWithoutControlChars = "";
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                switch (c)
                {
                    case '|': break;
                    case '<':
                        if (textWithoutControlChars.Length > 0)
                            textWithoutControlChars = 
                                textWithoutControlChars.Remove(textWithoutControlChars.Length - 1, 1);
                        break;
                    default: textWithoutControlChars += c; break;
                }
            }
            return Resources.ConsoleFont.MeasureString(textWithoutControlChars).X;
        }

        private void SetOutput(string textOutput, int index=-1)
        {
            string[] fullLines = textOutput.Split('\n');
            
            lines = new string[fullLines.Length];
            linePos = new Vector2[fullLines.Length + 1];
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = "";
            }

            if (index < 0) index = textOutput.Length;
            string textOutputPart = textOutput.Substring(0, index);
            string[] partLines = textOutputPart.Split('\n');
            for (int i = 0; i < partLines.Length; i++)
            {
                lines[i] = partLines[i];
            }

            if (layout == Layout.TopLeft)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    linePos[i].X = 0;
                    linePos[i].Y = Resources.ConsoleFont.LineSpacing * i;
                }
            }
            else if (layout == Layout.Centered)
            {
                int windowWidth = MainGame.Instance.Window.ClientBounds.Width;
                int windowHeight = MainGame.Instance.Window.ClientBounds.Height;
                for (int i = 0; i < fullLines.Length; i++)
                {
                    linePos[i].X = (int) ((windowWidth - StringWidth(fullLines[i])) / 2f);
                    linePos[i].Y = windowHeight / 2 - Resources.ConsoleFont.LineSpacing * (fullLines.Length - i + 2);
                }
            }
        }

        private void SetInput(string input)
        {
            inputLine = input;
            int windowWidth = MainGame.Instance.Window.ClientBounds.Width;
            int windowHeight = MainGame.Instance.Window.ClientBounds.Height;
            linePos[lines.Length].X = (int) ((windowWidth - StringWidth(inputLine)) / 2f);
            linePos[lines.Length].Y = windowHeight - Resources.ConsoleFont.LineSpacing * 2;
        }

        public void Update()
        {
            nextCharDelay = Math.Max(nextCharDelay - 1, 0);
            
            if (cursorY >= 0 && cursorY <= lines.Length)
            {
                blinkDelay = Math.Max(blinkDelay - 1, 0);
                if (blinkDelay == 0)
                {
                    showBlink = !showBlink;
                    blinkDelay = CURSOR_BLINK_DELAY;
                }
            }
            
            if (mode == Mode.Output)
            {
                if (nextCharDelay == 0)
                {
                    if (index < textOutput.Length)
                    {
                        if (index == textOutput.Length - 1)
                        {
                            SoundControl.Stop(Resources.SfxChar);
                            SoundControl.Play(Resources.SfxCharStop);
                        }
                        else if (textOutput[index] != ' '
                            && !CONTROL_CHARS.Contains(textOutput[index]))
                        {
                            SoundControl.Play(Resources.SfxChar);
                        }
                        switch (textOutput[index])
                        {
                            case '|':
                                textOutput = textOutput.Remove(index, 1);
                                break;
                            case '<':
                                textOutput = textOutput.Remove(index - 1, 2);
                                index--;
                                break;
                            default:
                                index++;
                                break;
                        }
                    }
                    switch (CurrentSpeed)
                    {
                        case Speed.UltraSlow:
                            nextCharDelay = 32;
                            break;
                        case Speed.VerySlow:
                            nextCharDelay = 16;
                            break;
                        case Speed.Slow:
                            nextCharDelay = 8;
                            break;
                        case Speed.Normal:
                            nextCharDelay = 4;
                            break;
                        case Speed.Fast:
                            nextCharDelay = 2;
                            break;
                        case Speed.VeryFast:
                            nextCharDelay = 1;
                            break;
                    }
                }
                SetOutput(textOutput, index);
                if (index >= textOutput.Length)
                {
                    mode = Mode.WaitForTime;
                    frameCountdown = AFTER_SHOW_WAIT_FRAMES;
                }
            }
            else if (mode == Mode.Input)
            {
                foreach (Keys key in Globals.Input.KeysPressedThisFrame())
                {
                    if (key >= Keys.A && key <= Keys.Z) {
                        if (Globals.Input.IsKeyCurrentlyPressed(Keys.LeftShift) || Globals.Input.IsKeyCurrentlyPressed(Keys.RightShift))
                        {
                            textInput += key.ToString();
                        }
                        else
                        {
                            textInput += key.ToString().ToLower();
                        }
                    }
                    else if (key >= Keys.D0 && key <= Keys.D9)
                    {
                        textInput += key.ToString()[1];
                    }
                    else if (key == Keys.Space && textInput.Length > 0)
                    {
                        textInput += " ";
                    }
                    else if (key == Keys.Back && textInput.Length > 0)
                    {
                        textInput = textInput.Remove(textInput.Length - 1);
                    }
                    else if (key == Keys.Enter && textInput.Length > 0)
                    {
                        mode = Mode.Nothing;
                        InputResult = textInput;
                        textInput = "";
                        cursorY = -1;
                    }
                }
                SetOutput(textOutput);
                SetInput(textInput);
            }
            else if (mode == Mode.WaitForKeyPress)
            {
                if (Globals.Input.WasAnyKeyPressedThisFrame())
                {
                    mode = Mode.Nothing;
                    cursorY = -1;
                }
                SetOutput(textOutput);
                SetInput(textInput);
            }
            else if (mode == Mode.WaitForTime)
            {
                frameCountdown--;
                if (frameCountdown <= 0)
                {
                    mode = Mode.Nothing;
                }
                SetOutput(textOutput);
                SetInput(textInput);
            }
            else if (mode == Mode.Nothing)
            {
                SetOutput(textOutput);
                SetInput(textInput);
            }

            if (showBlink)
            {
                if (cursorY >= 0 && cursorY < lines.Length)
                {
                    lines[cursorY] += CURSOR_CHAR;
                }
                else if (cursorY == lines.Length)
                {
                    inputLine += CURSOR_CHAR;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                spriteBatch.DrawString(Resources.ConsoleFont, lines[i], linePos[i],
                    Foreground, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
            }
            spriteBatch.DrawString(Resources.ConsoleFont, inputLine, linePos[lines.Length],
                Resources.TextColorInput, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
        }
    }
}
