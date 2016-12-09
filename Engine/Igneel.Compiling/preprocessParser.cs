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



public class PreprocessParser : Parser 
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

    public const int Elif = 7;
    public const int Ws = 12;
    public const int Ifdefine = 5;
    public const int Letter = 18;
    public const int Include = 4;
    public const int Number = 11;
    public const int Endif = 8;
    public const int Undefine = 10;
    public const int Preproc = 17;
    public const int Nl = 16;
    public const int Text = 14;
    public const int IncludeFile = 15;
    public const int Id = 13;
    public const int Define = 9;
    public const int Eof = -1;
    public const int Ifndefine = 6;
    
    
        public PreprocessParser(ITokenStream input) 
    		: base(input)
    	{
    		InitializeCyclicDfAs();
        }
        

    override public string[] TokenNames
	{
		get { return tokenNames; }
	}

    override public string GrammarFileName
	{
		get { return "E:\\Projects\\Igneel\\HLSLCompiler\\preprocess.g"; }
	}

    
    	




	private void InitializeCyclicDfAs()
	{
	}

 

}
}