using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Samurai
{
    public class Cursor : MonoBehaviour
    {
        [SerializeField]
        private Image _cursor;

        // Offset to adjust the position of the image relative to the cursor
        [SerializeField]
        public Vector2 offset = new Vector2(0f, 0f);
        private RectTransform canvasRect;

        private void Start()
        {
            // Get the RectTransform of the Canvas
            canvasRect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            // Get the current mouse position in screen coordinates
            Vector3 cursorPosition = Mouse.current.position.ReadValue();

            // Convert the screen coordinates to canvas coordinates
            Vector2 canvasPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, cursorPosition, null, out canvasPosition);

            // Apply offset
            canvasPosition += offset;

            // Set the position of the image to the cursor position
            _cursor.transform.localPosition = canvasPosition;
        }
    }
}