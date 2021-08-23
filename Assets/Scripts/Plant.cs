using System;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Tile
{
  public int age;
  public int cycleTime;
  private int currentStage = 0;
  public int gSize = 1;
  // [green, yellow, red, DEATH]
  
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
  private List<Color> stageColours = new List<Color>() {
    Color.green, Color.yellow, Color.red, Color.black
  };


  public void tick() {
    age++;
    checkChangeColor();
  }  

  void checkChangeColor() {
    Debug.Log(String.Format("Age:: {0}, CycleTime:: {1}", age, cycleTime));
    // age :: 3
    // 4 stages
    // cycle 2
    // 3 / 4
    if (age % cycleTime == 0) { // Next stage
      currentStage = (currentStage+1) % stageColours.Count;
      setColor(stageColours[currentStage]);
    }

    // age %= (stageColours.Count * cycleTime);
    if (age >= stageColours.Count * cycleTime) Destroy(this.gameObject);
  }

  // How is the area supposed to look according to your growthPattern
  // public 
}
