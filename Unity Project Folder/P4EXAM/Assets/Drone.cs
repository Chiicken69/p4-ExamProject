using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public List<Vector2> flagPoints = new List<Vector2>();
    public List<GameObject> flagObjects = new List<GameObject>();
    public GameObject flagPrefab;
    public int maxFlagCount = 3;
    public float moveSpeed = 2f;

    void Start()
    {
        StartCoroutine(PatrolFlags());
    }

    IEnumerator PatrolFlags()
    {
        while (true)
        {
            if (flagPoints.Count == 0)
            {
                yield return null;
                continue;
            }

            for (int i = 0; i < flagPoints.Count; i++)
            {
                Vector2 targetPos = flagPoints[i];
                while (Vector2.Distance(transform.position, targetPos) > 0.1f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
        }
    }
}
