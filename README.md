# Arpelle
Toy compiler and scripting language that is very much a work in progress. It produces C++ code. This was my summer project because I was unemployed and I wanted to learn about compilers.

Based off the TeenyTinyCompiler by @AZHenley, built in .NET 5 and C# instead of Python.

### Installation and Usage
I'll add binaries to the Release tab when I feel the project is far enough along to be considered "stable".

All you need is .NET 5 (SDK) and to run `dotnet build` in the root folder. The built file will be in the `bin/Debug/net5.0` folder.

Command Line Usage:

	arplc <input> [output]

When output is not given, it'll default to `arplc_output` in the current directory.


### Language
The language is BASIC like, kinda Python like.

Here's an incredibly simply program I use to test the features I've implemented so far.

    Set a As Number = 0
    Set b As Number = 1
    Set hello As String = "Hello World"
    Set flag As Boolean = True
    
    If flag == True Then
        Printout hello
        Set a = 1
        Set b = 2
        Input hello
    Else
        Printout "Not Hello World!"
    End
    
    While flag == True Repeat
        Printout hello
        Printout a
        Printout b
    End

#### Variable Assignment
Set a variable with a given datatype:

    Set <variable_name> AS <data_type> = <value>

Valid datatypes so far are Number (will automatically detect a float or integer), Boolean (True or False), and String

You can also reassign a variable, calculations aren't allowed yet but I'll add it eventually.

    Set <variable_name> = <value>

#### Console IO
Print a variable or string:

    Printout <value/variable>
  
 Get input and assign it to a variable. The variable must be set before hand with an initial value.

    Input <variable>
	
#### Control Structures and Loop
The compiler doesn't force any tab formatting, I just think it looks nice.

End should always end a block of code.

If and Else statements:
	
    If <expression> Do
	    <code>
    Else
        <code>
	End

While statements:

    While <expression> Repeat
	    <code>
	End