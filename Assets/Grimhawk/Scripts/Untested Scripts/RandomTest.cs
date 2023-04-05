using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using grimhawk.core;
using UnityEngine.UI;

public class RandomTest : MonoBehaviour
{
    PersistantData<Color> _color;
    public Color color
    {
        private set
        {
            if (_color == null)
            {
                _color = new PersistantData<Color>("_color", Color.white);
            }
            _color.value = value;

        }

        get
        {
            if (_color == null)
            {
                _color = new PersistantData<Color>("_color", Color.white);
            }
            return _color;
        }
    }

    private void Start()
    {
        Debug.Log(color);

    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            Debug.Log(color);
        }
            
    }
}
