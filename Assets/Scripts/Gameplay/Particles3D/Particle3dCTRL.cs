using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Семен
/// <summary>
/// Контроллер 3д частиц
/// </summary>
public class Particle3dCTRL : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] particles;

    [SerializeField]
    float SpeedMove = 0.1f;
    Vector2 TargetMove = new Vector2();

    RectTransform Myfield;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TestMove();
        TestDestroy();
    }

    bool isInicialized = false;
    float timeInicialize = 0;

    //Инициализировать частицу
    void Inizialize(RectTransform field, Vector2 posOnField) {
        if (isInicialized) return;
        isInicialized = true;
        timeInicialize = Time.unscaledTime;

        Myfield = field;

        float scaleMove = 1;
        //Перемещаем частицу на стартовую позицию
        transform.localPosition = new Vector3((field.pivot.x - 0.5f) * scaleMove + posOnField.x, (field.pivot.x - 0.5f) * scaleMove + posOnField.y, 0);

        particles = GetComponentsInChildren<ParticleSystem>();
    }

    void TestMove() {
    
    }
    
    void TestDestroy() {
        //Удаляем только если вышло время жизни, или если все частицы остановились
        if (Time.unscaledTime - timeInicialize > 10 || isAllParticlesStoped()) {
            Destroy(gameObject);
        }
        
        bool isAllParticlesStoped() {


            bool play = false;
            foreach (ParticleSystem particle in particles) {
                if (particle == null || particle.isStopped)
                    continue;

                play = true;
            }

            return !play;
        }
    }

    /// <summary>
    /// Создать эффект взрыва бомбы и получить ссылку на нее
    /// </summary>
    static Particle3dCTRL CreateBoomBomb(GameObject field, CellCTRL cellStartExplose)
    {

        GameObject ParticleObj = Instantiate(GameplayParticles3D.main.prefabBoomBomb, GameplayParticles3D.main.transform);
        Particle3dCTRL particle3DCTRL = ParticleObj.GetComponent<Particle3dCTRL>();

        RectTransform rectField = field.GetComponent<RectTransform>();

        if (particle3DCTRL == null)
        {
            Destroy(ParticleObj);
            return particle3DCTRL;
        }

        //Инициализируем данными частицу
        particle3DCTRL.Inizialize(rectField, cellStartExplose.pos);

        return particle3DCTRL;
    }

    /// <summary>
    /// Создать эффект взрыва ракеты и получить ссылку на нее
    /// </summary>
    static Particle3dCTRL CreateBoomRocket(Transform field, CellCTRL cellStartExplose) {
        GameObject ParticleObj = Instantiate(GameplayParticles3D.main.prefabBoomRocket, GameplayParticles3D.main.transform);
        Particle3dCTRL particle3DCTRL = ParticleObj.GetComponent<Particle3dCTRL>();

        RectTransform rectField = field.GetComponent<RectTransform>();

        if (particle3DCTRL == null)
        {
            Destroy(ParticleObj);
            return particle3DCTRL;
        }
        return particle3DCTRL;
    }
}
