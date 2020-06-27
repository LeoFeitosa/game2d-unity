using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum musicaFase
{
    FLORESTA, CAVERNA, GAMEOVER, THEEND
}

public enum gameState
{
    TITULO, GAMEPLAY, END, GAMEOVER
}

public class GameController : MonoBehaviour
{
    public gameState currentState;
    public GameObject painelTitulo, painelGameOver, painelEnd;

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

    public AudioClip musicFloresta, musicCaverna, musicGameOver, musicTheEnd;

    public int moedasColetadas;
    public Text moedasTxt;
    public Image[] coracoes;
    public int vida;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        foreach (GameObject o in fase)
        {
            o.SetActive(false);
        }
        fase[0].SetActive(true);

        heartController();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == gameState.TITULO && Input.GetKeyDown(KeyCode.Space))
        {
            currentState = gameState.GAMEPLAY;
            painelTitulo.SetActive(false);
        }
        else if (currentState == gameState.GAMEOVER && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (currentState == gameState.END && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
                clip = musicFloresta;
                break;
            case musicaFase.GAMEOVER:
                clip = musicGameOver;
                break;
            case musicaFase.THEEND:
                clip = musicTheEnd;
                break;
        }

        StartCoroutine("controleMusica", clip);
    }

    IEnumerator controleMusica(AudioClip musica)
    {
        musicSource.Stop();
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

    public void heartController()
    {
        foreach (Image h in coracoes)
        {
            h.enabled = false;
        }

        for (int v = 0; v < vida; v++)
        {
            coracoes[v].enabled = true;
        }
    }

    public void getHit()
    {
        vida -= 1;
        heartController();
        if (vida <= 0)
        {
            playerTransform.gameObject.SetActive(false);
            painelGameOver.SetActive(true);
            currentState = gameState.GAMEOVER;
            trocarMusica(musicaFase.GAMEOVER);
        }
    }

    public void getCoin()
    {
        moedasColetadas += 1;
        moedasTxt.text = moedasColetadas.ToString();
    }

    public void theEnd()
    {
        currentState = gameState.END;
        painelEnd.SetActive(true);
        trocarMusica(musicaFase.THEEND);
    }

}
