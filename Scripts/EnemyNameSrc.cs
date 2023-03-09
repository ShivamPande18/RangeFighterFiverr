using UnityEngine;
using TMPro;

public class EnemyNameSrc : MonoBehaviour
{
    public string[] names;
    public TMP_Text enemyName;

    private void Start()
    {
        enemyName.text = names[Random.Range(0, names.Length)];
    }
}