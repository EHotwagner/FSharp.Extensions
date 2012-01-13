// This file was autogenerated by running the script in this directory
module Lib33
// These are some constant expressions which can be accessed from context
let c = 1
let b = true
// This is a value we will access from the outside.
// We expect accessing the value 'x' will _not_ trigger initialization of this module
let x = 2
// This is a value we can access from the outside to definitely force initialziation of the module
let mutable forceInit = 1
// This sets a value in another module to indicate that initialization has happened
InitFlag33.init <- true
