using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public GameObject ItemObtainedScreen;
    public Image icon;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public GameObject InteractText;

    public GameObject InventoryScreen;
    public GameObject inventoryItemPrefab;
    public GameObject inventoryContentHead;

    public GameObject PauseScreen;
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixerGroup masterMixerGroup;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup sfxMixerGroup;
    private bool isPaused;

    public int itemsObtained = 0;
    public int itemsInLevel = 0;
    public string nextScene = "Level1";
    public GameObject LevelEnd;
    public TextMeshProUGUI[] rank;
    public TextMeshProUGUI itemCounter;
    public bool hasEnded = false;
    // Start is called before the first frame update
    void Start()
    {
        hasEnded = false;
        Time.timeScale = 1f;
        gm = this;
        masterVolume = Settings.MasterVolume;
        musicVolume = Settings.MusicVolume;
        sfxVolume = Settings.SFXVolume;
        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        UpdateMixerVolume();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextScene()
    {
        
        SceneManager.LoadScene(nextScene);
    }

    public void EndScreen()
    {
        hasEnded = true;
        Time.timeScale = 0f;
        itemCounter.text = itemsObtained + " / " + itemsInLevel;
        int itemsLeft = itemsInLevel - itemsObtained;
        switch (itemsLeft)
        {
            case 0:
                foreach (var t in rank)
                {
                    t.text = "S";
                }
                break;
            case 1:
                foreach (var t in rank)
                {
                    t.text = "A";
                }
                break;
            case 2:
                foreach (var t in rank)
                {
                    t.text = "B";
                }
                break;
            case 3:
                foreach (var t in rank)
                {
                    t.text = "C";
                }
                break;
            default:
                foreach (var t in rank)
                {
                    t.text = "D";
                }
                break;
        }
        LevelEnd.SetActive(true);
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        PauseScreen.SetActive(true);
    }

    public void Unpause()
    {
        isPaused = false;
        CloseItemObtained();
        CloseInventory();
        if(!LevelEnd.activeInHierarchy)
            Time.timeScale = 1;
        PauseScreen.SetActive(false);
        
    }

    public void ShowInventory()
    {
        Time.timeScale = 0;
        InventoryScreen.SetActive(true);
    }
    public void CloseInventory()
    {
        CloseItemObtained();
        if(!PauseScreen.activeInHierarchy && !LevelEnd.activeInHierarchy)
            Time.timeScale = 1;
        InventoryScreen.SetActive(false);
    }

    public GameObject AddToInventory(Item i)
    {
        var invI = Instantiate(inventoryItemPrefab, inventoryContentHead.transform);
        invI.GetComponent<InventoryItem>().init(i);
        return invI;
    }

    public void ShowItemObtained(Item i)
    {
        Time.timeScale = 0;
        title.text = i.title;
        description.text = i.description;
        icon.sprite = i.itemIcon;
        ItemObtainedScreen.SetActive(true);
    }

    public void CloseItemObtained()
    {
        if (!InventoryScreen.activeInHierarchy && !PauseScreen.activeInHierarchy && !LevelEnd.activeInHierarchy)
            Time.timeScale = 1f;
        ItemObtainedScreen.SetActive(false);
    }

    public void TurnOnInteractText()
    {
        if (!InteractText.activeInHierarchy)
        {
            InteractText.SetActive(true);
        }
    }
    public void TurnOffInteractText()
    {
        if (InteractText.activeInHierarchy)
        {
            InteractText.SetActive(false);
        }
    }
    public void UpdateMixerVolume()
    {
        print("Changed");
        masterMixerGroup.audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        sfxMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }
    public void OnMasterSliderValueChange(float value)
    {
        masterVolume = value;
        Settings.MasterVolume = masterVolume;
        UpdateMixerVolume();
    }
    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        Settings.MusicVolume = musicVolume;
        UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        sfxVolume = value;
        Settings.SFXVolume = sfxVolume;
        UpdateMixerVolume();
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("StartMenu");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }
            else if (isPaused)
            {
                Unpause();
            }
        }
    }
}
