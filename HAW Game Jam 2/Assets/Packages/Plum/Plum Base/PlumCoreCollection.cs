using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    public class TypeDictionary<T>{
        public TypeDictionary(){}
        public TypeDictionary(System.Action<TypeDictionary<T>> assembly){
            assembly?.Invoke(this);
        }
        public Dictionary<System.Type, T> rawDictionary = new Dictionary<System.Type, T>();
        public bool ContainsKey(System.Type key) => rawDictionary.ContainsKey(key);
        public void Add(System.Type type, T value){
            bool alreadyContains = rawDictionary.ContainsKey(type);
            if(alreadyContains) return;
            rawDictionary.Add(type, value);
        }

        public void Add<H>(T value) where H : T{
            Add(typeof(H), value);
        }

        public T Get(System.Type key){
            if(rawDictionary.ContainsKey(key)){
                return rawDictionary[key];
            }else{
                return default(T);
            }
        }

        public T Get<H>() where H : T{
            if(rawDictionary.ContainsKey(typeof(H))){
                return rawDictionary[typeof(H)];
            }else{
                return default(T);
            }
        }

        public void ForAll(Utility.GenericDelegate<T> onInstance){
            foreach(KeyValuePair<System.Type, T> instance in rawDictionary){
                onInstance?.Invoke(instance.Value);
            }
        }
    }
}
