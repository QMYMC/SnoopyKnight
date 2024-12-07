using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution
{
    public bool IsValid(string s)
    {
        int st = s.Length;
        Stack<char> stack = new Stack<char>();
        for (int i = 0; i < st; i++)
        {
            if (s[i] == '(' || s[i] == '[' || s[i] == '{')
            {
                stack.Push(s[i]);
            }
            else if (s[i] == ')' )
            {
                if (stack.Count != 0 && stack.Peek() == '(')
                {
                    stack.Pop();
                }
                else return false;
            }
            else if (s[i] == ']')
            {
                if (stack.Count != 0 && stack.Peek() == '[')
                {
                    stack.Pop();
                }
                else return false;
            }
            else if (s[i] == '}')
            {
                if (stack.Count != 0 && stack.Peek() == '{')
                {
                    stack.Pop();
                }
                else return false;
            }
        }


        return stack.Count==0;
    }
}
