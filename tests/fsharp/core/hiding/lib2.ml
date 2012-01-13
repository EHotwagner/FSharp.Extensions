//----------------------------------------------------------------------------
//
// Copyright (c) 2002-2010 Microsoft Corporation. 
//
// This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
// copy of the license can be found in the License.html file at the root of this distribution. 
// By using this source code in any fashion, you are agreeing to be bound 
// by the terms of the Apache License, Version 2.0.
//
// You must not remove this notice, or any other, from this software.
//----------------------------------------------------------------------------


type hidden = Lib.abstractType
type 'a visible = Lib.abstractType * 'a

let f1 (x: hidden) = ()
let f2 (x: hidden visible) = ()

exception D1 = Lib.A
exception D2 = Lib.C

let e3 = D1
let e2 = D2
