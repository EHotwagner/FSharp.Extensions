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



module UseForwardedTypes 

let CreateC() = new C()
let CreateD() = new D()
let CreateGenericD() = new GenericD<C>()
let CreateE() = new D.E()
let CreateEE() = new D.E.EE()

let CreateF() = new F()
let CreateG() = new G()
let CreateH() = new G.H()

let ConsumeC(x) = F.ConsumeC(x)
let ConsumeD(x) = F.ConsumeD(x)
let ConsumeGenericD(x) = F.ConsumeGenericD(x)
let ConsumeE(x) = F.ConsumeE(x)
let ConsumeEE(x) = F.ConsumeEE(x)

