using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class mission5 : MonoBehaviour
{
    [SerializeField] GameObject alarm1;
    [SerializeField] GameObject alarm2;
    [SerializeField] GameObject alarm3;
    [SerializeField] GameObject alarm4;
    [SerializeField] GameObject alarm5;

    void Start()
    {
        alarmMovement(alarm1, 135f, 0f, 0f);
        alarmMovement(alarm2, 135f, 0f, 0f);
        alarmMovement(alarm3, 135f, 0f, 0f);
        alarmMovement(alarm4, 135f, 90f, 90f);
        alarmMovement(alarm5, 135f, 90f, 90f);
    }

    void alarmMovement(GameObject alarm, float x, float y, float z)
    {
        float duration = Random.Range(2f, 4f); 
        float delay = Random.Range(0f, 1.5f);        

        alarm.transform.DORotate(new Vector3(x, y, z), duration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetDelay(delay); // Retraso inicial aleatorio
    }

    void Update()
    {
        
    }
}
