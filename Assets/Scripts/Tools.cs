using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> where T : new(){
    private static T instance_;

    public static T CreateInstance(){
        if(instance_ == null){
            instance_ = new T();
        }        
        return instance_;
    }

    public static T GetInstance(){
        return instance_;
    }

    public static void DeleteInstance(){
        if(instance_ != null){
            instance_ = default(T);
        }
    }
}