using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interfaze : MonoBehaviour {
  [SerializeField] Transform indicatorSpawnpoint;
  [SerializeField] Image totalImage;
  public Dictionary<Color, int> colorCounts = new Dictionary<Color, int>();

  public void updateColorIndicators(int total) {
    // get width 
  }

  public void addColorCount(Color color) {
    if (colorCounts.ContainsKey(color)) colorCounts[color]++;
    else colorCounts[color] = 1;
  }

  private void Update() {
    printCounts();
  }
  public void printCounts() {
    foreach (KeyValuePair<Color, int> kvp in colorCounts) {
      Debug.Log(String.Format("{0} : {1}", kvp.Key, kvp.Value));
    }
  }

}
