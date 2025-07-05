using System;
using System.Collections;
using MultiFPS_Shooting.Scripts.Player;
using Photon.Pun;
using Photon.Realtime;
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
    private string roomToJoin = "test";
    
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
        PhotonNetwork.JoinOrCreateRoom(roomToJoin, new RoomOptions() { MaxPlayers = 4 }, null);
        nameUI.SetActive(false);
        connectingUI.SetActive(true);
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

    public void SetRoomName(string roomName)
    {
        roomToJoin = roomName;
    }
}
