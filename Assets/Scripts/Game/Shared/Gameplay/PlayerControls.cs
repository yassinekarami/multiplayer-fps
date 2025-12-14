
using UnityEngine;


namespace Game.Shared.Gameplay
{
    public class PlayerControls : MonoBehaviour
    {
        public float moveSpeed = 3.5f;
        public float rotateSpeed = 100.0f;

        private Rigidbody rb;
        private Animator animator;

        private GameObject weaponParent;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            weaponParent = GameObject.Find("Weapons");
        }



        // Update is called once per frame
        void Update()
        {
            animator.SetBool("Movement", Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0);
            animator.SetFloat("BlendV", Input.GetAxis("Vertical"));
            animator.SetFloat("BlendH", Input.GetAxis("Horizontal"));
           
            if (Input.GetMouseButtonDown(0))
            {
                this.Shot();
            }
            if (Input.GetMouseButtonDown(1))
            {
                this.ChangeWeapon();
            }
        }
        private void FixedUpdate()
        {
            rb.MovePosition(
                rb.position + transform.forward * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime
                + transform.right * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime
                );
        }


        /// <summary>
        /// function to shot with the activate weapons
        /// </summary>
        private void Shot()
        {
            weaponParent.GetComponent<WeaponHandler>().Shot();
        }

        /// <summary>
        /// function to change the current weapon
        /// </summary>
        private void ChangeWeapon()
        {
            weaponParent.GetComponent<WeaponHandler>().ChangeWeapon();
        }
    }

}
