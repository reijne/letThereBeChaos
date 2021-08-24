using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
  public static int SIZE;
  [SerializeField] SpriteRenderer spriteRenderer;
  public Color color;
  public virtual void setColor(Color color) {
    spriteRenderer.color = color;
    this.color = color;
  }
}
