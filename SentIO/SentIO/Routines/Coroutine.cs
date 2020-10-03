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
