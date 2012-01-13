//----------------------------------------------------------------------------
//
// Copyright (c) 2002-2011 Microsoft Corporation. 
//
// This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
// copy of the license can be found in the License.html file at the root of this distribution. 
// By using this source code in any fashion, you are agreeing to be bound 
// by the terms of the Apache License, Version 2.0.
//
// You must not remove this notice, or any other, from this software.
//----------------------------------------------------------------------------

namespace CSharpLibrary
{


    public class IntArrayPropAttribute : System.Attribute
    {
        int[] v;
        public int[] Value { set { v = value; } get { return v; } }
    }


    public class ObjArrayPropAttribute : System.Attribute
    {
        object[] v;
        public object[] Value { set { v = value; } get { return v; } }
    }

    public class AnyPropAttribute : System.Attribute
    {
        object v;
        public object Value { set { v = value; } get { return v; } }
    }

    public class IntArrayAttribute : System.Attribute
    {
        int[] values;
        public IntArrayAttribute(int[] value) { values = value; }
        public int[] Value { get { return values; } }
    }

    public class ObjArrayAttribute : System.Attribute
    {
        object[] values;
        public ObjArrayAttribute(object[] value) { values = value; }
        public object[] Value { get { return values; } }
    }



    public class AnyAttribute : System.Attribute
    {
        object v;
        public AnyAttribute(object value) { v = value; }
        public object Value { get { return v; } }
    }

    [Any(new int[] { 42 })]
    [IntArray(new int[] { 42 })]
    [ObjArray(new object[] { 42 })]
    [IntArrayProp(Value = new int[] { 42 })]
    [ObjArrayProp(Value = new object[] { 42 })]
    [AnyProp(Value = new int[] { 42 })]
    public class TestClass { }
}
