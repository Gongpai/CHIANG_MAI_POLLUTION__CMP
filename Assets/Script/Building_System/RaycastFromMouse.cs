using UnityEngine;

namespace GDD
{
    public static class RaycastFromMouse
    {
        private static Camera m_camera;

        public static Camera camera
        {
            get => m_camera;
            set => m_camera = value;
        }
        public static Ray GetRayFromMouse()
        {
            m_camera = Camera.main;
            Vector2 mousePosition = Input.mousePosition;
            Ray ray = m_camera.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, m_camera.nearClipPlane));

            return ray;
        }
    }
}