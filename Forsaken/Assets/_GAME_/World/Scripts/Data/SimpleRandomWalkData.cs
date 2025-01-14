using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SimpleRandomWalkParameters_" ,menuName = "PCG/SimpleRandomWalkData")] // Procedural Content Generation (PCG)
public class SimpleRandomWalkData : ScriptableObject 
{
    public int iterations = 10, walkLength = 10;
    public bool startRandomly = true;
}
