// $ANTLR 3.0.1 E:\\Projects\\Igneel\\HLSLCompiler\\preprocess.g 2015-02-07 23:04:02
namespace 
	Igneel.Compiling.Parser

{

using System.Collections.Generic;	
using Igneel.Compiling;
using Igneel.Compiling.Declarations;
using Igneel.Compiling.Expressions;
using Igneel.Compiling.Statements;
using Igneel.Compiling.Preprocessors;
using System.Text;


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;



public class preprocessParser : Parser 
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"INCLUDE", 
		"IFDEFINE", 
		"IFNDEFINE", 
		"ELIF", 
		"ENDIF", 
		"DEFINE", 
		"UNDEFINE", 
		"NUMBER", 
		"WS", 
		"ID", 
		"TEXT", 
		"INCLUDE_FILE", 
		"NL", 
		"PREPROC", 
		"LETTER"
    };

    public const int ELIF = 7;
    public const int WS = 12;
    public const int IFDEFINE = 5;
    public const int LETTER = 18;
    public const int INCLUDE = 4;
    public const int NUMBER = 11;
    public const int ENDIF = 8;
    public const int UNDEFINE = 10;
    public const int PREPROC = 17;
    public const int NL = 16;
    public const int TEXT = 14;
    public const int INCLUDE_FILE = 15;
    public const int ID = 13;
    public const int DEFINE = 9;
    public const int EOF = -1;
    public const int IFNDEFINE = 6;
    
    
        public preprocessParser(ITokenStream input) 
    		: base(input)
    	{
    		InitializeCyclicDFAs();
        }
        

    override public string[] TokenNames
	{
		get { return tokenNames; }
	}

    override public string GrammarFileName
	{
		get { return "E:\\Projects\\Igneel\\HLSLCompiler\\preprocess.g"; }
	}

    
    	




	private void InitializeCyclicDFAs()
	{
	}

 

}
}