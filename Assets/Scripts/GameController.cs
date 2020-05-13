using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Camera cam;
    public Transform playerTransform;
    public float speedCam;

    public Transform limitCamLeft, limitCamRight, limitCamUp, limitCamDown;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        float posCamX = playerTransform.position.x;
        float posCamY = playerTransform.position.y;

        if (cam.transform.position.x < limitCamLeft.position.x && playerTransform.position.x < limitCamLeft.position.x)
        {
            posCamX = limitCamLeft.position.x;
        }
        else if (cam.transform.position.x > limitCamRight.position.x && playerTransform.position.x > limitCamRight.position.x)
        {
            posCamX = limitCamRight.position.x;
        }

        if (cam.transform.position.y < limitCamDown.position.y && playerTransform.position.y < limitCamDown.position.y)
        {
            posCamY = limitCamDown.position.y;
        }
        else if (cam.transform.position.y > limitCamUp.position.y && playerTransform.position.y > limitCamUp.position.y)
        {
            posCamY = limitCamUp.position.y;
        }

        Vector3 posCam = new Vector3(posCamX, posCamY, cam.transform.position.z);

        cam.transform.position = Vector3.Lerp(cam.transform.position, posCam, speedCam * Time.deltaTime);
    }
}
