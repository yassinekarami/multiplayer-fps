using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class LootAt : MonoBehaviour
{
    private Vector3 worldPosition;
    private Vector3 screenPosition;
    public GameObject crossHair;

    private void FixedUpdate()
    {
        screenPosition = Input.mousePosition;
        screenPosition.z = 6f;

        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        transform.position = worldPosition;

        crossHair.transform.position = Input.mousePosition;
    }
}
