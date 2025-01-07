using UnityEngine;
using UnityEngine.UI;

public class SoundOnOff : MonoBehaviour
{
    public Button button;                     // Gomb referencia
    public Sprite soundOnImage;               // Hang bekapcsolt ikon
    public Sprite soundOffImage;              // Hang kikapcsolt ikon
    private bool isMuted;                     // Némítás állapota

    void Start()
    {
        // Ellenőrizzük a mentett állapotot
        isMuted = PlayerPrefs.GetInt("SoundMuted", 0) == 1; // Alapértelmezett: hang be (0)

        // Frissítjük a gomb ikonját és a hang állapotát
        UpdateButtonIcon();
        ApplyMuteState();
    }

    public void ButtonClicked()
    {
        // Állapot váltása
        isMuted = !isMuted;

        // Állapot mentése
        PlayerPrefs.SetInt("SoundMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        // Frissítjük a hangok és ikonok állapotát
        UpdateButtonIcon();
        ApplyMuteState();
    }

    private void UpdateButtonIcon()
    {
        // Gomb ikon frissítése
        button.image.sprite = isMuted ? soundOffImage : soundOnImage;
    }

    private void ApplyMuteState()
    {
        // Az összes audió némítása
        AudioListener.pause = isMuted;  // Minden zene és hang egyszerre némítva
    }
}

/*using UnityEngine;
using UnityEngine.UI;

public class SoundOnOff : MonoBehaviour
{
    
    private Sprite soundOnImage; 
    public Sprite soundOffImage; 
    public Button button; 
    private bool isOn = false; 
    private bool isMuted;     // Némítás állapota

    /*public AudioSource audioSource1; 
    public AudioSource audioSource2; 
    public AudioSource audioSource3; 
    public AudioSource audioSource4; 
    public AudioSource audioSource5; 
    public AudioSource audioSource6;*
    // Start is called before the first frame update
    void Start()
    {
        soundOnImage = button.image.sprite; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClicked() 
    {
       if (isOn){
        button.image.sprite = soundOffImage; 
        isOn = false; 
        isMuted = true;
        AudioListener.pause = isMuted;
        /*audioSource1.mute = true; 
        audioSource2.mute = true; 
        audioSource3.mute = true; 
        audioSource4.mute = true; 
        audioSource5.mute = true; 
        audioSource6.mute = true; *


       }
    
    else 
    {
        button.image.sprite = soundOnImage; 
        isOn = true; 
        isMuted = false;
        AudioListener.pause = isMuted;
        /*audioSource1.mute = false; 
        audioSource2.mute = false; 
        audioSource3.mute = false; 
        audioSource4.mute = false; 
        audioSource5.mute = false; 
        audioSource6.mute = false; *
    }
    }

}*/