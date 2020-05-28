using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : MonoBehaviour
{
    public GameObject queenPrefab;
    public GameObject rookPrefab;
    public GameObject knightPrefab;

    private Transform queen;
    private Transform rook;
    private Transform knight;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        queen = queenPrefab.transform;
        rook = rookPrefab.transform;
        knight = knightPrefab.transform;
        queen.transform.position = new Vector3(0.0f, 0.0f, 0f);
        rook.transform.position = new Vector3(0.0f, 0.0f, 0f);
        knight.transform.position = new Vector3(0.0f, 0.0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
    }
}