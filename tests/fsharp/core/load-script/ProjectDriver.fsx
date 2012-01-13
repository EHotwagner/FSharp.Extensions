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

#load "ThisProject.fsx"

[<System.Obsolete("x")>]
let fn x = 0
let y = fn 1 // This would be an 'obsolete' warning but ThisProject.fsx nowarns it

printfn "Result = %d" (Namespace.Type.Method())

let rf = typeof<System.Web.Mobile.CookielessData>
printfn "Type from referenced assembly = %s" (rf.ToString())
