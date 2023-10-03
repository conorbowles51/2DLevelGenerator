## Introduction

This project is a tool for Unity's 2D workspace that allows users
to create procedurally generated levels in the editor. It accepts a preset
that will define the style of the level and then generates it and places tiles
in the scene.

## Using the tool
![dungeongeneratorSS](https://github.com/conorbowles51/2DLevelGenerator/assets/143211735/517061f9-1c21-4067-8670-6fd56332a2f2)

To use the tool, simply attach the script to an empty object, configure the settings how you like, and hit the generate button at the bottom.
You can create your own preset or use one of the three already provided.

Here are some of the results with different settings and presets:

![image](https://github.com/conorbowles51/2DLevelGenerator/assets/143211735/3c08cccd-24ec-419a-be19-5930479c7645)
![image](https://github.com/conorbowles51/2DLevelGenerator/assets/143211735/f0f02d36-0718-4c21-81dd-72bfddc4e3da)
![image](https://github.com/conorbowles51/2DLevelGenerator/assets/143211735/7175a6cd-1e6c-44c5-8fb2-83316bde766a)

## How it works

The main algorithm used in this project is the simple random walk algorithm. It takes a start position, moves one unit in a random direction, then repeats
this process for the duration of the walk length.

```
public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPosition);
        Vector2Int previousPosition = startPosition;

        for(int i = 0; i < walkLength; i++)
        {
            Vector2Int newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }

        return path;
    }
```
