#define ISDEBUG

using UnityEngine;
using System.Collections;

public class LinkMenu : MonoBehaviour
{


    #region Support Class

    [System.Serializable]
    public class ScrollableLinkPanel
    {
        #region Support Class

        [System.Serializable]
        public class Link
        {
            
            public string text  = "";
            public int sceneId  = 0; //Id of the Scene loaded upon click (calls a function from SceneLoader)
            public bool enabled = true;
            private Rect rect   = new Rect(0f, 0f, 0f, 0f);


            #region Constructor
            /// <summary>
            ///     Create a link button
            /// </summary>
            /// <param name="rect">Button rect</param>
            /// <param name="text">Button label</param>
            /// <param name="sceneId">Id of the scene loaded from SceneLoader</param>
            /// <param name="enabled">Enable/Disable the link</param>
            public Link(Rect rect, string text, int sceneId, bool enabled)
            {
                this.rect = rect;
                this.text = text;
                this.sceneId = sceneId;
                this.enabled = enabled;
            }
            #endregion

            #region Properties
            public Rect Rect
            {
                get { return rect; }
            }

            public float X
            {
                get { return rect.x; }
                set { rect.x = value; }
            }

            public float Y
            {
                get { return rect.y; }
                set { rect.y = value; }
            }

            public float Width
            {
                get { return rect.width; }
                set { rect.width = value; }
            }

            public float Height
            {
                get { return rect.height; }
                set { rect.height = value; }
            }

            public Link(Rect rect, string text, int sceneId)
                : this(rect, text, sceneId, true)
            {
            }
            #endregion

        }

        [System.Serializable]
        public class ScrollBarButton
        {
            public Texture2D normalTexture;
            public Texture2D hoverTexture;
            public Texture2D clickTexture;

            private Rect rect = new Rect(0.0f, 0.0f, 50f, 50f);
            private GUIStyle _style = new GUIStyle();

            public bool Draw()
            {
                //It sucks that the Inspector can't use Constructors and/or properties :(
                if (normalTexture)
                    _style.normal.background = normalTexture;
                else
                    Debug.LogWarning("No normal texture set. Use the inspector to set one.");
                if (hoverTexture)
                    _style.hover.background = hoverTexture;
                if (clickTexture)
                    _style.active.background = clickTexture;

                rect.width = normalTexture.width;
                rect.height = normalTexture.height;

                return GUI.Button(rect, "", _style);
            }

            #region Properties
            public Rect Rect
            {
                get { return rect; }
            }

            public float X
            {
                get { return rect.x; }
                set { rect.x = value; }
            }

            public float Y
            {
                get { return rect.y; }
                set { rect.y = value; }
            }

            public float Width
            {
                get { return rect.width; }
                set { rect.width = value; }
            }

            public float Height
            {
                get { return rect.height; }
                set { rect.height = value; }
            }
            #endregion
        }
        #endregion

        //---------------------------------------------------------------------------

        #region Inspector attributes

        public Font font;
        public int fontSize;
        public Texture2D transparent; //Hack... on hover event doesn't work if there's no background
        public Color enabledLink;
        public Color enabledHoverLink;
        public Color disabledLink;
        public Rect visibleAreaRect; //The visible area (provided from the Inspector)
        public float VerticalPadding = 10;
        public ScrollBarButton upButton;
        public ScrollBarButton downButton;
        public Link[] links;
        
        #endregion

        //---------------------------------------------------------------------------

        private Rect realAreaRect; //The real area (calculated in Height prop.)
        private Vector2 scrollViewVector = Vector2.zero;
        private float height = 0.0f;
        private float rowHeight = 0.0f;
        private GUIStyle enabledStyle;
        private GUIStyle disabledStyle;

        //Hack --
        //Cached values used upon changes made via Inspector
        //Let's hope that they make properties visible in the Inspector in the next release :D
        private int cachedLinksLength = int.MinValue;
        private int cachedFontSize = int.MinValue;
        private int cachedFontHash = 0;
        private Color cachedEnabledLink = new Color();
        private Color cachedDisabledLink = new Color();
        private bool changed = true;

        /// <summary>
        ///     Real height of the scrollable panel (non-visible area included)
        /// </summary>
        public float Height
        {
            get
            {
                if (cachedLinksLength != links.Length || cachedFontSize != fontSize)
                {
                    changed = true;

                    float max = float.MinValue;
                    float tmpHeight = 0.0f;
                    foreach (var link in links)
                    {
                        float currY = link.Y;
                        if (currY > max)
                        {
                            max = currY;
                            tmpHeight = max + link.Height;
                        }

                    }
                    height = tmpHeight;
                }
                return height;
            }
        }

        /// <summary>
        ///     Real Area Rect (non-visible area included)
        /// </summary>
        public Rect RealAreaRect
        {
            get
            {
                if (cachedLinksLength != links.Length)
                {
                    realAreaRect = new Rect(0.0f, 0.0f, visibleAreaRect.width, this.Height);
                }
                return realAreaRect;
            }
        }

        /// <summary>
        ///     Inintializes styles and link rects
        /// </summary>
        private void Initialize()
        {

            //Font part is not cache friendly in order to reflect Inspector editing.
            if (enabledStyle == null)
            {
                enabledStyle = new GUIStyle();
                disabledStyle = new GUIStyle();

                //Set the style of the viewable scrollbar to EMPTY
                GUI.skin.scrollView = GUIStyle.none;
                //hack: Known Issue (tracked by some user) - GUIStyle.none for vertical 
                //and horizontal scrollbars causes problems and generates a lot of warnings
                GUI.skin.horizontalScrollbar.fixedHeight = GUI.skin.horizontalScrollbar.fixedWidth = 0f;
                GUI.skin.verticalScrollbar.fixedHeight = GUI.skin.verticalScrollbar.fixedWidth = 0f;
            }

            if (font && (
                cachedFontSize != fontSize || 
                cachedFontHash != font.GetHashCode() ||
                !cachedEnabledLink.Equals(enabledLink) ||
                !cachedDisabledLink.Equals(disabledLink) 
                ))
            {
                changed = true;

                enabledStyle.font = font;
                enabledStyle.fontSize = fontSize;
                enabledStyle.normal.textColor =
                    enabledLink == Color.clear ? Color.black : enabledLink;
                enabledStyle.hover.textColor =
                    enabledHoverLink == Color.clear ? Color.green : enabledHoverLink;
                if (transparent)
                    enabledStyle.hover.background = transparent;
                    

                disabledStyle.font = font;
                disabledStyle.fontSize = fontSize;
                disabledStyle.normal.textColor = 
                    disabledLink == Color.clear ? Color.gray : disabledLink;
            }
            if(links.Length == 0) return;

            //Set link positions
            if (cachedLinksLength != links.Length || cachedFontSize != fontSize)
            {
                changed = true;

                float x = 0.0f;
                float height = enabledStyle.fontSize * 1.3f; //1.3 allows enough room for letters like g,p...
                rowHeight = height + VerticalPadding;
                links[0].X = x;
                links[0].Y = 0.0f;
                links[0].Width = visibleAreaRect.width;
                links[0].Height = height;
                for (int i = 1; i < links.Length; i++)
                {
                    links[i].X = x;
                    links[i].Y = links[i - 1].Y + height + VerticalPadding;
                    links[i].Width = visibleAreaRect.width;
                    links[i].Height = height;
                }
            }
        }

        /// <summary>
        ///     Cached values used to keep track of the object changes via Inspector
        /// </summary>
        private void FillCachedValues()
        {
            if (changed)
            {
                changed = false;
                cachedFontSize = fontSize;
                cachedLinksLength = links.Length;
                cachedFontHash = font.GetHashCode();
                cachedEnabledLink = enabledLink;
                cachedDisabledLink = disabledLink;
            }
        }

        /// <summary>
        ///     Adjust the position of the scroll vector so that no link in the list
        ///     is partially hidden.
        /// </summary>
        private void adjustScrollVector()
        {
            //trick, check the direction of the scroll to adjust the signs,
            //note: improves the smoothness
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            float mult = 1f;
            if (scrollDelta > 0f) mult = -mult;
            if (scrollViewVector.y > 1f)
            {
                if (scrollViewVector.y % rowHeight > rowHeight / 2)
                {
                    scrollViewVector.y += (scrollViewVector.y % rowHeight)*mult;
                }
                else
                {
                    scrollViewVector.y -= (scrollViewVector.y % rowHeight)*mult;
                }
            }
        }

        /// <summary>
        ///     Renders the object (to be used in OnGUI function)
        /// </summary>
        public void Draw()
        {
            Initialize();
            bool upVisible = false;
            bool downVisible = false;

            //START ScrollView
#if(ISDEBUG)
            scrollViewVector = GUI.BeginScrollView(visibleAreaRect, scrollViewVector, this.RealAreaRect);
#else
            scrollViewVector = GUI.BeginScrollView(visibleAreaRect, scrollViewVector, this.RealAreaRect, GUIStyle.none, GUIStyle.none);
#endif
            //set the position of the links so that no link is partially hidden
            adjustScrollVector();

            //check the beginning and end of the scrollable list
            if (scrollViewVector.y > 0f)
            {
                upVisible = true;
            }
            if (scrollViewVector.y < realAreaRect.height - visibleAreaRect.height)
            {
                downVisible = true;
            }
            

            //Links loop
            foreach (var link in links)
            {
                if (GUI.Button(link.Rect, link.text, link.enabled ? enabledStyle : disabledStyle))
                {
                    if (link.enabled)
                    {
                        try
                        {
                            SceneLoader sLoader = SceneLoader.Instance;
                            System.Reflection.MethodInfo mi =
                                sLoader.GetType().GetMethod("Scene" + link.sceneId);

                            mi.Invoke(sLoader, null);
                        }
                        catch (System.Exception e)
                        {
                            Debug.Log("Exception : "+ e.Message);
                        }
                    }
                }
            }

            // END the ScrollView
            GUI.EndScrollView();

            FillCachedValues();

            //TODO: cache upButton and downButton positions
            //Button up and down actions.
            if (upVisible)
            {
                upButton.Y = visibleAreaRect.y - upButton.Height - 5;
                upButton.X = (visibleAreaRect.x + visibleAreaRect.width) / 2 + upButton.Width;
                if (upButton.Draw())
                {
                    if (scrollViewVector.y > rowHeight)
                    {
                        scrollViewVector.y -= rowHeight;
                    }
                    else if (scrollViewVector.y > 0)
                    {
                        scrollViewVector.y = 0;
                    }
                }
            }
            if (downVisible)
            {
                downButton.Y = visibleAreaRect.y + visibleAreaRect.height + 5;
                downButton.X = (visibleAreaRect.x + visibleAreaRect.width) / 2 + downButton.Width;

                if (downButton.Draw())
                {
                    if (scrollViewVector.y < (realAreaRect.height - visibleAreaRect.height - rowHeight))
                    {
                        scrollViewVector.y += rowHeight;
                    }
                    else
                    {
                        scrollViewVector.y += rowHeight;
                    }
                }
            }
        }
    }
    #endregion


    //---------------------------------------------------------------------------


    public GUISkin Skin;
    public int Depth = 0;
    public ScrollableLinkPanel LinkPanel;
    public bool Enabled = true;


    void OnGUI()
    {
        GUI.enabled = Enabled;
        GUI.depth = Depth;
        LinkPanel.Draw();
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}
}
