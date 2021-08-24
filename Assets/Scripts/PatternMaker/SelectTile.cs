using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTile : Tile {
  public int x;
  public int y;
  public bool selected = false;
  public Color color;
  private void Start() {
    setColor(Color.gray);
  }

  private void OnMouseDown() {
    toggle();
  }

  public void toggle() {
    selected = !selected;
    if (selected) setColor(Color.white);
    else setColor(Color.gray);
  }
}
