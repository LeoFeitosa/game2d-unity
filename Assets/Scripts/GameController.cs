using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum musicaFase
{
    FLORESTA, CAVERNA
}

public class GameController : MonoBehaviour
{
    private Camera cam;
    public Transform playerTransform;

    public float speedCam;
    public Transform limitCamLeft, limitCamRight, limitCamUp, limitCamDown;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioClip sfxJump, sfxSlide, sfxCoin, sfxEnemyDead, sfxDamage;
    public AudioClip[] sfxSteep;

    public GameObject[] fase;

    public musicaFase musicaAtual;  

    public AudioClip musicFloresta, musicCaverna;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        foreach (GameObject o in fase)
        {
            o.SetActive(false);
        }
        fase[0].SetActive   (true);
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

    internal void playSFX(AudioClip sfxCoin)
    {
        throw new NotImplementedException();
    }

    public void playSFX(AudioClip sfxClip, float volume)
    {
        sfxSource.PlayOneShot(sfxClip, volume);
    }

    public void trocarMusica(musicaFase novaMusica)
    {
        AudioClip clip = null;

        switch (novaMusica)
        {
            case musicaFase.CAVERNA:
                clip = musicCaverna;
                break;
            case musicaFase.FLORESTA:
                clip = musicCaverna;
                break;
        }

        StartCoroutine("controleMusica", clip);
    }

    IEnumerator controleMusica(AudioClip musica)
    {
        float volumeMaximo = musicSource.volume;
        for (float volume = volumeMaximo; volume > 0; volume -= 0.01f)
        {
            musicSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }

        musicSource.clip = musica;
        musicSource.Play();

        for (float volume = 0; volume < volumeMaximo; volume += 0.01f)
        {
            musicSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }
    }
}
