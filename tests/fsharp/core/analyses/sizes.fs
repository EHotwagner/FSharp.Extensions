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



open System

module BasicSizeTests = 
    let constantUnit () = ()           // note, size 0 - seems reasonable 
    let constantInteger () = 1         // note, size 0 - seems reasonable 
    let constantValue x = x            // note, size 1 - seems reasonable (this may be a field lookup to access an environment variable)
    let fieldLookup1 x = x.contents   // note, size 2 - seems reasonable 
    let libraryCall1 x = System.Console.WriteLine(x:string) // size 2, seems reasonable
    let libraryCall2 x = new System.Object() // size 1, seems reasonable
    let constantData1 () = Some 1      // note, size 2, seems reasonable, perhaps too low
    let constantData2 () = None        // note, size 2, seems reasonable, perhaps too high 
    let constantTuple1 () = (1,2)      // note, size 1, seems reasonable, perhaps too low
    let constantTuple2 () = (1,2,3,4)  // note, size 1, seems reasonable, perhaps too low

    let indirectCall1 f = f()          // note, size 1, seems reasonable
    let indirectCall2 f x = f x        // note, size 2, seems reasonable
    let indirectCall3 f x y = f x y    // note, size 3, seems reasonable
    let sequenceOfIndirectCalls1 f x = f x; f x  // note, size 4 = 2 + 2
    let ifThenElse x  = if x then 1 else 2  // note, size 5, seems reasonable
    let patternMatch1 x  = match x with 1 -> 2 | 2 -> 1 | _ -> 3 // note, size 8, seems reasonable
    let forLoop1 f x  = for i = 1 to 5 do f () // note, size 6, seems reasonable
    let whileLoop1 f x  = while true do f () // note, size 6, seems reasonable

printfn "hello"
