using System;



namespace CsTest.Attributes 
{
    public class HttpMethod : Attribute {
        public string method;

        public HttpMethod(string name)
        {
            method = name;
        }
    }
}