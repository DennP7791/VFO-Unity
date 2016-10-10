using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using ExerciseCollections;

public class JsonCommunication : MonoBehaviour
{

    //Classes used in Json Data Serialization and Deserialization
    #region Json Data container classes

    public class JsonRequestArgs
    {
        public string method;
        public string @params;
        public int id;
    }


    public class JsonExercisePart
    {
        public int Id;
        public string Name;
        public bool Completed;
        public double Time;

        public JsonExercisePart()
        {
        }

        public JsonExercisePart(int id, string name, bool completed, double time)
        {
            Id = id;
            Name = name;
            Completed = completed;
            Time = time;
        }

        public override string ToString()
        {
            return Id + " " + Name + " " + Completed + " " + Time + "; ";
        }
    }

    public class JsonBaseExercise
    {
        public int Id;
        public double Score;
        public string Name;

        public JsonBaseExercise()
        {
        }

        public JsonBaseExercise(int id, string name, double score)
        {
            Id = id;
            Score = score;
            Name = name;
        }

        public override string ToString()
        {
            return "( "+Id + " " + Score + " " + Name+" )";
        }
    }

    public class JsonCategoryCollection
    {
        public int UserId;
        public JsonCategory[] Categories;

        public override string ToString()
        {
            string result = UserId+" < ";
            foreach (JsonCategory c in Categories)
            {
                result += c.ToString() + " ";
            }
            result += ">";
            return result;
        }
    }

    public class JsonCategory : JsonBaseExercise
    {
        public JsonExercise[] Exercises;

        public JsonCategory()
        {
        }

        public JsonCategory(int id, string name, double score)
            : base(id, name, score)
        {
        }

        public override string ToString()
        {
            string result = base.ToString() + " " + "{ ";
            foreach (JsonExercise e in Exercises)
            {
                result += e.ToString() + " ";
            }
            result += "}";
            return result;
        }
    }

    public class JsonExercise : JsonBaseExercise
    {
        public int SceneFunction;
        public JsonExercisePart[] Parts;


        public JsonExercise()
        {
        }

        public JsonExercise(int id, string name, double score, int sceneFunction)
            : base(id, name, score)
        {
            SceneFunction = sceneFunction;
        }
        public override string ToString()
        {
            string result = base.ToString() + "Function " + SceneFunction + "[ ";
            foreach (JsonExercisePart p in Parts)
            {
                result +=p.ToString() + " ";
            }
            result += "]";
            return result;
        }
    }

    #endregion

    public static IEnumerator RetrieveData()
    {
        Application.ExternalCall("GetUserId");
        int id = Global.Instance.UserId;

        //TODO: remove after the JScript function has been implemented
        id = 7;
        JsonRequestArgs jra = new JsonRequestArgs
        {
            method = "GetData",
            @params = "7",
            id = 0,
        };
        Debug.Log(JsonWriter.Serialize(jra));

        string url = "http://192.168.0.22:81/Service/GetData/"+id;
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            yield return null;
        }
        Debug.Log("results: " + www.text);
        JsonCategoryCollection cc = JsonReader.Deserialize<JsonCategoryCollection>(www.text);
        Debug.Log(cc.ToString());
        Global.Instance.categoryCollection = JsonCategoryCollectionToExerciseCategoryCollection(cc);
        Debug.Log(Global.Instance.categoryCollection.ToString());
    }   

    static ExerciseCategoryCollection JsonCategoryCollectionToExerciseCategoryCollection(JsonCategoryCollection collection)
    {
        Global.Instance.UserId = collection.UserId;
        ExerciseCategoryCollection ecc = new ExerciseCategoryCollection();
        foreach (JsonCategory c in collection.Categories)
        {
            ExerciseCategory tmpCategory = new ExerciseCategory(c.Id, c.Name, c.Score);
            ecc.Add(tmpCategory);
            foreach (JsonExercise e in c.Exercises)
            {
                Exercise tmpExercise = new Exercise(e.Id, e.Name, e.Score, e.SceneFunction);
                tmpCategory.Add(tmpExercise);
                foreach (JsonExercisePart p in e.Parts)
                {
                    ExercisePart tmpPart = new ExercisePart(p.Id, p.Name, p.Completed, p.Time);
                    tmpExercise.Add(tmpPart);
                }
            }
        }
        return ecc;
    }

    static JsonCategoryCollection ExerciseCategoryCollectionToJsonCategoryCollection(ExerciseCategoryCollection collection)
    {
        JsonCategoryCollection jcc = new JsonCategoryCollection();
        jcc.UserId = Global.Instance.UserId;
        List<JsonCategory> categoryList = new List<JsonCategory>();

        foreach (ExerciseCategory c in collection)
        {
            JsonCategory tmpCategory = new JsonCategory(c.ID, c.Name, c.Score);
            List<JsonExercise> exerciseList = new List<JsonExercise>(); 
            foreach (Exercise e in c)
            {
                JsonExercise tmpExercise = new JsonExercise(e.ID, e.Name, e.Score, e.Function);
                List<JsonExercisePart> partList = new List<JsonExercisePart>();
                foreach (ExercisePart p in e)
                {
                    JsonExercisePart tmpPart = new JsonExercisePart(p.ID, p.Name, p.Complete, p.Time);
                    partList.Add(tmpPart);
                }
                tmpExercise.Parts = partList.ToArray();
                exerciseList.Add(tmpExercise);
            }
            tmpCategory.Exercises = exerciseList.ToArray();
            categoryList.Add(tmpCategory);
        }
        jcc.Categories = categoryList.ToArray();
        return jcc;
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
