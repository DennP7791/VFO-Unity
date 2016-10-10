using UnityEngine;
using System.Collections;
using ExerciseCollections;

public class Util {

    public static void SetToolTipText(string text)
    {
        GameObject go = GameObject.Find("TopBar");
        if (go)
        {
            TobBarScript tbs = go.GetComponent<TobBarScript>();
            if (tbs)
            {
                tbs.ToolTipText = text;
            }
        }
        else 
        {
            Debug.LogWarning("Impossible to set ToolTip text, there's no topbar in current scene.");
        }
    }

    public static void FeedbackWindowCallback(Message msg, bool value)
    {
        if (!value) return;

        ExerciseCategory category = Global.Instance.categoryCollection[SceneLoader.Instance.CurrentCategory];
        if (category != null)
        {
            Exercise exercise = category[SceneLoader.Instance.CurrentScene];
            if (exercise != null)
            {
                exercise.Feedback = msg.Text; 
            }
        }
        else
            Debug.Log("Error in updating exercise feedback.");
    }

    public static Message FeedbackWindow(Rect rect, string msg, Message.CallBack callback)
    {
        Message msgObj = InstantiateResource<Message>("MessageBox");
        if (msgObj)
        {
            msgObj.FullScreen = true;
            msgObj.Rect = rect;
            msgObj.Text = msg;
            msgObj.Cancellable = true;
            msgObj.OnlyOk = false;
            msgObj.Closeable = false;
            msgObj.type = Message.Type.Info;
            msgObj.Editable = true;
            msgObj.Initialize(callback);
            return msgObj;
        }
        return null;
    }

    public static HelpBox HelpBox(string msg, HelpBox.CallBack callBackFunc)
    {
        HelpBox msgObj = InstantiateResource<HelpBox>("HelpBox");
        if (msgObj)
        {
            msgObj.FullScreen = false;
            
            msgObj.Text = msg;
            msgObj.Initialize(callBackFunc);
            msgObj.Depth = 100;
            return msgObj;
        }
        return null;
    }

    public static HelpBox HelpBox(string msg)
    {
        return HelpBox(msg, null);
    }

    public static Message InfoWindow(Rect rect, string msg, bool fullScreen, Message.Type type, bool closeable, bool cancellable, bool onlyOk, Message.CallBack callBackFunc)
    {
        Message msgObj = InstantiateResource<Message>("InfoWindow");
        if (msgObj)
        {
            msgObj.FullScreen = fullScreen;
            msgObj.Rect = rect;
            msgObj.Text = msg;
            msgObj.Cancellable = cancellable;
            msgObj.OnlyOk = onlyOk;
            msgObj.Closeable = closeable;
            msgObj.type = type;
            msgObj.Initialize(callBackFunc);
            return msgObj;
        }
        return null;
    }

    public static Message InfoWindow(Rect rect, string msg)
    {
        return InfoWindow(rect, msg, null);
    }

    public static Message InfoWindow(Rect rect, string msg, Message.CallBack callBackFunc)
    {
        return InfoWindow(rect, msg, true, Message.Type.Info, true, false, false, callBackFunc);
    }

    public static Message MessageBox(Rect rect, string msg, bool fullScreen, Message.Type type, bool closeable, bool cancellable, bool onlyOk, Message.CallBack callBackFunc)
    {
        Message msgObj = InstantiateResource<Message>("MessageBox");
        if (msgObj)
        {
            msgObj.FullScreen = fullScreen;
            msgObj.Rect = rect;
            msgObj.Text = msg;
            msgObj.Cancellable = cancellable;
            msgObj.OnlyOk = onlyOk;
            msgObj.Closeable = closeable;
            msgObj.type = type;
            msgObj.Initialize(callBackFunc);
            return msgObj;
        }
        return null;
    }

    public static Message InfoMessage(Rect rect, string msg)
    {
        return MessageBox(rect, msg, Message.Type.Info, false);
    }

    public static Message MessageBox(Rect rect, string msg, bool closeable)
    {
        return MessageBox(rect, msg, Message.Type.Info, closeable);
    }

    public static Message MessageBox(Rect rect, string msg, Message.Type type, bool closeable, bool fullScreen)
    {
        return MessageBox(rect, msg, fullScreen, type, closeable, false, false, null);
    }

    public static Message MessageBox(Rect rect, string msg, Message.Type type, bool closeable)
    {
        return MessageBox(rect, msg, false, type, closeable, false, false, null);
    }

    public static Message CancellableMessageBox(Rect rect, string msg, bool fullScreen, Message.Type type, Message.CallBack callBackFunc)
    {
        return MessageBox(rect, msg, fullScreen, type, false, true, false, callBackFunc);
    }

    public static Message CancellableMessageBox(Rect rect, string msg, Message.Type type, Message.CallBack callBackFunc)
    {
        return CancellableMessageBox(rect, msg, false, type, callBackFunc);
    }

    public static Message CancellableMessageBox(Rect rect, string msg, Message.CallBack callBackFunc)
    {
        return CancellableMessageBox(rect, msg, false, Message.Type.Info, callBackFunc);
    }

    public static Message OkMessageBox(Rect rect, string msg, bool fullScreen, Message.Type type, Message.CallBack callBackFunc)
    {
        return MessageBox(rect, msg, fullScreen, type, false, true, true, callBackFunc);
    }

    public static Message OkMessageBox(Rect rect, string msg, Message.Type type, Message.CallBack callBackFunc)
    {
        return OkMessageBox(rect, msg, false, type, callBackFunc);
    }

    public static Message OkMessageBox(Rect rect, string msg, Message.CallBack callBackFunc)
    {
        return OkMessageBox(rect, msg, false, Message.Type.Info, callBackFunc);
    }


    public static T FindInstancesWhoseNameStartsWith<T>(string name) where T : MonoBehaviour
    {
        T instance = null;
        Object[] objects = GameObject.FindObjectsOfType(typeof(T));
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].name.StartsWith(name))
            {
                instance = (T)objects[i];
                return instance;
            }
        }
        return null;
    }

    /// <summary>
    /// Instantiate a Resource of the specified type and name. 
    /// The resource is loaded from the Resources folder.
    /// </summary>
    /// <typeparam name="T">Type of the resource.</typeparam>
    /// <param name="name">Name of the resource, as it appears in the inspector.</param>
    /// <returns>The GameObjet representing an instance of the resource.</returns>
    public static T ToggleResource<T>(string name) where T : MonoBehaviour
    {
        Debug.Log("toggling " + name);
        T instance = null;
        Object[] objects = GameObject.FindObjectsOfType(typeof(T));
        
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].name.StartsWith(name))
            {
                instance = (T)objects[i];
                instance.enabled = !instance.enabled;
                Debug.Log("Instance is: " + instance.enabled);
                return instance;
            }
        }
 
        GameObject go = InstantiateResource(name);
        if (go)
        {
            return go.GetComponent<T>();
        }
        return null;
    }

    public static bool AnyVisibleResource<T>() where T : MonoBehaviour
    {
        T instance = null;
        Object[] objects = GameObject.FindObjectsOfType(typeof(T));
        for (int i = 0; i < objects.Length; i++)
        {
            instance = (T)objects[i];
            if(instance.enabled)
                return true;
        }
        return false;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T InstantiateResource<T>(string name) where T : MonoBehaviour
    {
        Debug.Log("Instantiating resource: " + name);
        Object resourceObj = Resources.Load(name);
        if (resourceObj)
        {
            GameObject go = (GameObject)Object.Instantiate(resourceObj);
            return go.GetComponent<T>();
        }
        return null;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject InstantiateResource(string name)
    {
        Object resourceObj = Resources.Load(name);
        if (resourceObj)
        {
            GameObject go = (GameObject)Object.Instantiate(resourceObj);
            return go;
        }
        return null;
    }

    public static void ToggleSubElementRenderer(GameObject go, string name)
    {
        GameObject tmp = FindElementInTransform(go.transform, name);
        if(tmp != null)
            tmp.GetComponent<Renderer>().enabled = !tmp.GetComponent<Renderer>().enabled;
    }

    public static GameObject FindSubElement(GameObject go, string name)
    {
        return FindElementInTransform(go.transform, name);
    }

    private static GameObject FindElementInTransform(Transform transform, string name)
    {
        GameObject go = null;
        if(transform.name.Equals(name))
            return transform.gameObject;
        foreach (Transform t in transform)
        {
           go = FindElementInTransform(t, name);
           if (go != null && go.name.Equals(name))
               return go;
        }
        return go;
    }

    public static void SetActiveInCompoundObject(GameObject go, bool active)
    {
        SetEnabledInTransform(go.transform, active);
    }

    private static void SetEnabledInTransform(Transform transform, bool active)
    {
        transform.gameObject.active = active;
        foreach (Transform t in transform)
        {
            SetEnabledInTransform(t, active);
        }
    }
}
