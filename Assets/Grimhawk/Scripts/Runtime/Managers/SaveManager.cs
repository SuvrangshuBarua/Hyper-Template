using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using grimhawk.core;

namespace grimhawk.managers
{
    public class SaveManager : GameBehavior
    {
        private PersistantData<int> _level;

        public int Level {
            private set
            {
                if (_level == null)
                {
                    _level = new PersistantData<int>("_level", 0);
                }
                _level.value = value;

            }

            get
            {
                if (_level == null)
                {
                    _level = new PersistantData<int>("_level", 0);
                }
                return _level;
            }
        }

        public void IncrementLevel() => Level++;

    }
}


