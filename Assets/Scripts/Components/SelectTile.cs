using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTile : Tile {
  public static Color normalColor = Color.gray;
  public int x;
  public int y;
  public bool selected = false;
  public Color chosenColor = Color.white;

  private void OnMouseDown() {
    toggle();
  }

  public void choseColor(Color color) {
    chosenColor = color;
  }

  public void toggle() {
    selected = !selected;
    if (selected) setColor(chosenColor);
    else setColor(normalColor);
  }

  public void select() {
    selected = true;
    setColor(chosenColor);
  }
}
