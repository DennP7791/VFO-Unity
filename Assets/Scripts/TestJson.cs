using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

public class TestJson : MonoBehaviour
{

    public class JsonRequestArgs
    {
        public string method;
        public string @params;
        public int id;
    }


    public class Part
    {
        public int Id;
        public string Name;
        public bool Completed;
        public double Time;

        public override string ToString()
        {
            return Id + " " + Name + " " + Completed + " " + Time + "; ";
        }
    }

    public class BExercise
    {
        public int Id;
        public double Score;
        public string Name;

        public override string ToString()
        {
            return "( "+Id + " " + Score + " " + Name+" )";
        }
    }

    public class CategoryCollection
    {
        public int UserId;
        public Category[] Categories;
        public override string ToString()
        {
            string result = UserId+" < ";
            foreach (Category c in Categories)
            {
                result += c.ToString() + " ";
            }
            result += ">";
            return result;
        }
    }

    public class Category : BExercise
    {
        public Exercise[] Exercises;
        public override string ToString()
        {
            string result = base.ToString() + " " + "{ ";
            foreach (Exercise e in Exercises)
            {
                result += e.ToString() + " ";
            }
            result += "}";
            return result;
        }
    }

    public class Exercise : BExercise
    {
        public string SceneFunction;
        public Part[] Parts;

        public override string ToString()
        {
            string result = base.ToString() + " " + "[ ";
            foreach (Part p in Parts)
            {
                result +=p.ToString() + " ";
            }
            result += "]";
            return result;
        }
    }


	// Use this for initialization
	void Start () {
        StartCoroutine(Test());

	}

    IEnumerator Test()
    {
            JsonRequestArgs jra = new JsonRequestArgs
            {
                method = "GetData",
                @params = "7",
                id = 0,
            };
            Debug.Log(JsonWriter.Serialize(jra));

            string url = "http://192.168.0.22:81/Service/GetData/7";
            WWW www = new WWW(url);
            while (!www.isDone)
            {
                yield return null;
            }
            Debug.Log("results: "+www.text);
            CategoryCollection cc = JsonReader.Deserialize<CategoryCollection>(www.text);
            Debug.Log(cc.ToString());

            string url2 = "http://192.168.0.22:81/Service/SaveData/";
            WWWForm form = new WWWForm();
            

            string serialized = JsonWriter.Serialize(cc);
            Debug.Log(serialized);
            form.headers["Accept"] = "text/plain, text/html, application/json";
            form.headers["Accept-Charset"] = "utf-8";
            form.AddField("data", serialized, Encoding.UTF8);
            Encoding encoding = Encoding.UTF8;
            byte[] bytes = encoding.GetBytes(serialized);
            

            string data = "";
            for (int i = 0; i < form.data.Length; i++)
            {
                data += (char)form.data[i];
            }
            Debug.Log("formdata: " + data);
            WWW www2 = new WWW(url2, bytes);


            StartCoroutine(WaitForRequest(www2));
            Debug.Log("Finished");
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Result: " + www.text);
        } else {
            Debug.Log("WWW Error: "+ www.error);
        }    
    }  

	// Update is called once per frame
	void Update () {
	
	}
}
