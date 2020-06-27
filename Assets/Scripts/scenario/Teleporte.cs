using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporte : MonoBehaviour
{
    private GameController _GameController;

    public Transform pontoSaida;
    public Transform posCamera;

    public Transform limitCamLeft, limitCamRight, limitCamUp, limitCamDown;

    public musicaFase novaMusica;

    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;

    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            foreach (GameObject o in _GameController.fase)
            {
                o.SetActive(false);
            }

            if (gameObject.tag == "teleporte1")
            {
                _GameController.fase[1].SetActive(true);
                _GameController.trocarMusica(musicaFase.CAVERNA);
            }
            if (gameObject.tag == "teleporte2")
            {
                _GameController.fase[2].SetActive(true);
                _GameController.trocarMusica(musicaFase.FLORESTA);
            }

            col.transform.position = pontoSaida.position;
            Camera.main.transform.position = posCamera.position;

            _GameController.limitCamLeft = limitCamLeft;
            _GameController.limitCamRight = limitCamRight;
            _GameController.limitCamUp = limitCamUp;
            _GameController.limitCamDown = limitCamDown;
        }
    }
}
