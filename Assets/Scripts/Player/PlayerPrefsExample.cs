using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPrefsExample : MonoBehaviour
{
    public TMP_InputField inputFieldName;
    public TMP_InputField inputFieldScore;

    // Start is called before the first frame update
    public void SaveData()
    {
        PlayerPrefs.SetString("PlayerName", inputFieldName.text);
        PlayerPrefs.SetInt("HighestScore", int.Parse(inputFieldScore.text));
    }

    public void LoadData()
    {
        inputFieldName.text = PlayerPrefs.GetString("PlayerName");
        inputFieldScore.text = PlayerPrefs.GetInt("HighestScore").ToString();
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteKey("PlayerName");
        PlayerPrefs.DeleteAll();
    }
}
