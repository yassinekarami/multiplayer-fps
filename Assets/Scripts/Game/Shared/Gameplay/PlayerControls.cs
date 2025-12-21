
using Cinemachine;
using Photon.Pun;
using System.Threading;
using UnityEngine;


namespace Game.Shared.Gameplay
{
    public class PlayerControls : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 3.5f;
        public float mouseSensitivity = 2.5f;
        public float gravity = -9.81f;

        private CharacterController controller;
        private Animator animator;
        private Camera playerCamera;
        private CinemachineVirtualCamera virtualCamera;

        private GameObject weaponParent;
        private Vector3 mouseWorldPosition;

        public GameObject cameraHolder;
        public GameObject aimReference;

        float mouseX;
        float mouseY;
        float xRotation = 0f;
        private Vector3 velocity;


        void Start()
        {
          
            if (GetComponent<PhotonView>() != null && GetComponent<PhotonView>().IsMine)
            {
                controller = GetComponent<CharacterController>();
                animator = GetComponent<Animator>();
                weaponParent = GameObject.Find("Weapons");
                cameraHolder.SetActive(true);
                playerCamera = cameraHolder.GetComponentInChildren<Camera>();
                virtualCamera = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
                virtualCamera.Follow = transform;
                virtualCamera.LookAt = transform;
            }

        }

        void Update()
        {
            if (!GetComponent<PhotonView>().IsMine || !GetComponent<PhotonView>().IsMine) return;

            HandleMovement();
            HandleMouseLook();
            HandleActions();
        }

        private void HandleMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move =
                transform.right * h +
                transform.forward * v;

            controller.Move(move * moveSpeed * Time.deltaTime);

            // Gravity
            if (controller.isGrounded && velocity.y < 0)
                velocity.y = -2f;

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            // Animator
            animator.SetBool("Movement", h != 0 || v != 0);
            animator.SetFloat("BlendH", h);
            animator.SetFloat("BlendV", v);
        }

        private void HandleMouseLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

            // Body rotation (Y)
            transform.Rotate(Vector3.up * mouseX);

            // Camera rotation (X)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            cameraHolder.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Aim reference (optional)
            if (aimReference)
            {
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition); 
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero); 
                if (groundPlane.Raycast(ray, out float distance)) 
                { mouseWorldPosition = ray.GetPoint(distance); }
            }
        }

        private void HandleActions()
        {
            if (Input.GetMouseButtonDown(0))
                Shot();

            if (Input.GetMouseButtonDown(1))
                ChangeWeapon();
        }

        private void Shot()
        {
            weaponParent.GetComponent<WeaponHandler>().Shot();
        }

        private void ChangeWeapon()
        {
            weaponParent.GetComponent<WeaponHandler>().ChangeWeapon();
        }
    }

}
