using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Queen.transform.position = transform.position(QueenStartX, QueenStartY, QueenStartZ);
        Rook.transform.position = transform.position(RookStartX, RookStartY, RookStartZ);
        Knight.transform.position = transform.position(RookStartX, RookStartY, RookStartZ);
    }

    // Update is called once per frame
    void Update()
    {
    }
}