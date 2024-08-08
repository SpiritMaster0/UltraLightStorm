using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public AudioSource sound;
    public AudioClip[] SongList;
    public TextMeshProUGUI demoText;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        demoText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(PlaySong());
        }
    }

    public IEnumerator PlaySong()
    {
        int songListIndex = TargetCircleManager.Instance.SongChoice;
        if (songListIndex >= 0 && songListIndex < SongList.Length)
        {
            yield return new WaitForSeconds(1); // Initial delay
            demoText.gameObject.SetActive(false);
            sound.clip = SongList[songListIndex];
            sound.Play();

            float startTime = Time.time;
            float songLength = sound.clip.length;

            while (Time.time - startTime < songLength)
            {
                // Check if the game is paused
                if (TargetCircleManager.Instance.pausedGame)
                {
                    sound.Pause();

                    // Wait until the game is resumed
                    yield return new WaitUntil(() => !TargetCircleManager.Instance.pausedGame);

                    sound.UnPause();
                    startTime += Time.time - startTime; // Adjust startTime to continue from where it was paused
                }
                else
                {
                    yield return null; // Continue to check every frame
                }
            }

            // Optionally, you could add code here to handle what happens after the song finishes
            // For example, reactivating demoText or performing other actions
            // yield return new WaitForSeconds(23f);
            // sound.Stop();
            // demoText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Invalid SongChoice index.");
        }
    }
}