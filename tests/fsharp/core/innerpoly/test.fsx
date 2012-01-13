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
let report_failure () = stderr.WriteLine " NO"; failures := true
let test s b = stderr.Write(s:string);  if b then stderr.WriteLine " OK" else report_failure() 



module TestNullIsGeneralizeable = begin

   open System.Collections.Generic
   let nullList : List<'a> = null
   
   // check this is generic
   
   let v1 = (nullList : List<int>)
   let v2 = (nullList : List<string>)
end

let f (x:'a) = 
  let rec g1 y z  = g2 y z  
  and g2 y z  = g1 y z  in
  g1 "a" 1, g1 1 "a", g2 "a" "b", g2 3 4


#if OCAML_RECORD_FIELDS
type z = { x : 'a. int -> 'a }

let z2 = { x = (fun x -> failwith "a") }

let f3 (x:int) = failwith "a"
let z3 = { x = f3 }

let f2 n = 
  let z2 = { x = (fun (x:int) -> failwith (string_of_int (x+n))) } in 
  let f3 (x:int) = failwith "a" in
  z2

let _ : string = try (f2 3).x(3) ^ "unused" with Failure _ -> ""
#endif



let id x = x    

type ('a,'b) r = {a : 'a list; b: 'b list list }
type ('a,'b) r2 = R2 of  'a list * 'b list  list

let () =
  // yes folks, OCaml and F# support let-polymorphism for non-trivial patterns such as these
  let a,b = None,None in 
  let _ = (a : int option) in
  let _ = (a : string option) in
  let _ = (b : int option) in
  let _ = (b : string option) in
  let f (x:'a) (y:'b) =
    let _ = (a : 'a option) in
    let _ = (a : 'b option) in
    let _ = (b : 'a option) in
    let _ = (b : 'b option) in
    () in
  f 1 "a";
  f 1 1;
  let {a=a;b=b} = {a=[];b=[[]]} in 
  let _ = (a : int list) in
  let _ = (a : string list) in
  let _ = (b : int list list) in
  let _ = (b : string list list) in
  let f (x:'a) (y:'b) =
    let _ = (a : 'a list) in
    let _ = (a : 'a list) in
    let _ = (b : 'b list list) in
    let _ = (b : 'b list list) in
    () in
  f 1 "a";
  f 1 1;
  let (R2(a,b)) = R2 ([],[[]]) in 
  let _ = (a : int list) in
  let _ = (a : string list) in
  let _ = (b : int list list) in
  let _ = (b : string list list) in
  let f (x:'a) (y:'b) =
    let _ = (a : 'a list) in
    let _ = (a : 'a list) in
    let _ = (b : 'b list list) in
    let _ = (b : 'b list list) in
    () in
  f 1 "a";
  f 1 1;
  let (R2((a as a2),(b as b2))) = R2 ([],[[]]) in 
  let _ = (a2 : int list) in
  let _ = (a2 : string list) in
  let _ = (b2 : int list list) in
  let _ = (b2 : string list list) in
  let f (x:'a) (y:'b) =
    let _ = (a2 : 'a list) in
    let _ = (a2 : 'a list) in
    let _ = (b2 : 'b list list) in
    let _ = (b2 : 'b list list) in
    () in
  f 1 "a";
  f 1 1;
  // possibly-failing versions of the above

  let [(a,b)] = [(None,None)] in 
  let _ = (a : int option) in
  let _ = (a : string option) in
  let _ = (b : int option) in
  let _ = (b : string option) in
  let f (x:'a) (y:'b) =
    let _ = (a : 'a option) in
    let _ = (a : 'b option) in
    let _ = (b : 'a option) in
    let _ = (b : 'b option) in
    () in
  f 1 "a";
  f 1 1;
  let [{a=a;b=b}] = [{a=[];b=[[]]}] in 
  let _ = (a : int list) in
  let _ = (a : string list) in
  let _ = (b : int list list) in
  let _ = (b : string list list) in
  let f (x:'a) (y:'b) =
    let _ = (a : 'a list) in
    let _ = (a : 'a list) in
    let _ = (b : 'b list list) in
    let _ = (b : 'b list list) in
    () in
  f 1 "a";
  f 1 1;
  let [(R2(a,b))] = [R2 ([],[[]])] in 
  let _ = (a : int list) in
  let _ = (a : string list) in
  let _ = (b : int list list) in
  let _ = (b : string list list) in
  let f (x:'a) (y:'b) =
    let _ = (a : 'a list) in
    let _ = (a : 'a list) in
    let _ = (b : 'b list list) in
    let _ = (b : 'b list list) in
    () in
  f 1 "a";
  f 1 1;
  let [(R2((a as a2),(b as b2)))] = [R2 ([],[[]])] in 
  let _ = (a2 : int list) in
  let _ = (a2 : string list) in
  let _ = (b2 : int list list) in
  let _ = (b2 : string list list) in
  let f (x:'a) (y:'b) =
    let _ = (a2 : 'a list) in
    let _ = (a2 : 'a list) in
    let _ = (b2 : 'b list list) in
    let _ = (b2 : 'b list list) in
    () in
  f 1 "a";
  f 1 1; 
  ()

    

let _ =
      let f x = x in
      f (printfn "%s") "Hello, world!\n";
      f (printfn "%d") 3;
      f (printfn "%s") "Hello, world!\n"

let test5365() =
      let f x = x in
      f (printfn "%s") "Hello, world!\n";
      f (printfn "%d") 3;
      f (printfn "%s") "Hello, world!\n"

do test5365() 
do test5365() 

module TestOptimizationOfTypeFunctionsWithSideEffects = begin
    let count = ref 0
    let f<'a> = incr count; !count


    do test "eoeo23c1" (f<int> = 1)
    do test "eoeo23c2" (f<int> = 2)
    do test "eoeo23c3" (f<string> = 3)

    let x1 = f<int>

    do test "eoeo23c4" (x1 = 4)
    do test "eoeo23c5" (x1 = 4)
end

module Bug1126BenjaminTeuber = begin
    let Run() =         
        // put in the declaration and the error vanishes
        let PrintAll (values(* : int seq*)) =                    
            for value in values do                            
                printf "%i" value  
            done 
        let CallPrintAll (values : int seq) =        
            printfn "Caling Sum" ;
            values |> PrintAll in           
            printfn "Done" ;
        let MyFun () =                    
            let mySeq = [5 ; 5] |> List.toSeq  in
            mySeq |> CallPrintAll in               
        MyFun()        
            
    do Run()
end

module FSharp_1_0_Bug1024 = begin
    let count = ref 1
    let x<'a> = (count := !count + 1); typeof<'a>
    
    do test "vnwo9wu1" (!count = 1)
    let z0<'a> =  x<'a>
    do test "vnwo9wu1" (!count = 1)
    let z1 =  x<int>
    do test "vnwo9wu2" (!count = 2)
    let z2 =  x<int>
    do test "vnwo9wu3" (!count = 3)

end
module FSharp_1_0_Bug1024B = begin
    let count = ref 1
    let r<'a> = (count := !count + 1); ref ([] : 'a list)
    do test "vnwo9wu1" (!count = 1)
    let x1 = r<int>

    do test "vnwo9wu1" (!count = 2)
    let z0 =  x1
    do test "vnwo9wu1" (!count = 2)
    let (z1,z2) =  (x1,x1)
    do test "vnwo9wu2" (!count = 2)
    let z3 =  x1
    do test "vnwo9wu3" (!count = 2)

end



do (stdout.WriteLine "Test Passed"; 
    System.IO.File.WriteAllText("test.ok","ok"); 
    exit 0)



 
