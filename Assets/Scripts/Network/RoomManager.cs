using Photon.Pun;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawn;
    [Space]
    [SerializeField] private GameObject roomCamera;
    private void Start()
    {
        Debug.Log("Connecting");
        PhotonNetwork.ConnectUsingSettings();
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
        roomCamera.SetActive((false));
        RespawnPLayer();
    }

    private void RespawnPLayer()
    {
        GameObject _player = PhotonNetwork.Instantiate(playerPrefab.name, playerSpawn.position, Quaternion.identity );
    }
}
