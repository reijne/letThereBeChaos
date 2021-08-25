using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
  public static float SIZE;
  [SerializeField] SpriteRenderer spriteRenderer;
  public Color color;
  private void Start() {
    transform.localScale = new Vector3(SIZE, SIZE, SIZE);
  }
  public virtual void setColor(Color color) {
    spriteRenderer.color = color;
    this.color = color;
  }
}
