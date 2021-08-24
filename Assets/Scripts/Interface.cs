using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour {
  [SerializeField] Transform indicatorSpawnpoint;
  [SerializeField] Image totalImage;
  public Dictionary<Color, int> colorCounts = new Dictionary<Color, int>();

  public void updateColorIndicators(int total) {
    // get width 
  }

}
