using System;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Tile {
  public static int count;
  public bool isAlive = true;
  int age = 0;
  private void Start() {
    count++;
  }

  public void tick() {
    age++;
    if (age == board.lifeTime) {
      kill();
    } else if (age >= board.deathTimeMult * board.lifeTime) {
      remove();
    }
  }

  public void revive(Color color) {
    isAlive = true;
    age = 0;
    setColor(color);
    gameObject.SetActive(true);
  }

  private void kill() {
    // gameObject.SetActive(false);
    setColor(new Color(0, 0, 0, 0));
    isAlive = false;
  }

  private void remove() {
    board.removePlant(pos);
  }

  private void OnMouseDown() {
    remove();
    if (Pattern.selected != null && pos != null) board.spawnPlant(pos, Pattern.selected.color);
  }
}
