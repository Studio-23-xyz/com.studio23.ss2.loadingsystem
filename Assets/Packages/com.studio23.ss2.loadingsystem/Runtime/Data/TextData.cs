using System;


namespace Studio23.SS2.SceneLoadingSystem.Data
{

    [Serializable]
    public class TextData
    {
        public TextType Type;
      
        public string Title;
        public string Description;
      

       
    }


    public enum TextType
    {
        None,
        Ui,
        GamePlay,
        Trivia,
        Message
    }

}
