using UnityEngine;
using UnityEngine.InputSystem;

namespace NeanderthalTools.Knapping
{
    [RequireComponent(typeof(Camera))]
    public class KnappingRaycaster : MonoBehaviour
    {
        private new Camera camera;
        private Mouse mouse;

        private void Awake()
        {
            camera = GetComponent<Camera>();
            mouse = Mouse.current;
        }

        private void Update()
        {
            if (!IsMouseButtonPressed())
            {
                return;
            }

            if (!Raycast(out var obj))
            {
                return;
            }

            var knappingStone = obj.GetComponentInParent<KnappingStone>();
            if (knappingStone == null)
            {
                return;
            }

            Knapp(knappingStone, obj);
        }

        private bool IsMouseButtonPressed()
        {
            return mouse.leftButton.wasPressedThisFrame;
        }

        private bool Raycast(out GameObject obj)
        {
            var mousePosition = mouse.position.ReadValue();
            var ray = camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                Debug.DrawLine(transform.position, hit.transform.position, Color.green, 2f);
                obj = hit.collider.gameObject;
                return true;
            }

            obj = null;

            return false;
        }

        private void Knapp(KnappingStone knappingStone, GameObject knappingStonePart)
        {
            knappingStone.Knapp(knappingStonePart);
        }
    }
}
