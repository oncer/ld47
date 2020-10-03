using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SentIO.Routines
{
    public interface ICoroutineYield
    {
        void Execute();        
        bool IsDone();
    }

    public class TextInstruction : ICoroutineYield
    {
        string text;
        int count;
        int delay;
        public TextInstruction(string text)
        {
            this.text = text;
        }
        public void Execute()
        {
            if (delay == 0)
            {
                Debug.Write(text[count]);
                count++;
                delay = 60;

                if (IsDone())
                {
                    Debug.Write(Environment.NewLine);
                }
            }
            delay--;
        }

        public bool IsDone()
        {
            return count >= text.Length;
        }
    }

    public class StringResult
    {
        public string Result { get; set; }
    }

    public class QuestionInstruction : ICoroutineYield
    {
        string text;
        int count;
        int delay;

        StringResult result;
        public QuestionInstruction(string text, StringResult result)
        {
            this.text = text;
            this.result = result;
        }
        public void Execute()
        {
            if (delay == 0)
            {
                Debug.Write(text[count]);
                count++;
                delay = 60;

                if (IsDone())
                {
                    Debug.Write(Environment.NewLine);
                }
            }
            delay--;
        }

        public bool IsDone()
        {
            return count >= text.Length;
        }
    }

    public class WaitInstruction : ICoroutineYield
    {
        int count;
        public WaitInstruction(int num)
        {
            count = num;
        }
        public void Execute()
        {
            count--;
        }

        public bool IsDone()
        {
            return count <= 0;
        }
    }

    public class Coroutine
    {
        private IEnumerator enumerator;
        private ICoroutineYield instruction;
        private bool enumeratorActive;

        public Coroutine(IEnumerator enumerator)
        {
            this.enumerator = enumerator;
            enumeratorActive = this.enumerator.MoveNext();
            instruction = this.enumerator.Current as ICoroutineYield;
        }

        public bool Advance()
        {
            if (instruction != null)
            {
                instruction.Execute();
                if (instruction.IsDone())
                {
                    instruction = null;
                }
            }

            if (instruction == null)
            {
                if (enumeratorActive)
                {
                    enumeratorActive = enumerator.MoveNext();
                    instruction = enumerator.Current as ICoroutineYield;
                }
                else
                {
                    return enumeratorActive;
                }
            }

            return true;
        }
    }
}
