using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NeanderthalTools.Knapping
{
    public class KeyboardRotator : MonoBehaviour
    {
        private Keyboard keyboard;
        private Vector2 axis;

        private void Awake()
        {
            keyboard = Keyboard.current;
        }

        private void Update()
        {
            if (keyboard.leftArrowKey.wasPressedThisFrame)
            {
                axis.x = -1;
            }
            else if (keyboard.rightArrowKey.wasPressedThisFrame)
            {
                axis.x = 1;
            }

            if (keyboard.upArrowKey.wasPressedThisFrame)
            {
                axis.y = 1;
            }
            else if (keyboard.downArrowKey.wasPressedThisFrame)
            {
                axis.y = -1;
            }
        }
    }
}
