using System;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Tile {
  public int x;
  public int y;

  private void OnMouseDown() {
    Debug.Log(String.Format("Setting color to {0}", color));
    Controls.selectedColor = color;
    Controls.show = true;
    Controls.offset = new Vector2Int(x - (Pattern.SIZE - 1) / 2, y - (Pattern.SIZE - 1) / 2);
  }
}
