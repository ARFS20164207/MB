using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerStats : MonoBehaviour
{
    public static LocalPlayerStats instance;

    public int coin1;//local coins
    public int coin2;//online coins
    public int wins;
    public int loses;


    private DateTime ultimaGuarda;


    private const string claveCoin1 = "poir";
    private const string claveCoin2 = "poit";
    private const string clavewins = "poiy";
    private const string claveloses = "poiu";
    private const string claveUltimaGuarda = "UltimaGuarda";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        LoadLocalData();

        // Verifica si ha pasado un día desde la última vez que se guardó
        if ((DateTime.Now - ultimaGuarda).Days >= 0)
        {
            // Realiza las operaciones que necesites antes de guardar
            coin1 += 5;
            coin2 += 10;

            // Guarda las monedas solo si ha pasado un día
            SaveLocalData();
        }

        // Aquí puedes usar las variables coin1 y coin2 como desees
        Debug.Log("Monedas actuales: Coin1=" + coin1 + ", Coin2=" + coin2);
    }

    public void SaveLocalData()
    {

        PlayerPrefs.SetString(claveUltimaGuarda, DateTime.Now.ToString());

        PlayerPrefs.SetInt(claveCoin1, coin1);
        PlayerPrefs.SetInt(claveCoin2, coin2);
        PlayerPrefs.SetInt(clavewins, wins);
        PlayerPrefs.SetInt(claveloses, loses);
        PlayerPrefs.Save();
    }
    public void LoadLocalData() 
    {
        coin1 = PlayerPrefs.GetInt(claveCoin1, 0);
        coin2 = PlayerPrefs.GetInt(claveCoin2, 0);
        coin2 = PlayerPrefs.GetInt(clavewins, 0);
        coin2 = PlayerPrefs.GetInt(claveloses, 0);
        ultimaGuarda = DateTime.Parse(PlayerPrefs.GetString(claveUltimaGuarda, DateTime.MinValue.ToString()));
    }

    private void EncriptInt()
    {

    }
}
