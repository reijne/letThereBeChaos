using System;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Tile {
  public int x;
  public int y;
  public static int count;
  int age = 0;
  private void Start() {
    count++;
  }

  private void OnMouseDown() {
    if (Controls.show) return;
    Debug.Log(String.Format("Setting color to {0}", color));
    Controls.selectedColor = color;
    Controls.offset = new Vector2Int(x - (Pattern.SIZE - 1) / 2, y - (Pattern.SIZE - 1) / 2);
    Debug.Log(String.Format("Updating offset to :: {0}", Controls.offset));
    Controls.show = true;
  }
}
