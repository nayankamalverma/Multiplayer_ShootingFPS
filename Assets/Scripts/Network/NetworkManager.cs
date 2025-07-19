using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        [Header("UI Object")]
        [SerializeField] private GameObject menuButtons;
        [Space]
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private TextMeshProUGUI loadingPanelText;
        [Space] 
        [SerializeField] private GameObject createRoomPanel;
        [SerializeField] private TMP_InputField roomNameInput;
        [SerializeField] private TextMeshProUGUI errorText;

        public static NetworkManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ClosePanels();
            loadingPanel.SetActive(true);
            loadingPanelText.text = "Connecting to network... ";
            PhotonNetwork.ConnectUsingSettings();
        }
        private void ClosePanels()
        {
            menuButtons.SetActive(false);
            loadingPanel.SetActive(false);
            createRoomPanel.SetActive(false);
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
            loadingPanelText.text = "Joining Lobby.";
        }

        public override void OnJoinedLobby()
        {
            ClosePanels();
            menuButtons.SetActive(true);
        }

        public void OpenRoomCreate()
        {
            ClosePanels();
            DeactivateErrorText();
            createRoomPanel.SetActive(true);
        }

        public void CreateRoom()
        {
            if (!string.IsNullOrWhiteSpace(roomNameInput.text))
            {
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = 8;
                PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions);

                ClosePanels();
                loadingPanelText.text = "Creating room...";
                loadingPanel.SetActive(true);
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            errorText.gameObject.SetActive(true);
            errorText.text = "Room name already exists. Please choose another.";
            Invoke(nameof(DeactivateErrorText),5);
        }

        private void DeactivateErrorText()
        {
            errorText.gameObject.SetActive(false);
        }

    }
}