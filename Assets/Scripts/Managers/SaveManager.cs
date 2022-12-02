using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using grimhawk.core;

namespace grimhawk.managers
{
    public class SaveManager : SingletonBehavior<SaveManager>
    {
        private PersistantData<int> _level;

        public int Level { 
            get => _level = new PersistantData<int>("_level", 0);
            set => _level.value = value;
        }

        public void IncrementLevel() => Level++;

    }
}


