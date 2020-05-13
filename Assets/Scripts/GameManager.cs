using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public static int Coins { get; set; }
    public int hp;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HP(int _hp)
    {
        hp = _hp;
    }

    public int GetHP()
    {
        return hp;
    }
}
