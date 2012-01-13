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


let failures = ref false
let report_failure () = 
  stderr.WriteLine " NO"; failures := true
let test s b = stderr.Write(s:string);  if b then stderr.WriteLine " OK" else report_failure() 


open System.Linq

let x = [1;2;3]

let xie = (x :> seq<_>)

xie.All(fun x -> x > 1)

xie.Average()
x.Average()

[<Struct>]
type S(v:int) =
    interface System.Collections.Generic.IEnumerable<int> with 
        member x.GetEnumerator() = (Seq.singleton v).GetEnumerator() 
    interface System.Collections.IEnumerable with 
        member x.GetEnumerator() = ((Seq.singleton v).GetEnumerator() :> System.Collections.IEnumerator)

let s : S = S(3)

s.Average()

        
[<Struct>]
type Struct(i:int) = 
    static let yellowStruct  = Struct(1)
    static let blueStruct  = Struct(0)

    static member YellowStruct  = yellowStruct
    static member BlueStruct  = blueStruct


(*--------------------*)  

  
let _ = 
  if !failures then (stdout.WriteLine "Test Failed"; exit 1) 

do (stdout.WriteLine "Test Passed"; 
    System.IO.File.WriteAllText("test.ok","ok"); 
    exit 0)
