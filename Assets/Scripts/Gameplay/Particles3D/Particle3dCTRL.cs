using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����
/// <summary>
/// ���������� 3� ������
/// </summary>
public class Particle3dCTRL : MonoBehaviour
{

    [SerializeField]
    float timeLifeMax = 10;
    [SerializeField]
    ParticleSystem[] particles;

    [SerializeField]
    float posZ = 0;

    [SerializeField]
    float SpeedMove = 0;
    [SerializeField]
    Vector2 TargetMove = new Vector2();
    [SerializeField]
    CellInternalObject targetInternalObject;
    [SerializeField]
    GameFieldCTRL.Combination comb;

    RectTransform Myfield;

    [Header("Audio")]
    [SerializeField]
    AudioClip audioClip;
    [SerializeField]
    Vector2 PitchDiapazone = new Vector2(1,1);

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

    //���������������� �������
    void Inizialize(RectTransform field, Vector2 posOnField) {
        if (isInicialized) return;
        isInicialized = true;
        timeInicialize = Time.unscaledTime;

        Myfield = field;

        float scaleMove = 1;
        //���������� ������� �� ��������� �������
        transform.localPosition = new Vector3(
            (field.pivot.x - 0.5f) * scaleMove + posOnField.x + 0.5f,
            (field.pivot.y - 0.5f) * scaleMove + posOnField.y + 0.5f, posZ);

        particles = GetComponentsInChildren<ParticleSystem>();

        if (audioClip) {
            SoundCTRL.main.SmartPlaySound(audioClip, 1, Random.Range(PitchDiapazone.x, PitchDiapazone.y));
        }
    }
    public void SetSpeed(float speed) {
        foreach (ParticleSystem particleSystem in particles) {
            particleSystem.startSpeed = particleSystem.startSpeed * speed;
        }
    }
    public void SetSize(float size) {
        foreach (ParticleSystem particleSystem in particles)
        {
            particleSystem.startSize = particleSystem.startSize * size;
        }
    }
    public void SetColor(Color color) {
        foreach (ParticleSystem particleSystem in particles)
        {
            particleSystem.startColor = color;
            particleSystem.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmisColor", color);
        }
    }

    public void SetTransformTarget(Vector2 target) {
        TargetMove = target;
    }

    //���������� ����, ���������� ������
    public void SetTransformTarget(CellInternalObject internalObject, GameFieldCTRL.Combination combNew) {
        targetInternalObject = internalObject;
        comb = combNew;
    }
    public void SetTransformSpeed(float speed) {
        SpeedMove = speed;
    }


    float timeLastMove = 0;
    void TestMove() {
        if (SpeedMove <= 0) return;

        //���� ���� ���������� ������ � �������� ����, ������� � ����
        if (targetInternalObject != null)
        {
            //����� � �������� ���� ������� ����������� �������
            TargetMove = (targetInternalObject.rectMy.pivot * -1) + new Vector2(0.5f, 0.5f);
        }

        //������������ ���� ���� �������� � ��������� �� ���� �� ����
        float dist = Vector2.Distance(TargetMove, gameObject.transform.localPosition);
        if (dist > 0.001f) {
            Vector2 vectorMove = (TargetMove - new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y)).normalized;
            //������������
            gameObject.transform.localPosition += new Vector3(vectorMove.x * SpeedMove * Time.deltaTime, vectorMove.y * SpeedMove * Time.deltaTime);

            //��������� ������ �� ���� �����
            Vector2 vectorMove2 = (TargetMove - new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y)).normalized;
            
            //���� ������ �� ���� ��������� ������ �� �������� ����, ������������ ����.
            if ((vectorMove - vectorMove2).magnitude > 0.01f) {
                gameObject.transform.localPosition = new Vector3(TargetMove.x, TargetMove.y);
            }

            timeLastMove = Time.unscaledTime;
        }

        //���������� ���������� �������
        if (targetInternalObject && Vector2.Distance(TargetMove, new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y)) < 0.01f)
        {
            targetInternalObject.myCell.Damage(null, comb);

            //������� ���� � �������� � ���������� �������
            targetInternalObject = null;
        }
    }
    
    void TestDestroy() {
        //������� ������ ���� ����� ����� �����, ��� ���� ��� ������� ������������
        if (Time.unscaledTime - timeInicialize > timeLifeMax || isAllParticlesStoped() || isStoppedMoveDestroy()) {

            Destroy(gameObject);
        }
        
        bool isAllParticlesStoped() {

            bool play = false;

            if (particles.Length == 0) return play;

            foreach (ParticleSystem particle in particles) {
                if (particle == null || particle.isStopped)
                    continue;

                play = true;
            }

            return !play;
        }

        bool isStoppedMoveDestroy() {
            bool result = false;
            if (SpeedMove > 0.01f && Vector2.Distance(TargetMove, new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y)) < 0.01f && Time.unscaledTime - timeLastMove > 5f) {
                result = true;
            }

            return result;
        }
    }

    /// <summary>
    /// ���� ������ ������� � ������� ���� ������ � ��������� ����� �� ����
    /// </summary>
    public static Particle3dCTRL CreateParticle(Transform field, CellCTRL cellStartExplose, GameObject prefabParticle) {
        GameObject ParticleObj = Instantiate(prefabParticle, GameplayParticles3D.main.transform);
        Particle3dCTRL particle3DCTRL = ParticleObj.GetComponent<Particle3dCTRL>();

        RectTransform rectField = field.GetComponent<RectTransform>();

        if (particle3DCTRL == null)
        {
            Destroy(ParticleObj);
            return particle3DCTRL;
        }

        //�������������� ������� �������
        particle3DCTRL.Inizialize(rectField, cellStartExplose.pos);

        return particle3DCTRL;
    }

    /// <summary>
    /// ������� ������ ������ ����� � �������� ������ �� ���
    /// </summary>
    public static Particle3dCTRL CreateBoomBomb(Transform field, CellCTRL cellStartExplose)
    {

        return CreateParticle(field, cellStartExplose, GameplayParticles3D.main.prefabBoomBomb);
    }

    public static Particle3dCTRL CreateBoomAll(Transform field, CellCTRL cellStartExplose)
    {

        return CreateParticle(field, cellStartExplose, GameplayParticles3D.main.prefabBoomRocket);
    }

    /// <summary>
    /// ������� ������ ������ ������ � �������� ������ �� ���
    /// </summary>
    public static Particle3dCTRL CreateBoomRocket(Transform field, CellCTRL cellStartExplose) {

        return CreateParticle(field, cellStartExplose, GameplayParticles3D.main.prefabBoomRocket);
    }

    /// <summary>
    /// ������� ������ ������ ������ � �������� ������ �� ���
    /// </summary>
    public static Particle3dCTRL CreateBoomSuperColor(Transform field, CellCTRL cellStartExplose)
    {

        return CreateParticle(field, cellStartExplose, GameplayParticles3D.main.prefabBoomSuperColor);
    }

    public static Particle3dCTRL CreateCellDamage(Transform field, CellCTRL cellStartExplose) {

        return CreateParticle(field, cellStartExplose, GameplayParticles3D.main.prefabCellDamage);
    }

    public static Particle3dCTRL CreateSpawnMold(Transform field, CellCTRL cellStartExplose)
    {

        return CreateParticle(field, cellStartExplose, GameplayParticles3D.main.prefabSpawnMold);
    }

    public static Particle3dCTRL CreateDestroyRock(Transform field, CellCTRL cellStartExplose)
    {

        return CreateParticle(field, cellStartExplose, GameplayParticles3D.main.prefabDestroyRock);
    }

    public static Particle3dCTRL CreateDestroyBox(Transform field, CellCTRL cellStartExplose)
    {

        return CreateParticle(field, cellStartExplose, GameplayParticles3D.main.prefabDestroyBox);
    }
}
