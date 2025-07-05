using System;
using TMPro;
using UnityEngine;

public class RoomItemButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomNameText;

    public void OnJoinRoomButtonClick()
    {
        RoomList.Instance.JoinRoomByName(roomNameText.text);
    }
}
