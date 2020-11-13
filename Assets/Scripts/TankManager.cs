using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    public Color playerColor = Color.white;
    public Transform spawnPoint = null;
    [HideInInspector] public int playerNumber = 0;
    [HideInInspector] public string coloredPlayerText = "";
    [HideInInspector] public GameObject instance = null;          
    [HideInInspector] public int wins;

    private TankController m_TankController;
    private GameObject m_CanvasGameObject;


    public void Setup()
    {
        m_TankController = instance.GetComponent<TankController>();
        m_CanvasGameObject = instance.GetComponentInChildren<Canvas>().gameObject;

        m_TankController.playerNumber = this.playerNumber;
        instance.name = "Tank " + playerNumber;
        coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + playerNumber + "</color>";
        wins = 0;

        MeshRenderer[] renderers = instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = playerColor;
    }


    public void DisableControl()
    {
        m_TankController.ToggleTankControl(false, false);
        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        m_TankController.ToggleTankControl(true, true);
        m_CanvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = spawnPoint.rotation;

        instance.SetActive(false);
        instance.SetActive(true);
    }
}
