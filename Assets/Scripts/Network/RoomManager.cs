using System;
using System.Collections;
using MultiFPS_Shooting.Scripts.Player;
using Photon.Pun;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawn;
    [Space]
    [SerializeField] private GameObject roomCamera;
    [Space]
    [SerializeField] private GameObject nameUI;
    [SerializeField] private GameObject connectingUI;

    private static string s = "1234567890abcd";

    private string nickname = "player_";
    
    public static RoomManager instance;
    private bool isFirstTime = true;
    private void Awake()
    {
        instance = this;
        nickname+= +HashCode.Combine(s);
    }

    public void ChangeName(string inputName)
    {
        nickname = inputName;
    }

    public void JoinRoomButtonPress()
    {
        Debug.Log("Connecting");
        PhotonNetwork.ConnectUsingSettings();
        nameUI.SetActive(false);
        connectingUI.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster(); 
        Debug.Log("Connected to server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        PhotonNetwork.JoinOrCreateRoom("Test", null,null);
        Debug.Log("Created room");
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room");
        roomCamera.SetActive(false);
        SpawnPlayer();
    }

    public void Respawn() => StartCoroutine(Spawn());
    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2f);
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        isFirstTime = false;
        GameObject _player = PhotonNetwork.Instantiate(playerPrefab.name, playerSpawn.position, Quaternion.identity);
        _player.GetComponent<PlayerHealth>().isLocal = true;
        _player.GetComponent<PhotonView>().RPC("SetNickName",RpcTarget.AllBuffered,nickname);
        PhotonNetwork.LocalPlayer.NickName = nickname;
    }
}
