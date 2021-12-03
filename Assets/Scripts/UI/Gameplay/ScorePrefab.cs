using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//alexandr
/// <summary>
/// очки вылетающие при разрушении игровых обьектов
/// </summary>
public class ScorePrefab : MonoBehaviour
{
    private Text ScoreText;
    private const float TimeLife = 2f;
    private Vector2 movePosition;
    private RectTransform rTransform;
    public GameObject parent;

    private void Awake()
    {
        ScoreText = GetComponent<Text>();
        rTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Destroy(parent, TimeLife);

        movePosition = new Vector2(rTransform.position.x, rTransform.position.y + Random.Range(100f, 200f));
    }

    private void Update()
    {
        rTransform.position = Vector2.Lerp(rTransform.position, movePosition, Time.deltaTime * 1/TimeLife);
    }

    /// <summary>
    /// при создании обьекта необходимо задать ему значение очков
    /// </summary>
    /// <param name="score"></param>
    public void SetText(int score)
    {
        ScoreText.text = score.ToString();
    }
}
