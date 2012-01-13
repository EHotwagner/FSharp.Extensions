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



let testEliminableBinding () = 
  let localShouldNotAppear = "stringShouldNotAppear" in 3

let testEliminableInnerFunction () = 
  let localFunctionShouldNotAppear x = "stringShouldNotAppear" in 3


let testMultipleEliminableBindings1 () = 
  let localShouldNotAppear1,localShouldNotAppear2 = "stringShouldNotAppear1","stringShouldNotAppear2" in 3

let testMultipleEliminableBindings2 () = 
  let localShouldNotAppear1 = "stringShouldNotAppear1" in 
  let localShouldNotAppear2 = localShouldNotAppear1 in 
  let localShouldNotAppear3 = localShouldNotAppear2 in 
  4

let testBetaVarAndBindingElim1 () = 
  let localShouldNotAppear1 = "liveString1" in 
  localShouldNotAppear1 

let testBetaVarAndBindingElim2 () = 
  let localShouldNotAppear1 = "liveString1" in 
  let localShouldNotAppear2 = localShouldNotAppear1 in 
  localShouldNotAppear2 

let testBetaVarAndBindingElim3 () = 
  let localShouldNotAppear1 = "liveString1" in 
  let localShouldNotAppear2 = localShouldNotAppear1 in 
  localShouldNotAppear1 


let testDeadStringInSeq () = ignore ("stringShouldNotAppear"); 3
let testDeadBindingInSeq () = (let localShouldNotAppear1 = "stringShouldNotAppear" in ()); 3

let testSimpleInliner1 () = 
  let simpleInlineeShouldNotAppear () = 3 in
  simpleInlineeShouldNotAppear ()

let testSimpleInliner2 () = 
  let simpleInlineeShouldNotAppear () = 3 in
  simpleInlineeShouldNotAppear () + simpleInlineeShouldNotAppear ()

let testSimplePolymorphicInliner1 () = 
  let simplePolymorphicInlineeShouldNotAppear x = 3 in
  ignore (simplePolymorphicInlineeShouldNotAppear 3); simplePolymorphicInlineeShouldNotAppear 4

let testSimpleInliner3 () = 
  let simpleInlineeShouldNotAppear () () = 3 in
  simpleInlineeShouldNotAppear () ()

let testBetaReduce1 () = 
  (fun simpleVarShouldNotAppear -> simpleVarShouldNotAppear + simpleVarShouldNotAppear) 3

let testSimpleProjection1 () = 
  let pairShouldNotAppear = ("stringFromPairShouldNotAppear","stringFromPair2") in 
  match pairShouldNotAppear with (varShouldNotAppear1,varShouldNotAppear2) -> varShouldNotAppear2

let testSimpleProjection2 () = 
  let tripleShouldNotAppear = ("stringFromPairShouldNotAppear",2,"stringFromPairShouldNotAppear") in 
  match tripleShouldNotAppear with (varShouldNotAppear1,varShouldNotAppear2,varShouldNotAppear3) -> varShouldNotAppear2
let testSimpleTupleMatchGetReduced () =
  match ("stringFromPairShouldNotAppear",2,"stringFromPairShouldNotAppear") with
    (varShouldNotAppear1,varShouldNotAppear2,varShouldNotAppear3) -> varShouldNotAppear2

let testSimpleListMatch1GetsReduced () =
  match [] with
    [] -> ()
  | _ -> failwith "stringFromSimpleListMatch1ShouldNotAppear"

let testSimpleListMatch2GetsReduced () =
  match [1] with
    [x] -> x
  | _ -> failwith "stringFromSimpleListMatch2ShouldNotAppear"

let testSimpleListMatch3GetsReduced (a,b,c) =
  let varShouldNotAppear1 = [a;b;c] in 
  match varShouldNotAppear1 with
    [x;y;z] -> y
  | _ -> failwith "stringFromSimpleListMatch3GetsReducedShouldNotAppear"

let testSimpleUnionProjection1 () =
  let x = ["stringCanAppear"; "stringShouldNotAppear"] in match x with h::_ -> h


let testSimpleUnionProjection2 () =
  let x = ["stringShouldNotAppear"; "stringCanAppear"] in match x with _::h::_ -> h

let testNestedTupleMatchGetsReduced () =
  match ("stringFromNestedTupleMatchShouldNotAppear",(2,"stringFromNestedTupleMatchShouldNotAppear")) with
    (varShouldNotAppear1,(varShouldNotAppear2,varShouldNotAppear3)) -> varShouldNotAppear2

let testUnitMatchIsOptimized () = 
  match () with 
    varShouldNotAppear -> varShouldNotAppear



