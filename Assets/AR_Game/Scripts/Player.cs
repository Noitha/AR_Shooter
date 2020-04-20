using UnityEngine;

namespace AR_Game.Scripts
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        
        /// <summary>
        /// Camera component - reference for raycast.
        /// </summary>
        public Camera playerCamera;
        
        /// <summary>
        /// Ray information.
        /// </summary>
        private Ray _ray;
        
        /// <summary>
        /// Ray cast hit information.
        /// </summary>
        private RaycastHit _raycastHit;

        #region Unity Methods

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }
            
            Destroy(gameObject);
        }

        private void Update()
        {
            if (Input.touchCount == 0)
            {
                return;
            }

            var touch = Input.GetTouch(0);

            if (touch.phase != TouchPhase.Began)
            {
                return;
            }
            
            _ray = playerCamera.ScreenPointToRay(Screen.safeArea.center);

            Shoot(Physics.Raycast(_ray, out _raycastHit, 20));
        }

        #endregion
        
        /// <summary>
        /// Shoot
        /// </summary>
        /// <param name="hit"></param>
        private void Shoot(bool hit)
        {
            if (!hit)
            {
                return;
            }
            
            if (!_raycastHit.transform.CompareTag("Enemy"))
            {
                return;
            }

            _raycastHit.transform.GetComponent<Enemy>().Kill();
        }
    }
}