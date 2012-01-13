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
// See notes in a.cs

public class UseForwardedTypes
{
    static public C CreateC() { return (new C()); }
    static public D CreateD() { return (new D()); }
    static public GenericD<C> CreateGenericD() { return (new GenericD<C>()); }
    static public D.E CreateE() { return (new D.E()); }
    static public D.E.EE CreateEE() { return (new D.E.EE()); }

    static public F CreateF() { return (new F()); }
    static public G CreateG() { return (new G()); }
    static public G.H CreateH() { return (new G.H()); }

    static public void ConsumeC(C x) { F.ConsumeC(x); }
    static public void ConsumeD(D x) { F.ConsumeD(x); }
    static public void ConsumeGenericD(GenericD<C> x) { F.ConsumeGenericD(x);  }
    static public void ConsumeE(D.E x) { F.ConsumeE(x); }
    static public void ConsumeEE(D.E.EE x) { F.ConsumeEE(x); }

}

