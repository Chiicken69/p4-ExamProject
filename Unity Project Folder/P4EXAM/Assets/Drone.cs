using System.Collections;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private bool _carryingItem;

    //private bool moveObjectSmoothIsRunning;
    //private bool _timeElapsed

    [SerializeField] private Sprite _sprite;

    private void Start()
    {
        StartCoroutine(MoveDrone(new Vector2(0, 0), new Vector2(5, 5)));
    }

    IEnumerator  MoveDrone(Vector2 PositionA, Vector2 PositionB)
    {
        Debug.Log("MoveDrone Routine Started");
        bool _moveObjectSmoothIsRunning = true;
        float _timeElapsed = 0f;
        Vector3 pos = new Vector3();
        float durationByDistance;


        durationByDistance = Vector2.Distance(PositionA, PositionB);
        durationByDistance = durationByDistance / _speed;

        while (_timeElapsed < durationByDistance)
        {
           // pos = Vector2.Lerp(PositionA, PositionB, _timeElapsed / durationByDistance);
            pos.x = Mathf.SmoothStep(PositionA.x, PositionB.x, _timeElapsed / durationByDistance);
            pos.y = Mathf.SmoothStep(PositionA.y, PositionB.y, _timeElapsed / durationByDistance);
            print("CoroutinePOSITION: " + pos);
            
            this.transform.position = pos;

            _timeElapsed += Time.deltaTime;
            yield return null;


        }

       // coroutine = null;
        Debug.Log("MoveObjRoutineSTOPPED");

    }
            
}
