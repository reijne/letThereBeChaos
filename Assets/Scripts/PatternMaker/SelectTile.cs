using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTile : Tile {
  public int x;
  public int y;
  public bool selected = false;
  public Color chosenColor = Color.white;
  private void Start() {
    setColor(Color.gray);
  }

  private void OnMouseDown() {
    toggle();
  }

  public void choseColor(Color color) {
    chosenColor = color;
  }

  public void toggle() {
    selected = !selected;
    if (selected) setColor(chosenColor);
    else setColor(Color.gray);
  }
}
