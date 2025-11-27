using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float rotateSpeed = 100.0f;

    private Rigidbody rb;
    private Animator animator;
    public bool canMode = true;
    public bool isDead = false;
    private Vector3 startPos;
    private bool respawned = false;
    private GameObject respawnPanel;
    public bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        startPos = transform.position;
        respawnPanel = GameObject.Find("RespawnPanel");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead == false)
        {
            respawnPanel.SetActive(false);
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

            Vector3 rotateY = new Vector3(0, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime, 0);
            if (movement != Vector3.zero)
            {
                rb.MoveRotation(rb.rotation * Quaternion.Euler(rotateY));
            }

            rb.MovePosition(
                rb.position + transform.forward * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime
                + transform.forward * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime
                );

            animator.SetFloat("BlendV", Input.GetAxis("Vertical"));
            animator.SetFloat("BlendH", Input.GetAxis("Horizontal"));
        }
    }

    private void Update()
    {
        if (isDead == false)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * 1200f * Time.deltaTime, ForceMode.VelocityChange);
            }
        }
        if (isDead == true && respawned == false && gameOver == false)
        {
            respawned = true;
            respawnPanel.SetActive(true);
            respawnPanel.GetComponent<RespawnTimer>().enabled = true;
            StartCoroutine(RespawnWait());
        }
        
    }
    IEnumerator RespawnWait()
    {
        yield return new WaitForSeconds(3);
        isDead = true;
        respawned = false;
        transform.position = startPos;
        GetComponent<DisplayColor>().Respawn(GetComponent<PhotonView>().Owner.NickName);
    }

}
