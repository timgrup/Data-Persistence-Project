using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHallOfFame : MonoBehaviour
{
    public GameObject List;
    public GameObject listEntryPrefab;

    void Start()
    {
        var hallOfFameListReference = GameManager.Instance.HallOfFameList;

        for (int i = 0; i < hallOfFameListReference.Count; i++)
        {
            GameObject newListEntry = Instantiate(listEntryPrefab, List.transform);
            newListEntry.transform.localPosition = new Vector3(0, -40 * i, 0);
            newListEntry.GetComponent<TMPro.TextMeshProUGUI>().text = $"{i + 1}. {hallOfFameListReference[i].PlayerName}: {hallOfFameListReference[i].Score}";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
