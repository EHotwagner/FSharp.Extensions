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

let x1 = 1
let x2 = "hello"
let x3 = None
let x4 = None : int option
let x5 = []
let x6 = [1;2;3]
let x7 = new System.Windows.Forms.Form(Text="x7 form")
let x8 = Array2D.init 5 5 (fun i j -> i*10 + j)
let x9 = lazy (exit 999; "this lazy value should not be forced!!")  
