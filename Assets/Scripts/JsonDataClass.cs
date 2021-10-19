using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class JsonDataClass
{
    public List<QuestionList> results;
}

[Serializable]
public class QuestionList
{
    public string category;
    public string type;
    public string difficulty;
    public string question;
    public string correct_answer;
    public List<string> incorrect_answers;
}
