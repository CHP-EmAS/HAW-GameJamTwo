using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//Not Namspace'd
public enum Axis    //yes, vector3.zero, vector3.right etc. would also work, but this is easier to understand. Currenty Unused lmao
{
    ZERO,
    X,
    Y,
    Z,
    XY,
    XZ,
    YZ,
    XYZ,
}
public enum UpdateMode{
    UPDATE = 0,
    FIXEDUPDATE = 1,
    NONE = 3,
}
public enum InjectionPoint{
    AWAKE,
    START,
    UPDATE,
}
public enum LoopBehaviour{
    NONE,
    BREAK,
    CONTINUE,
}
//Used to have "2D"-Dictionaries
public struct Key2D<T, A>{
    public readonly T keyA;
    public readonly A keyB;
    public Key2D(T inT, A inA){
        keyA = inT;
        keyB = inA;
    }
}
//https://forum.unity.com/threads/passing-ref-variable-to-coroutine.379640/
//Use this to pass variables by reference in whatever
//.... I miss pointers xD
public class Ref<T>{
    public T value;
    public Ref(T reference){
        value = reference;
    }
    public Ref(){}
}


public static class Utility
{
    public static bool InRange<T>(this int n, T[] input){
        return !((n < 0) || (n >= input.Length));
    }

    public static void ClampArr<T>(this int n, T[] input){
        n = Mathf.Clamp(n, 0, input.Length - 1);
    }
    public static bool UseComponentSafe<T>(this GameObject target, out T comp){
        T component;
        target.TryGetComponent<T>(out component);
        comp = component;
        return component != null;
    }

    public static T GetComponentSafe<T>(this GameObject tar){
        T comp;
        tar.TryGetComponent<T>(out comp);
        return comp;
    }

    public static Vector3 WorldToSpritePosition(Vector3 worldPos, SpriteRenderer renderer){
        //Vector2 closestPoint = renderer.bounds.ClosestPoint((Vector2)worldPos);
        Vector2 localPositionInPixels = (Vector2)renderer.transform.InverseTransformPoint(worldPos) * renderer.sprite.pixelsPerUnit + renderer.sprite.pivot;
        return localPositionInPixels;
    }


    public static Vector2[] extrustion = new Vector2[9]{
        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(-1, 1),
        new Vector2(-1, 0),
        new Vector2(-1, -1),
        new Vector2(0, -1),
        new Vector2(1, -1),
        new Vector2(1, 0),
        new Vector2(1, 1),
    };
    public static Vector3 ReadAxis(Axis input){
        Vector3 result;
        switch (input)
        {
            default:
            result = Vector3.zero;
            break;

            case Axis.ZERO:
            result = Vector3.zero;
            break;

            case Axis.X:
            result = new Vector3(1, 0, 0);
            break;

            case Axis.Y:
            result = new Vector3(0, 1, 0);
            break;

            case Axis.Z:
            result = new Vector3(0, 0, 1);
            break;

            case Axis.XY:
            result = new Vector3(1, 1, 0);
            break;

            case Axis.YZ:
            result = new Vector3(0, 1, 1);
            break;

            case Axis.XZ:
            result = new Vector3(1, 0, 1);
            break;

            case Axis.XYZ:
            result = new Vector3(1, 1, 1);
            break;
        }
        return result;
    }
    public delegate void ArgumentelessDelegate();
    public delegate void BoolDelegate(bool x);
    public delegate void CharDelegate(char x);
    public delegate void IntDelegate(int x);
    public delegate void FloatDelegate(float x);
    public delegate void GameObjectDelegate(GameObject x);
    public delegate void SpriteDelegate(Sprite x);
    public delegate void TransformDelegate(Transform x);
    public delegate void VectorDelegate(Vector3 x);
    public delegate void Vector2Delegate(Vector2 x);
    public delegate void GenericDelegate<T>(T value);
    public delegate void ActionRef<T>(ref T value);

    public delegate bool BoolRetDelegate();
    //Misc
    public static LayerMask layermask_Empty;

    //A Method to play an audiosource with more control and reduced repetition
    public static void PlayDetailed(this AudioSource audioSource, AudioClip clip, float originalPitch, float pitchModification, float delay, bool loop, bool timeModification)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;

        if (timeModification)
        {
            audioSource.pitch = originalPitch + UnityEngine.Random.Range(-pitchModification, pitchModification) * Time.timeScale;
        }
        else
        {
            audioSource.pitch = originalPitch +  UnityEngine.Random.Range(-pitchModification, pitchModification);
        }


        audioSource.PlayDelayed(delay);
    }


    //A Method to play an audiosource easier, when multiple audiosources are available
    public static void PlayFast(this AudioSource audioSource, AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.pitch = ( UnityEngine.Random.Range(0.9f, 1.1f)) * Time.timeScale;
        audioSource.Play();
    }

    public static void PlayFastNoScale(this AudioSource audioSource, AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.pitch = ( UnityEngine.Random.Range(0.9f, 1.1f));
        audioSource.Play();
    }

    public static void PlayFastOneShotSafe(this AudioSource source, AudioClip clip){
        if(clip == null) return;
        source.pitch = ( UnityEngine.Random.Range(0.9f, 1.1f)) * Time.timeScale;
        source.PlayOneShot(clip);
    }

    public static void PlayFast(this AudioSource audioSource)
    {
        audioSource.pitch = ( UnityEngine.Random.Range(0.9f, 1.1f)) * Time.timeScale;
        audioSource.Play();
    }

    public static void PlayFastDelayed(this AudioSource audioSource, AudioClip clip, float delay){
        audioSource.clip = clip;
        audioSource.pitch = ( UnityEngine.Random.Range(0.9f, 1.1f)) * Time.timeScale;
        audioSource.PlayDelayed(delay);
    }


    // A simple Chance for if statements
    public static bool Chance(int chance)
    {
        int n =  Mathf.RoundToInt(UnityEngine.Random.value * 100);

        if (n <= chance) return true;
        else return false;
    }


    //Lerp but float-inaccuracy is taken in
    public static Vector3 SmoothLerp(Vector3 from, Vector3 target, float valueBuffer, float speed)
    {

        if (Vector3.Distance(from, target) > valueBuffer)
        {
            from = Vector3.Lerp(from, target, speed * 50 * Time.deltaTime);
        }
        else
        {
            from = target;
        }

        return from;
    }

    //Lerp but float-inaccuracy is taken in
    public static float SmoothLerpValue(float from, float target, float valueBuffer, float speed)
    {

        if (Mathf.Abs(from - target) > valueBuffer)
        {
            from = Mathf.Lerp(from, target, speed * 50 * Time.deltaTime);
        }
        else
        {
            from = target;
        }

        return from;
    }

    //Inaccurate wobbly lerp
    //detaltimedSpeed: //https://stackoverflow.com/questions/43720669/lerp-with-time-deltatime
    public static float DeltaTimedLerpT(float speed, int frameRateBase){
        return 1 - MathF.Pow(1 - (speed + .1f) * 1.5f, Time.deltaTime * frameRateBase);
    }
    public static float WobbleLerp(float from, float target, ref float tracker, float speed, float intensity, float dt){
        float x = Mathf.Lerp(tracker, (target - from), dt * speed);
        tracker = x;
        return from + x * intensity * dt;
    }

    public static Vector3 WobbleLerpV3(Vector3 from, Vector3 target, ref Vector3 tracker, float speed, float intensity, float dt){
        Vector3 x = Vector3.Lerp(tracker, (target - from), dt * speed);
        tracker = x;
        return from + x * intensity * dt;
    }

    public static Quaternion WobbleLerpLookRotation(ref Vector3 from, Vector3 targetDir, ref Vector3 tracker, float speed, float intensity, float dt){
        Vector3 nDir = targetDir.normalized;
        from = Utility.WobbleLerpV3(from, nDir, ref tracker, speed, intensity, dt);

        return Quaternion.LookRotation(from);
    }

    
    public static Vector3 WobbleLerpVector(Vector3 from, Vector3 target, ref Vector3 tracker, float speed, float intensity, Axis axis, float dt){      
        float deltatime = dt * 120;
        float deltatimedSpeed = deltatime;      //<- deleted DeltaTimedLerpT

        Vector3 x = Vector3.Lerp(tracker, (target - from), deltatimedSpeed);
        tracker = x;
        Vector3 axisApplication = ReadAxis(axis);
        Vector3 rawResult = from + x * intensity * deltatime;
        Vector3 result = new Vector3(rawResult.x * axisApplication.x, rawResult.y * axisApplication.y, rawResult.z * axisApplication.z);
        return result;
    }

    //v this does not work qwq
    public static Quaternion WobbleLerpQuaternion(Quaternion from, Quaternion target, ref Quaternion tracker, float speed, float intensity){
        float deltaTime = Time.deltaTime * 120;
        //https://forum.unity.com/threads/subtracting-quaternions.317649/
        Quaternion x = Quaternion.Lerp(tracker, (target * Quaternion.Inverse(target)), deltaTime);
        tracker = x;
        return from * (x * Quaternion.LerpUnclamped(Quaternion.identity, x, intensity * deltaTime));
    }
   
    //returns all children of the object
    public static GameObject[] GetAllChildren(this Transform transform)
    {
        List<GameObject> childList = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            childList.Add(transform.GetChild(i).gameObject);
        }

        return childList.ToArray();
    }

    public static Transform[] GetAllChildrenTF(this Transform transform)
    {
        List<Transform> childList = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            childList.Add(transform.GetChild(i));
        }

        return childList.ToArray();
    }

    
    public static void DestroyAllChildren(this Transform transform, bool areYouSure)
    {
        if(!areYouSure) return;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public static void DestroyAllChildren(this Transform transform, params int[] ignore)
    {
        List<int> ign = new List<int>();
        ign.AddRange(ignore);
        List<GameObject> toDestroy = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if(ign.Contains(i)) continue;
            toDestroy.Add(transform.GetChild(i).gameObject);
        }

        GenericUtility<GameObject>.ForAll(toDestroy.ToArray(), x => GameObject.DestroyImmediate(x));
    }


    //returns a random integer
    public static int RandomNumber(int length)
    {
        int num;
        num =  UnityEngine.Random.Range(0, length);
        return num;
    }


    //Changes the hue of an color
    public static Color ChangeHue(this Color OriginalColorRGB, float H)
    {
        float localH, localS, localV;
        Color.RGBToHSV(OriginalColorRGB, out localH, out localS, out localV);

        localH += H;
        localH = Mathf.Clamp01(localH);

        return Color.HSVToRGB(localH, localS, localV);
    }

    public static Color SetHue(this Color OriginalColorRGB, float H)
    {
        float localH, localS, localV;
        Color.RGBToHSV(OriginalColorRGB, out localH, out localS, out localV);

        localH = H;
        localH = Mathf.Clamp01(localH);

        return Color.HSVToRGB(localH, localS, localV);
    }

    public static Color ChangeSaturation(this Color OriginalColorRGB, float S)
    {
        float localH, localS, localV;
        Color.RGBToHSV(OriginalColorRGB, out localH, out localS, out localV);

        localS += S;
        localS = Mathf.Clamp01(localS);

        return Color.HSVToRGB(localH, localS, localV);
    }

    public static Color SetSaturation(this Color OriginalColorRGB, float S)
    {
        float localH, localS, localV;
        Color.RGBToHSV(OriginalColorRGB, out localH, out localS, out localV);

        localS = S;
        localS = Mathf.Clamp01(localS);

        return Color.HSVToRGB(localH, localS, localV);
    }

    public static Color ChangeValue(this Color OriginalColorRGB, float V)
    {
        float localH, localS, localV;
        Color.RGBToHSV(OriginalColorRGB, out localH, out localS, out localV);

        localV += V;
        localV = Mathf.Clamp01(localV);

        return Color.HSVToRGB(localH, localS, localV);
    }

    
    public static Color SetValue(this Color OriginalColorRGB, float V)
    {
        float localH, localS, localV;
        Color.RGBToHSV(OriginalColorRGB, out localH, out localS, out localV);

        localV = V;
        localV = Mathf.Clamp01(localV);

        return Color.HSVToRGB(localH, localS, localV);
    }

    public static Color ToHSV(this Color OriginalColorRGB)
    {
        float localH, localS, localV;
        Color.RGBToHSV(OriginalColorRGB, out localH, out localS, out localV);

        return new Color(localH, localS, localV);
    }

    public static Color ToRGB(this Color originalColorHSV){
      return Color.HSVToRGB(originalColorHSV.r, originalColorHSV.g, originalColorHSV.b);
    }

    //rotate towards an object
    public static void RotateTowardsMath(this Transform input, Vector3 towards, float offset)
    {
        float rotZ;

        Vector3 difference = towards - input.position;
        rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        input.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
    }

    public static void RotateTowards(this Transform input, Vector3 towards, float speed){
        Vector3 lookDir = Vector3.RotateTowards(input.forward, towards - input.position, speed * Time.deltaTime, 0f);
        input.rotation = Quaternion.LookRotation(lookDir);
    }

    public static void RotateTowardsSmoothDamp(this Transform input, Vector3 origin, Vector3 axesMultiplication, float angles, float length){
        Vector3 targetAngles = (new Vector3(angles * axesMultiplication.x, angles * axesMultiplication.y, angles * axesMultiplication.z));
        Vector3 smooth = axesMultiplication;
        Vector3 finalEuler = Vector3.SmoothDamp(input.localEulerAngles, origin + targetAngles, ref smooth, length);
        input.localRotation = Quaternion.Euler(finalEuler);
        Debug.Log(targetAngles + "   " + finalEuler + "   " + angles);
    }

    public static void RotateTowardsSmoothDamp(this Transform input, Vector3 axesMultiplication, float angles, float length){
        Vector3 targetAngles = (new Vector3(angles * axesMultiplication.x, angles * axesMultiplication.y, angles * axesMultiplication.z));
        Vector3 smooth = axesMultiplication;
        Vector3 finalEuler = Vector3.SmoothDamp(input.localEulerAngles, targetAngles, ref smooth, length);
        input.localRotation = Quaternion.Euler(finalEuler);
        Debug.Log(targetAngles + "   " + finalEuler + "   " + angles);
    }

    //set framerate independent
    public static float TimeMultiplication()
    {
        float returnValue = Time.deltaTime * 50;
        return returnValue;
    }

    //change the timescale smoothly
    public static void SetTime(float mutliplicator)
    {
        Time.timeScale = mutliplicator;
        if(mutliplicator != 0)Time.fixedDeltaTime = 0.02f * mutliplicator;
    }


    //Sinus Time
    public static float SinTime(float multiplier)
    {
        return Mathf.Sin(Time.time * multiplier);
    }


    //Bool but with custom buffer for floating point inaccuracy
    public static bool EqualsAround(float inValue, float targetValue, float valueBuffer)
    {
        if(Mathf.Abs(inValue - targetValue) < valueBuffer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //warning: for some reason resolution may be halved
    public static Texture2D AsNewTexture(this Sprite s){
        Texture2D product = new Texture2D(s.texture.width, s.texture.height);
        //Texture2D product = new Texture2D((int)s.rect.width, (int)s.rect.height);
        product.filterMode = FilterMode.Point;
        //Color[] pixels = s.texture.GetPixels((int)s.textureRect.x, (int)s.textureRect.y, (int)s.textureRect.width, (int)s.texture.height);
        Color[] pixels = s.texture.GetPixels();
        product.SetPixels(pixels);
        product.Apply();
        return product;
    }
    public static Texture2D FromTexture(Texture2D original){
        Texture2D toReturn = new Texture2D(original.width, original.height);
        toReturn.SetPixels(original.GetPixels());
        toReturn.Apply();
        return toReturn;
    }
    public static Texture2D ToTex2D(this RenderTexture tex)
    {
        Texture2D target = new Texture2D(tex.height, tex.width, TextureFormat.ARGB32, false);
        RenderTexture.active = tex;
        target.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        target.Apply();
        target.filterMode = FilterMode.Point;
        return target;
    }

    public static Vector2 GetTexelSize(this Texture2D tex){
        return new Vector2(1.0f / tex.width, 1.0f / tex.height);
    }

    public static Sprite ToSprite(this Texture2D tex){
        Sprite s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        s.name = tex.name;
        return s;
    }

    public static Sprite ToSprite(this Texture2D tex, Vector2 pivot){
        Vector2 texelSize = tex.GetTexelSize();
        Sprite s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.Scale(pivot, texelSize));
        s.name = tex.name;
        return s;
    }

    public static RenderTexture ToRenderTex(this Texture2D tex, bool allowRandomWrite){
        RenderTexture rt = new RenderTexture(tex.width, tex.height, 0);
        rt.enableRandomWrite = allowRandomWrite;
        RenderTexture.active = rt;
        Graphics.Blit(tex, rt);
        rt.filterMode = FilterMode.Point;
        return rt;
    }

    public static int Smallest(int[] input){
        List<int> intList = new List<int>();
        for (int i = 0; i < input.Length; i++)
        {
            intList.Add(input[i]);
        }
        intList.Sort();
        return intList[0];
    }

    public static int SmallestIndex(int[] input, out int index){
        List<int> intList = new List<int>();
        List<int> cachedList = new List<int>();
        for (int i = 0; i < input.Length; i++)
        {
            intList.Add(input[i]);
            cachedList.Add(input[i]);
        }

        cachedList.Sort();
        index = intList.IndexOf(cachedList[0]);

        return cachedList[0];
    }

    public static int[] Sort(this int[] input){

        List<int> toSort = new List<int>();
        for (int i = 0; i < input.Length; i++)
        {
            toSort.Add(input[i]);
        }
        return toSort.ToArray();
    }

    public static int[] SmallestIndexes(int[] input, out int[] index){
        List<int> intList = new List<int>();
        List<int> cachedList = new List<int>();
        for (int i = 0; i < input.Length; i++)
        {
            intList.Add(input[i]);
            cachedList.Add(input[i]);
        }

        cachedList.Sort();
        index = new int[cachedList.Count];
        for (int y = 0; y < cachedList.Count; y++)
        {
            index[y] = intList.IndexOf(cachedList[y]);
        }      

        return cachedList.ToArray();
    }

    public static Gradient DefineGradient(Color start, Color end){
        
        Gradient target = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = start;
        colorKeys[0].time = 0f;

        colorKeys[1].color = end;
        colorKeys[1].time = 1f;


        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1;
        alphaKeys[0].time = 0f;

        alphaKeys[1].alpha = 1;
        alphaKeys[1].time = 1f;


        target.SetKeys(colorKeys, alphaKeys);

        return target;
    }

    public static Vector2 CoreMovement(){
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public static int OneOrMinusOne(){
        return  UnityEngine.Random.Range(0, 2) * 2 - 1;
    }

    public static Quaternion RotZero(){
        return Quaternion.Euler(new Vector3(0, 0, 0));
    }

    public static Quaternion RotYRand(Vector3 originalEuler){
        return Quaternion.Euler(originalEuler + new Vector3(0,  UnityEngine.Random.Range(0, 360), 0));
    }

    //toSet should be this
    public static float WobbleLerp(float target, float toSet, float tracker, float speed){
        return toSet + Mathf.Lerp(tracker, (target - toSet), speed * Time.deltaTime * 100);
    }

    public static Vector2Int FindIndex2D<T>(T[,] matrix, T value){

        int xArr = matrix.GetLength(0);
        int yArr = matrix.GetLength(1);

        for (int x = 0; x < xArr; x++)
        {
            for (int y = 0; y < yArr; y++)
            {
                if(matrix[x, y].Equals(value)){
                    return new Vector2Int(x, y);
                }
            }
        }

        return Vector2Int.zero;
    }

    public static float Saturate(float input){
        return Mathf.Clamp(input, 0, 1);
    }

    public static float MaxSaturate(float input){
        return Mathf.Clamp(input, -1, 1);
    }


    public static RaycastHit RayHit(Vector3 origin, Vector3 direction, float length, LayerMask mask){
        RaycastHit cached;
        Physics.Raycast(origin, direction, out cached, length, mask);
        return cached;
    }

    //put this in update!

    public static void CountDownAction(ref float input, System.Action toExecute){
        if(input > 0){
            input -= Time.deltaTime;
        }
        else if (input <= 0){
            toExecute?.Invoke();
            input = 0;
        }
    }

    public static void FlagCondition(bool condition, ref bool flag, System.Action onChangeTrue, System.Action onChangeFalse){
        if(condition){
            if(flag){
                onChangeTrue?.Invoke();
                flag = false;
            }
        }
        else{
            if(!flag){
                onChangeFalse?.Invoke();
                flag = true;
            }
        }
    }

    public static string RandomFromArray(this string[] input){
    return input[UnityEngine.Random.Range(0, input.Length)];
    }

    public static Vector3 SmoothStepV3(Vector3 from, Vector3 to, float t){
        float x, y, z;
        x = Mathf.SmoothStep(from.x, to.x, t);
        y = Mathf.SmoothStep(from.y, to.y, t);
        z = Mathf.SmoothStep(from.z, to.z, t);
        return new Vector3(x, y, z);
    }

    public static float PercentageDistance(Vector3 A, Vector3 B, Vector3 current){
        Vector3 pt = A + B;
        pt.x = current.x / pt.x;
        pt.y = current.y / pt.y;
        pt.z = current.z / pt.z;
        float percentage = (pt.x + pt.y + pt.z) / 3;
        return percentage;
    }

    public static int StepInt(float x, float y){
        if(x > y){
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public static int Rounded05(float toRound){
        if(toRound % 1 <= .5f) return Mathf.FloorToInt(toRound);
        else return Mathf.CeilToInt(toRound);
    }

    public static IEnumerator CurveInterpolation(float speed, AnimationCurve curve, System.Action<float> onChangeValue){
        float timer = 0;
        while(timer < 1){
            timer += Time.unscaledDeltaTime * speed;
            onChangeValue?.Invoke(curve.Evaluate(timer));
            yield return new WaitForEndOfFrame();
        }
        onChangeValue?.Invoke(curve.Evaluate(1));
    }

    public static IEnumerator CurveInterpolationTimed(float speed, AnimationCurve curve, System.Action<float> onChangeValue){
        float timer = 0;
        while(timer < 1){
            timer += Time.deltaTime * speed;
            onChangeValue?.Invoke(curve.Evaluate(timer));
            yield return new WaitForEndOfFrame();
        }
        onChangeValue?.Invoke(curve.Evaluate(1));
    }

    private const float soundSpeed = 343.2f; //https://de.wikipedia.org/wiki/Schallgeschwindigkeit
    public static float SoundDistance(Vector3 origin, Vector3 audioListenerPos){
        return Vector3.Distance(origin, audioListenerPos) / soundSpeed;
    }

    public static void CircleAroundYAxis(this Transform transform, Vector3 pivot, float t, float range){
        float x = Mathf.Cos(t) * range;
        float z = Mathf.Sin(t) * range;

        transform.position = pivot + new Vector3(x, 0, z);
    }

    public static void CircleAroundYAxisSmoothed(this Transform transform, Vector3 pivot, float t, float range){
        float x = Mathf.Cos(t) * range;
        float z = Mathf.Sin(t) * range;

        transform.position = Vector3.Lerp(transform.position, pivot + new Vector3(x, 0, z), .1f * TimeMultiplication());
    }

    public static bool IsInBounds(Vector2 check, Vector2 posClamp, Vector2 negClamp){
        return check.x >= negClamp.x && check.x <= posClamp.x && check.y >= negClamp.y && check.y <= posClamp.y;    //basically "isclamped"?
    }

    //https://stackoverflow.com/questions/17764680/check-if-a-character-is-a-vowel-or-consonant
    public static bool IsVowel(this char c){
        string check = "aeiouAEIOUöüäÖÜÄ";
        return check.Contains(c.ToString());
    }

    //https://forum.unity.com/threads/terrain-vertice-information.43235/    ???This is not for Sign0


    public static int Sign0(float input){
        if(input != 0){
            return (int)Mathf.Sign(input);
        }
        else
        {
            return 0;
        }
    }

    public static IEnumerator TypeString(TMPro.TextMeshProUGUI text, string target, float timeBetweenChars, bool resetTextOnStart, System.Action<int> onCharTyped, System.Action onStringTyped){
        if(resetTextOnStart) text.text = "";
        for (int i = 0; i < target.Length; i++)
        {
            yield return new WaitForSeconds(timeBetweenChars);
            text.text += target[i];
            onCharTyped?.Invoke(i);
        }
        onStringTyped?.Invoke();
    }

    public static void DestroyAllGO(bool areYouSure){
        if(!areYouSure) return;

        GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();
        for (int i = 0; i < gos.Length; i++)
        {
            GameObject.Destroy(gos[i]);
        }
    }

    public static Vector2 CorrectedMousePosition(){
        return Input.mousePosition;
    }  

    public static Vector3 PlaneMousePosition(Camera cam, float depthZ, Vector3 planeNormal){
        Camera refCam = cam;
        Ray ray = refCam.ScreenPointToRay(Input.mousePosition);
        Plane refPlane = new Plane(planeNormal.normalized, new Vector3(0, 0, depthZ));
        float dst;
        refPlane.Raycast(ray, out dst);
        return ray.GetPoint(dst);
    }

    public static float Posterize(float input, float steps){
        return Mathf.Floor(input / (1.0f / steps)) * (1.0f / steps);
    }

    public static float TileValue(float value, float maxValue, uint steps){
        float ret = 0;
        for (int i = 0; i < steps; i++)
        {
            if(value <= maxValue * i/steps){
                ret = maxValue * i/steps;
                break;
            }
        }
        return ret;
    }

    //https://stackoverflow.com/questions/1531695/round-to-nearest-five
    public static float TileValue2(float value, float rounder){
        return Mathf.Round(value / rounder) * rounder;
    }
    
    public static float Angle(this Vector2 dir){
        return Mathf.Atan2(dir.y, dir.x);
    }

    public static Color RandomColor(float alpha){
        return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), alpha);
    }

    public static Vector3 TiledDirection(Vector3 dir, float amount){
        Vector2 angleV = new Vector2(dir.x, dir.z);
        float angle = angleV.Angle();
        float degAngle = angle * Mathf.Rad2Deg;
        degAngle = TileValue2(degAngle, amount);

        angle = degAngle * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(angle), dir.y, Mathf.Sin(angle)).normalized * dir.magnitude;
    }

    public static Vector3 TiledDirectionXY(Vector3 dir, float amount){
        Vector2 angleV = new Vector2(dir.x, dir.y);
        float angle = angleV.Angle();
        float degAngle = angle * Mathf.Rad2Deg;
        degAngle = TileValue2(degAngle, amount);

        angle = degAngle * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), dir.z).normalized * dir.magnitude;
    }

    public static void LookRotate(this Transform input, Vector3 towards, float offset, Axis axisApplication, Vector3 initialRotation)
    {
        Vector3 axis = ReadAxis(axisApplication);
        float rotZ;

        Vector3 difference = towards - input.position;
        rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        float rot = rotZ + offset;
        Vector3 result = new Vector3(rot * axis.x, rot * axis.y, rot * axis.z) + initialRotation;

        input.rotation = Quaternion.Euler(result);
    }

    public static void LookRotateResult(Transform input, Vector3 towards, float offset, Axis axisApplication, Vector3 initialRotation, out Quaternion Out)
    {
        Vector3 axis = ReadAxis(axisApplication);
        float rotZ;

        Vector3 difference = towards - input.position;
        rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        float rot = rotZ + offset;
        Vector3 result = new Vector3(rot * axis.x, rot * axis.y, rot * axis.z) + initialRotation;

        Out = Quaternion.Euler(result);
    }

    public static bool InRange(float t, Vector2 minMax){
        bool d = (minMax.x <= t && t <= minMax.y);
        return d;
    }

    //https://stackoverflow.com/questions/59449628/check-when-two-vector3-lines-intersect-unity3d
    public static bool VektorIntersection(out Vector3 intersection, Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if( Mathf.Abs(planarFactor) < 0.0001f 
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) 
                    / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }

    //https://docs.unity3d.com/ScriptReference/Mesh-colors.html
    public static void SetMeshColorRaw(MeshFilter meshf, Color color){
        Mesh mesh = meshf.mesh;
        Vector3[] vertices = mesh.vertices;

        // create new colors array where the colors will be created.
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
            colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);

        // assign the array of colors to the Mesh.
        mesh.colors = colors;
    }

    //https://stackoverflow.com/questions/2272149/round-to-5-or-other-number-in-python
    public static int BaseRound(int x, int b = 5){
        return b * Mathf.RoundToInt(x / b);
    }

    //https://gamedev.stackexchange.com/questions/70075/how-can-i-find-the-perpendicular-to-a-2d-vector
    //Clockwise orthogonal vector
    public static Vector2 OrthogonalV2CW(this Vector2 input){
        return new Vector2(input.y, -input.x);
    }

    //Counter clockwise o...
    public static Vector2 OrthogonalV2CoCW(this Vector2 input){
        return new Vector2(-input.y, input.x);
    }

    public static void ClampVectorSimple(float x, float y, float z, ref Vector3 input){
        input = new Vector3(
            Mathf.Clamp(input.x, -x, x),
            Mathf.Clamp(input.y, -y, y),
            Mathf.Clamp(input.z, -z, z)
        );
    }
    //https://answers.unity.com/questions/141775/limit-local-rotation.html
    public static float ClampSingleAngle(float angle, float min, float max)
    {
        if(angle<90||angle>270){
            if(angle>180) angle -= 360;
            if(max>180) max -= 360;
            if(min>180) min -= 360;
        }
        angle = Mathf.Clamp(angle, min, max);
        if(angle < 0) angle += 360;
        return angle;
    }
    public static Quaternion ClampAngleQ(float x, float y, float z, Quaternion q){

        Vector3 angleCorrected = new Vector3(
            ClampSingleAngle(q.eulerAngles.x, -x, x),
            ClampSingleAngle(q.eulerAngles.y, -y, y),
            ClampSingleAngle(q.eulerAngles.z, -z, z)
        );
        return Quaternion.Euler(angleCorrected.x, angleCorrected.y, angleCorrected.z);
    }

    public static Vector3 Clamp01(this Vector3 vector){
        return Vector3.ClampMagnitude(vector, 1);
    }

    public static Vector2Int ToVec2DInt(this Vector2 vec){
        return new Vector2Int((int)vec.x, (int)vec.y);
    }

    public static Color Invert(this Color colRGB){
        return new Color(1 - colRGB.r, 1 - colRGB.g, 1 - colRGB.b, colRGB.a);
    }

    //v returns true if x is larger or equal to y
    public static bool StepBool(float x, float y){
        return x >= y;
    }

    public static char LowerCase(this char c){
        c = Char.ToLower(c);
        return c;
    }

    public static char UpperCase(this char c){
        c = Char.ToUpper(c);
        return c;
    }

    public static string LowerCase(this string input){
        return Noun(input);
    }

    public static string Noun(this string input){
        char[] c = input.ToCharArray();
        c[0] = c[0].UpperCase();
        for (int i = 1; i < c.Length; i++)
        {
            c[i] = c[i].LowerCase();
        }
        input = new string(c);
        return input;
    }    
    
    //https://stackoverflow.com/questions/228038/best-way-to-reverse-a-string
    public static string Reverse(this string s){
        char[] array = s.ToCharArray();
        Array.Reverse(array);
        return new string(array);
    }

    public static void Log(params object[] arr){
        string log = "";
        foreach (object item in arr)
        {
            log += " " + item + " ";
        }
        Debug.Log(log);
    }

    public static PolygonCollider2D UpdatePolygonCollider2D(this PolygonCollider2D collider2D, Sprite target, float tolerance = 0.05f){
        PolygonCollider2D newCollider = collider2D.gameObject.AddComponent<PolygonCollider2D>();
        UnityEngine.Object.Destroy(collider2D);
        return newCollider;
        List<Vector2> points = new List<Vector2>();
        List<Vector2> finPoints = new List<Vector2>();
        collider2D.pathCount = target.GetPhysicsShapeCount();
        for (int i = 0; i < collider2D.pathCount; i++)
        {
            target.GetPhysicsShape(i, points);
            if(tolerance >= 0)LineUtility.Simplify(points, tolerance, finPoints);
            collider2D.SetPath(i, finPoints);
        }

    }

    public static bool IsDefault(this PolygonCollider2D collider2D, float threshhold = .05f){
        if(collider2D.pathCount == 1 || collider2D.GetPath(0).Length == 5){
            return(
                Vector2.Distance(collider2D.GetPath(0)[0], new Vector2(0, .16f)) <= threshhold &&
                Vector2.Distance(collider2D.GetPath(0)[1], new Vector2(-.15f, .049f)) <= threshhold
            );
        }
        else{
            return false;
        }
    }
}





#region UTIL_STRUCTS
public struct List2D<T>{
    public List2D(int n){
        list = new List<List<T>>();
    }
    public List<List<T>> list;

    private void ListPrecaution(int x, int y){
        if(list[x] == null) list[x] = new List<T>();
    }
    public T Get(int x, int y){
        ListPrecaution(x, y);
        if(OutOfRange(x, y)) return list[0][0];
        return list[x][y];
    }

    public void Set(int x, int y, T input){
        ListPrecaution(x, y);
        list[x][y] = input;
    }

    public void Add(int x, T input){
        ListPrecaution(x, 0);
        list[x].Add(input);
    }

    public bool OutOfRange(int x, int y){
        if(list.Count > x) return true;
        if(list[x].Count > y) return true;
        return false;
    }
}

public enum PhysicsSpace
{
    _3D = 0,
    _2D = 1
}

public enum RayCastType{
    LINE = 0,
    ROUND = 1,
    BOX = 2,
}

[System.Serializable]
public struct WorldRayCast : ISerializationCallbackReceiver{
    public void OnBeforeSerialize(){}
    public void OnAfterDeserialize(){}

    public Color drawColor;
    public Vector3 offset;
    public bool stopDrawing;
    [Range(0, 360)]public float maxLength;
    public LayerMask mask;
    public PhysicsSpace mode;
    public RayCastType type;

    [Header("Specifics")]
    public Vector3 boxScale;

    public bool GetHitMode(Vector3 pivot, Vector3 dir, bool ignoreTriggers, out Vector3 position)
    {
        if(mode == PhysicsSpace._2D)
        {
            Vector2 didHit;
            bool hit = GetHit2D(pivot, dir, ignoreTriggers, out didHit);
            position = didHit;
            return hit;
        }
        else
        {
            return GetHit(pivot, dir, ignoreTriggers, out position);
        }
    }
    public bool GetHitMode(Vector3 pivot, Vector3 dir, bool ignoreTriggers, out GameObject Out, out Vector3 position)
    {
        if (mode == PhysicsSpace._2D)
        {
            Vector2 pos;
            bool n = GetHit2D(pivot, dir, ignoreTriggers, out Out, out pos);
            position = pos;
            return n;
        }
        else
        {
            return GetHit(pivot, dir, ignoreTriggers, out Out, out position);
        }
    }


    public bool GetHit2D(Vector2 pivot, Vector2 dir, bool ignoreTriggers, out Vector2 position)
    {
        bool product = GetHit2D(pivot, dir, ignoreTriggers, out GameObject o, out position);
        return product;
    }

    public bool GetHit2D(Vector2 pivot, Vector2 dir, bool ignoreTriggers, out GameObject Out, out Vector2 position)
    {
        RaycastHit2D hit;
        switch(type){

            case RayCastType.LINE:
                hit = Physics2D.Raycast(pivot + new Vector2(offset.x, offset.y), dir, maxLength, mask);
            break;

            case RayCastType.ROUND:
                hit = Physics2D.CircleCast(pivot + new Vector2(offset.x, offset.y), maxLength, dir, maxLength, mask);
            break;

            case RayCastType.BOX:
                hit = Physics2D.BoxCast(pivot + new Vector2(offset.x, offset.y), boxScale, maxLength, dir, 0, mask);
            break;

            default:
                hit = Physics2D.Raycast(pivot + new Vector2(offset.x, offset.y), dir, maxLength, mask);
            break;

        }


        if (hit.collider != null)
        {
            position = hit.point;
            Out = hit.collider.gameObject;
            return true;
        }
        else
        {
            position = pivot + dir * 999;
            Out = null;
            return false;
        }
    }

    //v get a raycast hit!
    public bool GetHit(Vector3 pivot, Vector3 direction, bool ignoreTriggers, out Vector3 position){
        bool product = GetHit(pivot, direction, ignoreTriggers, out GameObject g, out position);
        return product;
    }

    public bool GetHit(Vector3 pivot, Vector3 direction, bool ignoreTriggers, out GameObject Out, out Vector3 position){
        //v should I collide with triggers?
        QueryTriggerInteraction triggerInteraction = ignoreTriggers? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide;

        RaycastHit hit;
        bool result;

        switch(type){

            case RayCastType.LINE:
             result = Physics.Raycast(pivot + offset, direction, out hit, maxLength, mask);
            break;

            case RayCastType.ROUND:
                result = Physics.SphereCast(pivot + offset, maxLength, direction, out hit, mask);
            break;

            case RayCastType.BOX:
                result = Physics.BoxCast(pivot + offset, boxScale, direction, out hit, Quaternion.identity, maxLength, mask, triggerInteraction);
            break;

            default:
             result = Physics.Raycast(pivot + offset, direction, out hit, maxLength, mask);
            break;
        }   

        

        if(result){
            position = hit.point;
            Out = hit.transform.gameObject;
            return true;
        }
        else{
            Out = null;
            position = pivot + direction * 999;
            return false;
        }

    }

    public RaycastHit2D[] GetHitOverlapped2D(Vector3 pivot, Vector3 dir){
        RaycastHit2D[] hits = null;
        switch (type)
        {
            case RayCastType.LINE:
                hits = Physics2D.RaycastAll(pivot + offset, dir, maxLength, mask);
            break;

            case RayCastType.ROUND:
                hits = Physics2D.CircleCastAll(pivot + offset, maxLength, dir, maxLength, mask);
            break;
            
            case RayCastType.BOX:
                hits = Physics2D.BoxCastAll(pivot + offset, boxScale, maxLength, dir, maxLength, mask);
            break;
        }
        return hits;
    }

    public RaycastHit[] GetHitOverlapped(Vector3 pivot, Vector3 dir, bool ignoreTriggers){
        RaycastHit[] hits = null;
        QueryTriggerInteraction triggerInteraction = ignoreTriggers? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide;
        switch (type)
        {
            case RayCastType.LINE:
                hits = Physics.RaycastAll(pivot + offset, dir, maxLength, mask, triggerInteraction);
            break;

            case RayCastType.ROUND:
                hits = Physics.SphereCastAll(pivot + offset, maxLength, dir, maxLength, mask, triggerInteraction);
            break;
            
            case RayCastType.BOX:
                hits = Physics.BoxCastAll(pivot + offset, boxScale, dir, Quaternion.identity, maxLength, mask, triggerInteraction);
            break;
        }
        return hits;
    }

    //V same as hit but debugs collision (if collided)
    public bool GetHitDebug(Vector3 pivot, Vector3 direction, bool ignoreTriggers, out Vector3 position){
        bool product = GetHit(pivot, direction, ignoreTriggers, out GameObject g, out position);
        if(g != null) Debug.Log(g.name + "<- raycasthit");
        else Debug.Log("Failed to hit! from " + pivot + " to " + direction);
        return product;
    }

    public void Draw(Vector3 pivot, Vector3 direction) => Draw(pivot, direction, drawColor);
    public void DrawDefault(Vector3 pivot, Vector3 direction) => Draw(pivot, direction, Color.magenta);

    public void Draw(Vector3 pivot, Vector3 direction, Color color){
        if(stopDrawing) return;
        Gizmos.color = color;    

        switch(type){

            case RayCastType.LINE:
            Gizmos.DrawRay(pivot + offset, direction * maxLength);
            break;

            case RayCastType.ROUND:
                Gizmos.DrawWireSphere(pivot + offset, maxLength);
            break;

            case RayCastType.BOX:
                Gizmos.DrawWireCube(pivot + offset, boxScale);
            break;

            default:
                Gizmos.DrawRay(pivot + offset, direction * maxLength);
            break;
        }    
    }
}


[System.Serializable]
public struct SmoothDampFloat{
    public float initialVelocity;
    public float Evaluate(float from, float to, float speed){
        return Mathf.SmoothDamp(from, to, ref initialVelocity, speed);
    }
}

[System.Serializable]
public struct SmoothDampVector{
    public Vector3 initialVelocity;
    public Vector3 Evaluate(Vector3 from, Vector3 to, float speed){
        return Vector3.SmoothDamp(from, to, ref initialVelocity, speed);
    }
}

[System.Serializable]
public struct Wave{
    public enum WaveMode{
        SIN = 0,
        COS = 1
    }
    public WaveMode mode;
    public float speed;
    public float offset;
    public float intensity;
    public float addition;
    public float Value(float t){
        System.Func<float, float> realT = mode == WaveMode.SIN? Mathf.Sin : Mathf.Cos;
        float value = realT(offset + t * speed);
        return (value + addition) * intensity;
    }
}

[System.Serializable]
public struct Circle{
    public Color gizmoColor;
    public float radius;
    public Vector2 Evaluate(float t){
        float x = Mathf.Sin(t) * radius;
        float y = Mathf.Cos(t) * radius;
        return new Vector2(x, y);
    }
#if UNITY_EDITOR
    public void DrawGizmo(Vector3 center, Vector3 up, Color color = default(Color)){
        if(color == default(Color)){
            color = gizmoColor;
        }
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawWireDisc(center, up, radius);
    }
#else
    public void DrawGizmo(Vector3 up, Color color = default(Color)){

    }
#endif
}


#endregion