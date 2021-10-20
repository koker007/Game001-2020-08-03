using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//alexandr
/// <summary>
/// очки вылетающие при разрушении игровых обьектов
/// </summary>
public class ScoreCTRL : MonoBehaviour
{
    //private const float TimeLife = 2f;
    private Vector2 movePosition;
    [SerializeField]
    private RectTransform rTransform;
    [SerializeField]
    private Text ScoreText;


    private void Start()
    {
        //Destroy(gameObject, TimeLife);
        movePosition = new Vector2(rTransform.position.x, rTransform.position.y + Random.Range(100f, 200f));
    }

    private void Update()
    {
        //rTransform.position = Vector2.Lerp(rTransform.position, movePosition, Time.deltaTime * 1/TimeLife);
    }

    /// <summary>
    /// при создании обьекта необходимо задать ему значение очков
    /// </summary>
    /// <param name="score"></param>
    public void Inicialize(int score, Color color)
    {
        ScoreText.text = score.ToString();
        ScoreText.color = color;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
