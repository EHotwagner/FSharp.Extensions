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

#light


let internal x = System.DateTime.Now

let useInternalValue() = x


type r = internal { x : int }

let rValue = { x = 1 }
let useInternalField(r) = r.x

type u = internal |  C of int

let useInternalTag() = C(1)

type internal x = XX | YY

let useInternalType() = box XX

