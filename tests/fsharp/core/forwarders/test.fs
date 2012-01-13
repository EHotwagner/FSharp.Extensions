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



let x = (new C(), new D(), new D.E(), new D.E.EE(), new F(), new G(), new G.H())

let y = UseForwardedTypes.CreateC()

// This tests type equivalence for forwarded types
let z1 = (UseForwardedTypes.CreateC() = new C())
let z2 = (UseForwardedTypes.CreateD() = new D())
let z3 = (UseForwardedTypes.CreateGenericD() = new GenericD<C>())
let z4 = (UseForwardedTypes.CreateE() = new D.E())
let z4b = (UseForwardedTypes.CreateEE() = new D.E.EE())
let z5 = (UseForwardedTypes.CreateF() = new F())
let z6 = (UseForwardedTypes.CreateG() = new G())
let z7 = (UseForwardedTypes.CreateH() = new G.H())

UseForwardedTypes.ConsumeC(UseForwardedTypes.CreateC())
UseForwardedTypes.ConsumeC(new C())
UseForwardedTypes.ConsumeD(UseForwardedTypes.CreateD())
UseForwardedTypes.ConsumeD(new D())
UseForwardedTypes.ConsumeGenericD(new GenericD<C>())
UseForwardedTypes.ConsumeGenericD(UseForwardedTypes.CreateGenericD())
UseForwardedTypes.ConsumeE(UseForwardedTypes.CreateE())
UseForwardedTypes.ConsumeE(new D.E())
UseForwardedTypes.ConsumeEE(new D.E.EE())

