using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericUtility<T>
{
    //Basically a shortcut to the return stuff here
    public static T RandomFromArray(T[] inputArr){
        return inputArr[Random.Range(0, inputArr.Length)];
    }  

    public static void ForAll(T[] input, System.Action<T> action){
        foreach (T item in input)
        {
            action?.Invoke(item);
        }
    }

    public static IEnumerator ForAllSliced(T[] input, System.Action<T> action, System.Action onEnd){
        foreach (T item in input)
        {
            action?.Invoke(item);
            yield return new WaitForEndOfFrame();
        }
        onEnd?.Invoke();
    }

    public static T GetRandom(T[] inputArr, out int index){
        index = Random.Range(0, inputArr.Length);
        return inputArr[index];
    }  

    public static T GetRandom(List<T> refList){
        int val = Random.Range(0, refList.Count);
        return refList[val];
    }

    public static int GetRandom(T en){
        int index = Random.Range(0, System.Enum.GetNames(typeof(T)).Length);
        return index;
    }
    public static int GetEnumLength(){
        return System.Enum.GetNames(typeof(T)).Length;
    }
    public static void AddToArray(ref T[] initial, T item){
        List<T> list = new List<T>();
        list.AddRange(initial);
        list.Add(item);
        initial = list.ToArray();
    }

    public static T RandomFromArrayNonRep(T[] inputArr, ref int last){
        int now =  (int)Random.Range(0, inputArr.Length);
        if(now == last){
            now += 1;
            if(now >= inputArr.Length){
                now -= 2;   //2 because one was added before
            }
        }
        last = now;
        return inputArr[now];
    }

    public static T RandomFromListRemove(List<T> reference){
        T selection = RandomFromArray(reference.ToArray());
        reference.Remove(selection);
        return selection;
    }

    public static void LogWholeArray(T[] inputArr){
        for (int i = 0; i < inputArr.Length; i++)
        {
            Debug.Log(inputArr[i] + " at index: " + i);
        }
    }

    public static void LogWholeArray(T[] inputArr, string message){
        for (int i = 0; i < inputArr.Length; i++)
        {
            Debug.Log(inputArr[i] + " at index: " + i);
        }
        Debug.Log(message);
    }


    public static void LogWholeArray(T[] inputArr, System.Action<T> further){
        for (int i = 0; i < inputArr.Length; i++)
        {
            Debug.Log(inputArr[i] + " at index: " + i);
            further?.Invoke(inputArr[i]);
        }
    }

    public static T ComponentAction(GameObject target, System.Action<T> onGot){
        T instance = default(T);
        bool success = target.TryGetComponent<T>(out instance);
        if(success){
            onGot?.Invoke(instance);
            instance = target.GetComponent<T>();        //<- guaranteed to work!
        }
        else{
            Debug.Log("WARNING: trid get component of type " + typeof(T).GetType() + " from: " + target + " which didnt work!");
        }
        return instance;
    }

#region NeighbouringIndices
    public static Vector3Int[] NeighnouringIndices3D(T[,,] target, Vector3Int currentIndex, Vector3Int ignoreDimensions){
        List<Vector3Int> indices = new List<Vector3Int>();

        for (int i = -1; i <= 1; i += 2) //two iterations, first i = -1, second i = 1
        {
            int x = 0, y = 0, z = 0;
            if(currentIndex.x + i < target.GetLength(0) && currentIndex.x + i >= 0 && ignoreDimensions.x >= 1){
                x = currentIndex.x + i;
                Vector3Int newX = new Vector3Int(x, currentIndex.y, currentIndex.z);
                indices.Add(newX);
            } 

            if(currentIndex.y + i < target.GetLength(1) && currentIndex.y + i >= 0 && ignoreDimensions.y >= 1){
                y = currentIndex.y + i;
                Vector3Int newY = new Vector3Int(currentIndex.x, y, currentIndex.z);
                indices.Add(newY);
            } 

            if(currentIndex.z + i < target.GetLength(2) && currentIndex.z + i  >= 0 && ignoreDimensions.z >= 1){
                z = currentIndex.z + i;
                Vector3Int newZ = new Vector3Int(currentIndex.x, currentIndex.y, z);
                indices.Add(newZ);
            } 
        }

        return indices.ToArray();
    }    

    public static Vector2Int[] NeighbouringIndices2D(T[,] target, Vector2Int currentIndex, Vector2Int ignoreDimensions){
        List<Vector2Int> indices = new List<Vector2Int>();

        for (int i = -1; i <= 1; i += 2) //two iterations, first i = -1, second i = 1
        {
            //left + right
            int x = 0, y = 0;
            if(currentIndex.x + i < target.GetLength(0) && currentIndex.x + i >= 0 && ignoreDimensions.x >= 1){
                x = currentIndex.x + i;
                Vector2Int newX = new Vector2Int(x, currentIndex.y);
                indices.Add(newX);
            } 

            //down + up
            if(currentIndex.y + i < target.GetLength(1) && currentIndex.y + i >= 0 && ignoreDimensions.y >= 1){
                y = currentIndex.y + i;
                Vector2Int newY = new Vector2Int(currentIndex.x, y);
                indices.Add(newY);
            } 
        }

        return indices.ToArray();
    }

    public static Vector2Int[] NeighbouringIndices2DNineFull(T[,] target, Vector2Int currentIndex){
        return NeighbouringIndices2DNineFull(target, currentIndex, Vector2Int.zero);
    }    

    public static Vector3Int[] NeighbouringIndices2DNineFull3D(T[,,] target, Vector3Int currentIndex, Vector2Int ignoreDimensions){
        List<Vector3Int> indices = new List<Vector3Int>();
        for (int i = -1; i <= 1; i += 2) //two iterations, first i = -1, second i = 1
        {
            //left + right
            int x = 0, y = 0;
            if(currentIndex.x + i < target.GetLength(0) && currentIndex.x + i >= 0 && ignoreDimensions.x >= 1){
                x = currentIndex.x + i;
                Vector3Int newX = new Vector3Int(x, currentIndex.y, 0);
                indices.Add(newX);
            } 

            //down + up
            if(currentIndex.y + i < target.GetLength(1) && currentIndex.y + i >= 0 && ignoreDimensions.y >= 1){
                y = currentIndex.y + i;
                Vector3Int newY = new Vector3Int(currentIndex.x, y, 0);
                indices.Add(newY);
            } 

            //left down + right up
            if(currentIndex.y + i < target.GetLength(1) && currentIndex.y + i >= 0 && ignoreDimensions.y >= 1 && currentIndex.x + i < target.GetLength(0) && currentIndex.x + i >= 0 && ignoreDimensions.x >= 1){
                y = currentIndex.y + i;
                x = currentIndex.x + i;
                Vector3Int newY = new Vector3Int(x, y, 0);
                indices.Add(newY);
            } 

            if(currentIndex.y + i < target.GetLength(1) && currentIndex.y + i >= 0 && ignoreDimensions.y >= 1 && currentIndex.x - i < target.GetLength(0) && currentIndex.x - i >= 0 && ignoreDimensions.x >= 1){
                y = currentIndex.y + i;
                x = currentIndex.x - i;
                Vector3Int newY = new Vector3Int(x, y, 0);
                indices.Add(newY);
            } 
        }

        return indices.ToArray();
    } 

    public static Vector2Int[] NeighbouringIndices2D(T[,] target, Vector2Int currentIndex){
        return NeighbouringIndices2D(target, currentIndex, Vector2Int.one);
    }
    public static Vector2Int[] NeighbouringIndices2DNineFull(T[,] target, Vector2Int currentIndex, Vector2Int ignoreDimensions){
        List<Vector2Int> indices = new List<Vector2Int>();
        for (int i = -1; i <= 1; i += 2) //two iterations, first i = -1, second i = 1
        {
            //left + right
            int x = 0, y = 0;
            if(currentIndex.x + i < target.GetLength(0) && currentIndex.x + i >= 0 && ignoreDimensions.x < 1){
                x = currentIndex.x + i;
                Vector2Int newX = new Vector2Int(x, currentIndex.y);
                indices.Add(newX);
            } 

            //down + up
            if(currentIndex.y + i < target.GetLength(1) && currentIndex.y + i >= 0 && ignoreDimensions.y < 1){
                y = currentIndex.y + i;
                Vector2Int newY = new Vector2Int(currentIndex.x, y);
                indices.Add(newY);
            } 

            //left down + right up
            if(currentIndex.y + i < target.GetLength(1) && currentIndex.y + i >= 0 && ignoreDimensions.y < 1 && currentIndex.x + i < target.GetLength(0) && currentIndex.x + i >= 0 && ignoreDimensions.x < 1){
                y = currentIndex.y + i;
                x = currentIndex.x + i;
                Vector2Int newY = new Vector2Int(x, y);
                indices.Add(newY);
            } 

            if(currentIndex.y + i < target.GetLength(1) && currentIndex.y + i >= 0 && ignoreDimensions.y < 1 && currentIndex.x - i < target.GetLength(0) && currentIndex.x - i >= 0 && ignoreDimensions.x < 1){
                y = currentIndex.y + i;
                x = currentIndex.x - i;
                Vector2Int newY = new Vector2Int(x, y);
                indices.Add(newY);
            } 
        }

        return indices.ToArray();
    }

    public static List<T> NeighbouringT(T[,] inputArr, Vector2Int currentIndex, bool fullNine){
        List<T> neighbours = new List<T>();
        Vector2Int[] indices = fullNine? NeighbouringIndices2DNineFull(inputArr, currentIndex, Vector2Int.zero) : NeighbouringIndices2D(inputArr, currentIndex, Vector2Int.zero);
        for (int i = 0; i < indices.Length; i++)
        {
            neighbours.Add(inputArr[indices[i].x, indices[i].y]);
        }
        return neighbours;
    }

#endregion
}
