using System.Collections.Generic;
using UnityEngine;

public class NoteRecorder : MonoBehaviour
{
    public AudioSource audioSource; // Assign your AudioSource in the Inspector
    public List<float> noteTimes;   // List to store timestamps of notes
    public List<string> finishedNoteTimes; // List to store formatted TargetCircleData strings

    void Start()
    {
        noteTimes = new List<float>();
        finishedNoteTimes = new List<string>();
    }

    void Update()
    {
        // Check for key press to record note
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RecordNote();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GenerateTargetCircleData();
        }
    }

    void RecordNote()
    {
        if (audioSource != null)
        {
            // Record the current time of the song
            float currentTime = audioSource.time;
            noteTimes.Add(currentTime);

            // Print to console for verification
            Debug.Log("Note recorded at: " + currentTime);
        }
        else
        {
            Debug.LogError("AudioSource not assigned.");
        }
    }

    public void GenerateTargetCircleData()
    {
        Debug.Log("targetCircleDataLists.Add(new List<TargetCircleData> {");

        finishedNoteTimes.Clear(); // Clear previous data if any

        foreach (var time in noteTimes)
        {
            // Format the string and add to the finishedNoteTimes list
            string formattedData = $"\n    new TargetCircleData(new Vector2(0f, 0f), 0f, {time}f, 23, 3f, 10, false),";
            finishedNoteTimes.Add(formattedData);

            // Print formatted data for debugging
            Debug.Log(formattedData);
        }

        Debug.Log("});");
    }
}