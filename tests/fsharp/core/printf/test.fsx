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


open Printf
let failures = ref false
let report_failure () = 
  stderr.WriteLine " NO"; failures := true



let argv = System.Environment.GetCommandLineArgs() 
let SetCulture() = 
  if argv.Length > 2 && argv.[1] = "--culture" then  begin
    let cultureString = argv.[2] in 
    let culture = new System.Globalization.CultureInfo(cultureString) in 
    stdout.WriteLine ("Running under culture "+culture.ToString()+"...");
    System.Threading.Thread.CurrentThread.CurrentCulture <-  culture
  end 
  
do SetCulture()    

let test t s1 s2 = 
  if s1 <> s2 then 
    (stderr.WriteLine ("test "+t+": expected \n\t'"+s2+"' but produced \n\t'"+s1+"'");
     failures := true)
  else
    stdout.WriteLine ("test "+t+": correctly produced '"+s1+"'")   


let adjust1 obj n1 = unbox ((unbox obj) n1)


let _ = test "cewoui2a" (sprintf "%o" 0) "0"
let _ = test "cewoui2b" (sprintf "%o" 0) "0"
let _ = test "cewoui2c" (sprintf "%o" 5) "5"
let _ = test "cewoui2q" (sprintf "%o" 8) "10"
let _ = test "cewoui2w" (sprintf "%o" 15) "17"
let _ = test "cewoui2e" (sprintf "%o" System.Int32.MinValue) "20000000000"
let _ = test "cewoui2r" (sprintf "%o" 0xffffffff) "37777777777"
let _ = test "cewoui2t" (sprintf "%o" (System.Int32.MinValue+1)) "20000000001"
let _ = test "cewoui2y" (sprintf "%o" System.Int32.MaxValue) "17777777777"

let _ = test "cewoui2u" (sprintf "%o" (-1)) "37777777777"

let f = sprintf "%o"

let _ = test "cewoui2a" (f 0) "0"
let _ = test "cewoui2s" (f 0) "0"
let _ = test "cewoui2d" (f 5) "5"
let _ = test "cewoui2f" (f 8) "10"
let _ = test "cewoui2g" (f 15) "17"
let _ = test "cewoui2h" (f System.Int32.MinValue) "20000000000"
let _ = test "cewoui2j" (f 0xffffffff) "37777777777"
let _ = test "cewoui2lk" (f (System.Int32.MinValue+1)) "20000000001"
let _ = test "cewoui2l" (f System.Int32.MaxValue) "17777777777"

let _ = test "cewoui212" (f (-1)) "37777777777"

// Test nothing comes out until all arguments have been applied
let _ = test "csd3oui2!" (let buf = new System.Text.StringBuilder() in ignore (bprintf buf "%x%x" 0); buf.ToString()) ""
let _ = test "csd3oui2!" (let buf = new System.Text.StringBuilder() in ignore (bprintf buf "%x%x" 0 1); buf.ToString()) "01"
let _ = test "csd3oui2!" (let buf = new System.Text.StringBuilder() in ignore (bprintf buf "%s"); buf.ToString()) ""
let _ = test "csd3oui2!" (let buf = new System.Text.StringBuilder() in ignore (bprintf buf "%s" "abc"); buf.ToString()) "abc"

let _ = test "cewoui2!" (sprintf "%x" 0) "0"
let _ = test "cewoui26" (sprintf "%x" 5) "5"
let _ = test "cewoui2f" (sprintf "%x" 8) "8"
let _ = test "cewoui29" (sprintf "%x" 15) "f"
let _ = test "cewoui2Z" (sprintf "%x" System.Int32.MinValue) "80000000"
let _ = test "cewoui2X" (sprintf "%x" 0xffffffff) "ffffffff"
let _ = test "cewoui2A" (sprintf "%x" (System.Int32.MinValue+1)) "80000001"
let _ = test "cewoui2Q" (sprintf "%x" System.Int32.MaxValue) "7fffffff"

let fx = sprintf "%x"
let _ = test "cewoui2W" (fx 0) "0"
let _ = test "cewoui2E" (fx 5) "5"
let _ = test "cewoui2R" (fx 8) "8"
let _ = test "cewoui2T" (fx 15) "f"
let _ = test "cewoui2Y" (fx System.Int32.MinValue) "80000000"
let _ = test "cewoui2U" (fx 0xffffffff) "ffffffff"
let _ = test "cewoui2I" (fx (System.Int32.MinValue+1)) "80000001"
let _ = test "cewoui2O" (fx System.Int32.MaxValue) "7fffffff"

let _ = test "cewoui2Z" (sprintf "%X" 0) "0"
let _ = test "cewoui2X" (sprintf "%X" 5) "5"
let _ = test "cewoui2C" (sprintf "%X" 8) "8"
let _ = test "cewoui2V" (sprintf "%X" 15) "F"
let _ = test "cewoui2v" (sprintf "%X" System.Int32.MinValue) "80000000"
let _ = test "cewoui2B" (sprintf "%X" 0xffffffff) "FFFFFFFF"
let _ = test "cewoui2N" (sprintf "%X" (System.Int32.MinValue+1)) "80000001"
let _ = test "cewoui2M" (sprintf "%X" System.Int32.MaxValue) "7FFFFFFF"

let _ = test "cewou44a" (sprintf "%u" 0) "0"
let _ = test "cewou44b" (sprintf "%u" 5) "5"
let _ = test "cewou44c" (sprintf "%u" 8) "8"
let _ = test "cewou44d" (sprintf "%u" 15) "15"
let _ = test "cewou44e" (sprintf "%u" System.Int32.MinValue) "2147483648"
let _ = test "cewou44f" (sprintf "%u" 0xffffffff) "4294967295"
let _ = test "cewou44g" (sprintf "%u" (System.Int32.MinValue+1)) "2147483649"
let _ = test "cewou44h" (sprintf "%u" System.Int32.MaxValue) "2147483647"

let _ = test "cewou45a" (sprintf "%d" 0ul) "0"
let _ = test "cewou45b" (sprintf "%d" 5ul) "5"
let _ = test "cewou45c" (sprintf "%d" 8ul) "8"
let _ = test "cewou45d" (sprintf "%d" 15ul) "15"
let _ = test "cewou45e" (sprintf "%d" 2147483648ul) "2147483648"
let _ = test "cewou45f" (sprintf "%d" 4294967295ul) "4294967295"
let _ = test "cewou45g" (sprintf "%d" 2147483649ul) "2147483649"
let _ = test "cewou45h" (sprintf "%d" 2147483647ul) "2147483647"

let _ = test "cewou46a" (sprintf "%d" 0ul) "0"
let _ = test "cewou46b" (sprintf "%d" 5ul) "5"
let _ = test "cewou46c" (sprintf "%d" 8ul) "8"
let _ = test "cewou46d" (sprintf "%d" 15ul) "15"
let _ = test "cewou46e" (sprintf "%d" 2147483648ul) "2147483648"
let _ = test "cewou46f" (sprintf "%d" 4294967295ul) "4294967295"
let _ = test "cewou46g" (sprintf "%d" 2147483649ul) "2147483649"
let _ = test "cewou46h" (sprintf "%d" 2147483647ul) "2147483647"

let _ = test "ceew903" (sprintf "%u" System.Int64.MaxValue) "9223372036854775807"
let _ = test "ceew903" (sprintf "%u" System.Int64.MinValue) "9223372036854775808"
let _ = test "ceew903" (sprintf "%d" System.Int64.MaxValue) "9223372036854775807"
let _ = test "ceew903" (sprintf "%d" System.Int64.MinValue) "-9223372036854775808"

let _ = test "ceew903" (sprintf "%u" System.Int64.MaxValue) "9223372036854775807"
let _ = test "ceew903" (sprintf "%u" System.Int64.MinValue) "9223372036854775808"
let _ = test "ceew903" (sprintf "%d" System.Int64.MaxValue) "9223372036854775807"
let _ = test "ceew903" (sprintf "%d" System.Int64.MinValue) "-9223372036854775808"

let _ = test "cewou47a" (sprintf "%d" 0n) "0"
let _ = test "cewou47b" (sprintf "%d" 5n) "5"
let _ = test "cewou47c" (sprintf "%d" 8n) "8"
let _ = test "cewou47d" (sprintf "%d" 15n) "15"
let _ = test "cewou47e" (sprintf "%u" 2147483648n) "2147483648"
let _ = test "cewou47f" (sprintf "%u" 4294967295n) "4294967295"
let _ = test "cewou47g" (sprintf "%u" 2147483649n) "2147483649"
let _ = test "cewou47h" (sprintf "%u" 2147483647n) "2147483647"

let _ = test "cewou47a" (sprintf "%d" 0n) "0"
let _ = test "cewou47b" (sprintf "%d" 5n) "5"
let _ = test "cewou47c" (sprintf "%d" 8n) "8"
let _ = test "cewou47d" (sprintf "%d" 15n) "15"
let _ = test "cewou47e" (sprintf "%u" 2147483648n) "2147483648"
let _ = test "cewou47f" (sprintf "%u" 4294967295n) "4294967295"
let _ = test "cewou47g" (sprintf "%u" 2147483649n) "2147483649"
let _ = test "cewou47h" (sprintf "%u" 2147483647n) "2147483647"

let _ = test "cewou48a" (sprintf "%d" 0un) "0"
let _ = test "cewou48b" (sprintf "%d" 5un) "5"
let _ = test "cewou48c" (sprintf "%d" 8un) "8"
let _ = test "cewou48d" (sprintf "%d" 15un) "15"
let _ = test "cewou48e" (sprintf "%u" 2147483648un) "2147483648"
let _ = test "cewou48f" (sprintf "%u" 4294967295un) "4294967295"
let _ = test "cewou48g" (sprintf "%u" 2147483649un) "2147483649"
let _ = test "cewou48h" (sprintf "%u" 2147483647un) "2147483647"

let _ = test "cewou59a" (sprintf "%d" 0un) "0"
let _ = test "cewou59b" (sprintf "%d" 5un) "5"
let _ = test "cewou59c" (sprintf "%d" 8un) "8"
let _ = test "cewou59d" (sprintf "%d" 15un) "15"
let _ = test "cewou59e" (sprintf "%u" 2147483648un) "2147483648"
let _ = test "cewou59f" (sprintf "%u" 4294967295un) "4294967295"
let _ = test "cewou59g" (sprintf "%u" 2147483649un) "2147483649"
let _ = test "cewou59h" (sprintf "%u" 2147483647un) "2147483647"

let _ = test "cewou49a" (sprintf "%d" 0) "0"
let _ = test "cewou49b" (sprintf "%d" 5) "5"
let _ = test "cewou49c" (sprintf "%+d" 5) "+5"
let _ = test "cewou49d" (sprintf "% d" 5) " 5"
let _ = test "cewou49e" (sprintf "%+4d" 5) "  +5"
let _ = test "cewou49f" (sprintf "%-+4d" 5) "+5  "
let _ = test "cewou49g" (sprintf "%-4d" 5) "5   "
let _ = test "cewou49h" (sprintf "%- 4d" 5) " 5  "
let _ = test "cewou49i" (sprintf "%- d" 5) " 5"
let _ = test "cewou49j" (sprintf "% d" 5) " 5"
let _ = test "weioj31" (sprintf "%*d" 3 5) "  5"
let _ = test "weioj31" (sprintf "%3d" 5) "  5"
let _ = test "weioj32" (sprintf "%1d" 5) "5"
let _ = test "weioj32" (sprintf "%*d" 1 5) "5"
let _ = test "weioj33" (sprintf "%2d" 500) "500"
let _ = test "weioj33" (sprintf "%*d" 2 500) "500"
let _ = test "weioj34" (sprintf "%3d" 500) "500"
let _ = test "weioj34" (sprintf "%*d" 3 500) "500"
let _ = test "weioj35" (sprintf "%d" 501) "501"
let _ = test "weioj36" (sprintf "%2d" (-4)) "-4"
let _ = test "weioj36" (sprintf "%*d" 2 (-4)) "-4"
let _ = test "weioj37" (sprintf "%1d" (-4)) "-4"
let _ = test "weioj37" (sprintf "%*d" 1 (-4)) "-4"
let _ = test "weioj38" (sprintf "%d" (-401)) "-401"
let _ = test "weioj39" (sprintf "%d" 2147483647) "2147483647"
let _ = test "weioj3a" (sprintf "%d" (-2147483647)) "-2147483647"
let _ = test "weioj3s" (sprintf "%d" (-2147483648)) "-2147483648"


let _ = test "weioj3d" (sprintf "print test %O with suffix" 1) "print test 1 with suffix"
let _ = test "weioj3f" (sprintf "print test %O %O with suffix" 1 "xyz") "print test 1 xyz with suffix"
let _ = test "weioj3g" (sprintf "print test %M with suffix" (System.Convert.ToDecimal(3))) "print test 3 with suffix"
let _ = test "weioj3h" (sprintf "print test %M with suffix" (System.Convert.ToDecimal(3.02))) "print test 3.02 with suffix"

let _ = test "weioj3j" (sprintf "%O" 3I) "3"


let _ = test "weiodasj3" (sprintf "%f" 0.0) "0.000000"
let _ = test "weiogwej3" (sprintf "%10f" 0.0) "  0.000000"
let _ = test "weiogwej3" (sprintf "%*f" 10 0.0) "  0.000000"
let _ = test "weiobtj3" (sprintf "%7f" 0.0) "0.000000"
let _ = test "weiobtj3" (sprintf "%*f" 7 0.0) "0.000000"
let _ = test "weiorwej3" (sprintf "%7.1f" 0.0) "    0.0"
let _ = test "weiorwej3" (sprintf "%*.1f" 7 0.0) "    0.0"
let _ = test "weiorwej3" (sprintf "%7.*f" 1 0.0) "    0.0"
let _ = test "weiorwej3" (sprintf "%*.*f" 7 1 0.0) "    0.0"
let _ = test "weivewoj3" (sprintf "%7.2f" 0.0) "   0.00"
let _ = test "weivewoj3" (sprintf "%*.2f" 7 0.0) "   0.00"
let _ = test "weivewoj3" (sprintf "%*.*f" 7 2 0.0) "   0.00"
let _ = test "weivewoj3" (sprintf "%7.*f" 2 0.0) "   0.00"
let _ = test "weiqfoj3" (sprintf "%7.0f" 0.0) "      0"
let _ = test "weiqfoj3" (sprintf "%*.0f" 7 0.0) "      0"
let _ = test "weiqfoj3" (sprintf "%7.*f" 0 0.0) "      0"
let _ = test "weiqfoj3" (sprintf "%*.*f" 7 0 0.0) "      0"
let _ = test "weieroj3" (sprintf "%10.2e" 1.0) " 1.00e+000"
let _ = test "weieroj3" (sprintf "%*.2e" 10 1.0) " 1.00e+000"
let _ = test "weio34j3" (sprintf "%10.2E" 1.0) " 1.00E+000"
let _ = test "weio34j3" (sprintf "%10.*E" 2 1.0) " 1.00E+000"
let _ = test "weiberoj3" (sprintf "%10.3E" 1.0) "1.000E+000"
let _ = test "weiberoj3" (sprintf "%10.*E" 3 1.0) "1.000E+000"
let _ = test "weiqfwoj3" (sprintf "%10g" 1.0) "         1"
let _ = test "weiqfwoj3" (sprintf "%*g" 10 1.0) "         1"
let _ = test "weiof33j3" (sprintf "%10g" 1.01) "      1.01"
let _ = test "weiof33j3" (sprintf "%*g" 10 1.01) "      1.01"
let _ = test "wei54goj3" (sprintf "%-10g" 1.01) "1.01      "
let _ = test "wei54goj3" (sprintf "%-*g" 10 1.01) "1.01      "
let _ = test "weioqf3j3" (sprintf "%g" 1.01) "1.01"
(* NEG: let _ = test "weioqf3j3" (sprintf "%g" 1) "1.01" *)


let _ = test "wekodasj3" (sprintf "%f" 0.0f) "0.000000"
let _ = test "wekogwej3" (sprintf "%10f" 0.0f) "  0.000000"
let _ = test "wekobtj3" (sprintf "%7f" 0.0f) "0.000000"
let _ = test "wekorwej3" (sprintf "%7.1f" 0.0f) "    0.0"
let _ = test "wekvewoj3" (sprintf "%7.2f" 0.0f) "   0.00"
let _ = test "wekqfoj3" (sprintf "%7.0f" 0.0f) "      0"
let _ = test "wekeroj3" (sprintf "%10.2e" 1.0f) " 1.00e+000"
let _ = test "weko34j3" (sprintf "%10.2E" 1.0f) " 1.00E+000"
let _ = test "wekberoj3" (sprintf "%10.3E" 1.0f) "1.000E+000"
let _ = test "wekqfwoj3" (sprintf "%10g" 1.0f) "         1"
let _ = test "wekof33j3" (sprintf "%10g" 1.01f) "      1.01"
let _ = test "wek54goj3" (sprintf "%-10g" 1.01f) "1.01      "
let _ = test "wekoqf3j3" (sprintf "%g" 1.01f) "1.01"


let _ = test "weioj3Q" (sprintf "%a" (fun () -> string) 10) "10"
let _ = test "weioj3W" (sprintf "%a%a" (fun () s -> s+s) "a" (fun () s -> s+s) "b") "aabb"
(* NEG: let _ = test "weioj3" (sprintf "%a" (fun () -> string_of_int) "a") "10" *)

let _ = test "weioj3ff" (try failwithf "%a%a" (fun () s -> s+s) "a" (fun () s -> s+s) "b" with Failure s -> s) "aabb"
let _ = test "weioj3ffdd" (string (try if true then failwithf "%s" "abc" else 1 with Failure "abc" -> 2)) "2"
let _ = test "weioj3ffd2" (try if true then failwithf "%s" "abc" else "d"with Failure "abc" -> "e") "e"

let _ = test "weioj3" (sprintf "%t" (fun () -> "10")) "10"
  
let bug600 = sprintf "%d"
let _ = test "bug600a1" (bug600 2) "2" 
let _ = test "bug600b1" (bug600 2) "2" (* not 22! *)

let bug600b = sprintf "%s"
let _ = test "bug600a2" (bug600b "2") "2" 
let _ = test "bug600b2" (bug600b "2") "2" (* not 22! *)

let bug600c = sprintf "%x"
let _ = test "bug600a3" (bug600c 2) "2" 
let _ = test "bug600b3" (bug600c 2) "2" (* not 22! *)

let _ = 
  if !failures then (stdout.WriteLine "Test Failed"; exit 1) 

let _ = test "ckwoih" (sprintf "%x" 0xFFy) ("ff")
let _ = test "ckwoih" (sprintf "%x" 0xFFFFs) ("ffff")
let _ = test "ckwoih" (sprintf "%x" 0xFFFFFFFF) ("ffffffff")
let _ = test "ckwoih" (sprintf "%x" 0xFFFFFFFFFFFFFFFFL) ("ffffffffffffffff")
let _ = test "ckwoih" (sprintf "%x" 0xFFFFFFFFn) ("ffffffff")

let _ = test "ckwoih" (sprintf "%x" 0xFFuy) ("ff")
let _ = test "ckwoih" (sprintf "%x" 0xFFFFus) ("ffff")
let _ = test "ckwoih" (sprintf "%x" 0xFFFFFFFFu) ("ffffffff")
let _ = test "ckwoih" (sprintf "%x" 0xFFFFFFFFFFFFFFFFUL) ("ffffffffffffffff")
let _ = test "ckwoih" (sprintf "%x" 0xFFFFFFFFun) ("ffffffff")

// Check one with a suffix
module CheckDisplayAttributes1 =
    [<StructuredFormatDisplay("{StructuredDisplay}N")>]
    type Foo() = 
       member x.StructuredDisplay = 3

    test "cenwoiwe1" (sprintf "%A" (Foo())) "3N"

// Check one with a prefix
module CheckDisplayAttributes2 =

    [<StructuredFormatDisplay("N{StructuredDisplay}")>]
    type Foo() = 
       member x.StructuredDisplay = 3

    test "cenwoiwe2" (sprintf "%A" (Foo())) "N3"
    
// Check one with a prefix returning a string
module CheckDisplayAttributes3 =

    [<StructuredFormatDisplay("N{StructuredDisplay}")>]
    type Foo() = 
       member x.StructuredDisplay = "3"

    test "cenwoiwe3" (sprintf "%A" (Foo())) "N3"

// Check one returning a string
module CheckDisplayAttributes4 =

    [<StructuredFormatDisplay("{StructuredDisplay}")>]
    type Foo() = 
       member x.StructuredDisplay = "3"

    test "cenwoiwe4" (sprintf "%A" (Foo())) "3"
    
// Check one with an internal property
module CheckDisplayAttributes5 =

    [<StructuredFormatDisplay("{StructuredDisplay}")>]
    type Foo() = 
       member internal x.StructuredDisplay = "3"

    test "cenwoiwe5" (sprintf "%A" (Foo())) "3"
    
// Check one with spaces
module CheckDisplayAttributes6 =

    [<StructuredFormatDisplay("  {StructuredDisplay}  ")>]
    type Foo() = 
       member internal x.StructuredDisplay = "3"

    test "cenwoiwe6" (sprintf "%A" (Foo())) "  3  "
    
// Check an ill-formed StructuredFormatDisplay string is not shown
module CheckDisplayAttributes7 =

    [<StructuredFormatDisplay("{StructuredDisplay")>]
    type Foo() = 
       member internal x.StructuredDisplay = "3"
       override x.ToString() = "2"

    test "cenwoiwe7" (sprintf "%A" (Foo())) "2"
    
// Check one returning a list
module CheckDisplayAttributes8 =

    [<StructuredFormatDisplay("{StructuredDisplay}")>]
    type Foo() = 
       member internal x.StructuredDisplay = [1;2]
       override x.ToString() = "2"

    test "cenwoiwe8" (sprintf "%A" (Foo())) "[1; 2]"


let _ = 
  if !failures then (printf "Test Failed"; exit 1) 

do (stdout.WriteLine "Test Passed"; 
    System.IO.File.WriteAllText("test.ok","ok"); 
    exit 0)

