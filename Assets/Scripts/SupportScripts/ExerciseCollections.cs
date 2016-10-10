using System;
using System.Collections;
using System.Collections.Generic;

namespace ExerciseCollections
{

    public class ExerciseCategoryCollection : GenericExercise<ExerciseCategory>
    {
        #region Constructors

        public ExerciseCategoryCollection()
            : base(0, "")
        {
        }

        #endregion
    }

    //-------------------------------------------------------------------------

    public class ExerciseCategory : GenericExercise<Exercise>
    {
        #region Constructors

        public ExerciseCategory(int id)
            : base(id, "")
        {
        }

        public ExerciseCategory(int id, string name)
            : base(id, name)
        {
        }

        public ExerciseCategory(int id, string name, double score)
            : base(id, name, score)
        {
        }

        #endregion

        public void Update()
        {
            double result = 0.0;
            if (this.Count > 0)
            {
                foreach (Exercise e in this)
                {
                    result += e.Score;
                }
                result /= this.Count;
            }
            Score = result;
        }
    }

    //-------------------------------------------------------------------------

    public class Exercise : GenericExercise<ExercisePart>
    {
        private int _function;
        private string _feedback;
        private bool _attempted;

        public override string ToString()
        {
            return base.ToString() + " (Function: "+_function+", "+"Feedback: "+_feedback+")";
        }

        #region Constructors

        public Exercise(int id)
            : this(id, "")
        {
        }

        public Exercise(int id, string name)
            : this(id, name, 0.0)
        {
        }

        public Exercise(int id, string name, double score)
            : this(id, name, score, -1)
        {
        }

        public Exercise(int id, string name, double score, int function)
            : this(id, name, score, function, "")
        {
        }

        public Exercise(int id, string name, double score, int function, string feedback)
            : this(id, name, score, function, "", false)
        {
        }

        public Exercise(int id, string name, double score, int function, string feedback, bool attempted)
            : base(id, name, score)
        {
            _function = function;
            _feedback = feedback;
            _attempted = attempted;
        }
        #endregion

        #region Attributes

        public int Function
        {
            set { _function = value; }
            get { return _function; }
        }

        public string Feedback
        {
            set { _feedback = value; }
            get { return _feedback; }
        }

        public bool Attempted
        {
            set { _attempted = value; }
            get { return _attempted; }
        }

        #endregion
    }

    //-------------------------------------------------------------------------

    public class BaseExercisePart
    {
        private int _id;
        private string _name;

        public BaseExercisePart(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public override string ToString()
        {
            return "id: "+ _id + ", name: " + _name;
        }

        #region Attributes

        public string Name
        {
            protected set { _name = value; }
            get { return _name; }
        }

        public int ID
        {
            protected set { _id = value; }
            get { return _id; }
        }

        #endregion

    }

    //-------------------------------------------------------------------------

    public class ExercisePart : BaseExercisePart
    {
        private bool _complete;
        private double _time;

        #region Constructors

        public ExercisePart(int id, string name, bool complete)
            : this(id, name, complete, 0.0)
        {
        }

        public ExercisePart(int id, string name, bool complete, double time)
            : base(id, name)
        {
            _complete = complete;
            _time = time;
        }

        #endregion

        #region Attributes

        public bool Complete
        {
            set { _complete = value; }
            get { return _complete; }
        }

        public double Time
        {
            set { _time = value; }
            get { return _time; }
        }

        #endregion

        public override string ToString()
        {
            return this.GetType().ToString()
                +" [ "+base.ToString() + " data: (" + _complete + ", " + _time + ")" + " ]";
        }
    }

    //-------------------------------------------------------------------------
    public class Key
    {
        public Key(int key)
        {
            Value = key;
        }
        public override int GetHashCode()
        {
            return Value;
        }
        public override bool Equals(object obj)
        {
            return Value.Equals(obj);
        }
        public static bool operator ==(Key obj1, Key obj2)
        {
            return obj1.Value == obj2.Value;
        }
        public static bool operator !=(Key obj1, Key obj2)
        {
            return obj1.Value != obj2.Value;
        }

        public int Value { private set; get; }
    }

    public class GenericExercise<T> : BaseExercisePart, IEnumerable<T> where T : BaseExercisePart
    {
        private double _score;
        private Dictionary<int, T> _items = new Dictionary<int, T>();

        #region Constructors

        public GenericExercise(int id)
            : this(id, "")
        {
        }

        public GenericExercise(int id, string name)
            : this(id, name, 0.0)
        {
        }

        public GenericExercise(int id, string name, double score)
            : base(id, name)
        {
            _score = score;
        }

        #endregion

        public void Add(T item)
        {
            _items.Add(item.ID, item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (KeyValuePair<int, T> item in _items)
            {
                yield return item.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            string result = this.GetType().ToString() + 
                " [ " + base.ToString() + ", score: " + _score + ", items: "+this.Count+" ] ";
            foreach (T item in this)
            {
                result += "\n" + item.ToString();
            }
            return result;
        }

        #region Attributes

        public int Count
        {
            get { return _items.Count; }
        }
        
        public T this[int id]
        {
            get
            {
                try
                {
                    return _items[id];
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public double Score
        {
            set { _score = value; }
            get { return _score; }
        }

        #endregion
    }

}

