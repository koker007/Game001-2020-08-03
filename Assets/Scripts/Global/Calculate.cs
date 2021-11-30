using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//—емен
//—крипт с различными вычислени€ми которые нужны врем€ от времени
public class Calculate : MonoBehaviour
{
    public const float PIinOnegrad = 0.01745329f; // оличество пи радиан в одном градусе

    //—местить указанную величину на определенное расто€ние к величине которой стремимс€, и если перебор приравн€ть к стрем€щейс€ величине
    static public float GetValueLinear(float from, float to, float moveValue) {

        float result = from;

        //убавление
        if (result > to)
        {
            result -= moveValue;

            //≈сли перелет целевого значени€
            if (result < to) 
                return to;
        }
        //ѕрибавление
        else if (result < to)
        {
            result += moveValue;
            //если перелет целевого значени€
            if (result > to)
                return to;
        }

        return result;
    }
}
