using System;
using System.Collections.Generic;
using UnityEngine;

public class Pattern {
  public static int SIZE = 3;
  public Dictionary<(int, int), bool> selectedTiles = new Dictionary<(int, int), bool>();
  public string toString() {
    Debug.Log(selectedTiles.Count);
    string pattern = "";
    foreach (KeyValuePair<(int, int), bool> kvp in selectedTiles) {
      pattern += String.Format("({0},{1}) : {2}\n", kvp.Key.Item1, kvp.Key.Item2, kvp.Value);
    }
    return pattern;
  }
}
