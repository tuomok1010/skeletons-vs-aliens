using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIController : MonoBehaviour
{
    [SerializeField] GameObject skeletonsScore;
    [SerializeField] GameObject aliensScore;

    TextMeshProUGUI skeletonsScoreTMP;
    TextMeshProUGUI aliensScoreTMP;

    private void Awake()
    {
        skeletonsScoreTMP = skeletonsScore.GetComponent<TextMeshProUGUI>();
        aliensScoreTMP = aliensScore.GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        skeletonsScoreTMP.text = GameManager.GetPlayerScore(GameManager.PlayerFaction.SKELETONS).ToString();
        aliensScoreTMP.text = GameManager.GetPlayerScore(GameManager.PlayerFaction.ALIENS).ToString();
    }
}
