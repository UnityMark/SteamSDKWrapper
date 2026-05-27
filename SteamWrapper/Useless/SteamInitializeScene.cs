using Mark.Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SteamInitializeScene : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private SteamBoostrap _boostrap;
    [SerializeField] private TMP_Text _labelNickname;
    [SerializeField] private TMP_Text _labelId;
    [SerializeField] private TMP_Text _labelState;
    [SerializeField] private TMP_Text _labelCountPlayers;
    [SerializeField] private TMP_Text _labelClan;
    [SerializeField] private int _indexClan;
    

    private void Start()
    {
        _image.sprite = _boostrap.GetSteamComponent<SteamAvatarService>().GetMyLargeAvatar();
        _labelNickname.text = _boostrap.PlayerService.Name;
        _labelId.text += _boostrap.PlayerService.Id.ToString();
        _labelState.text = _boostrap.PlayerService.State.ToString();
        SetClan();
        LateInitialize();
    }

    private void SetClan()
    {
        int countClans = _boostrap.GetSteamComponent<SteamClanService>().CachedClans.Count;
        Debug.Log($"[SteamClanService] Count clans: {countClans}");
        if (countClans > 0 && _indexClan >= 0 && _indexClan < countClans - 1)
        {
            _labelClan.text = _boostrap.GetSteamComponent<SteamClanService>().CachedClans[_indexClan].Tag;
        }
        else
        {
            _labelClan.text = "UKW";
        }
    }

    private async void LateInitialize()
    {
        await Awaitable.NextFrameAsync();
        _boostrap.GetSteamComponent<SteamStatsPlayerOnlineService>().UpdatePlayerCount();
        _labelCountPlayers.text += _boostrap.GetSteamComponent<SteamStatsPlayerOnlineService>().CurrentPlayerCount.ToString();
    }
}
 