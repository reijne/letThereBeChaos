using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
  public static int SIZE;
  [SerializeField] SpriteRenderer spriteRenderer;

  protected void setColor(Color color) {
    spriteRenderer.color = color;
  }
}
