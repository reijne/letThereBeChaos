using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
  public static float SIZE;
  [SerializeField] SpriteRenderer spriteRenderer;
  public Board board;
  public Vector2 pos;
  public Color color;
  private void Start() {
    transform.localScale = new Vector3(SIZE, SIZE, SIZE);
  }
  public virtual void setColor(Color color) {
    spriteRenderer.color = color;
    this.color = color;
  }

  private void OnMouseDown() {
    if (Pattern.selected.color != null) board.spawnPlant(pos, Pattern.selected.color);
  }
}
