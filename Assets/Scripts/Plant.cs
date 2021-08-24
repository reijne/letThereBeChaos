using System;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Tile {
  public int gSize;
  public Color color;

  private void Start() {
    setColor(color);
  }
  // -1,-1 : true
  // -1, 9 : false
  public Dictionary<(int, int), bool> growthPattern = new Dictionary<(int, int), bool>() {
    {(-1, -1), true},
    {(-1,  0), true},
    {(-1,  1), true},
    {( 0, -1), true},
    {( 0,  0), true},
    {( 0,  1), true},
    {( 1, -1), true},
    {( 1,  0), true},
    {( 1,  1), true},
  };

  private void OnMouseDown() {
    Controls.selectedColor = color;
    Controls.show();
  }
}
