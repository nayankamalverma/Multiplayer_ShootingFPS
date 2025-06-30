using UnityEngine;
using System.Linq;
using MultiFPS_Shooting.Input.Input;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private GameObject playersHolder;
    [SerializeField] private float refershRate;

    [Header("UI")] 
    [SerializeField] private GameObject[] slots;
    [SerializeField] private TextMeshProUGUI[] scoreTexts;
    [SerializeField] private TextMeshProUGUI[] nameTexts;

    private void Start()
    {
        InvokeRepeating(nameof(RefreshLeaderBoard),1f,refershRate);
    }

    private void Update()
    {
        playersHolder.SetActive(Input.GetKey(KeyCode.Tab));
    }

    private void RefreshLeaderBoard()
    {
        foreach (var slot in slots)
        {
            slot.SetActive(false);
        }

        var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();

        int i = 0;
        foreach (var player in sortedPlayerList)
        {
            slots[i].SetActive(true);

            nameTexts[i].text = player.NickName;
            scoreTexts[i].text = "Kills: "+player.GetScore();

            i++;
        }
    }
}
