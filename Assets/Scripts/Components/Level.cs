using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
  public float time; //
  public int frequency; //
  public int desiredPercentage; //
  public int randomPerTick; //
  public int patternTileCount; //
  public float eraserFraction;
  public AudioClip theme; //
  public string startingMessage;
  public bool erasorMode; //
  public bool hardcore; //
  public Color desiredColor; //
  public List<Color> otherColours; //
}
