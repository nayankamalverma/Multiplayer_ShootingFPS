using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

 
public class RoomList : MonoBehaviourPunCallbacks
{
    public static RoomList Instance;

    [SerializeField] private RoomManager roomManger;
    [SerializeField] private GameObject loobyUI;
    [Header("UI")]
    [SerializeField] private Transform roomListParent;
    [SerializeField] private GameObject roomListItemPrefab;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    private void Awake()
    {
        Instance = this;
    }
    IEnumerator Start()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();

        }

        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (cachedRoomList.Count <= 0)
        {
            cachedRoomList = roomList;
        }
        else
        {
            foreach (var room in roomList)
            {
                for (int i = 0; i < cachedRoomList.Count; i++)
                {
                    if (cachedRoomList[i].Name == room.Name)
                    {
                        List<RoomInfo> newList = cachedRoomList;

                        if (room.RemovedFromList)
                        {
                            newList.Remove(newList[i]);
                        }
                        else
                        {
                            newList[i] = room;
                        }

                        cachedRoomList = newList;
                    }
                }
            }
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (Transform roomItem in roomListParent)
        {
            Destroy(roomItem.gameObject);
        }

        foreach (var room in cachedRoomList)
        {
            GameObject roomItem = Instantiate(roomListItemPrefab, roomListParent);

            roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
            roomItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount + "/" + room.MaxPlayers;
        }
    }

    public void JoinRoomByName(string roomName)
    {
        roomManger.SetRoomName(roomName);
        roomManger.gameObject.SetActive(true);
        loobyUI.SetActive(false);
    }
}
