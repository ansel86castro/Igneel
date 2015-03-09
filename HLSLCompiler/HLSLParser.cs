// $ANTLR 3.0.1 E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g 2015-03-08 14:43:04
namespace 
	Igneel.Compiling.Parser

{

using System.Collections.Generic;	
using Igneel.Compiling;
using Igneel.Compiling.Declarations;
using Igneel.Compiling.Expressions;
using Igneel.Compiling.Statements;
using Igneel.Graphics;



using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;

using IDictionary	= System.Collections.IDictionary;
using Hashtable 	= System.Collections.Hashtable;


public class HLSLParser : Parser 
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"EQUAL", 
		"NEQUAL", 
		"LESS", 
		"LEQUAL", 
		"GREATER", 
		"GEQUAL", 
		"NOT", 
		"MUL", 
		"DIV", 
		"ADD", 
		"INCREMENT", 
		"SUB", 
		"DECREMENT", 
		"OR", 
		"AND", 
		"XOR", 
		"DOT", 
		"COMMA", 
		"SEMICOLON", 
		"COLON", 
		"LBRACKET", 
		"RBRACKET", 
		"LPAREN", 
		"RPAREN", 
		"LBRACE", 
		"RBRACE", 
		"ASSIGN", 
		"ADDASSIGN", 
		"SUBASSIGN", 
		"MULASSIGN", 
		"DIVASSIGN", 
		"QUESTION", 
		"IF", 
		"ELSE", 
		"WHILE", 
		"DO", 
		"FOR", 
		"BREAK", 
		"CONTINUE", 
		"RETURN", 
		"DISCARD", 
		"CONST", 
		"UNIFORM", 
		"STATIC", 
		"CENTROID", 
		"LINEAR", 
		"NOINTERP", 
		"IN", 
		"OUT", 
		"INOUT", 
		"STRUCT", 
		"REG", 
		"PACK", 
		"CBUFFER", 
		"TBUFFER", 
		"ID", 
		"NUMBER", 
		"STRING_LITERAL", 
		"BOOL_CONSTANT", 
		"ZERO", 
		"OCTAL_DIGIT", 
		"NON_ZERO_DIGIT", 
		"DIGIT", 
		"OCTAL_CONSTANT", 
		"HEX_DIGIT", 
		"HEX_CONSTANT", 
		"DIGIT_SEQUENCE", 
		"EXPONENT", 
		"FLOAT_SUFFIX", 
		"LETTER", 
		"COMMENT", 
		"WS", 
		"EscapeSequence", 
		"OctalEscape"
    };

    public const int EXPONENT = 71;
    public const int FLOAT_SUFFIX = 72;
    public const int WHILE = 38;
    public const int LETTER = 73;
    public const int CONST = 45;
    public const int LBRACE = 28;
    public const int DO = 39;
    public const int FOR = 40;
    public const int SUB = 15;
    public const int UNIFORM = 46;
    public const int NOT = 10;
    public const int ID = 59;
    public const int AND = 18;
    public const int EOF = -1;
    public const int SUBASSIGN = 32;
    public const int OCTAL_DIGIT = 64;
    public const int BREAK = 41;
    public const int LPAREN = 26;
    public const int ZERO = 63;
    public const int IF = 36;
    public const int INOUT = 53;
    public const int LBRACKET = 24;
    public const int LEQUAL = 7;
    public const int RPAREN = 27;
    public const int LINEAR = 49;
    public const int STRING_LITERAL = 61;
    public const int GREATER = 8;
    public const int IN = 51;
    public const int BOOL_CONSTANT = 62;
    public const int CONTINUE = 42;
    public const int COMMA = 21;
    public const int GEQUAL = 9;
    public const int EQUAL = 4;
    public const int LESS = 6;
    public const int RETURN = 43;
    public const int NOINTERP = 50;
    public const int DIGIT = 66;
    public const int RBRACKET = 25;
    public const int COMMENT = 74;
    public const int DOT = 20;
    public const int ADD = 13;
    public const int TBUFFER = 58;
    public const int NEQUAL = 5;
    public const int XOR = 19;
    public const int MULASSIGN = 33;
    public const int DIVASSIGN = 34;
    public const int RBRACE = 29;
    public const int NON_ZERO_DIGIT = 65;
    public const int STATIC = 47;
    public const int ELSE = 37;
    public const int NUMBER = 60;
    public const int HEX_DIGIT = 68;
    public const int STRUCT = 54;
    public const int SEMICOLON = 22;
    public const int HEX_CONSTANT = 69;
    public const int REG = 55;
    public const int MUL = 11;
    public const int DECREMENT = 16;
    public const int COLON = 23;
    public const int INCREMENT = 14;
    public const int WS = 75;
    public const int DISCARD = 44;
    public const int QUESTION = 35;
    public const int CENTROID = 48;
    public const int ADDASSIGN = 31;
    public const int OUT = 52;
    public const int CBUFFER = 57;
    public const int DIGIT_SEQUENCE = 70;
    public const int OR = 17;
    public const int ASSIGN = 30;
    public const int OCTAL_CONSTANT = 67;
    public const int DIV = 12;
    public const int PACK = 56;
    public const int OctalEscape = 77;
    public const int EscapeSequence = 76;
    
    
        public HLSLParser(ITokenStream input) 
    		: base(input)
    	{
    		InitializeCyclicDFAs();
            ruleMemo = new IDictionary[46+1];
         }
        

    override public string[] TokenNames
	{
		get { return tokenNames; }
	}

    override public string GrammarFileName
	{
		get { return "E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g"; }
	}

    
    Scope scope;
    int line,column;
    
    List<RecognitionException> _errors = new List<RecognitionException>();
    
    public Scope GlobalScope{ get { return scope; } set{ scope = value; }}
    public List<RecognitionException> Errors { get { return _errors; } }
    
     public override void ReportError(RecognitionException e)
     {
                _errors.Add(e);
                base.ReportError(e);
     }
    	 


    
    // $ANTLR start program
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:103:1: program returns [ProgramFile file = new ProgramFile()] : d= declaration (d= declaration )* EOF ;
    public HLSLProgram program() // throws RecognitionException [1]
    {   

        HLSLProgram file =  new HLSLProgram();
    
        Declaration d = null;
        
    
         
        	List<Declaration> decs = new List<Declaration>();
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:107:2: (d= declaration (d= declaration )* EOF )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:107:3: d= declaration (d= declaration )* EOF
            {
            	PushFollow(FOLLOW_declaration_in_program588);
            	d = declaration();
            	followingStackPointer_--;
            	if (failed) return file;
            	if ( backtracking == 0 ) 
            	{
            	   decs.Add(d); 
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:107:33: (d= declaration )*
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);
            	    
            	    if ( (LA1_0 == LBRACKET || (LA1_0 >= UNIFORM && LA1_0 <= STATIC) || LA1_0 == STRUCT || (LA1_0 >= CBUFFER && LA1_0 <= ID)) )
            	    {
            	        alt1 = 1;
            	    }
            	    
            	
            	    switch (alt1) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:107:35: d= declaration
            			    {
            			    	PushFollow(FOLLOW_declaration_in_program595);
            			    	d = declaration();
            			    	followingStackPointer_--;
            			    	if (failed) return file;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	   decs.Add(d); 
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop1;
            	    }
            	} while (true);
            	
            	loop1:
            		;	// Stops C# compiler whinging that label 'loop1' has no statements

            	if ( backtracking == 0 ) 
            	{
            	   
            	  		file.Declarations = decs.ToArray(); 		
            	  	
            	}
            	Match(input,EOF,FOLLOW_EOF_in_program607); if (failed) return file;
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return file;
    }
    // $ANTLR end program

    
    // $ANTLR start declaration
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:113:1: declaration returns [Declaration node] : ( (uni= UNIFORM | st= STATIC (con= CONST )? ) type= type_ref name= ID v= variable_declaration[type] | attr= attributes type= type_ref (e= fixed_array )? name= ID f= function_declaration | type= type_ref (name= ID (v= variable_declaration[type] | f= function_declaration ) | e= fixed_array name= ID f= function_declaration ) | s= struct_declaration | cb= cbuffer_declaration );
    public Declaration declaration() // throws RecognitionException [1]
    {   

        Declaration node = null;
    
        IToken uni = null;
        IToken st = null;
        IToken con = null;
        IToken name = null;
        TypeRef type = null;

        GlobalVariableDeclaration v = null;

        List<AttributeDeclaration> attr = null;

        int e = 0;

        UserFunctionDeclaration f = null;

        StructDeclaration s = null;

        ConstantBufferDeclaration cb = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:114:2: ( (uni= UNIFORM | st= STATIC (con= CONST )? ) type= type_ref name= ID v= variable_declaration[type] | attr= attributes type= type_ref (e= fixed_array )? name= ID f= function_declaration | type= type_ref (name= ID (v= variable_declaration[type] | f= function_declaration ) | e= fixed_array name= ID f= function_declaration ) | s= struct_declaration | cb= cbuffer_declaration )
            int alt7 = 5;
            switch ( input.LA(1) ) 
            {
            case UNIFORM:
            case STATIC:
            	{
                alt7 = 1;
                }
                break;
            case LBRACKET:
            	{
                alt7 = 2;
                }
                break;
            case ID:
            	{
                alt7 = 3;
                }
                break;
            case STRUCT:
            	{
                alt7 = 4;
                }
                break;
            case CBUFFER:
            case TBUFFER:
            	{
                alt7 = 5;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return node;}
            	    NoViableAltException nvae_d7s0 =
            	        new NoViableAltException("113:1: declaration returns [Declaration node] : ( (uni= UNIFORM | st= STATIC (con= CONST )? ) type= type_ref name= ID v= variable_declaration[type] | attr= attributes type= type_ref (e= fixed_array )? name= ID f= function_declaration | type= type_ref (name= ID (v= variable_declaration[type] | f= function_declaration ) | e= fixed_array name= ID f= function_declaration ) | s= struct_declaration | cb= cbuffer_declaration );", 7, 0, input);
            
            	    throw nvae_d7s0;
            }
            
            switch (alt7) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:115:2: (uni= UNIFORM | st= STATIC (con= CONST )? ) type= type_ref name= ID v= variable_declaration[type]
                    {
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:115:2: (uni= UNIFORM | st= STATIC (con= CONST )? )
                    	int alt3 = 2;
                    	int LA3_0 = input.LA(1);
                    	
                    	if ( (LA3_0 == UNIFORM) )
                    	{
                    	    alt3 = 1;
                    	}
                    	else if ( (LA3_0 == STATIC) )
                    	{
                    	    alt3 = 2;
                    	}
                    	else 
                    	{
                    	    if ( backtracking > 0 ) {failed = true; return node;}
                    	    NoViableAltException nvae_d3s0 =
                    	        new NoViableAltException("115:2: (uni= UNIFORM | st= STATIC (con= CONST )? )", 3, 0, input);
                    	
                    	    throw nvae_d3s0;
                    	}
                    	switch (alt3) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:115:4: uni= UNIFORM
                    	        {
                    	        	uni = (IToken)input.LT(1);
                    	        	Match(input,UNIFORM,FOLLOW_UNIFORM_in_declaration627); if (failed) return node;
                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:115:18: st= STATIC (con= CONST )?
                    	        {
                    	        	st = (IToken)input.LT(1);
                    	        	Match(input,STATIC,FOLLOW_STATIC_in_declaration633); if (failed) return node;
                    	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:115:28: (con= CONST )?
                    	        	int alt2 = 2;
                    	        	int LA2_0 = input.LA(1);
                    	        	
                    	        	if ( (LA2_0 == CONST) )
                    	        	{
                    	        	    alt2 = 1;
                    	        	}
                    	        	switch (alt2) 
                    	        	{
                    	        	    case 1 :
                    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:115:29: con= CONST
                    	        	        {
                    	        	        	con = (IToken)input.LT(1);
                    	        	        	Match(input,CONST,FOLLOW_CONST_in_declaration638); if (failed) return node;
                    	        	        
                    	        	        }
                    	        	        break;
                    	        	
                    	        	}

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	PushFollow(FOLLOW_type_ref_in_declaration645);
                    	type = type_ref();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	name = (IToken)input.LT(1);
                    	Match(input,ID,FOLLOW_ID_in_declaration649); if (failed) return node;
                    	PushFollow(FOLLOW_variable_declaration_in_declaration653);
                    	v = variable_declaration(type);
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	  	        
                    	  	        v.Name = name.Text;
                    	  	        v.Line = name.Line;
                    	  	        v.Column = name.CharPositionInLine;
                    	  		if(uni!=null)
                    	  			v.Storage = VarStorage.Uniform;
                    	  		else if(st!=null)
                    	  		{
                    	  		    v.Storage = con!=null?(VarStorage.Static | VarStorage.Const) : VarStorage.Static;
                    	  		}
                    	  		node = v;
                    	  	   
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:128:4: attr= attributes type= type_ref (e= fixed_array )? name= ID f= function_declaration
                    {
                    	PushFollow(FOLLOW_attributes_in_declaration668);
                    	attr = attributes();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	PushFollow(FOLLOW_type_ref_in_declaration672);
                    	type = type_ref();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:128:34: (e= fixed_array )?
                    	int alt4 = 2;
                    	int LA4_0 = input.LA(1);
                    	
                    	if ( (LA4_0 == LBRACKET) )
                    	{
                    	    alt4 = 1;
                    	}
                    	switch (alt4) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:128:35: e= fixed_array
                    	        {
                    	        	PushFollow(FOLLOW_fixed_array_in_declaration677);
                    	        	e = fixed_array();
                    	        	followingStackPointer_--;
                    	        	if (failed) return node;
                    	        	if ( backtracking == 0 ) 
                    	        	{
                    	        	  type.Elements = e;
                    	        	}
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	name = (IToken)input.LT(1);
                    	Match(input,ID,FOLLOW_ID_in_declaration684); if (failed) return node;
                    	PushFollow(FOLLOW_function_declaration_in_declaration688);
                    	f = function_declaration();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	   
                    	  	  	f.ReturnTypeInfo = type;
                    	  	  	f.Line = name.Line;
                    	  	        f.Column = name.CharPositionInLine;
                    	  	  	f.Name = name.Text;	
                    	  	  	f.Attributes = attr.ToArray();  
                    	  	  	node = f;
                    	  	  
                    	}
                    
                    }
                    break;
                case 3 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:137:4: type= type_ref (name= ID (v= variable_declaration[type] | f= function_declaration ) | e= fixed_array name= ID f= function_declaration )
                    {
                    	PushFollow(FOLLOW_type_ref_in_declaration701);
                    	type = type_ref();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:137:18: (name= ID (v= variable_declaration[type] | f= function_declaration ) | e= fixed_array name= ID f= function_declaration )
                    	int alt6 = 2;
                    	int LA6_0 = input.LA(1);
                    	
                    	if ( (LA6_0 == ID) )
                    	{
                    	    alt6 = 1;
                    	}
                    	else if ( (LA6_0 == LBRACKET) )
                    	{
                    	    alt6 = 2;
                    	}
                    	else 
                    	{
                    	    if ( backtracking > 0 ) {failed = true; return node;}
                    	    NoViableAltException nvae_d6s0 =
                    	        new NoViableAltException("137:18: (name= ID (v= variable_declaration[type] | f= function_declaration ) | e= fixed_array name= ID f= function_declaration )", 6, 0, input);
                    	
                    	    throw nvae_d6s0;
                    	}
                    	switch (alt6) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:137:19: name= ID (v= variable_declaration[type] | f= function_declaration )
                    	        {
                    	        	name = (IToken)input.LT(1);
                    	        	Match(input,ID,FOLLOW_ID_in_declaration706); if (failed) return node;
                    	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:137:27: (v= variable_declaration[type] | f= function_declaration )
                    	        	int alt5 = 2;
                    	        	int LA5_0 = input.LA(1);
                    	        	
                    	        	if ( ((LA5_0 >= SEMICOLON && LA5_0 <= LBRACKET) || LA5_0 == ASSIGN) )
                    	        	{
                    	        	    alt5 = 1;
                    	        	}
                    	        	else if ( (LA5_0 == LPAREN) )
                    	        	{
                    	        	    alt5 = 2;
                    	        	}
                    	        	else 
                    	        	{
                    	        	    if ( backtracking > 0 ) {failed = true; return node;}
                    	        	    NoViableAltException nvae_d5s0 =
                    	        	        new NoViableAltException("137:27: (v= variable_declaration[type] | f= function_declaration )", 5, 0, input);
                    	        	
                    	        	    throw nvae_d5s0;
                    	        	}
                    	        	switch (alt5) 
                    	        	{
                    	        	    case 1 :
                    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:137:28: v= variable_declaration[type]
                    	        	        {
                    	        	        	PushFollow(FOLLOW_variable_declaration_in_declaration711);
                    	        	        	v = variable_declaration(type);
                    	        	        	followingStackPointer_--;
                    	        	        	if (failed) return node;
                    	        	        	if ( backtracking == 0 ) 
                    	        	        	{
                    	        	        	   v.Name= name.Text;
                    	        	        	  							  v.Line = name.Line; 
                    	        	        	  							  v.Column = name.CharPositionInLine;
                    	        	        	  							  node = v;  
                    	        	        	}
                    	        	        
                    	        	        }
                    	        	        break;
                    	        	    case 2 :
                    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:141:10: f= function_declaration
                    	        	        {
                    	        	        	PushFollow(FOLLOW_function_declaration_in_declaration728);
                    	        	        	f = function_declaration();
                    	        	        	followingStackPointer_--;
                    	        	        	if (failed) return node;
                    	        	        	if ( backtracking == 0 ) 
                    	        	        	{
                    	        	        	   f.Name = name.Text;
                    	        	        	  			  			     f.ReturnTypeInfo = type;
                    	        	        	  			  			     f.Line = name.Line; 
                    	        	        	  						     f.Column = name.CharPositionInLine;
                    	        	        	  			  			     f.Line = name.Line; 
                    	        	        	  						     f.Column = name.CharPositionInLine;
                    	        	        	  						     node = f;
                    	        	        	}
                    	        	        
                    	        	        }
                    	        	        break;
                    	        	
                    	        	}

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:149:6: e= fixed_array name= ID f= function_declaration
                    	        {
                    	        	PushFollow(FOLLOW_fixed_array_in_declaration744);
                    	        	e = fixed_array();
                    	        	followingStackPointer_--;
                    	        	if (failed) return node;
                    	        	name = (IToken)input.LT(1);
                    	        	Match(input,ID,FOLLOW_ID_in_declaration748); if (failed) return node;
                    	        	PushFollow(FOLLOW_function_declaration_in_declaration753);
                    	        	f = function_declaration();
                    	        	followingStackPointer_--;
                    	        	if (failed) return node;
                    	        	if ( backtracking == 0 ) 
                    	        	{
                    	        	   f.Name = name.Text;
                    	        	  						     type.Elements = e;
                    	        	  			  			     f.ReturnTypeInfo = type;
                    	        	  			  			     f.Line = name.Line; 
                    	        	  						     f.Column = name.CharPositionInLine;
                    	        	  						     node = f;
                    	        	}
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    
                    }
                    break;
                case 4 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:156:4: s= struct_declaration
                    {
                    	PushFollow(FOLLOW_struct_declaration_in_declaration769);
                    	s = struct_declaration();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	  node = s;
                    	}
                    
                    }
                    break;
                case 5 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:157:4: cb= cbuffer_declaration
                    {
                    	PushFollow(FOLLOW_cbuffer_declaration_in_declaration782);
                    	cb = cbuffer_declaration();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	  node = cb;
                    	}
                    
                    }
                    break;
            
            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return node;
    }
    // $ANTLR end declaration

    
    // $ANTLR start attributes
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:160:1: attributes returns [List<AttributeDeclaration>attrs = new List<AttributeDeclaration>()] : (a= attribute )+ ;
    public List<AttributeDeclaration> attributes() // throws RecognitionException [1]
    {   

        List<AttributeDeclaration> attrs =  new List<AttributeDeclaration>();
    
        AttributeDeclaration a = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:161:2: ( (a= attribute )+ )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:161:4: (a= attribute )+
            {
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:161:4: (a= attribute )+
            	int cnt8 = 0;
            	do 
            	{
            	    int alt8 = 2;
            	    int LA8_0 = input.LA(1);
            	    
            	    if ( (LA8_0 == LBRACKET) )
            	    {
            	        alt8 = 1;
            	    }
            	    
            	
            	    switch (alt8) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:161:5: a= attribute
            			    {
            			    	PushFollow(FOLLOW_attribute_in_attributes802);
            			    	a = attribute();
            			    	followingStackPointer_--;
            			    	if (failed) return attrs;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	   attrs.Add(a); 
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt8 >= 1 ) goto loop8;
            			    if ( backtracking > 0 ) {failed = true; return attrs;}
            		            EarlyExitException eee =
            		                new EarlyExitException(8, input);
            		            throw eee;
            	    }
            	    cnt8++;
            	} while (true);
            	
            	loop8:
            		;	// Stops C# compiler whinging that label 'loop8' has no statements

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return attrs;
    }
    // $ANTLR end attributes

    
    // $ANTLR start variable_declaration
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:164:1: variable_declaration[TypeRef type] returns [GlobalVariableDeclaration node = new GlobalVariableDeclaration()] : (elements= fixed_array )? (sem= semantic )? ( ASSIGN v= expression )? (packoffset= packoffset_modifier )? (reg= register_modifier )? SEMICOLON ;
    public GlobalVariableDeclaration variable_declaration(TypeRef type) // throws RecognitionException [1]
    {   

        GlobalVariableDeclaration node =  new GlobalVariableDeclaration();
    
        int elements = 0;

        string sem = null;

        Expression v = null;

        PackOffsetDeclaration packoffset = null;

        RegisterDeclaration reg = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:166:2: ( (elements= fixed_array )? (sem= semantic )? ( ASSIGN v= expression )? (packoffset= packoffset_modifier )? (reg= register_modifier )? SEMICOLON )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:166:3: (elements= fixed_array )? (sem= semantic )? ( ASSIGN v= expression )? (packoffset= packoffset_modifier )? (reg= register_modifier )? SEMICOLON
            {
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:166:3: (elements= fixed_array )?
            	int alt9 = 2;
            	int LA9_0 = input.LA(1);
            	
            	if ( (LA9_0 == LBRACKET) )
            	{
            	    alt9 = 1;
            	}
            	switch (alt9) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:166:4: elements= fixed_array
            	        {
            	        	PushFollow(FOLLOW_fixed_array_in_variable_declaration826);
            	        	elements = fixed_array();
            	        	followingStackPointer_--;
            	        	if (failed) return node;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  type.Elements = elements;
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:167:4: (sem= semantic )?
            	int alt10 = 2;
            	int LA10_0 = input.LA(1);
            	
            	if ( (LA10_0 == COLON) )
            	{
            	    int LA10_1 = input.LA(2);
            	    
            	    if ( (LA10_1 == ID) )
            	    {
            	        alt10 = 1;
            	    }
            	}
            	switch (alt10) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:167:5: sem= semantic
            	        {
            	        	PushFollow(FOLLOW_semantic_in_variable_declaration838);
            	        	sem = semantic();
            	        	followingStackPointer_--;
            	        	if (failed) return node;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  node.Semantic = sem;
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:168:4: ( ASSIGN v= expression )?
            	int alt11 = 2;
            	int LA11_0 = input.LA(1);
            	
            	if ( (LA11_0 == ASSIGN) )
            	{
            	    alt11 = 1;
            	}
            	switch (alt11) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:168:5: ASSIGN v= expression
            	        {
            	        	Match(input,ASSIGN,FOLLOW_ASSIGN_in_variable_declaration848); if (failed) return node;
            	        	PushFollow(FOLLOW_expression_in_variable_declaration852);
            	        	v = expression();
            	        	followingStackPointer_--;
            	        	if (failed) return node;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  node.Initializer = v;
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:169:4: (packoffset= packoffset_modifier )?
            	int alt12 = 2;
            	int LA12_0 = input.LA(1);
            	
            	if ( (LA12_0 == COLON) )
            	{
            	    int LA12_1 = input.LA(2);
            	    
            	    if ( (LA12_1 == PACK) )
            	    {
            	        alt12 = 1;
            	    }
            	}
            	switch (alt12) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:169:5: packoffset= packoffset_modifier
            	        {
            	        	PushFollow(FOLLOW_packoffset_modifier_in_variable_declaration864);
            	        	packoffset = packoffset_modifier();
            	        	followingStackPointer_--;
            	        	if (failed) return node;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  node.PackOffset=packoffset;
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:170:4: (reg= register_modifier )?
            	int alt13 = 2;
            	int LA13_0 = input.LA(1);
            	
            	if ( (LA13_0 == COLON) )
            	{
            	    alt13 = 1;
            	}
            	switch (alt13) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:170:5: reg= register_modifier
            	        {
            	        	PushFollow(FOLLOW_register_modifier_in_variable_declaration875);
            	        	reg = register_modifier();
            	        	followingStackPointer_--;
            	        	if (failed) return node;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	   node.Register = reg; 
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_variable_declaration883); if (failed) return node;
            	if ( backtracking == 0 ) 
            	{
            	   node.TypeInfo=type; 
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return node;
    }
    // $ANTLR end variable_declaration

    
    // $ANTLR start fixed_array
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:173:1: fixed_array returns [int elements] : LBRACKET d= NUMBER RBRACKET ;
    public int fixed_array() // throws RecognitionException [1]
    {   

        int elements = 0;
    
        IToken d = null;
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:174:2: ( LBRACKET d= NUMBER RBRACKET )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:174:4: LBRACKET d= NUMBER RBRACKET
            {
            	Match(input,LBRACKET,FOLLOW_LBRACKET_in_fixed_array898); if (failed) return elements;
            	d = (IToken)input.LT(1);
            	Match(input,NUMBER,FOLLOW_NUMBER_in_fixed_array902); if (failed) return elements;
            	if ( backtracking == 0 ) 
            	{
            	   elements = int.Parse(d.Text);  
            	}
            	Match(input,RBRACKET,FOLLOW_RBRACKET_in_fixed_array906); if (failed) return elements;
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return elements;
    }
    // $ANTLR end fixed_array

    
    // $ANTLR start type_ref
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:176:1: type_ref returns [TypeRef type = new TypeRef()] : id= ID ( LESS p= type_ref ( ( COMMA ) p= type_ref )* GREATER )? ;
    public TypeRef type_ref() // throws RecognitionException [1]
    {   

        TypeRef type =  new TypeRef();
    
        IToken id = null;
        TypeRef p = null;
        
    
        	
        	List<TypeRef>g = new List<TypeRef>();
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:181:2: (id= ID ( LESS p= type_ref ( ( COMMA ) p= type_ref )* GREATER )? )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:181:4: id= ID ( LESS p= type_ref ( ( COMMA ) p= type_ref )* GREATER )?
            {
            	id = (IToken)input.LT(1);
            	Match(input,ID,FOLLOW_ID_in_type_ref926); if (failed) return type;
            	if ( backtracking == 0 ) 
            	{
            	  type.Name = id.Text;
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:181:32: ( LESS p= type_ref ( ( COMMA ) p= type_ref )* GREATER )?
            	int alt15 = 2;
            	int LA15_0 = input.LA(1);
            	
            	if ( (LA15_0 == LESS) )
            	{
            	    alt15 = 1;
            	}
            	switch (alt15) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:181:33: LESS p= type_ref ( ( COMMA ) p= type_ref )* GREATER
            	        {
            	        	Match(input,LESS,FOLLOW_LESS_in_type_ref930); if (failed) return type;
            	        	PushFollow(FOLLOW_type_ref_in_type_ref934);
            	        	p = type_ref();
            	        	followingStackPointer_--;
            	        	if (failed) return type;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	   g.Add(p); 
            	        	}
            	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:181:62: ( ( COMMA ) p= type_ref )*
            	        	do 
            	        	{
            	        	    int alt14 = 2;
            	        	    int LA14_0 = input.LA(1);
            	        	    
            	        	    if ( (LA14_0 == COMMA) )
            	        	    {
            	        	        alt14 = 1;
            	        	    }
            	        	    
            	        	
            	        	    switch (alt14) 
            	        		{
            	        			case 1 :
            	        			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:181:63: ( COMMA ) p= type_ref
            	        			    {
            	        			    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:181:63: ( COMMA )
            	        			    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:181:64: COMMA
            	        			    	{
            	        			    		Match(input,COMMA,FOLLOW_COMMA_in_type_ref939); if (failed) return type;
            	        			    	
            	        			    	}

            	        			    	PushFollow(FOLLOW_type_ref_in_type_ref944);
            	        			    	p = type_ref();
            	        			    	followingStackPointer_--;
            	        			    	if (failed) return type;
            	        			    	if ( backtracking == 0 ) 
            	        			    	{
            	        			    	   g.Add(p); 
            	        			    	}
            	        			    
            	        			    }
            	        			    break;
            	        	
            	        			default:
            	        			    goto loop14;
            	        	    }
            	        	} while (true);
            	        	
            	        	loop14:
            	        		;	// Stops C# compiler whinging that label 'loop14' has no statements

            	        	Match(input,GREATER,FOLLOW_GREATER_in_type_ref949); if (failed) return type;
            	        
            	        }
            	        break;
            	
            	}

            	if ( backtracking == 0 ) 
            	{
            	  		
            	  		type.GenericArgs = g.ToArray();
            	  		type.Line = id.Line;
            	  		type.Column = id.CharPositionInLine;
            	  	
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return type;
    }
    // $ANTLR end type_ref

    
    // $ANTLR start semantic
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:188:1: semantic returns [string s] : COLON ID ;
    public string semantic() // throws RecognitionException [1]
    {   

        string s = null;
    
        IToken ID1 = null;
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:189:2: ( COLON ID )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:189:4: COLON ID
            {
            	Match(input,COLON,FOLLOW_COLON_in_semantic967); if (failed) return s;
            	ID1 = (IToken)input.LT(1);
            	Match(input,ID,FOLLOW_ID_in_semantic969); if (failed) return s;
            	if ( backtracking == 0 ) 
            	{
            	   s=ID1.Text; 
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return s;
    }
    // $ANTLR end semantic

    
    // $ANTLR start constructor
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:191:1: constructor returns [VariableInitializer ini] : b= LBRACE e= expression ( COMMA e= expression )* RBRACE ;
    public VariableInitializer constructor() // throws RecognitionException [1]
    {   

        VariableInitializer ini = null;
    
        IToken b = null;
        Expression e = null;
        
    
        
        	List<Expression> list = new List<Expression>();
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:195:2: (b= LBRACE e= expression ( COMMA e= expression )* RBRACE )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:195:4: b= LBRACE e= expression ( COMMA e= expression )* RBRACE
            {
            	b = (IToken)input.LT(1);
            	Match(input,LBRACE,FOLLOW_LBRACE_in_constructor990); if (failed) return ini;
            	PushFollow(FOLLOW_expression_in_constructor994);
            	e = expression();
            	followingStackPointer_--;
            	if (failed) return ini;
            	if ( backtracking == 0 ) 
            	{
            	  list.Add(e);
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:195:40: ( COMMA e= expression )*
            	do 
            	{
            	    int alt16 = 2;
            	    int LA16_0 = input.LA(1);
            	    
            	    if ( (LA16_0 == COMMA) )
            	    {
            	        alt16 = 1;
            	    }
            	    
            	
            	    switch (alt16) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:195:41: COMMA e= expression
            			    {
            			    	Match(input,COMMA,FOLLOW_COMMA_in_constructor998); if (failed) return ini;
            			    	PushFollow(FOLLOW_expression_in_constructor1002);
            			    	e = expression();
            			    	followingStackPointer_--;
            			    	if (failed) return ini;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	  list.Add(e);
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop16;
            	    }
            	} while (true);
            	
            	loop16:
            		;	// Stops C# compiler whinging that label 'loop16' has no statements

            	Match(input,RBRACE,FOLLOW_RBRACE_in_constructor1007); if (failed) return ini;
            	if ( backtracking == 0 ) 
            	{
            	   
            	  		ini = new VariableInitializer
            	  		{
            	  			Expressions = list.ToArray(),
            	  			Line = b.Line,
            	  			Column = b.CharPositionInLine
            	  		}; 
            	  	
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ini;
    }
    // $ANTLR end constructor

    
    // $ANTLR start register_modifier
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:206:1: register_modifier returns [RegisterDeclaration r] : COLON REG LPAREN v= lvalue RPAREN ;
    public RegisterDeclaration register_modifier() // throws RecognitionException [1]
    {   

        RegisterDeclaration r = null;
    
        IToken COLON2 = null;
        Expression v = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:207:2: ( COLON REG LPAREN v= lvalue RPAREN )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:207:4: COLON REG LPAREN v= lvalue RPAREN
            {
            	COLON2 = (IToken)input.LT(1);
            	Match(input,COLON,FOLLOW_COLON_in_register_modifier1026); if (failed) return r;
            	Match(input,REG,FOLLOW_REG_in_register_modifier1029); if (failed) return r;
            	Match(input,LPAREN,FOLLOW_LPAREN_in_register_modifier1031); if (failed) return r;
            	PushFollow(FOLLOW_lvalue_in_register_modifier1035);
            	v = lvalue();
            	followingStackPointer_--;
            	if (failed) return r;
            	Match(input,RPAREN,FOLLOW_RPAREN_in_register_modifier1037); if (failed) return r;
            	if ( backtracking == 0 ) 
            	{
            	  r = new RegisterDeclaration(v, COLON2.Line, COLON2.CharPositionInLine); 
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return r;
    }
    // $ANTLR end register_modifier

    
    // $ANTLR start packoffset_modifier
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:209:1: packoffset_modifier returns [PackOffsetDeclaration p] : COLON PACK LPAREN v= lvalue RPAREN ;
    public PackOffsetDeclaration packoffset_modifier() // throws RecognitionException [1]
    {   

        PackOffsetDeclaration p = null;
    
        IToken COLON3 = null;
        Expression v = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:210:2: ( COLON PACK LPAREN v= lvalue RPAREN )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:210:4: COLON PACK LPAREN v= lvalue RPAREN
            {
            	COLON3 = (IToken)input.LT(1);
            	Match(input,COLON,FOLLOW_COLON_in_packoffset_modifier1052); if (failed) return p;
            	Match(input,PACK,FOLLOW_PACK_in_packoffset_modifier1054); if (failed) return p;
            	Match(input,LPAREN,FOLLOW_LPAREN_in_packoffset_modifier1056); if (failed) return p;
            	PushFollow(FOLLOW_lvalue_in_packoffset_modifier1060);
            	v = lvalue();
            	followingStackPointer_--;
            	if (failed) return p;
            	Match(input,RPAREN,FOLLOW_RPAREN_in_packoffset_modifier1062); if (failed) return p;
            	if ( backtracking == 0 ) 
            	{
            	  p = new PackOffsetDeclaration(v, COLON3.Line, COLON3.CharPositionInLine); 
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return p;
    }
    // $ANTLR end packoffset_modifier

    
    // $ANTLR start cbuffer_declaration
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:212:1: cbuffer_declaration returns [ConstantBufferDeclaration cb = new ConstantBufferDeclaration()] : (c= CBUFFER | c= TBUFFER ) name= ID LBRACE (type= type_ref name= ID d= variable_declaration[type] )+ RBRACE ( SEMICOLON )? ;
    public ConstantBufferDeclaration cbuffer_declaration() // throws RecognitionException [1]
    {   

        ConstantBufferDeclaration cb =  new ConstantBufferDeclaration();
    
        IToken c = null;
        IToken name = null;
        TypeRef type = null;

        GlobalVariableDeclaration d = null;
        
    
        
        	List<GlobalVariableDeclaration>decs = new List<GlobalVariableDeclaration>();
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:217:2: ( (c= CBUFFER | c= TBUFFER ) name= ID LBRACE (type= type_ref name= ID d= variable_declaration[type] )+ RBRACE ( SEMICOLON )? )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:217:5: (c= CBUFFER | c= TBUFFER ) name= ID LBRACE (type= type_ref name= ID d= variable_declaration[type] )+ RBRACE ( SEMICOLON )?
            {
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:217:5: (c= CBUFFER | c= TBUFFER )
            	int alt17 = 2;
            	int LA17_0 = input.LA(1);
            	
            	if ( (LA17_0 == CBUFFER) )
            	{
            	    alt17 = 1;
            	}
            	else if ( (LA17_0 == TBUFFER) )
            	{
            	    alt17 = 2;
            	}
            	else 
            	{
            	    if ( backtracking > 0 ) {failed = true; return cb;}
            	    NoViableAltException nvae_d17s0 =
            	        new NoViableAltException("217:5: (c= CBUFFER | c= TBUFFER )", 17, 0, input);
            	
            	    throw nvae_d17s0;
            	}
            	switch (alt17) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:217:6: c= CBUFFER
            	        {
            	        	c = (IToken)input.LT(1);
            	        	Match(input,CBUFFER,FOLLOW_CBUFFER_in_cbuffer_declaration1086); if (failed) return cb;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  cb.Type = BufferType.CBuffer;
            	        	}
            	        
            	        }
            	        break;
            	    case 2 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:217:47: c= TBUFFER
            	        {
            	        	c = (IToken)input.LT(1);
            	        	Match(input,TBUFFER,FOLLOW_TBUFFER_in_cbuffer_declaration1091); if (failed) return cb;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	   cb.Type = BufferType.TBuffer; 
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	name = (IToken)input.LT(1);
            	Match(input,ID,FOLLOW_ID_in_cbuffer_declaration1102); if (failed) return cb;
            	if ( backtracking == 0 ) 
            	{
            	  cb.Name= name.Text;
            	}
            	Match(input,LBRACE,FOLLOW_LBRACE_in_cbuffer_declaration1105); if (failed) return cb;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:218:42: (type= type_ref name= ID d= variable_declaration[type] )+
            	int cnt18 = 0;
            	do 
            	{
            	    int alt18 = 2;
            	    int LA18_0 = input.LA(1);
            	    
            	    if ( (LA18_0 == ID) )
            	    {
            	        alt18 = 1;
            	    }
            	    
            	
            	    switch (alt18) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:219:9: type= type_ref name= ID d= variable_declaration[type]
            			    {
            			    	PushFollow(FOLLOW_type_ref_in_cbuffer_declaration1120);
            			    	type = type_ref();
            			    	followingStackPointer_--;
            			    	if (failed) return cb;
            			    	name = (IToken)input.LT(1);
            			    	Match(input,ID,FOLLOW_ID_in_cbuffer_declaration1124); if (failed) return cb;
            			    	PushFollow(FOLLOW_variable_declaration_in_cbuffer_declaration1128);
            			    	d = variable_declaration(type);
            			    	followingStackPointer_--;
            			    	if (failed) return cb;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	  
            			    	  		    		  d.Name= name.Text;
            			    	  		    		  d.Line = name.Line; 
            			    	  				  d.Column = name.CharPositionInLine;
            			    	  	    		  	  decs.Add(d);
            			    	  	   		  
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt18 >= 1 ) goto loop18;
            			    if ( backtracking > 0 ) {failed = true; return cb;}
            		            EarlyExitException eee =
            		                new EarlyExitException(18, input);
            		            throw eee;
            	    }
            	    cnt18++;
            	} while (true);
            	
            	loop18:
            		;	// Stops C# compiler whinging that label 'loop18' has no statements

            	Match(input,RBRACE,FOLLOW_RBRACE_in_cbuffer_declaration1152); if (failed) return cb;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:226:15: ( SEMICOLON )?
            	int alt19 = 2;
            	int LA19_0 = input.LA(1);
            	
            	if ( (LA19_0 == SEMICOLON) )
            	{
            	    alt19 = 1;
            	}
            	switch (alt19) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:226:15: SEMICOLON
            	        {
            	        	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_cbuffer_declaration1154); if (failed) return cb;
            	        
            	        }
            	        break;
            	
            	}

            	if ( backtracking == 0 ) 
            	{
            	  
            	  	     	cb.Constants = decs.ToArray(); 
            	  	     	cb.Line = c.Line;
            	  	     	cb.Column = c.CharPositionInLine;
            	  	    
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return cb;
    }
    // $ANTLR end cbuffer_declaration

    
    // $ANTLR start struct_declaration
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:234:1: struct_declaration returns [StructDeclaration dec = new StructDeclaration()] : STRUCT ID LBRACE (m= member_declaration )+ RBRACE SEMICOLON ;
    public StructDeclaration struct_declaration() // throws RecognitionException [1]
    {   

        StructDeclaration dec =  new StructDeclaration();
    
        IToken ID4 = null;
        IToken STRUCT5 = null;
        StructMemberDeclaration m = null;
        
    
        
        	List<StructMemberDeclaration> members = new List<StructMemberDeclaration>();
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:238:2: ( STRUCT ID LBRACE (m= member_declaration )+ RBRACE SEMICOLON )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:238:4: STRUCT ID LBRACE (m= member_declaration )+ RBRACE SEMICOLON
            {
            	STRUCT5 = (IToken)input.LT(1);
            	Match(input,STRUCT,FOLLOW_STRUCT_in_struct_declaration1181); if (failed) return dec;
            	ID4 = (IToken)input.LT(1);
            	Match(input,ID,FOLLOW_ID_in_struct_declaration1183); if (failed) return dec;
            	if ( backtracking == 0 ) 
            	{
            	   dec.Name = ID4.Text; 
            	}
            	Match(input,LBRACE,FOLLOW_LBRACE_in_struct_declaration1186); if (failed) return dec;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:238:45: (m= member_declaration )+
            	int cnt20 = 0;
            	do 
            	{
            	    int alt20 = 2;
            	    int LA20_0 = input.LA(1);
            	    
            	    if ( ((LA20_0 >= CENTROID && LA20_0 <= NOINTERP) || LA20_0 == ID) )
            	    {
            	        alt20 = 1;
            	    }
            	    
            	
            	    switch (alt20) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:238:46: m= member_declaration
            			    {
            			    	PushFollow(FOLLOW_member_declaration_in_struct_declaration1191);
            			    	m = member_declaration();
            			    	followingStackPointer_--;
            			    	if (failed) return dec;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	  members.Add(m);
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt20 >= 1 ) goto loop20;
            			    if ( backtracking > 0 ) {failed = true; return dec;}
            		            EarlyExitException eee =
            		                new EarlyExitException(20, input);
            		            throw eee;
            	    }
            	    cnt20++;
            	} while (true);
            	
            	loop20:
            		;	// Stops C# compiler whinging that label 'loop20' has no statements

            	Match(input,RBRACE,FOLLOW_RBRACE_in_struct_declaration1196); if (failed) return dec;
            	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_struct_declaration1198); if (failed) return dec;
            	if ( backtracking == 0 ) 
            	{
            	  
            	  		dec.Members = members.ToArray();
            	  		dec.Line = STRUCT5.Line;
            	  	     	dec.Column = STRUCT5.CharPositionInLine;		
            	  	
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return dec;
    }
    // $ANTLR end struct_declaration

    
    // $ANTLR start member_declaration
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:246:1: member_declaration returns [StructMemberDeclaration m = new StructMemberDeclaration()] : (i= interpolation )? type= type_ref name= ID (e= fixed_array )? (s= semantic )? SEMICOLON ;
    public StructMemberDeclaration member_declaration() // throws RecognitionException [1]
    {   

        StructMemberDeclaration m =  new StructMemberDeclaration();
    
        IToken name = null;
        InterpolationMode? i = null;

        TypeRef type = null;

        int e = 0;

        string s = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:247:2: ( (i= interpolation )? type= type_ref name= ID (e= fixed_array )? (s= semantic )? SEMICOLON )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:247:5: (i= interpolation )? type= type_ref name= ID (e= fixed_array )? (s= semantic )? SEMICOLON
            {
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:247:5: (i= interpolation )?
            	int alt21 = 2;
            	int LA21_0 = input.LA(1);
            	
            	if ( ((LA21_0 >= CENTROID && LA21_0 <= NOINTERP)) )
            	{
            	    alt21 = 1;
            	}
            	switch (alt21) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:247:6: i= interpolation
            	        {
            	        	PushFollow(FOLLOW_interpolation_in_member_declaration1219);
            	        	i = interpolation();
            	        	followingStackPointer_--;
            	        	if (failed) return m;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  m.Interpolation=(InterpolationMode)i;
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	PushFollow(FOLLOW_type_ref_in_member_declaration1229);
            	type = type_ref();
            	followingStackPointer_--;
            	if (failed) return m;
            	if ( backtracking == 0 ) 
            	{
            	  m.TypeInfo=type;
            	}
            	name = (IToken)input.LT(1);
            	Match(input,ID,FOLLOW_ID_in_member_declaration1234); if (failed) return m;
            	if ( backtracking == 0 ) 
            	{
            	  m.Name=name.Text; m.Line=name.Line; m.Column = name.CharPositionInLine; 
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:249:5: (e= fixed_array )?
            	int alt22 = 2;
            	int LA22_0 = input.LA(1);
            	
            	if ( (LA22_0 == LBRACKET) )
            	{
            	    alt22 = 1;
            	}
            	switch (alt22) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:249:6: e= fixed_array
            	        {
            	        	PushFollow(FOLLOW_fixed_array_in_member_declaration1244);
            	        	e = fixed_array();
            	        	followingStackPointer_--;
            	        	if (failed) return m;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  type.Elements = e;
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:249:42: (s= semantic )?
            	int alt23 = 2;
            	int LA23_0 = input.LA(1);
            	
            	if ( (LA23_0 == COLON) )
            	{
            	    alt23 = 1;
            	}
            	switch (alt23) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:249:43: s= semantic
            	        {
            	        	PushFollow(FOLLOW_semantic_in_member_declaration1252);
            	        	s = semantic();
            	        	followingStackPointer_--;
            	        	if (failed) return m;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  m.Semantic=s;
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_member_declaration1257); if (failed) return m;
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return m;
    }
    // $ANTLR end member_declaration

    
    // $ANTLR start interpolation
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:252:1: interpolation returns [InterpolationMode? i] : ( CENTROID | LINEAR | NOINTERP );
    public InterpolationMode? interpolation() // throws RecognitionException [1]
    {   

        InterpolationMode? i = null;
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:253:2: ( CENTROID | LINEAR | NOINTERP )
            int alt24 = 3;
            switch ( input.LA(1) ) 
            {
            case CENTROID:
            	{
                alt24 = 1;
                }
                break;
            case LINEAR:
            	{
                alt24 = 2;
                }
                break;
            case NOINTERP:
            	{
                alt24 = 3;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return i;}
            	    NoViableAltException nvae_d24s0 =
            	        new NoViableAltException("252:1: interpolation returns [InterpolationMode? i] : ( CENTROID | LINEAR | NOINTERP );", 24, 0, input);
            
            	    throw nvae_d24s0;
            }
            
            switch (alt24) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:253:3: CENTROID
                    {
                    	Match(input,CENTROID,FOLLOW_CENTROID_in_interpolation1270); if (failed) return i;
                    	if ( backtracking == 0 ) 
                    	{
                    	  i = InterpolationMode.Centroid;
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:254:4: LINEAR
                    {
                    	Match(input,LINEAR,FOLLOW_LINEAR_in_interpolation1277); if (failed) return i;
                    	if ( backtracking == 0 ) 
                    	{
                    	  i = InterpolationMode.Linear;
                    	}
                    
                    }
                    break;
                case 3 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:255:4: NOINTERP
                    {
                    	Match(input,NOINTERP,FOLLOW_NOINTERP_in_interpolation1285); if (failed) return i;
                    	if ( backtracking == 0 ) 
                    	{
                    	  i = InterpolationMode.NoInterpolation;
                    	}
                    
                    }
                    break;
            
            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return i;
    }
    // $ANTLR end interpolation

    
    // $ANTLR start function_declaration
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:258:1: function_declaration returns [UserFunctionDeclaration dec = new UserFunctionDeclaration()] : LPAREN (p= parameters )? RPAREN (s= semantic )? body= block_statement ;
    public UserFunctionDeclaration function_declaration() // throws RecognitionException [1]
    {   

        UserFunctionDeclaration dec =  new UserFunctionDeclaration();
    
        List<ParameterDeclaration> p = null;

        string s = null;

        BlockStatement body = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:259:2: ( LPAREN (p= parameters )? RPAREN (s= semantic )? body= block_statement )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:259:3: LPAREN (p= parameters )? RPAREN (s= semantic )? body= block_statement
            {
            	Match(input,LPAREN,FOLLOW_LPAREN_in_function_declaration1300); if (failed) return dec;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:259:10: (p= parameters )?
            	int alt25 = 2;
            	int LA25_0 = input.LA(1);
            	
            	if ( (LA25_0 == UNIFORM || (LA25_0 >= IN && LA25_0 <= INOUT) || LA25_0 == ID) )
            	{
            	    alt25 = 1;
            	}
            	switch (alt25) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:259:11: p= parameters
            	        {
            	        	PushFollow(FOLLOW_parameters_in_function_declaration1305);
            	        	p = parameters();
            	        	followingStackPointer_--;
            	        	if (failed) return dec;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  dec.Parameters=p.ToArray();
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	Match(input,RPAREN,FOLLOW_RPAREN_in_function_declaration1310); if (failed) return dec;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:260:2: (s= semantic )?
            	int alt26 = 2;
            	int LA26_0 = input.LA(1);
            	
            	if ( (LA26_0 == COLON) )
            	{
            	    alt26 = 1;
            	}
            	switch (alt26) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:260:3: s= semantic
            	        {
            	        	PushFollow(FOLLOW_semantic_in_function_declaration1317);
            	        	s = semantic();
            	        	followingStackPointer_--;
            	        	if (failed) return dec;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  dec.ReturnSemantic = s;
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	PushFollow(FOLLOW_block_statement_in_function_declaration1325);
            	body = block_statement();
            	followingStackPointer_--;
            	if (failed) return dec;
            	if ( backtracking == 0 ) 
            	{
            	  dec.Body=body;
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return dec;
    }
    // $ANTLR end function_declaration

    
    // $ANTLR start attribute
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:264:1: attribute returns [AttributeDeclaration attr = new AttributeDeclaration()] : LBRACKET ID (args= attribute_parameters )? RBRACKET ;
    public AttributeDeclaration attribute() // throws RecognitionException [1]
    {   

        AttributeDeclaration attr =  new AttributeDeclaration();
    
        IToken ID6 = null;
        List<Expression> args = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:265:2: ( LBRACKET ID (args= attribute_parameters )? RBRACKET )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:265:6: LBRACKET ID (args= attribute_parameters )? RBRACKET
            {
            	Match(input,LBRACKET,FOLLOW_LBRACKET_in_attribute1343); if (failed) return attr;
            	ID6 = (IToken)input.LT(1);
            	Match(input,ID,FOLLOW_ID_in_attribute1345); if (failed) return attr;
            	if ( backtracking == 0 ) 
            	{
            	  attr.Name = ID6.Text;
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:265:41: (args= attribute_parameters )?
            	int alt27 = 2;
            	int LA27_0 = input.LA(1);
            	
            	if ( (LA27_0 == RPAREN) )
            	{
            	    alt27 = 1;
            	}
            	switch (alt27) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:265:42: args= attribute_parameters
            	        {
            	        	PushFollow(FOLLOW_attribute_parameters_in_attribute1351);
            	        	args = attribute_parameters();
            	        	followingStackPointer_--;
            	        	if (failed) return attr;
            	        
            	        }
            	        break;
            	
            	}

            	Match(input,RBRACKET,FOLLOW_RBRACKET_in_attribute1355); if (failed) return attr;
            	if ( backtracking == 0 ) 
            	{
            	  
            	  		attr.Arguments = args!=null?args.ToArray():null;
            	  		attr.Line = ID6.Line;
            	  		attr.Column = ID6.CharPositionInLine;
            	  	
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return attr;
    }
    // $ANTLR end attribute

    
    // $ANTLR start attribute_parameters
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:272:1: attribute_parameters returns [List<Expression>exp = new List<Expression>()] : ( RPAREN (e= attribute_expresion ) ( COMMA e= attribute_expresion )* LPAREN ) ;
    public List<Expression> attribute_parameters() // throws RecognitionException [1]
    {   

        List<Expression> exp =  new List<Expression>();
    
        LiteralExpression e = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:273:2: ( ( RPAREN (e= attribute_expresion ) ( COMMA e= attribute_expresion )* LPAREN ) )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:273:4: ( RPAREN (e= attribute_expresion ) ( COMMA e= attribute_expresion )* LPAREN )
            {
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:273:4: ( RPAREN (e= attribute_expresion ) ( COMMA e= attribute_expresion )* LPAREN )
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:273:5: RPAREN (e= attribute_expresion ) ( COMMA e= attribute_expresion )* LPAREN
            	{
            		Match(input,RPAREN,FOLLOW_RPAREN_in_attribute_parameters1372); if (failed) return exp;
            		// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:273:12: (e= attribute_expresion )
            		// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:273:13: e= attribute_expresion
            		{
            			PushFollow(FOLLOW_attribute_expresion_in_attribute_parameters1377);
            			e = attribute_expresion();
            			followingStackPointer_--;
            			if (failed) return exp;
            			if ( backtracking == 0 ) 
            			{
            			  exp.Add(e);
            			}
            		
            		}

            		// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:273:49: ( COMMA e= attribute_expresion )*
            		do 
            		{
            		    int alt28 = 2;
            		    int LA28_0 = input.LA(1);
            		    
            		    if ( (LA28_0 == COMMA) )
            		    {
            		        alt28 = 1;
            		    }
            		    
            		
            		    switch (alt28) 
            			{
            				case 1 :
            				    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:273:50: COMMA e= attribute_expresion
            				    {
            				    	Match(input,COMMA,FOLLOW_COMMA_in_attribute_parameters1382); if (failed) return exp;
            				    	PushFollow(FOLLOW_attribute_expresion_in_attribute_parameters1386);
            				    	e = attribute_expresion();
            				    	followingStackPointer_--;
            				    	if (failed) return exp;
            				    	if ( backtracking == 0 ) 
            				    	{
            				    	  exp.Add(e);
            				    	}
            				    
            				    }
            				    break;
            		
            				default:
            				    goto loop28;
            		    }
            		} while (true);
            		
            		loop28:
            			;	// Stops C# compiler whinging that label 'loop28' has no statements

            		Match(input,LPAREN,FOLLOW_LPAREN_in_attribute_parameters1392); if (failed) return exp;
            	
            	}

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return exp;
    }
    // $ANTLR end attribute_parameters

    
    // $ANTLR start attribute_expresion
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:275:1: attribute_expresion returns [LiteralExpression lit] : (e= constant_exp | t= ID | t= STRING_LITERAL );
    public LiteralExpression attribute_expresion() // throws RecognitionException [1]
    {   

        LiteralExpression lit = null;
    
        IToken t = null;
        Expression e = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:276:2: (e= constant_exp | t= ID | t= STRING_LITERAL )
            int alt29 = 3;
            switch ( input.LA(1) ) 
            {
            case NUMBER:
            case BOOL_CONSTANT:
            	{
                alt29 = 1;
                }
                break;
            case ID:
            	{
                alt29 = 2;
                }
                break;
            case STRING_LITERAL:
            	{
                alt29 = 3;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return lit;}
            	    NoViableAltException nvae_d29s0 =
            	        new NoViableAltException("275:1: attribute_expresion returns [LiteralExpression lit] : (e= constant_exp | t= ID | t= STRING_LITERAL );", 29, 0, input);
            
            	    throw nvae_d29s0;
            }
            
            switch (alt29) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:276:5: e= constant_exp
                    {
                    	PushFollow(FOLLOW_constant_exp_in_attribute_expresion1409);
                    	e = constant_exp();
                    	followingStackPointer_--;
                    	if (failed) return lit;
                    	if ( backtracking == 0 ) 
                    	{
                    	   lit = (LiteralExpression)e;
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:277:4: t= ID
                    {
                    	t = (IToken)input.LT(1);
                    	Match(input,ID,FOLLOW_ID_in_attribute_expresion1418); if (failed) return lit;
                    	if ( backtracking == 0 ) 
                    	{
                    	  lit = new SimbolExpression(t.Text){ Line = t.Line, Column = t.CharPositionInLine };
                    	}
                    
                    }
                    break;
                case 3 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:278:4: t= STRING_LITERAL
                    {
                    	t = (IToken)input.LT(1);
                    	Match(input,STRING_LITERAL,FOLLOW_STRING_LITERAL_in_attribute_expresion1427); if (failed) return lit;
                    	if ( backtracking == 0 ) 
                    	{
                    	  lit = new LiteralExpression<string>(t.Text){ Line = t.Line, Column = t.CharPositionInLine };
                    	}
                    
                    }
                    break;
            
            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return lit;
    }
    // $ANTLR end attribute_expresion

    
    // $ANTLR start parameters
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:280:1: parameters returns [List<ParameterDeclaration> l =new List<ParameterDeclaration>()] : p= parameter_declaration ( COMMA p= parameter_declaration )* ;
    public List<ParameterDeclaration> parameters() // throws RecognitionException [1]
    {   

        List<ParameterDeclaration> l = new List<ParameterDeclaration>();
    
        ParameterDeclaration p = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:281:2: (p= parameter_declaration ( COMMA p= parameter_declaration )* )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:281:4: p= parameter_declaration ( COMMA p= parameter_declaration )*
            {
            	PushFollow(FOLLOW_parameter_declaration_in_parameters1444);
            	p = parameter_declaration();
            	followingStackPointer_--;
            	if (failed) return l;
            	if ( backtracking == 0 ) 
            	{
            	  l.Add(p);
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:281:39: ( COMMA p= parameter_declaration )*
            	do 
            	{
            	    int alt30 = 2;
            	    int LA30_0 = input.LA(1);
            	    
            	    if ( (LA30_0 == COMMA) )
            	    {
            	        alt30 = 1;
            	    }
            	    
            	
            	    switch (alt30) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:281:40: COMMA p= parameter_declaration
            			    {
            			    	Match(input,COMMA,FOLLOW_COMMA_in_parameters1448); if (failed) return l;
            			    	PushFollow(FOLLOW_parameter_declaration_in_parameters1452);
            			    	p = parameter_declaration();
            			    	followingStackPointer_--;
            			    	if (failed) return l;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	  l.Add(p);
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop30;
            	    }
            	} while (true);
            	
            	loop30:
            		;	// Stops C# compiler whinging that label 'loop30' has no statements

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return l;
    }
    // $ANTLR end parameters

    
    // $ANTLR start parameter_declaration
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:284:1: parameter_declaration returns [ParameterDeclaration p = new ParameterDeclaration()] : (q= parameter_qualifier )? type= type_ref name= ID (e= fixed_array )? (s= semantic )? ( ASSIGN v= expression )? ;
    public ParameterDeclaration parameter_declaration() // throws RecognitionException [1]
    {   

        ParameterDeclaration p =  new ParameterDeclaration();
    
        IToken name = null;
        ParamQualifier? q = null;

        TypeRef type = null;

        int e = 0;

        string s = null;

        Expression v = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:285:2: ( (q= parameter_qualifier )? type= type_ref name= ID (e= fixed_array )? (s= semantic )? ( ASSIGN v= expression )? )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:285:3: (q= parameter_qualifier )? type= type_ref name= ID (e= fixed_array )? (s= semantic )? ( ASSIGN v= expression )?
            {
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:285:3: (q= parameter_qualifier )?
            	int alt31 = 2;
            	int LA31_0 = input.LA(1);
            	
            	if ( (LA31_0 == UNIFORM || (LA31_0 >= IN && LA31_0 <= INOUT)) )
            	{
            	    alt31 = 1;
            	}
            	switch (alt31) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:285:4: q= parameter_qualifier
            	        {
            	        	PushFollow(FOLLOW_parameter_qualifier_in_parameter_declaration1473);
            	        	q = parameter_qualifier();
            	        	followingStackPointer_--;
            	        	if (failed) return p;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	   p.Qualifier =(ParamQualifier)q; 
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	PushFollow(FOLLOW_type_ref_in_parameter_declaration1484);
            	type = type_ref();
            	followingStackPointer_--;
            	if (failed) return p;
            	if ( backtracking == 0 ) 
            	{
            	  p.TypeInfo = type;
            	}
            	name = (IToken)input.LT(1);
            	Match(input,ID,FOLLOW_ID_in_parameter_declaration1492); if (failed) return p;
            	if ( backtracking == 0 ) 
            	{
            	    p.Name = name.Text; 
            	  		   p.Line=name.Line; 
            	  		   p.Column=name.CharPositionInLine; 
            	  		
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:291:2: (e= fixed_array )?
            	int alt32 = 2;
            	int LA32_0 = input.LA(1);
            	
            	if ( (LA32_0 == LBRACKET) )
            	{
            	    alt32 = 1;
            	}
            	switch (alt32) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:291:3: e= fixed_array
            	        {
            	        	PushFollow(FOLLOW_fixed_array_in_parameter_declaration1502);
            	        	e = fixed_array();
            	        	followingStackPointer_--;
            	        	if (failed) return p;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  p.TypeInfo.Elements = e;
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:292:2: (s= semantic )?
            	int alt33 = 2;
            	int LA33_0 = input.LA(1);
            	
            	if ( (LA33_0 == COLON) )
            	{
            	    alt33 = 1;
            	}
            	switch (alt33) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:292:3: s= semantic
            	        {
            	        	PushFollow(FOLLOW_semantic_in_parameter_declaration1512);
            	        	s = semantic();
            	        	followingStackPointer_--;
            	        	if (failed) return p;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  p.Semantic = s;
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:292:33: ( ASSIGN v= expression )?
            	int alt34 = 2;
            	int LA34_0 = input.LA(1);
            	
            	if ( (LA34_0 == ASSIGN) )
            	{
            	    alt34 = 1;
            	}
            	switch (alt34) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:292:34: ASSIGN v= expression
            	        {
            	        	Match(input,ASSIGN,FOLLOW_ASSIGN_in_parameter_declaration1518); if (failed) return p;
            	        	PushFollow(FOLLOW_expression_in_parameter_declaration1522);
            	        	v = expression();
            	        	followingStackPointer_--;
            	        	if (failed) return p;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  p.Initializer = v;
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return p;
    }
    // $ANTLR end parameter_declaration

    
    // $ANTLR start parameter_qualifier
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:295:1: parameter_qualifier returns [ParamQualifier? q] : ( IN | OUT | INOUT | UNIFORM );
    public ParamQualifier? parameter_qualifier() // throws RecognitionException [1]
    {   

        ParamQualifier? q = null;
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:296:2: ( IN | OUT | INOUT | UNIFORM )
            int alt35 = 4;
            switch ( input.LA(1) ) 
            {
            case IN:
            	{
                alt35 = 1;
                }
                break;
            case OUT:
            	{
                alt35 = 2;
                }
                break;
            case INOUT:
            	{
                alt35 = 3;
                }
                break;
            case UNIFORM:
            	{
                alt35 = 4;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return q;}
            	    NoViableAltException nvae_d35s0 =
            	        new NoViableAltException("295:1: parameter_qualifier returns [ParamQualifier? q] : ( IN | OUT | INOUT | UNIFORM );", 35, 0, input);
            
            	    throw nvae_d35s0;
            }
            
            switch (alt35) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:296:4: IN
                    {
                    	Match(input,IN,FOLLOW_IN_in_parameter_qualifier1539); if (failed) return q;
                    	if ( backtracking == 0 ) 
                    	{
                    	   q = ParamQualifier.In; 
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:297:4: OUT
                    {
                    	Match(input,OUT,FOLLOW_OUT_in_parameter_qualifier1546); if (failed) return q;
                    	if ( backtracking == 0 ) 
                    	{
                    	   q = ParamQualifier.Out; 
                    	}
                    
                    }
                    break;
                case 3 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:298:4: INOUT
                    {
                    	Match(input,INOUT,FOLLOW_INOUT_in_parameter_qualifier1553); if (failed) return q;
                    	if ( backtracking == 0 ) 
                    	{
                    	   q = ParamQualifier.InOut; 
                    	}
                    
                    }
                    break;
                case 4 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:299:4: UNIFORM
                    {
                    	Match(input,UNIFORM,FOLLOW_UNIFORM_in_parameter_qualifier1560); if (failed) return q;
                    	if ( backtracking == 0 ) 
                    	{
                    	   q = ParamQualifier.Uniform; 
                    	}
                    
                    }
                    break;
            
            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return q;
    }
    // $ANTLR end parameter_qualifier

    
    // $ANTLR start block_statement
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:301:1: block_statement returns [BlockStatement b = new BlockStatement()] : LBRACE (s= statement )* RBRACE ;
    public BlockStatement block_statement() // throws RecognitionException [1]
    {   

        BlockStatement b =  new BlockStatement();
    
        IToken LBRACE7 = null;
        Statement s = null;
        
    
         var l = new List<Statement>(); 
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:303:2: ( LBRACE (s= statement )* RBRACE )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:303:4: LBRACE (s= statement )* RBRACE
            {
            	LBRACE7 = (IToken)input.LT(1);
            	Match(input,LBRACE,FOLLOW_LBRACE_in_block_statement1579); if (failed) return b;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:303:11: (s= statement )*
            	do 
            	{
            	    int alt36 = 2;
            	    int LA36_0 = input.LA(1);
            	    
            	    if ( (LA36_0 == INCREMENT || LA36_0 == DECREMENT || LA36_0 == LBRACKET || LA36_0 == LBRACE || LA36_0 == IF || (LA36_0 >= WHILE && LA36_0 <= CONST) || LA36_0 == ID) )
            	    {
            	        alt36 = 1;
            	    }
            	    
            	
            	    switch (alt36) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:303:12: s= statement
            			    {
            			    	PushFollow(FOLLOW_statement_in_block_statement1584);
            			    	s = statement();
            			    	followingStackPointer_--;
            			    	if (failed) return b;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	  l.Add(s);
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop36;
            	    }
            	} while (true);
            	
            	loop36:
            		;	// Stops C# compiler whinging that label 'loop36' has no statements

            	Match(input,RBRACE,FOLLOW_RBRACE_in_block_statement1589); if (failed) return b;
            	if ( backtracking == 0 ) 
            	{
            	   
            	  	 	b.Statements = l.ToArray();
            	  	 	b.Line = LBRACE7.Line;
            	  	 	b.Column = LBRACE7.CharPositionInLine;
            	  	 
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return b;
    }
    // $ANTLR end block_statement

    
    // $ANTLR start expression
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:312:1: expression returns [Expression n] : e= or_expr ( QUESTION v= expression COLON f= expression )? ;
    public Expression expression() // throws RecognitionException [1]
    {   

        Expression n = null;
    
        IToken QUESTION8 = null;
        Expression e = null;

        Expression v = null;

        Expression f = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:313:2: (e= or_expr ( QUESTION v= expression COLON f= expression )? )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:313:4: e= or_expr ( QUESTION v= expression COLON f= expression )?
            {
            	PushFollow(FOLLOW_or_expr_in_expression1610);
            	e = or_expr();
            	followingStackPointer_--;
            	if (failed) return n;
            	if ( backtracking == 0 ) 
            	{
            	  n=e;
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:313:19: ( QUESTION v= expression COLON f= expression )?
            	int alt37 = 2;
            	int LA37_0 = input.LA(1);
            	
            	if ( (LA37_0 == QUESTION) )
            	{
            	    alt37 = 1;
            	}
            	switch (alt37) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:313:20: QUESTION v= expression COLON f= expression
            	        {
            	        	QUESTION8 = (IToken)input.LT(1);
            	        	Match(input,QUESTION,FOLLOW_QUESTION_in_expression1613); if (failed) return n;
            	        	PushFollow(FOLLOW_expression_in_expression1617);
            	        	v = expression();
            	        	followingStackPointer_--;
            	        	if (failed) return n;
            	        	Match(input,COLON,FOLLOW_COLON_in_expression1619); if (failed) return n;
            	        	PushFollow(FOLLOW_expression_in_expression1623);
            	        	f = expression();
            	        	followingStackPointer_--;
            	        	if (failed) return n;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	   
            	        	  			n = new TernaryExpression(e,v,f)
            	        	  			{
            	        	  			  Line = QUESTION8.Line,
            	        	  			  Column = QUESTION8.CharPositionInLine
            	        	  			};
            	        	  		
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return n;
    }
    // $ANTLR end expression

    
    // $ANTLR start or_expr
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:323:1: or_expr returns [Expression n] : e= xor_expr (o= OR r= xor_expr )* ;
    public Expression or_expr() // throws RecognitionException [1]
    {   

        Expression n = null;
    
        IToken o = null;
        Expression e = null;

        Expression r = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:324:2: (e= xor_expr (o= OR r= xor_expr )* )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:324:4: e= xor_expr (o= OR r= xor_expr )*
            {
            	PushFollow(FOLLOW_xor_expr_in_or_expr1646);
            	e = xor_expr();
            	followingStackPointer_--;
            	if (failed) return n;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:324:15: (o= OR r= xor_expr )*
            	do 
            	{
            	    int alt38 = 2;
            	    int LA38_0 = input.LA(1);
            	    
            	    if ( (LA38_0 == OR) )
            	    {
            	        alt38 = 1;
            	    }
            	    
            	
            	    switch (alt38) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:324:16: o= OR r= xor_expr
            			    {
            			    	o = (IToken)input.LT(1);
            			    	Match(input,OR,FOLLOW_OR_in_or_expr1651); if (failed) return n;
            			    	PushFollow(FOLLOW_xor_expr_in_or_expr1655);
            			    	r = xor_expr();
            			    	followingStackPointer_--;
            			    	if (failed) return n;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	   
            			    	  	  	e = new BinaryLogical { Left = e, Right = r, Operator = BinaryOperator.Or ,
            			    	  	  		Line = o.Line,
            			    	  		        Column = o.CharPositionInLine
            			    	  	  	}; 
            			    	  	  
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop38;
            	    }
            	} while (true);
            	
            	loop38:
            		;	// Stops C# compiler whinging that label 'loop38' has no statements

            	if ( backtracking == 0 ) 
            	{
            	  n=e;
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return n;
    }
    // $ANTLR end or_expr

    
    // $ANTLR start xor_expr
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:333:1: xor_expr returns [Expression n] : e= and_expr (x= XOR r= and_expr )* ;
    public Expression xor_expr() // throws RecognitionException [1]
    {   

        Expression n = null;
    
        IToken x = null;
        Expression e = null;

        Expression r = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:334:2: (e= and_expr (x= XOR r= and_expr )* )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:334:4: e= and_expr (x= XOR r= and_expr )*
            {
            	PushFollow(FOLLOW_and_expr_in_xor_expr1680);
            	e = and_expr();
            	followingStackPointer_--;
            	if (failed) return n;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:334:15: (x= XOR r= and_expr )*
            	do 
            	{
            	    int alt39 = 2;
            	    int LA39_0 = input.LA(1);
            	    
            	    if ( (LA39_0 == XOR) )
            	    {
            	        alt39 = 1;
            	    }
            	    
            	
            	    switch (alt39) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:334:16: x= XOR r= and_expr
            			    {
            			    	x = (IToken)input.LT(1);
            			    	Match(input,XOR,FOLLOW_XOR_in_xor_expr1685); if (failed) return n;
            			    	PushFollow(FOLLOW_and_expr_in_xor_expr1690);
            			    	r = and_expr();
            			    	followingStackPointer_--;
            			    	if (failed) return n;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	  
            			    	  		e = new BinaryLogical 
            			    	  		{ Left = e, 
            			    	  		  Right = r, Operator = BinaryOperator.Xor ,
            			    	  		  Line = x.Line,
            			    	  		  Column = x.CharPositionInLine
            			    	  		};
            			    	  	
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop39;
            	    }
            	} while (true);
            	
            	loop39:
            		;	// Stops C# compiler whinging that label 'loop39' has no statements

            	if ( backtracking == 0 ) 
            	{
            	  n=e;
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return n;
    }
    // $ANTLR end xor_expr

    
    // $ANTLR start and_expr
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:345:1: and_expr returns [Expression n] : e= rel_exp (a= AND r= rel_exp )* ;
    public Expression and_expr() // throws RecognitionException [1]
    {   

        Expression n = null;
    
        IToken a = null;
        Expression e = null;

        Expression r = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:346:2: (e= rel_exp (a= AND r= rel_exp )* )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:346:3: e= rel_exp (a= AND r= rel_exp )*
            {
            	PushFollow(FOLLOW_rel_exp_in_and_expr1712);
            	e = rel_exp();
            	followingStackPointer_--;
            	if (failed) return n;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:346:13: (a= AND r= rel_exp )*
            	do 
            	{
            	    int alt40 = 2;
            	    int LA40_0 = input.LA(1);
            	    
            	    if ( (LA40_0 == AND) )
            	    {
            	        alt40 = 1;
            	    }
            	    
            	
            	    switch (alt40) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:346:14: a= AND r= rel_exp
            			    {
            			    	a = (IToken)input.LT(1);
            			    	Match(input,AND,FOLLOW_AND_in_and_expr1717); if (failed) return n;
            			    	PushFollow(FOLLOW_rel_exp_in_and_expr1721);
            			    	r = rel_exp();
            			    	followingStackPointer_--;
            			    	if (failed) return n;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	  
            			    	  		e = new BinaryLogical { Left = e, Right = r, Operator = BinaryOperator.And ,
            			    	  			Line = a.Line,
            			    	  		        Column = a.CharPositionInLine
            			    	  		};
            			    	  	
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop40;
            	    }
            	} while (true);
            	
            	loop40:
            		;	// Stops C# compiler whinging that label 'loop40' has no statements

            	if ( backtracking == 0 ) 
            	{
            	  n=e;
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return n;
    }
    // $ANTLR end and_expr

    
    // $ANTLR start rel_exp
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:355:1: rel_exp returns [Expression n] : e= add_expr (t= EQUAL r= add_expr | t= NEQUAL r= add_expr | t= LESS r= add_expr | t= LEQUAL r= add_expr | t= GREATER r= add_expr | t= GEQUAL r= add_expr )? ;
    public Expression rel_exp() // throws RecognitionException [1]
    {   

        Expression n = null;
    
        IToken t = null;
        Expression e = null;

        Expression r = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:356:2: (e= add_expr (t= EQUAL r= add_expr | t= NEQUAL r= add_expr | t= LESS r= add_expr | t= LEQUAL r= add_expr | t= GREATER r= add_expr | t= GEQUAL r= add_expr )? )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:356:4: e= add_expr (t= EQUAL r= add_expr | t= NEQUAL r= add_expr | t= LESS r= add_expr | t= LEQUAL r= add_expr | t= GREATER r= add_expr | t= GEQUAL r= add_expr )?
            {
            	PushFollow(FOLLOW_add_expr_in_rel_exp1744);
            	e = add_expr();
            	followingStackPointer_--;
            	if (failed) return n;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:356:15: (t= EQUAL r= add_expr | t= NEQUAL r= add_expr | t= LESS r= add_expr | t= LEQUAL r= add_expr | t= GREATER r= add_expr | t= GEQUAL r= add_expr )?
            	int alt41 = 7;
            	switch ( input.LA(1) ) 
            	{
            	    case EQUAL:
            	    	{
            	        alt41 = 1;
            	        }
            	        break;
            	    case NEQUAL:
            	    	{
            	        alt41 = 2;
            	        }
            	        break;
            	    case LESS:
            	    	{
            	        alt41 = 3;
            	        }
            	        break;
            	    case LEQUAL:
            	    	{
            	        alt41 = 4;
            	        }
            	        break;
            	    case GREATER:
            	    	{
            	        alt41 = 5;
            	        }
            	        break;
            	    case GEQUAL:
            	    	{
            	        alt41 = 6;
            	        }
            	        break;
            	}
            	
            	switch (alt41) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:357:10: t= EQUAL r= add_expr
            	        {
            	        	t = (IToken)input.LT(1);
            	        	Match(input,EQUAL,FOLLOW_EQUAL_in_rel_exp1759); if (failed) return n;
            	        	PushFollow(FOLLOW_add_expr_in_rel_exp1764);
            	        	r = add_expr();
            	        	followingStackPointer_--;
            	        	if (failed) return n;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  e = new BinaryRelational { Left = e, Right = r, Operator = BinaryOperator.Equals , Line = t.Line, Column = t.CharPositionInLine };
            	        	}
            	        
            	        }
            	        break;
            	    case 2 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:358:10: t= NEQUAL r= add_expr
            	        {
            	        	t = (IToken)input.LT(1);
            	        	Match(input,NEQUAL,FOLLOW_NEQUAL_in_rel_exp1779); if (failed) return n;
            	        	PushFollow(FOLLOW_add_expr_in_rel_exp1784);
            	        	r = add_expr();
            	        	followingStackPointer_--;
            	        	if (failed) return n;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  e = new BinaryRelational { Left = e, Right = r, Operator = BinaryOperator.NotEqual ,Line = t.Line, Column = t.CharPositionInLine};
            	        	}
            	        
            	        }
            	        break;
            	    case 3 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:359:10: t= LESS r= add_expr
            	        {
            	        	t = (IToken)input.LT(1);
            	        	Match(input,LESS,FOLLOW_LESS_in_rel_exp1801); if (failed) return n;
            	        	PushFollow(FOLLOW_add_expr_in_rel_exp1808);
            	        	r = add_expr();
            	        	followingStackPointer_--;
            	        	if (failed) return n;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  e = new BinaryRelational { Left = e, Right = r, Operator = BinaryOperator.Less, Line = t.Line, Column = t.CharPositionInLine };
            	        	}
            	        
            	        }
            	        break;
            	    case 4 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:360:10: t= LEQUAL r= add_expr
            	        {
            	        	t = (IToken)input.LT(1);
            	        	Match(input,LEQUAL,FOLLOW_LEQUAL_in_rel_exp1823); if (failed) return n;
            	        	PushFollow(FOLLOW_add_expr_in_rel_exp1828);
            	        	r = add_expr();
            	        	followingStackPointer_--;
            	        	if (failed) return n;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  e = new BinaryRelational { Left = e, Right = r, Operator = BinaryOperator.LessEquals ,Line = t.Line, Column = t.CharPositionInLine};
            	        	}
            	        
            	        }
            	        break;
            	    case 5 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:361:10: t= GREATER r= add_expr
            	        {
            	        	t = (IToken)input.LT(1);
            	        	Match(input,GREATER,FOLLOW_GREATER_in_rel_exp1843); if (failed) return n;
            	        	PushFollow(FOLLOW_add_expr_in_rel_exp1847);
            	        	r = add_expr();
            	        	followingStackPointer_--;
            	        	if (failed) return n;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  e = new BinaryRelational { Left = e, Right = r, Operator = BinaryOperator.Greater ,Line = t.Line, Column = t.CharPositionInLine};
            	        	}
            	        
            	        }
            	        break;
            	    case 6 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:362:10: t= GEQUAL r= add_expr
            	        {
            	        	t = (IToken)input.LT(1);
            	        	Match(input,GEQUAL,FOLLOW_GEQUAL_in_rel_exp1862); if (failed) return n;
            	        	PushFollow(FOLLOW_add_expr_in_rel_exp1867);
            	        	r = add_expr();
            	        	followingStackPointer_--;
            	        	if (failed) return n;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  e = new BinaryRelational { Left = e, Right = r, Operator = BinaryOperator.GreaterEquals ,Line = t.Line, Column = t.CharPositionInLine};
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	if ( backtracking == 0 ) 
            	{
            	  n=e;
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return n;
    }
    // $ANTLR end rel_exp

    
    // $ANTLR start add_expr
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:366:1: add_expr returns [Expression n] : e= mul_expr ( (t= ADD | t= SUB ) r= mul_expr )* ;
    public Expression add_expr() // throws RecognitionException [1]
    {   

        Expression n = null;
    
        IToken t = null;
        Expression e = null;

        Expression r = null;
        
    
         BinaryOperator op = default(BinaryOperator); 
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:368:2: (e= mul_expr ( (t= ADD | t= SUB ) r= mul_expr )* )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:368:3: e= mul_expr ( (t= ADD | t= SUB ) r= mul_expr )*
            {
            	PushFollow(FOLLOW_mul_expr_in_add_expr1899);
            	e = mul_expr();
            	followingStackPointer_--;
            	if (failed) return n;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:368:14: ( (t= ADD | t= SUB ) r= mul_expr )*
            	do 
            	{
            	    int alt43 = 2;
            	    int LA43_0 = input.LA(1);
            	    
            	    if ( (LA43_0 == ADD || LA43_0 == SUB) )
            	    {
            	        alt43 = 1;
            	    }
            	    
            	
            	    switch (alt43) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:368:15: (t= ADD | t= SUB ) r= mul_expr
            			    {
            			    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:368:15: (t= ADD | t= SUB )
            			    	int alt42 = 2;
            			    	int LA42_0 = input.LA(1);
            			    	
            			    	if ( (LA42_0 == ADD) )
            			    	{
            			    	    alt42 = 1;
            			    	}
            			    	else if ( (LA42_0 == SUB) )
            			    	{
            			    	    alt42 = 2;
            			    	}
            			    	else 
            			    	{
            			    	    if ( backtracking > 0 ) {failed = true; return n;}
            			    	    NoViableAltException nvae_d42s0 =
            			    	        new NoViableAltException("368:15: (t= ADD | t= SUB )", 42, 0, input);
            			    	
            			    	    throw nvae_d42s0;
            			    	}
            			    	switch (alt42) 
            			    	{
            			    	    case 1 :
            			    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:368:16: t= ADD
            			    	        {
            			    	        	t = (IToken)input.LT(1);
            			    	        	Match(input,ADD,FOLLOW_ADD_in_add_expr1905); if (failed) return n;
            			    	        	if ( backtracking == 0 ) 
            			    	        	{
            			    	        	  op = BinaryOperator.Addition; 
            			    	        	}
            			    	        
            			    	        }
            			    	        break;
            			    	    case 2 :
            			    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:368:56: t= SUB
            			    	        {
            			    	        	t = (IToken)input.LT(1);
            			    	        	Match(input,SUB,FOLLOW_SUB_in_add_expr1912); if (failed) return n;
            			    	        	if ( backtracking == 0 ) 
            			    	        	{
            			    	        	   op = BinaryOperator.Substraction;
            			    	        	}
            			    	        
            			    	        }
            			    	        break;
            			    	
            			    	}

            			    	PushFollow(FOLLOW_mul_expr_in_add_expr1933);
            			    	r = mul_expr();
            			    	followingStackPointer_--;
            			    	if (failed) return n;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	   e = new  BinaryAritmetic{ Left = e, Right = r, Operator = op ,Line = t.Line, Column = t.CharPositionInLine }; 
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop43;
            	    }
            	} while (true);
            	
            	loop43:
            		;	// Stops C# compiler whinging that label 'loop43' has no statements

            	if ( backtracking == 0 ) 
            	{
            	  n=e;
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return n;
    }
    // $ANTLR end add_expr

    
    // $ANTLR start mul_expr
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:373:1: mul_expr returns [Expression n] : e= unary_expr ( (t= MUL | t= DIV ) r= unary_expr )* ;
    public Expression mul_expr() // throws RecognitionException [1]
    {   

        Expression n = null;
    
        IToken t = null;
        Expression e = null;

        Expression r = null;
        
    
         BinaryOperator op = default(BinaryOperator); 
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:375:2: (e= unary_expr ( (t= MUL | t= DIV ) r= unary_expr )* )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:375:3: e= unary_expr ( (t= MUL | t= DIV ) r= unary_expr )*
            {
            	PushFollow(FOLLOW_unary_expr_in_mul_expr1973);
            	e = unary_expr();
            	followingStackPointer_--;
            	if (failed) return n;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:375:16: ( (t= MUL | t= DIV ) r= unary_expr )*
            	do 
            	{
            	    int alt45 = 2;
            	    int LA45_0 = input.LA(1);
            	    
            	    if ( ((LA45_0 >= MUL && LA45_0 <= DIV)) )
            	    {
            	        alt45 = 1;
            	    }
            	    
            	
            	    switch (alt45) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:375:17: (t= MUL | t= DIV ) r= unary_expr
            			    {
            			    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:375:17: (t= MUL | t= DIV )
            			    	int alt44 = 2;
            			    	int LA44_0 = input.LA(1);
            			    	
            			    	if ( (LA44_0 == MUL) )
            			    	{
            			    	    alt44 = 1;
            			    	}
            			    	else if ( (LA44_0 == DIV) )
            			    	{
            			    	    alt44 = 2;
            			    	}
            			    	else 
            			    	{
            			    	    if ( backtracking > 0 ) {failed = true; return n;}
            			    	    NoViableAltException nvae_d44s0 =
            			    	        new NoViableAltException("375:17: (t= MUL | t= DIV )", 44, 0, input);
            			    	
            			    	    throw nvae_d44s0;
            			    	}
            			    	switch (alt44) 
            			    	{
            			    	    case 1 :
            			    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:375:18: t= MUL
            			    	        {
            			    	        	t = (IToken)input.LT(1);
            			    	        	Match(input,MUL,FOLLOW_MUL_in_mul_expr1979); if (failed) return n;
            			    	        	if ( backtracking == 0 ) 
            			    	        	{
            			    	        	   op = BinaryOperator.Multiplication; 
            			    	        	}
            			    	        
            			    	        }
            			    	        break;
            			    	    case 2 :
            			    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:375:65: t= DIV
            			    	        {
            			    	        	t = (IToken)input.LT(1);
            			    	        	Match(input,DIV,FOLLOW_DIV_in_mul_expr1986); if (failed) return n;
            			    	        	if ( backtracking == 0 ) 
            			    	        	{
            			    	        	   op =BinaryOperator.Division; 
            			    	        	}
            			    	        
            			    	        }
            			    	        break;
            			    	
            			    	}

            			    	PushFollow(FOLLOW_unary_expr_in_mul_expr1995);
            			    	r = unary_expr();
            			    	followingStackPointer_--;
            			    	if (failed) return n;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	    e = new  BinaryAritmetic{ Left = e, Right = r, Operator = op ,Line = t.Line, Column = t.CharPositionInLine};
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop45;
            	    }
            	} while (true);
            	
            	loop45:
            		;	// Stops C# compiler whinging that label 'loop45' has no statements

            	if ( backtracking == 0 ) 
            	{
            	  n=e;
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return n;
    }
    // $ANTLR end mul_expr

    
    // $ANTLR start unary_expr
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:378:1: unary_expr returns [Expression n] : (e= increment | t= NOT (v= lvalue | v= parent_exp | b= BOOL_CONSTANT ) | t= SUB (v= lvalue | v= parent_exp | v= constant_exp ) | c= parent_exp ( (v= lvalue | v= parent_exp | v= constant_exp ) )? | cons= constructor | lit= constant_exp );
    public Expression unary_expr() // throws RecognitionException [1]
    {   

        Expression n = null;
    
        IToken t = null;
        IToken b = null;
        Expression e = null;

        Expression v = null;

        Expression c = null;

        VariableInitializer cons = null;

        Expression lit = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:379:2: (e= increment | t= NOT (v= lvalue | v= parent_exp | b= BOOL_CONSTANT ) | t= SUB (v= lvalue | v= parent_exp | v= constant_exp ) | c= parent_exp ( (v= lvalue | v= parent_exp | v= constant_exp ) )? | cons= constructor | lit= constant_exp )
            int alt50 = 6;
            switch ( input.LA(1) ) 
            {
            case INCREMENT:
            case DECREMENT:
            case ID:
            	{
                alt50 = 1;
                }
                break;
            case NOT:
            	{
                alt50 = 2;
                }
                break;
            case SUB:
            	{
                alt50 = 3;
                }
                break;
            case LPAREN:
            	{
                alt50 = 4;
                }
                break;
            case LBRACE:
            	{
                alt50 = 5;
                }
                break;
            case NUMBER:
            case BOOL_CONSTANT:
            	{
                alt50 = 6;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return n;}
            	    NoViableAltException nvae_d50s0 =
            	        new NoViableAltException("378:1: unary_expr returns [Expression n] : (e= increment | t= NOT (v= lvalue | v= parent_exp | b= BOOL_CONSTANT ) | t= SUB (v= lvalue | v= parent_exp | v= constant_exp ) | c= parent_exp ( (v= lvalue | v= parent_exp | v= constant_exp ) )? | cons= constructor | lit= constant_exp );", 50, 0, input);
            
            	    throw nvae_d50s0;
            }
            
            switch (alt50) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:380:2: e= increment
                    {
                    	PushFollow(FOLLOW_increment_in_unary_expr2018);
                    	e = increment();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	if ( backtracking == 0 ) 
                    	{
                    	   n=e; 
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:381:4: t= NOT (v= lvalue | v= parent_exp | b= BOOL_CONSTANT )
                    {
                    	t = (IToken)input.LT(1);
                    	Match(input,NOT,FOLLOW_NOT_in_unary_expr2026); if (failed) return n;
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:381:10: (v= lvalue | v= parent_exp | b= BOOL_CONSTANT )
                    	int alt46 = 3;
                    	switch ( input.LA(1) ) 
                    	{
                    	case ID:
                    		{
                    	    alt46 = 1;
                    	    }
                    	    break;
                    	case LPAREN:
                    		{
                    	    alt46 = 2;
                    	    }
                    	    break;
                    	case BOOL_CONSTANT:
                    		{
                    	    alt46 = 3;
                    	    }
                    	    break;
                    		default:
                    		    if ( backtracking > 0 ) {failed = true; return n;}
                    		    NoViableAltException nvae_d46s0 =
                    		        new NoViableAltException("381:10: (v= lvalue | v= parent_exp | b= BOOL_CONSTANT )", 46, 0, input);
                    	
                    		    throw nvae_d46s0;
                    	}
                    	
                    	switch (alt46) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:381:11: v= lvalue
                    	        {
                    	        	PushFollow(FOLLOW_lvalue_in_unary_expr2031);
                    	        	v = lvalue();
                    	        	followingStackPointer_--;
                    	        	if (failed) return n;
                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:381:22: v= parent_exp
                    	        {
                    	        	PushFollow(FOLLOW_parent_exp_in_unary_expr2037);
                    	        	v = parent_exp();
                    	        	followingStackPointer_--;
                    	        	if (failed) return n;
                    	        
                    	        }
                    	        break;
                    	    case 3 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:382:5: b= BOOL_CONSTANT
                    	        {
                    	        	b = (IToken)input.LT(1);
                    	        	Match(input,BOOL_CONSTANT,FOLLOW_BOOL_CONSTANT_in_unary_expr2046); if (failed) return n;
                    	        	if ( backtracking == 0 ) 
                    	        	{
                    	        	  v=new LiteralExpression<bool>(bool.Parse(b.Text)){ Line = b.Line, Column = b.CharPositionInLine }; 
                    	        	}
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	if ( backtracking == 0 ) 
                    	{
                    	   
                    	  		n = new UnaryExpression(v, UnaryOperator.Not ){Line = t.Line, Column = t.CharPositionInLine}; 
                    	  	
                    	}
                    
                    }
                    break;
                case 3 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:386:4: t= SUB (v= lvalue | v= parent_exp | v= constant_exp )
                    {
                    	t = (IToken)input.LT(1);
                    	Match(input,SUB,FOLLOW_SUB_in_unary_expr2060); if (failed) return n;
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:386:10: (v= lvalue | v= parent_exp | v= constant_exp )
                    	int alt47 = 3;
                    	switch ( input.LA(1) ) 
                    	{
                    	case ID:
                    		{
                    	    alt47 = 1;
                    	    }
                    	    break;
                    	case LPAREN:
                    		{
                    	    alt47 = 2;
                    	    }
                    	    break;
                    	case NUMBER:
                    	case BOOL_CONSTANT:
                    		{
                    	    alt47 = 3;
                    	    }
                    	    break;
                    		default:
                    		    if ( backtracking > 0 ) {failed = true; return n;}
                    		    NoViableAltException nvae_d47s0 =
                    		        new NoViableAltException("386:10: (v= lvalue | v= parent_exp | v= constant_exp )", 47, 0, input);
                    	
                    		    throw nvae_d47s0;
                    	}
                    	
                    	switch (alt47) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:386:11: v= lvalue
                    	        {
                    	        	PushFollow(FOLLOW_lvalue_in_unary_expr2065);
                    	        	v = lvalue();
                    	        	followingStackPointer_--;
                    	        	if (failed) return n;
                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:386:22: v= parent_exp
                    	        {
                    	        	PushFollow(FOLLOW_parent_exp_in_unary_expr2071);
                    	        	v = parent_exp();
                    	        	followingStackPointer_--;
                    	        	if (failed) return n;
                    	        
                    	        }
                    	        break;
                    	    case 3 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:386:37: v= constant_exp
                    	        {
                    	        	PushFollow(FOLLOW_constant_exp_in_unary_expr2078);
                    	        	v = constant_exp();
                    	        	followingStackPointer_--;
                    	        	if (failed) return n;
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	if ( backtracking == 0 ) 
                    	{
                    	   n = new UnaryExpression(v, UnaryOperator.Neg ){Line = t.Line, Column = t.CharPositionInLine}; 
                    	}
                    
                    }
                    break;
                case 4 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:387:4: c= parent_exp ( (v= lvalue | v= parent_exp | v= constant_exp ) )?
                    {
                    	PushFollow(FOLLOW_parent_exp_in_unary_expr2090);
                    	c = parent_exp();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	if ( backtracking == 0 ) 
                    	{
                    	  n=c;
                    	}
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:387:24: ( (v= lvalue | v= parent_exp | v= constant_exp ) )?
                    	int alt49 = 2;
                    	int LA49_0 = input.LA(1);
                    	
                    	if ( (LA49_0 == LPAREN || (LA49_0 >= ID && LA49_0 <= NUMBER) || LA49_0 == BOOL_CONSTANT) )
                    	{
                    	    alt49 = 1;
                    	}
                    	switch (alt49) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:387:25: (v= lvalue | v= parent_exp | v= constant_exp )
                    	        {
                    	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:387:25: (v= lvalue | v= parent_exp | v= constant_exp )
                    	        	int alt48 = 3;
                    	        	switch ( input.LA(1) ) 
                    	        	{
                    	        	case ID:
                    	        		{
                    	        	    alt48 = 1;
                    	        	    }
                    	        	    break;
                    	        	case LPAREN:
                    	        		{
                    	        	    alt48 = 2;
                    	        	    }
                    	        	    break;
                    	        	case NUMBER:
                    	        	case BOOL_CONSTANT:
                    	        		{
                    	        	    alt48 = 3;
                    	        	    }
                    	        	    break;
                    	        		default:
                    	        		    if ( backtracking > 0 ) {failed = true; return n;}
                    	        		    NoViableAltException nvae_d48s0 =
                    	        		        new NoViableAltException("387:25: (v= lvalue | v= parent_exp | v= constant_exp )", 48, 0, input);
                    	        	
                    	        		    throw nvae_d48s0;
                    	        	}
                    	        	
                    	        	switch (alt48) 
                    	        	{
                    	        	    case 1 :
                    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:387:26: v= lvalue
                    	        	        {
                    	        	        	PushFollow(FOLLOW_lvalue_in_unary_expr2098);
                    	        	        	v = lvalue();
                    	        	        	followingStackPointer_--;
                    	        	        	if (failed) return n;
                    	        	        
                    	        	        }
                    	        	        break;
                    	        	    case 2 :
                    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:387:37: v= parent_exp
                    	        	        {
                    	        	        	PushFollow(FOLLOW_parent_exp_in_unary_expr2104);
                    	        	        	v = parent_exp();
                    	        	        	followingStackPointer_--;
                    	        	        	if (failed) return n;
                    	        	        
                    	        	        }
                    	        	        break;
                    	        	    case 3 :
                    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:387:52: v= constant_exp
                    	        	        {
                    	        	        	PushFollow(FOLLOW_constant_exp_in_unary_expr2110);
                    	        	        	v = constant_exp();
                    	        	        	followingStackPointer_--;
                    	        	        	if (failed) return n;
                    	        	        
                    	        	        }
                    	        	        break;
                    	        	
                    	        	}

                    	        	if ( backtracking == 0 ) 
                    	        	{
                    	        	   n = new CastExpression(c, v); 
                    	        	}
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    
                    }
                    break;
                case 5 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:388:4: cons= constructor
                    {
                    	PushFollow(FOLLOW_constructor_in_unary_expr2123);
                    	cons = constructor();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	if ( backtracking == 0 ) 
                    	{
                    	   n = cons;
                    	}
                    
                    }
                    break;
                case 6 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:389:4: lit= constant_exp
                    {
                    	PushFollow(FOLLOW_constant_exp_in_unary_expr2131);
                    	lit = constant_exp();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	if ( backtracking == 0 ) 
                    	{
                    	  n =lit;
                    	}
                    
                    }
                    break;
            
            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return n;
    }
    // $ANTLR end unary_expr

    
    // $ANTLR start parent_exp
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:392:1: parent_exp returns [Expression n] : LPAREN c= expression RPAREN ;
    public Expression parent_exp() // throws RecognitionException [1]
    {   

        Expression n = null;
    
        IToken LPAREN9 = null;
        Expression c = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:393:2: ( LPAREN c= expression RPAREN )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:393:4: LPAREN c= expression RPAREN
            {
            	LPAREN9 = (IToken)input.LT(1);
            	Match(input,LPAREN,FOLLOW_LPAREN_in_parent_exp2147); if (failed) return n;
            	PushFollow(FOLLOW_expression_in_parent_exp2151);
            	c = expression();
            	followingStackPointer_--;
            	if (failed) return n;
            	if ( backtracking == 0 ) 
            	{
            	   n = new ParenEncloseExpression(c, LPAREN9.Line, LPAREN9.CharPositionInLine); 
            	}
            	Match(input,RPAREN,FOLLOW_RPAREN_in_parent_exp2155); if (failed) return n;
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return n;
    }
    // $ANTLR end parent_exp

    
    // $ANTLR start increment
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:395:1: increment returns [Expression n] : (e= lvalue (t= INCREMENT | t= DECREMENT )? | (t= INCREMENT | t= DECREMENT ) e= lvalue );
    public Expression increment() // throws RecognitionException [1]
    {   

        Expression n = null;
    
        IToken t = null;
        Expression e = null;
        
    
         UnaryOperator op = default(UnaryOperator); 
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:397:2: (e= lvalue (t= INCREMENT | t= DECREMENT )? | (t= INCREMENT | t= DECREMENT ) e= lvalue )
            int alt53 = 2;
            int LA53_0 = input.LA(1);
            
            if ( (LA53_0 == ID) )
            {
                alt53 = 1;
            }
            else if ( (LA53_0 == INCREMENT || LA53_0 == DECREMENT) )
            {
                alt53 = 2;
            }
            else 
            {
                if ( backtracking > 0 ) {failed = true; return n;}
                NoViableAltException nvae_d53s0 =
                    new NoViableAltException("395:1: increment returns [Expression n] : (e= lvalue (t= INCREMENT | t= DECREMENT )? | (t= INCREMENT | t= DECREMENT ) e= lvalue );", 53, 0, input);
            
                throw nvae_d53s0;
            }
            switch (alt53) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:397:4: e= lvalue (t= INCREMENT | t= DECREMENT )?
                    {
                    	PushFollow(FOLLOW_lvalue_in_increment2175);
                    	e = lvalue();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	if ( backtracking == 0 ) 
                    	{
                    	   n=e; 
                    	}
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:397:21: (t= INCREMENT | t= DECREMENT )?
                    	int alt51 = 3;
                    	int LA51_0 = input.LA(1);
                    	
                    	if ( (LA51_0 == INCREMENT) )
                    	{
                    	    alt51 = 1;
                    	}
                    	else if ( (LA51_0 == DECREMENT) )
                    	{
                    	    alt51 = 2;
                    	}
                    	switch (alt51) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:397:22: t= INCREMENT
                    	        {
                    	        	t = (IToken)input.LT(1);
                    	        	Match(input,INCREMENT,FOLLOW_INCREMENT_in_increment2181); if (failed) return n;
                    	        	if ( backtracking == 0 ) 
                    	        	{
                    	        	   n = new  UnaryExpression(e,UnaryOperator.PostInc){Line = t.Line, Column = t.CharPositionInLine};
                    	        	}
                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:398:8: t= DECREMENT
                    	        {
                    	        	t = (IToken)input.LT(1);
                    	        	Match(input,DECREMENT,FOLLOW_DECREMENT_in_increment2196); if (failed) return n;
                    	        	if ( backtracking == 0 ) 
                    	        	{
                    	        	   n = new  UnaryExpression(e,UnaryOperator.PostDec){Line = t.Line, Column = t.CharPositionInLine};
                    	        	}
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:399:6: (t= INCREMENT | t= DECREMENT ) e= lvalue
                    {
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:399:6: (t= INCREMENT | t= DECREMENT )
                    	int alt52 = 2;
                    	int LA52_0 = input.LA(1);
                    	
                    	if ( (LA52_0 == INCREMENT) )
                    	{
                    	    alt52 = 1;
                    	}
                    	else if ( (LA52_0 == DECREMENT) )
                    	{
                    	    alt52 = 2;
                    	}
                    	else 
                    	{
                    	    if ( backtracking > 0 ) {failed = true; return n;}
                    	    NoViableAltException nvae_d52s0 =
                    	        new NoViableAltException("399:6: (t= INCREMENT | t= DECREMENT )", 52, 0, input);
                    	
                    	    throw nvae_d52s0;
                    	}
                    	switch (alt52) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:399:8: t= INCREMENT
                    	        {
                    	        	t = (IToken)input.LT(1);
                    	        	Match(input,INCREMENT,FOLLOW_INCREMENT_in_increment2210); if (failed) return n;
                    	        	if ( backtracking == 0 ) 
                    	        	{
                    	        	   op = UnaryOperator.PreInc;
                    	        	}
                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:400:8: t= DECREMENT
                    	        {
                    	        	t = (IToken)input.LT(1);
                    	        	Match(input,DECREMENT,FOLLOW_DECREMENT_in_increment2224); if (failed) return n;
                    	        	if ( backtracking == 0 ) 
                    	        	{
                    	        	   op = UnaryOperator.PreDec;
                    	        	}
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	PushFollow(FOLLOW_lvalue_in_increment2232);
                    	e = lvalue();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	if ( backtracking == 0 ) 
                    	{
                    	  n = new UnaryExpression(e, op){Line = t.Line, Column = t.CharPositionInLine};
                    	}
                    
                    }
                    break;
            
            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return n;
    }
    // $ANTLR end increment

    
    // $ANTLR start lvalue
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:403:1: lvalue returns [Expression n] : e= primary_exp ( ( DOT (m= ID LPAREN (args= argument_list )? RPAREN | m= ID ) | LBRACKET indexer= expression RBRACKET ) )* ;
    public Expression lvalue() // throws RecognitionException [1]
    {   

        Expression n = null;
    
        IToken m = null;
        IToken LBRACKET10 = null;
        Expression e = null;

        List<RealArguments> args = null;

        Expression indexer = null;
        
    
         	
        	var function = false;
        	var arrayIndex = false;
             
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:2: (e= primary_exp ( ( DOT (m= ID LPAREN (args= argument_list )? RPAREN | m= ID ) | LBRACKET indexer= expression RBRACKET ) )* )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:4: e= primary_exp ( ( DOT (m= ID LPAREN (args= argument_list )? RPAREN | m= ID ) | LBRACKET indexer= expression RBRACKET ) )*
            {
            	PushFollow(FOLLOW_primary_exp_in_lvalue2258);
            	e = primary_exp();
            	followingStackPointer_--;
            	if (failed) return n;
            	if ( backtracking == 0 ) 
            	{
            	   n = e ;
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:27: ( ( DOT (m= ID LPAREN (args= argument_list )? RPAREN | m= ID ) | LBRACKET indexer= expression RBRACKET ) )*
            	do 
            	{
            	    int alt57 = 2;
            	    int LA57_0 = input.LA(1);
            	    
            	    if ( (LA57_0 == DOT || LA57_0 == LBRACKET) )
            	    {
            	        alt57 = 1;
            	    }
            	    
            	
            	    switch (alt57) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:28: ( DOT (m= ID LPAREN (args= argument_list )? RPAREN | m= ID ) | LBRACKET indexer= expression RBRACKET )
            			    {
            			    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:28: ( DOT (m= ID LPAREN (args= argument_list )? RPAREN | m= ID ) | LBRACKET indexer= expression RBRACKET )
            			    	int alt56 = 2;
            			    	int LA56_0 = input.LA(1);
            			    	
            			    	if ( (LA56_0 == DOT) )
            			    	{
            			    	    alt56 = 1;
            			    	}
            			    	else if ( (LA56_0 == LBRACKET) )
            			    	{
            			    	    alt56 = 2;
            			    	}
            			    	else 
            			    	{
            			    	    if ( backtracking > 0 ) {failed = true; return n;}
            			    	    NoViableAltException nvae_d56s0 =
            			    	        new NoViableAltException("408:28: ( DOT (m= ID LPAREN (args= argument_list )? RPAREN | m= ID ) | LBRACKET indexer= expression RBRACKET )", 56, 0, input);
            			    	
            			    	    throw nvae_d56s0;
            			    	}
            			    	switch (alt56) 
            			    	{
            			    	    case 1 :
            			    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:29: DOT (m= ID LPAREN (args= argument_list )? RPAREN | m= ID )
            			    	        {
            			    	        	Match(input,DOT,FOLLOW_DOT_in_lvalue2262); if (failed) return n;
            			    	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:33: (m= ID LPAREN (args= argument_list )? RPAREN | m= ID )
            			    	        	int alt55 = 2;
            			    	        	int LA55_0 = input.LA(1);
            			    	        	
            			    	        	if ( (LA55_0 == ID) )
            			    	        	{
            			    	        	    int LA55_1 = input.LA(2);
            			    	        	    
            			    	        	    if ( (LA55_1 == LPAREN) )
            			    	        	    {
            			    	        	        alt55 = 1;
            			    	        	    }
            			    	        	    else if ( ((LA55_1 >= EQUAL && LA55_1 <= GEQUAL) || (LA55_1 >= MUL && LA55_1 <= RBRACKET) || LA55_1 == RPAREN || (LA55_1 >= RBRACE && LA55_1 <= QUESTION)) )
            			    	        	    {
            			    	        	        alt55 = 2;
            			    	        	    }
            			    	        	    else 
            			    	        	    {
            			    	        	        if ( backtracking > 0 ) {failed = true; return n;}
            			    	        	        NoViableAltException nvae_d55s1 =
            			    	        	            new NoViableAltException("408:33: (m= ID LPAREN (args= argument_list )? RPAREN | m= ID )", 55, 1, input);
            			    	        	    
            			    	        	        throw nvae_d55s1;
            			    	        	    }
            			    	        	}
            			    	        	else 
            			    	        	{
            			    	        	    if ( backtracking > 0 ) {failed = true; return n;}
            			    	        	    NoViableAltException nvae_d55s0 =
            			    	        	        new NoViableAltException("408:33: (m= ID LPAREN (args= argument_list )? RPAREN | m= ID )", 55, 0, input);
            			    	        	
            			    	        	    throw nvae_d55s0;
            			    	        	}
            			    	        	switch (alt55) 
            			    	        	{
            			    	        	    case 1 :
            			    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:34: m= ID LPAREN (args= argument_list )? RPAREN
            			    	        	        {
            			    	        	        	m = (IToken)input.LT(1);
            			    	        	        	Match(input,ID,FOLLOW_ID_in_lvalue2267); if (failed) return n;
            			    	        	        	Match(input,LPAREN,FOLLOW_LPAREN_in_lvalue2269); if (failed) return n;
            			    	        	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:46: (args= argument_list )?
            			    	        	        	int alt54 = 2;
            			    	        	        	int LA54_0 = input.LA(1);
            			    	        	        	
            			    	        	        	if ( (LA54_0 == NOT || (LA54_0 >= INCREMENT && LA54_0 <= DECREMENT) || LA54_0 == LPAREN || LA54_0 == LBRACE || (LA54_0 >= ID && LA54_0 <= NUMBER) || LA54_0 == BOOL_CONSTANT) )
            			    	        	        	{
            			    	        	        	    alt54 = 1;
            			    	        	        	}
            			    	        	        	switch (alt54) 
            			    	        	        	{
            			    	        	        	    case 1 :
            			    	        	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:47: args= argument_list
            			    	        	        	        {
            			    	        	        	        	PushFollow(FOLLOW_argument_list_in_lvalue2274);
            			    	        	        	        	args = argument_list();
            			    	        	        	        	followingStackPointer_--;
            			    	        	        	        	if (failed) return n;
            			    	        	        	        
            			    	        	        	        }
            			    	        	        	        break;
            			    	        	        	
            			    	        	        	}

            			    	        	        	Match(input,RPAREN,FOLLOW_RPAREN_in_lvalue2278); if (failed) return n;
            			    	        	        	if ( backtracking == 0 ) 
            			    	        	        	{
            			    	        	        	   
            			    	        	        	  						 n = new FunCallExpression{ 
            			    	        	        	  							FunctionName = m.Text,
            			    	        	        	  							Left = n,
            			    	        	        	  							Parameters =args!=null?args.ToArray():new RealArguments[0],
            			    	        	        	  							Line = m.Line,
            			    	        	        	  							Column = m.CharPositionInLine};
            			    	        	        	  						
            			    	        	        	}
            			    	        	        
            			    	        	        }
            			    	        	        break;
            			    	        	    case 2 :
            			    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:416:8: m= ID
            			    	        	        {
            			    	        	        	m = (IToken)input.LT(1);
            			    	        	        	Match(input,ID,FOLLOW_ID_in_lvalue2291); if (failed) return n;
            			    	        	        	if ( backtracking == 0 ) 
            			    	        	        	{
            			    	        	        	  
            			    	        	        	  						n = new MemberExpression{
            			    	        	        	  							Left = n,
            			    	        	        	  							MemberName = m.Text,
            			    	        	        	  							Line = m.Line,
            			    	        	        	  							Column = m.CharPositionInLine};						
            			    	        	        	  					
            			    	        	        	}
            			    	        	        
            			    	        	        }
            			    	        	        break;
            			    	        	
            			    	        	}

            			    	        
            			    	        }
            			    	        break;
            			    	    case 2 :
            			    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:423:16: LBRACKET indexer= expression RBRACKET
            			    	        {
            			    	        	LBRACKET10 = (IToken)input.LT(1);
            			    	        	Match(input,LBRACKET,FOLLOW_LBRACKET_in_lvalue2311); if (failed) return n;
            			    	        	PushFollow(FOLLOW_expression_in_lvalue2315);
            			    	        	indexer = expression();
            			    	        	followingStackPointer_--;
            			    	        	if (failed) return n;
            			    	        	Match(input,RBRACKET,FOLLOW_RBRACKET_in_lvalue2317); if (failed) return n;
            			    	        	if ( backtracking == 0 ) 
            			    	        	{
            			    	        	   
            			    	        	  				          n = new IndexArrayExpression
            			    	        	  						{
            			    	        	  							Left = n,
            			    	        	  							Indexer = indexer,
            			    	        	  							Line = LBRACKET10.Line,
            			    	        	  							Column =LBRACKET10.CharPositionInLine
            			    	        	  						};
            			    	        	  					
            			    	        	}
            			    	        
            			    	        }
            			    	        break;
            			    	
            			    	}

            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop57;
            	    }
            	} while (true);
            	
            	loop57:
            		;	// Stops C# compiler whinging that label 'loop57' has no statements

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return n;
    }
    // $ANTLR end lvalue

    
    // $ANTLR start primary_exp
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:435:1: primary_exp returns [Expression e] : n= ID ( LPAREN (args= argument_list )? RPAREN )? ;
    public Expression primary_exp() // throws RecognitionException [1]
    {   

        Expression e = null;
    
        IToken n = null;
        List<RealArguments> args = null;
        
    
         
        	var function = false; 	
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:439:2: (n= ID ( LPAREN (args= argument_list )? RPAREN )? )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:439:3: n= ID ( LPAREN (args= argument_list )? RPAREN )?
            {
            	n = (IToken)input.LT(1);
            	Match(input,ID,FOLLOW_ID_in_primary_exp2351); if (failed) return e;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:439:8: ( LPAREN (args= argument_list )? RPAREN )?
            	int alt59 = 2;
            	int LA59_0 = input.LA(1);
            	
            	if ( (LA59_0 == LPAREN) )
            	{
            	    alt59 = 1;
            	}
            	switch (alt59) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:439:9: LPAREN (args= argument_list )? RPAREN
            	        {
            	        	Match(input,LPAREN,FOLLOW_LPAREN_in_primary_exp2354); if (failed) return e;
            	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:439:16: (args= argument_list )?
            	        	int alt58 = 2;
            	        	int LA58_0 = input.LA(1);
            	        	
            	        	if ( (LA58_0 == NOT || (LA58_0 >= INCREMENT && LA58_0 <= DECREMENT) || LA58_0 == LPAREN || LA58_0 == LBRACE || (LA58_0 >= ID && LA58_0 <= NUMBER) || LA58_0 == BOOL_CONSTANT) )
            	        	{
            	        	    alt58 = 1;
            	        	}
            	        	switch (alt58) 
            	        	{
            	        	    case 1 :
            	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:439:17: args= argument_list
            	        	        {
            	        	        	PushFollow(FOLLOW_argument_list_in_primary_exp2359);
            	        	        	args = argument_list();
            	        	        	followingStackPointer_--;
            	        	        	if (failed) return e;
            	        	        
            	        	        }
            	        	        break;
            	        	
            	        	}

            	        	Match(input,RPAREN,FOLLOW_RPAREN_in_primary_exp2363); if (failed) return e;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	   function = true; 
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	if ( backtracking == 0 ) 
            	{
            	  
            	  		if(function)
            	  		{
            	  			e = new FunCallExpression{ 				
            	  				FunctionName = n.Text,						
            	  				Parameters = args!=null?args.ToArray():new RealArguments[0],
            	  				Line = n.Line,
            	  				Column = n.CharPositionInLine
            	  			};
            	  		}
            	  		else
            	  		{
            	  			e = new VariableExpression{ 
            	  				Name = n.Text,
            	  				Line = n.Line,
            	  				Column = n.CharPositionInLine
            	  			};
            	  		}
            	  	
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return e;
    }
    // $ANTLR end primary_exp

    
    // $ANTLR start argument_list
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:461:1: argument_list returns [List<RealArguments> l = new List<RealArguments>()] : (n= ID ASSIGN )? v= expression ( COMMA (n= ID ASSIGN )? v= expression )* ;
    public List<RealArguments> argument_list() // throws RecognitionException [1]
    {   

        List<RealArguments> l =  new List<RealArguments>();
    
        IToken n = null;
        Expression v = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:463:2: ( (n= ID ASSIGN )? v= expression ( COMMA (n= ID ASSIGN )? v= expression )* )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:463:4: (n= ID ASSIGN )? v= expression ( COMMA (n= ID ASSIGN )? v= expression )*
            {
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:463:4: (n= ID ASSIGN )?
            	int alt60 = 2;
            	int LA60_0 = input.LA(1);
            	
            	if ( (LA60_0 == ID) )
            	{
            	    int LA60_1 = input.LA(2);
            	    
            	    if ( (LA60_1 == ASSIGN) )
            	    {
            	        alt60 = 1;
            	    }
            	}
            	switch (alt60) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:463:5: n= ID ASSIGN
            	        {
            	        	n = (IToken)input.LT(1);
            	        	Match(input,ID,FOLLOW_ID_in_argument_list2389); if (failed) return l;
            	        	Match(input,ASSIGN,FOLLOW_ASSIGN_in_argument_list2391); if (failed) return l;
            	        
            	        }
            	        break;
            	
            	}

            	PushFollow(FOLLOW_expression_in_argument_list2397);
            	v = expression();
            	followingStackPointer_--;
            	if (failed) return l;
            	if ( backtracking == 0 ) 
            	{
            	   l.Add(new RealArguments{ ParameterName = n!=null?n.Text:null, Value = v ,Line = v.Line, Column = v.Column });
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:464:4: ( COMMA (n= ID ASSIGN )? v= expression )*
            	do 
            	{
            	    int alt62 = 2;
            	    int LA62_0 = input.LA(1);
            	    
            	    if ( (LA62_0 == COMMA) )
            	    {
            	        alt62 = 1;
            	    }
            	    
            	
            	    switch (alt62) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:464:5: COMMA (n= ID ASSIGN )? v= expression
            			    {
            			    	Match(input,COMMA,FOLLOW_COMMA_in_argument_list2406); if (failed) return l;
            			    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:464:11: (n= ID ASSIGN )?
            			    	int alt61 = 2;
            			    	int LA61_0 = input.LA(1);
            			    	
            			    	if ( (LA61_0 == ID) )
            			    	{
            			    	    int LA61_1 = input.LA(2);
            			    	    
            			    	    if ( (LA61_1 == ASSIGN) )
            			    	    {
            			    	        alt61 = 1;
            			    	    }
            			    	}
            			    	switch (alt61) 
            			    	{
            			    	    case 1 :
            			    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:464:12: n= ID ASSIGN
            			    	        {
            			    	        	n = (IToken)input.LT(1);
            			    	        	Match(input,ID,FOLLOW_ID_in_argument_list2411); if (failed) return l;
            			    	        	Match(input,ASSIGN,FOLLOW_ASSIGN_in_argument_list2413); if (failed) return l;
            			    	        
            			    	        }
            			    	        break;
            			    	
            			    	}

            			    	PushFollow(FOLLOW_expression_in_argument_list2419);
            			    	v = expression();
            			    	followingStackPointer_--;
            			    	if (failed) return l;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	   l.Add(new RealArguments{ ParameterName =n!=null?n.Text:null, Value = v ,Line = v.Line, Column = v.Column}); 
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop62;
            	    }
            	} while (true);
            	
            	loop62:
            		;	// Stops C# compiler whinging that label 'loop62' has no statements

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return l;
    }
    // $ANTLR end argument_list

    
    // $ANTLR start constant_exp
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:467:1: constant_exp returns [Expression l] : (v= BOOL_CONSTANT | t= NUMBER );
    public Expression constant_exp() // throws RecognitionException [1]
    {   

        Expression l = null;
    
        IToken v = null;
        IToken t = null;
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:468:2: (v= BOOL_CONSTANT | t= NUMBER )
            int alt63 = 2;
            int LA63_0 = input.LA(1);
            
            if ( (LA63_0 == BOOL_CONSTANT) )
            {
                alt63 = 1;
            }
            else if ( (LA63_0 == NUMBER) )
            {
                alt63 = 2;
            }
            else 
            {
                if ( backtracking > 0 ) {failed = true; return l;}
                NoViableAltException nvae_d63s0 =
                    new NoViableAltException("467:1: constant_exp returns [Expression l] : (v= BOOL_CONSTANT | t= NUMBER );", 63, 0, input);
            
                throw nvae_d63s0;
            }
            switch (alt63) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:471:3: v= BOOL_CONSTANT
                    {
                    	v = (IToken)input.LT(1);
                    	Match(input,BOOL_CONSTANT,FOLLOW_BOOL_CONSTANT_in_constant_exp2447); if (failed) return l;
                    	if ( backtracking == 0 ) 
                    	{
                    	   l = new LiteralExpression<bool>(bool.Parse(v.Text)){ Line = v.Line, Column = v.CharPositionInLine }; 
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:472:4: t= NUMBER
                    {
                    	t = (IToken)input.LT(1);
                    	Match(input,NUMBER,FOLLOW_NUMBER_in_constant_exp2458); if (failed) return l;
                    	if ( backtracking == 0 ) 
                    	{
                    	  
                    	  		int ivalue;		
                    	  		if(int.TryParse(t.Text, out ivalue))
                    	  			l = new LiteralExpression<int>(ivalue){ Line =t.Line, Column = t.CharPositionInLine }; 
                    	  		else
                    	  			l = new LiteralExpression<float>(float.Parse(t.Text)){ Line = t.Line, Column =t.CharPositionInLine }; 		
                    	  	
                    	}
                    
                    }
                    break;
            
            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return l;
    }
    // $ANTLR end constant_exp

    
    // $ANTLR start statement
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:482:1: statement returns [Statement node] : (e= lvalue_statement SEMICOLON | list= local_declarations SEMICOLON | (attr= attribute )? (n= selection_stmt | n= iteration_stmt ) | n= block_statement | n= jump_stmt SEMICOLON );
    public Statement statement() // throws RecognitionException [1]
    {   

        Statement node = null;
    
        Expression e = null;

        List<LocalDeclaration> list = null;

        AttributeDeclaration attr = null;

        Statement n = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:483:2: (e= lvalue_statement SEMICOLON | list= local_declarations SEMICOLON | (attr= attribute )? (n= selection_stmt | n= iteration_stmt ) | n= block_statement | n= jump_stmt SEMICOLON )
            int alt66 = 5;
            switch ( input.LA(1) ) 
            {
            case INCREMENT:
            case DECREMENT:
            	{
                alt66 = 1;
                }
                break;
            case ID:
            	{
                int LA66_3 = input.LA(2);
                
                if ( (LA66_3 == INCREMENT || LA66_3 == DECREMENT || LA66_3 == DOT || LA66_3 == SEMICOLON || LA66_3 == LBRACKET || LA66_3 == LPAREN || (LA66_3 >= ASSIGN && LA66_3 <= DIVASSIGN)) )
                {
                    alt66 = 1;
                }
                else if ( (LA66_3 == LESS || LA66_3 == ID) )
                {
                    alt66 = 2;
                }
                else 
                {
                    if ( backtracking > 0 ) {failed = true; return node;}
                    NoViableAltException nvae_d66s3 =
                        new NoViableAltException("482:1: statement returns [Statement node] : (e= lvalue_statement SEMICOLON | list= local_declarations SEMICOLON | (attr= attribute )? (n= selection_stmt | n= iteration_stmt ) | n= block_statement | n= jump_stmt SEMICOLON );", 66, 3, input);
                
                    throw nvae_d66s3;
                }
                }
                break;
            case CONST:
            	{
                alt66 = 2;
                }
                break;
            case LBRACKET:
            case IF:
            case WHILE:
            case DO:
            case FOR:
            	{
                alt66 = 3;
                }
                break;
            case LBRACE:
            	{
                alt66 = 4;
                }
                break;
            case BREAK:
            case CONTINUE:
            case RETURN:
            case DISCARD:
            	{
                alt66 = 5;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return node;}
            	    NoViableAltException nvae_d66s0 =
            	        new NoViableAltException("482:1: statement returns [Statement node] : (e= lvalue_statement SEMICOLON | list= local_declarations SEMICOLON | (attr= attribute )? (n= selection_stmt | n= iteration_stmt ) | n= block_statement | n= jump_stmt SEMICOLON );", 66, 0, input);
            
            	    throw nvae_d66s0;
            }
            
            switch (alt66) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:483:3: e= lvalue_statement SEMICOLON
                    {
                    	PushFollow(FOLLOW_lvalue_statement_in_statement2477);
                    	e = lvalue_statement();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_statement2479); if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	  node=new LValueStatement(e);
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:484:3: list= local_declarations SEMICOLON
                    {
                    	PushFollow(FOLLOW_local_declarations_in_statement2487);
                    	list = local_declarations();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_statement2489); if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	  
                    	  	      if(list.Count == 1)
                    	  		node=new SingleLocalDeclarationStement(list[0]);
                    	  	     else
                    	  	     	node=new MultipleLocalDeclarationStatement(list.ToArray());
                    	  	
                    	}
                    
                    }
                    break;
                case 3 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:491:3: (attr= attribute )? (n= selection_stmt | n= iteration_stmt )
                    {
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:491:3: (attr= attribute )?
                    	int alt64 = 2;
                    	int LA64_0 = input.LA(1);
                    	
                    	if ( (LA64_0 == LBRACKET) )
                    	{
                    	    alt64 = 1;
                    	}
                    	switch (alt64) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:491:4: attr= attribute
                    	        {
                    	        	PushFollow(FOLLOW_attribute_in_statement2500);
                    	        	attr = attribute();
                    	        	followingStackPointer_--;
                    	        	if (failed) return node;
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:491:21: (n= selection_stmt | n= iteration_stmt )
                    	int alt65 = 2;
                    	int LA65_0 = input.LA(1);
                    	
                    	if ( (LA65_0 == IF) )
                    	{
                    	    alt65 = 1;
                    	}
                    	else if ( ((LA65_0 >= WHILE && LA65_0 <= FOR)) )
                    	{
                    	    alt65 = 2;
                    	}
                    	else 
                    	{
                    	    if ( backtracking > 0 ) {failed = true; return node;}
                    	    NoViableAltException nvae_d65s0 =
                    	        new NoViableAltException("491:21: (n= selection_stmt | n= iteration_stmt )", 65, 0, input);
                    	
                    	    throw nvae_d65s0;
                    	}
                    	switch (alt65) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:491:22: n= selection_stmt
                    	        {
                    	        	PushFollow(FOLLOW_selection_stmt_in_statement2508);
                    	        	n = selection_stmt();
                    	        	followingStackPointer_--;
                    	        	if (failed) return node;
                    	        	if ( backtracking == 0 ) 
                    	        	{
                    	        	   ((SelectionStatement)n).Attribute = attr; 
                    	        	}
                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:491:87: n= iteration_stmt
                    	        {
                    	        	PushFollow(FOLLOW_iteration_stmt_in_statement2515);
                    	        	n = iteration_stmt();
                    	        	followingStackPointer_--;
                    	        	if (failed) return node;
                    	        	if ( backtracking == 0 ) 
                    	        	{
                    	        	   ((LoopStatement)n).Attribute = attr; 
                    	        	}
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	if ( backtracking == 0 ) 
                    	{
                    	  node=n;
                    	}
                    
                    }
                    break;
                case 4 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:492:3: n= block_statement
                    {
                    	PushFollow(FOLLOW_block_statement_in_statement2525);
                    	n = block_statement();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	  node=n;
                    	}
                    
                    }
                    break;
                case 5 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:493:3: n= jump_stmt SEMICOLON
                    {
                    	PushFollow(FOLLOW_jump_stmt_in_statement2534);
                    	n = jump_stmt();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_statement2536); if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	  node=n;
                    	}
                    
                    }
                    break;
            
            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return node;
    }
    // $ANTLR end statement

    
    // $ANTLR start lvalue_statement
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:496:1: lvalue_statement returns [Expression node] : ( (inc= INCREMENT | t= DECREMENT ) n= lvalue | n= lvalue ( ( (t= ASSIGN | t= ADDASSIGN | t= SUBASSIGN | t= MULASSIGN | t= DIVASSIGN ) e= expression ) | t= INCREMENT | t= DECREMENT )? );
    public Expression lvalue_statement() // throws RecognitionException [1]
    {   

        Expression node = null;
    
        IToken inc = null;
        IToken t = null;
        Expression n = null;

        Expression e = null;
        
    
         AssignOp op = default(AssignOp); 
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:498:2: ( (inc= INCREMENT | t= DECREMENT ) n= lvalue | n= lvalue ( ( (t= ASSIGN | t= ADDASSIGN | t= SUBASSIGN | t= MULASSIGN | t= DIVASSIGN ) e= expression ) | t= INCREMENT | t= DECREMENT )? )
            int alt70 = 2;
            int LA70_0 = input.LA(1);
            
            if ( (LA70_0 == INCREMENT || LA70_0 == DECREMENT) )
            {
                alt70 = 1;
            }
            else if ( (LA70_0 == ID) )
            {
                alt70 = 2;
            }
            else 
            {
                if ( backtracking > 0 ) {failed = true; return node;}
                NoViableAltException nvae_d70s0 =
                    new NoViableAltException("496:1: lvalue_statement returns [Expression node] : ( (inc= INCREMENT | t= DECREMENT ) n= lvalue | n= lvalue ( ( (t= ASSIGN | t= ADDASSIGN | t= SUBASSIGN | t= MULASSIGN | t= DIVASSIGN ) e= expression ) | t= INCREMENT | t= DECREMENT )? );", 70, 0, input);
            
                throw nvae_d70s0;
            }
            switch (alt70) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:499:2: (inc= INCREMENT | t= DECREMENT ) n= lvalue
                    {
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:499:2: (inc= INCREMENT | t= DECREMENT )
                    	int alt67 = 2;
                    	int LA67_0 = input.LA(1);
                    	
                    	if ( (LA67_0 == INCREMENT) )
                    	{
                    	    alt67 = 1;
                    	}
                    	else if ( (LA67_0 == DECREMENT) )
                    	{
                    	    alt67 = 2;
                    	}
                    	else 
                    	{
                    	    if ( backtracking > 0 ) {failed = true; return node;}
                    	    NoViableAltException nvae_d67s0 =
                    	        new NoViableAltException("499:2: (inc= INCREMENT | t= DECREMENT )", 67, 0, input);
                    	
                    	    throw nvae_d67s0;
                    	}
                    	switch (alt67) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:499:3: inc= INCREMENT
                    	        {
                    	        	inc = (IToken)input.LT(1);
                    	        	Match(input,INCREMENT,FOLLOW_INCREMENT_in_lvalue_statement2562); if (failed) return node;
                    	        	if ( backtracking == 0 ) 
                    	        	{
                    	        	  t=inc;
                    	        	}
                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:499:27: t= DECREMENT
                    	        {
                    	        	t = (IToken)input.LT(1);
                    	        	Match(input,DECREMENT,FOLLOW_DECREMENT_in_lvalue_statement2569); if (failed) return node;
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	PushFollow(FOLLOW_lvalue_in_lvalue_statement2574);
                    	n = lvalue();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	   node = new UnaryExpression(n, inc!=null? UnaryOperator.PreInc: UnaryOperator.PreDec ){Line = t.Line, Column = t.CharPositionInLine}; 
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:500:4: n= lvalue ( ( (t= ASSIGN | t= ADDASSIGN | t= SUBASSIGN | t= MULASSIGN | t= DIVASSIGN ) e= expression ) | t= INCREMENT | t= DECREMENT )?
                    {
                    	PushFollow(FOLLOW_lvalue_in_lvalue_statement2582);
                    	n = lvalue();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	  node = n;
                    	}
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:500:24: ( ( (t= ASSIGN | t= ADDASSIGN | t= SUBASSIGN | t= MULASSIGN | t= DIVASSIGN ) e= expression ) | t= INCREMENT | t= DECREMENT )?
                    	int alt69 = 4;
                    	switch ( input.LA(1) ) 
                    	{
                    	    case ASSIGN:
                    	    case ADDASSIGN:
                    	    case SUBASSIGN:
                    	    case MULASSIGN:
                    	    case DIVASSIGN:
                    	    	{
                    	        alt69 = 1;
                    	        }
                    	        break;
                    	    case INCREMENT:
                    	    	{
                    	        alt69 = 2;
                    	        }
                    	        break;
                    	    case DECREMENT:
                    	    	{
                    	        alt69 = 3;
                    	        }
                    	        break;
                    	}
                    	
                    	switch (alt69) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:500:25: ( (t= ASSIGN | t= ADDASSIGN | t= SUBASSIGN | t= MULASSIGN | t= DIVASSIGN ) e= expression )
                    	        {
                    	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:500:25: ( (t= ASSIGN | t= ADDASSIGN | t= SUBASSIGN | t= MULASSIGN | t= DIVASSIGN ) e= expression )
                    	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:500:26: (t= ASSIGN | t= ADDASSIGN | t= SUBASSIGN | t= MULASSIGN | t= DIVASSIGN ) e= expression
                    	        	{
                    	        		// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:500:26: (t= ASSIGN | t= ADDASSIGN | t= SUBASSIGN | t= MULASSIGN | t= DIVASSIGN )
                    	        		int alt68 = 5;
                    	        		switch ( input.LA(1) ) 
                    	        		{
                    	        		case ASSIGN:
                    	        			{
                    	        		    alt68 = 1;
                    	        		    }
                    	        		    break;
                    	        		case ADDASSIGN:
                    	        			{
                    	        		    alt68 = 2;
                    	        		    }
                    	        		    break;
                    	        		case SUBASSIGN:
                    	        			{
                    	        		    alt68 = 3;
                    	        		    }
                    	        		    break;
                    	        		case MULASSIGN:
                    	        			{
                    	        		    alt68 = 4;
                    	        		    }
                    	        		    break;
                    	        		case DIVASSIGN:
                    	        			{
                    	        		    alt68 = 5;
                    	        		    }
                    	        		    break;
                    	        			default:
                    	        			    if ( backtracking > 0 ) {failed = true; return node;}
                    	        			    NoViableAltException nvae_d68s0 =
                    	        			        new NoViableAltException("500:26: (t= ASSIGN | t= ADDASSIGN | t= SUBASSIGN | t= MULASSIGN | t= DIVASSIGN )", 68, 0, input);
                    	        		
                    	        			    throw nvae_d68s0;
                    	        		}
                    	        		
                    	        		switch (alt68) 
                    	        		{
                    	        		    case 1 :
                    	        		        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:500:27: t= ASSIGN
                    	        		        {
                    	        		        	t = (IToken)input.LT(1);
                    	        		        	Match(input,ASSIGN,FOLLOW_ASSIGN_in_lvalue_statement2590); if (failed) return node;
                    	        		        	if ( backtracking == 0 ) 
                    	        		        	{
                    	        		        	   op = AssignOp.Assign;
                    	        		        	}
                    	        		        
                    	        		        }
                    	        		        break;
                    	        		    case 2 :
                    	        		        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:500:61: t= ADDASSIGN
                    	        		        {
                    	        		        	t = (IToken)input.LT(1);
                    	        		        	Match(input,ADDASSIGN,FOLLOW_ADDASSIGN_in_lvalue_statement2596); if (failed) return node;
                    	        		        	if ( backtracking == 0 ) 
                    	        		        	{
                    	        		        	   op = AssignOp.AddAssign;
                    	        		        	}
                    	        		        
                    	        		        }
                    	        		        break;
                    	        		    case 3 :
                    	        		        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:500:103: t= SUBASSIGN
                    	        		        {
                    	        		        	t = (IToken)input.LT(1);
                    	        		        	Match(input,SUBASSIGN,FOLLOW_SUBASSIGN_in_lvalue_statement2604); if (failed) return node;
                    	        		        	if ( backtracking == 0 ) 
                    	        		        	{
                    	        		        	   op = AssignOp.SubAssign;
                    	        		        	}
                    	        		        
                    	        		        }
                    	        		        break;
                    	        		    case 4 :
                    	        		        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:500:146: t= MULASSIGN
                    	        		        {
                    	        		        	t = (IToken)input.LT(1);
                    	        		        	Match(input,MULASSIGN,FOLLOW_MULASSIGN_in_lvalue_statement2613); if (failed) return node;
                    	        		        	if ( backtracking == 0 ) 
                    	        		        	{
                    	        		        	   op = AssignOp.MulAssign;
                    	        		        	}
                    	        		        
                    	        		        }
                    	        		        break;
                    	        		    case 5 :
                    	        		        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:500:188: t= DIVASSIGN
                    	        		        {
                    	        		        	t = (IToken)input.LT(1);
                    	        		        	Match(input,DIVASSIGN,FOLLOW_DIVASSIGN_in_lvalue_statement2621); if (failed) return node;
                    	        		        	if ( backtracking == 0 ) 
                    	        		        	{
                    	        		        	   op = AssignOp.DivAssign;
                    	        		        	}
                    	        		        
                    	        		        }
                    	        		        break;
                    	        		
                    	        		}

                    	        		PushFollow(FOLLOW_expression_in_lvalue_statement2632);
                    	        		e = expression();
                    	        		followingStackPointer_--;
                    	        		if (failed) return node;
                    	        		if ( backtracking == 0 ) 
                    	        		{
                    	        		  
                    	        		  	    	     	node = new LValueAssign(op, n, e){Line = t.Line, Column = t.CharPositionInLine};
                    	        		  	    	     
                    	        		}
                    	        	
                    	        	}

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:504:9: t= INCREMENT
                    	        {
                    	        	t = (IToken)input.LT(1);
                    	        	Match(input,INCREMENT,FOLLOW_INCREMENT_in_lvalue_statement2646); if (failed) return node;
                    	        	if ( backtracking == 0 ) 
                    	        	{
                    	        	   node = new UnaryExpression(n, UnaryOperator.PostInc){Line = t.Line, Column = t.CharPositionInLine};
                    	        	}
                    	        
                    	        }
                    	        break;
                    	    case 3 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:505:12: t= DECREMENT
                    	        {
                    	        	t = (IToken)input.LT(1);
                    	        	Match(input,DECREMENT,FOLLOW_DECREMENT_in_lvalue_statement2664); if (failed) return node;
                    	        	if ( backtracking == 0 ) 
                    	        	{
                    	        	   node = new UnaryExpression(n, UnaryOperator.PostDec){Line = t.Line, Column = t.CharPositionInLine};
                    	        	}
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    
                    }
                    break;
            
            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return node;
    }
    // $ANTLR end lvalue_statement

    
    // $ANTLR start local_declarations
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:508:1: local_declarations returns [List<LocalDeclaration> list = new List<LocalDeclaration>()] : (c= CONST )? type= type_ref l= local_declaration ( COMMA l= local_declaration )* ;
    public List<LocalDeclaration> local_declarations() // throws RecognitionException [1]
    {   

        List<LocalDeclaration> list =  new List<LocalDeclaration>();
    
        IToken c = null;
        TypeRef type = null;

        LocalDeclaration l = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:509:2: ( (c= CONST )? type= type_ref l= local_declaration ( COMMA l= local_declaration )* )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:509:3: (c= CONST )? type= type_ref l= local_declaration ( COMMA l= local_declaration )*
            {
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:509:3: (c= CONST )?
            	int alt71 = 2;
            	int LA71_0 = input.LA(1);
            	
            	if ( (LA71_0 == CONST) )
            	{
            	    alt71 = 1;
            	}
            	switch (alt71) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:509:4: c= CONST
            	        {
            	        	c = (IToken)input.LT(1);
            	        	Match(input,CONST,FOLLOW_CONST_in_local_declarations2687); if (failed) return list;
            	        
            	        }
            	        break;
            	
            	}

            	PushFollow(FOLLOW_type_ref_in_local_declarations2693);
            	type = type_ref();
            	followingStackPointer_--;
            	if (failed) return list;
            	PushFollow(FOLLOW_local_declaration_in_local_declarations2697);
            	l = local_declaration();
            	followingStackPointer_--;
            	if (failed) return list;
            	if ( backtracking == 0 ) 
            	{
            	   
            	  					l.TypeInfo.Name = type.Name;
            	  					l.TypeInfo.GenericArgs = type.GenericArgs;
            	  					l.Storage=(c!=null)?VarStorage.Const:VarStorage.Undefined;
            	  					list.Add(l);
            	  		      		
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:515:3: ( COMMA l= local_declaration )*
            	do 
            	{
            	    int alt72 = 2;
            	    int LA72_0 = input.LA(1);
            	    
            	    if ( (LA72_0 == COMMA) )
            	    {
            	        alt72 = 1;
            	    }
            	    
            	
            	    switch (alt72) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:515:4: COMMA l= local_declaration
            			    {
            			    	Match(input,COMMA,FOLLOW_COMMA_in_local_declarations2704); if (failed) return list;
            			    	PushFollow(FOLLOW_local_declaration_in_local_declarations2708);
            			    	l = local_declaration();
            			    	followingStackPointer_--;
            			    	if (failed) return list;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	   l.TypeInfo.Name = type.Name;
            			    	  				      l.TypeInfo.GenericArgs = type.GenericArgs;
            			    	  				      l.Storage=(c!=null)?VarStorage.Const:VarStorage.Undefined;
            			    	  				      list.Add(l);
            			    	  				
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop72;
            	    }
            	} while (true);
            	
            	loop72:
            		;	// Stops C# compiler whinging that label 'loop72' has no statements

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return list;
    }
    // $ANTLR end local_declarations

    
    // $ANTLR start local_declaration
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:524:1: local_declaration returns [LocalDeclaration n = new LocalDeclaration()] : id= ID (elements= fixed_array )? ( ASSIGN e= expression )? ;
    public LocalDeclaration local_declaration() // throws RecognitionException [1]
    {   

        LocalDeclaration n =  new LocalDeclaration();
    
        IToken id = null;
        int elements = 0;

        Expression e = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:525:2: (id= ID (elements= fixed_array )? ( ASSIGN e= expression )? )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:525:4: id= ID (elements= fixed_array )? ( ASSIGN e= expression )?
            {
            	id = (IToken)input.LT(1);
            	Match(input,ID,FOLLOW_ID_in_local_declaration2737); if (failed) return n;
            	if ( backtracking == 0 ) 
            	{
            	  n.Name = id.Text;
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:525:29: (elements= fixed_array )?
            	int alt73 = 2;
            	int LA73_0 = input.LA(1);
            	
            	if ( (LA73_0 == LBRACKET) )
            	{
            	    alt73 = 1;
            	}
            	switch (alt73) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:525:30: elements= fixed_array
            	        {
            	        	PushFollow(FOLLOW_fixed_array_in_local_declaration2743);
            	        	elements = fixed_array();
            	        	followingStackPointer_--;
            	        	if (failed) return n;
            	        
            	        }
            	        break;
            	
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:525:53: ( ASSIGN e= expression )?
            	int alt74 = 2;
            	int LA74_0 = input.LA(1);
            	
            	if ( (LA74_0 == ASSIGN) )
            	{
            	    alt74 = 1;
            	}
            	switch (alt74) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:525:54: ASSIGN e= expression
            	        {
            	        	Match(input,ASSIGN,FOLLOW_ASSIGN_in_local_declaration2748); if (failed) return n;
            	        	PushFollow(FOLLOW_expression_in_local_declaration2752);
            	        	e = expression();
            	        	followingStackPointer_--;
            	        	if (failed) return n;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	   n.Initializer = e; 
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	if ( backtracking == 0 ) 
            	{
            	  
            	  		n.Line = id.Line;
            	  		n.Column = id.CharPositionInLine;
            	  		n.TypeInfo = new TypeRef(null, elements, null);
            	  	
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return n;
    }
    // $ANTLR end local_declaration

    
    // $ANTLR start selection_stmt
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:535:1: selection_stmt returns [SelectionStatement n] : t= IF LPAREN c= expression RPAREN v= statement ( ( ELSE statement )=> ELSE f= statement )? ;
    public SelectionStatement selection_stmt() // throws RecognitionException [1]
    {   

        SelectionStatement n = null;
    
        IToken t = null;
        Expression c = null;

        Statement v = null;

        Statement f = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:536:3: (t= IF LPAREN c= expression RPAREN v= statement ( ( ELSE statement )=> ELSE f= statement )? )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:536:6: t= IF LPAREN c= expression RPAREN v= statement ( ( ELSE statement )=> ELSE f= statement )?
            {
            	t = (IToken)input.LT(1);
            	Match(input,IF,FOLLOW_IF_in_selection_stmt2779); if (failed) return n;
            	Match(input,LPAREN,FOLLOW_LPAREN_in_selection_stmt2781); if (failed) return n;
            	PushFollow(FOLLOW_expression_in_selection_stmt2785);
            	c = expression();
            	followingStackPointer_--;
            	if (failed) return n;
            	Match(input,RPAREN,FOLLOW_RPAREN_in_selection_stmt2787); if (failed) return n;
            	PushFollow(FOLLOW_statement_in_selection_stmt2791);
            	v = statement();
            	followingStackPointer_--;
            	if (failed) return n;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:537:11: ( ( ELSE statement )=> ELSE f= statement )?
            	int alt75 = 2;
            	int LA75_0 = input.LA(1);
            	
            	if ( (LA75_0 == ELSE) )
            	{
            	    int LA75_1 = input.LA(2);
            	    
            	    if ( (synpred1()) )
            	    {
            	        alt75 = 1;
            	    }
            	}
            	switch (alt75) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:537:12: ( ELSE statement )=> ELSE f= statement
            	        {
            	        	Match(input,ELSE,FOLLOW_ELSE_in_selection_stmt2820); if (failed) return n;
            	        	PushFollow(FOLLOW_statement_in_selection_stmt2824);
            	        	f = statement();
            	        	followingStackPointer_--;
            	        	if (failed) return n;
            	        
            	        }
            	        break;
            	
            	}

            	if ( backtracking == 0 ) 
            	{
            	   n = new SelectionStatement(c,v,f){Line = t.Line, Column = t.CharPositionInLine}; 
            	}
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return n;
    }
    // $ANTLR end selection_stmt

    
    // $ANTLR start iteration_stmt
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:540:1: iteration_stmt returns [LoopStatement n] : (t= WHILE LPAREN c= expression RPAREN b= statement | t= DO b= statement WHILE LPAREN c= expression RPAREN | t= FOR LPAREN (ini= for_ini )? SEMICOLON c= expression SEMICOLON (list= lvalue_statements )? RPAREN b= statement );
    public LoopStatement iteration_stmt() // throws RecognitionException [1]
    {   

        LoopStatement n = null;
    
        IToken t = null;
        Expression c = null;

        Statement b = null;

        List<ASTNode> ini = null;

        List<ASTNode> list = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:541:2: (t= WHILE LPAREN c= expression RPAREN b= statement | t= DO b= statement WHILE LPAREN c= expression RPAREN | t= FOR LPAREN (ini= for_ini )? SEMICOLON c= expression SEMICOLON (list= lvalue_statements )? RPAREN b= statement )
            int alt78 = 3;
            switch ( input.LA(1) ) 
            {
            case WHILE:
            	{
                alt78 = 1;
                }
                break;
            case DO:
            	{
                alt78 = 2;
                }
                break;
            case FOR:
            	{
                alt78 = 3;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return n;}
            	    NoViableAltException nvae_d78s0 =
            	        new NoViableAltException("540:1: iteration_stmt returns [LoopStatement n] : (t= WHILE LPAREN c= expression RPAREN b= statement | t= DO b= statement WHILE LPAREN c= expression RPAREN | t= FOR LPAREN (ini= for_ini )? SEMICOLON c= expression SEMICOLON (list= lvalue_statements )? RPAREN b= statement );", 78, 0, input);
            
            	    throw nvae_d78s0;
            }
            
            switch (alt78) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:542:2: t= WHILE LPAREN c= expression RPAREN b= statement
                    {
                    	t = (IToken)input.LT(1);
                    	Match(input,WHILE,FOLLOW_WHILE_in_iteration_stmt2846); if (failed) return n;
                    	Match(input,LPAREN,FOLLOW_LPAREN_in_iteration_stmt2848); if (failed) return n;
                    	PushFollow(FOLLOW_expression_in_iteration_stmt2852);
                    	c = expression();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	Match(input,RPAREN,FOLLOW_RPAREN_in_iteration_stmt2854); if (failed) return n;
                    	PushFollow(FOLLOW_statement_in_iteration_stmt2858);
                    	b = statement();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	if ( backtracking == 0 ) 
                    	{
                    	   n = new WhileStatement{ Condition=c, Body = b, Line = t.Line, Column = t.CharPositionInLine };
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:543:3: t= DO b= statement WHILE LPAREN c= expression RPAREN
                    {
                    	t = (IToken)input.LT(1);
                    	Match(input,DO,FOLLOW_DO_in_iteration_stmt2866); if (failed) return n;
                    	PushFollow(FOLLOW_statement_in_iteration_stmt2870);
                    	b = statement();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	Match(input,WHILE,FOLLOW_WHILE_in_iteration_stmt2872); if (failed) return n;
                    	Match(input,LPAREN,FOLLOW_LPAREN_in_iteration_stmt2874); if (failed) return n;
                    	PushFollow(FOLLOW_expression_in_iteration_stmt2878);
                    	c = expression();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	Match(input,RPAREN,FOLLOW_RPAREN_in_iteration_stmt2880); if (failed) return n;
                    	if ( backtracking == 0 ) 
                    	{
                    	   n = new DoWhileStatement{ Condition=c, Body = b ,Line = t.Line, Column = t.CharPositionInLine };
                    	}
                    
                    }
                    break;
                case 3 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:544:3: t= FOR LPAREN (ini= for_ini )? SEMICOLON c= expression SEMICOLON (list= lvalue_statements )? RPAREN b= statement
                    {
                    	t = (IToken)input.LT(1);
                    	Match(input,FOR,FOLLOW_FOR_in_iteration_stmt2888); if (failed) return n;
                    	Match(input,LPAREN,FOLLOW_LPAREN_in_iteration_stmt2890); if (failed) return n;
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:544:16: (ini= for_ini )?
                    	int alt76 = 2;
                    	int LA76_0 = input.LA(1);
                    	
                    	if ( (LA76_0 == INCREMENT || LA76_0 == DECREMENT || LA76_0 == CONST || LA76_0 == ID) )
                    	{
                    	    alt76 = 1;
                    	}
                    	switch (alt76) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:544:17: ini= for_ini
                    	        {
                    	        	PushFollow(FOLLOW_for_ini_in_iteration_stmt2895);
                    	        	ini = for_ini();
                    	        	followingStackPointer_--;
                    	        	if (failed) return n;
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_iteration_stmt2899); if (failed) return n;
                    	PushFollow(FOLLOW_expression_in_iteration_stmt2903);
                    	c = expression();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_iteration_stmt2905); if (failed) return n;
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:544:64: (list= lvalue_statements )?
                    	int alt77 = 2;
                    	int LA77_0 = input.LA(1);
                    	
                    	if ( (LA77_0 == INCREMENT || LA77_0 == DECREMENT || LA77_0 == ID) )
                    	{
                    	    alt77 = 1;
                    	}
                    	switch (alt77) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:544:65: list= lvalue_statements
                    	        {
                    	        	PushFollow(FOLLOW_lvalue_statements_in_iteration_stmt2910);
                    	        	list = lvalue_statements();
                    	        	followingStackPointer_--;
                    	        	if (failed) return n;
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	Match(input,RPAREN,FOLLOW_RPAREN_in_iteration_stmt2914); if (failed) return n;
                    	PushFollow(FOLLOW_statement_in_iteration_stmt2918);
                    	b = statement();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	if ( backtracking == 0 ) 
                    	{
                    	  
                    	  		n = new ForStatement{
                    	  			Initializer = ini.ToArray(),
                    	  			Increment = list.ToArray(),
                    	  			Condition=c, 
                    	  			Body = b,
                    	  			Line = t.Line,
                    	  		        Column = t.CharPositionInLine};
                    	  	
                    	}
                    
                    }
                    break;
            
            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return n;
    }
    // $ANTLR end iteration_stmt

    
    // $ANTLR start lvalue_statements
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:556:1: lvalue_statements returns [List<ASTNode> list = new List<ASTNode>()] : n= lvalue_statement ( COMMA n= lvalue_statement )* ;
    public List<ASTNode> lvalue_statements() // throws RecognitionException [1]
    {   

        List<ASTNode> list =  new List<ASTNode>();
    
        Expression n = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:557:2: (n= lvalue_statement ( COMMA n= lvalue_statement )* )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:557:4: n= lvalue_statement ( COMMA n= lvalue_statement )*
            {
            	PushFollow(FOLLOW_lvalue_statement_in_lvalue_statements2938);
            	n = lvalue_statement();
            	followingStackPointer_--;
            	if (failed) return list;
            	if ( backtracking == 0 ) 
            	{
            	  list.Add(n);
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:557:37: ( COMMA n= lvalue_statement )*
            	do 
            	{
            	    int alt79 = 2;
            	    int LA79_0 = input.LA(1);
            	    
            	    if ( (LA79_0 == COMMA) )
            	    {
            	        alt79 = 1;
            	    }
            	    
            	
            	    switch (alt79) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:557:38: COMMA n= lvalue_statement
            			    {
            			    	Match(input,COMMA,FOLLOW_COMMA_in_lvalue_statements2942); if (failed) return list;
            			    	PushFollow(FOLLOW_lvalue_statement_in_lvalue_statements2946);
            			    	n = lvalue_statement();
            			    	followingStackPointer_--;
            			    	if (failed) return list;
            			    	if ( backtracking == 0 ) 
            			    	{
            			    	  list.Add(n);
            			    	}
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop79;
            	    }
            	} while (true);
            	
            	loop79:
            		;	// Stops C# compiler whinging that label 'loop79' has no statements

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return list;
    }
    // $ANTLR end lvalue_statements

    
    // $ANTLR start for_ini
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:559:1: for_ini returns [List<ASTNode> node] : (l= local_declarations | n= lvalue_statements );
    public List<ASTNode> for_ini() // throws RecognitionException [1]
    {   

        List<ASTNode> node = null;
    
        List<LocalDeclaration> l = null;

        List<ASTNode> n = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:560:2: (l= local_declarations | n= lvalue_statements )
            int alt80 = 2;
            switch ( input.LA(1) ) 
            {
            case CONST:
            	{
                alt80 = 1;
                }
                break;
            case ID:
            	{
                int LA80_2 = input.LA(2);
                
                if ( (LA80_2 == LESS || LA80_2 == ID) )
                {
                    alt80 = 1;
                }
                else if ( (LA80_2 == INCREMENT || LA80_2 == DECREMENT || (LA80_2 >= DOT && LA80_2 <= SEMICOLON) || LA80_2 == LBRACKET || LA80_2 == LPAREN || (LA80_2 >= ASSIGN && LA80_2 <= DIVASSIGN)) )
                {
                    alt80 = 2;
                }
                else 
                {
                    if ( backtracking > 0 ) {failed = true; return node;}
                    NoViableAltException nvae_d80s2 =
                        new NoViableAltException("559:1: for_ini returns [List<ASTNode> node] : (l= local_declarations | n= lvalue_statements );", 80, 2, input);
                
                    throw nvae_d80s2;
                }
                }
                break;
            case INCREMENT:
            case DECREMENT:
            	{
                alt80 = 2;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return node;}
            	    NoViableAltException nvae_d80s0 =
            	        new NoViableAltException("559:1: for_ini returns [List<ASTNode> node] : (l= local_declarations | n= lvalue_statements );", 80, 0, input);
            
            	    throw nvae_d80s0;
            }
            
            switch (alt80) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:561:4: l= local_declarations
                    {
                    	PushFollow(FOLLOW_local_declarations_in_for_ini2969);
                    	l = local_declarations();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	   
                    	  	  	node = new List<ASTNode>();
                    	  	  	foreach (var item in l)
                    	  	  		node.Add(item);
                    	  	  
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:567:4: n= lvalue_statements
                    {
                    	PushFollow(FOLLOW_lvalue_statements_in_for_ini2981);
                    	n = lvalue_statements();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	   node = n;
                    	}
                    
                    }
                    break;
            
            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return node;
    }
    // $ANTLR end for_ini

    
    // $ANTLR start jump_stmt
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:570:1: jump_stmt returns [Statement node] : (t= BREAK | t= CONTINUE | t= DISCARD | t= RETURN (e= expression )? );
    public Statement jump_stmt() // throws RecognitionException [1]
    {   

        Statement node = null;
    
        IToken t = null;
        Expression e = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:571:2: (t= BREAK | t= CONTINUE | t= DISCARD | t= RETURN (e= expression )? )
            int alt82 = 4;
            switch ( input.LA(1) ) 
            {
            case BREAK:
            	{
                alt82 = 1;
                }
                break;
            case CONTINUE:
            	{
                alt82 = 2;
                }
                break;
            case DISCARD:
            	{
                alt82 = 3;
                }
                break;
            case RETURN:
            	{
                alt82 = 4;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return node;}
            	    NoViableAltException nvae_d82s0 =
            	        new NoViableAltException("570:1: jump_stmt returns [Statement node] : (t= BREAK | t= CONTINUE | t= DISCARD | t= RETURN (e= expression )? );", 82, 0, input);
            
            	    throw nvae_d82s0;
            }
            
            switch (alt82) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:571:4: t= BREAK
                    {
                    	t = (IToken)input.LT(1);
                    	Match(input,BREAK,FOLLOW_BREAK_in_jump_stmt2999); if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	  node = new JumpStatement{ Jump = JumpType.Break, Line =t.Line, Column = t.CharPositionInLine };
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:572:4: t= CONTINUE
                    {
                    	t = (IToken)input.LT(1);
                    	Match(input,CONTINUE,FOLLOW_CONTINUE_in_jump_stmt3008); if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	  node = new JumpStatement{ Jump = JumpType.Continue, Line =t.Line, Column = t.CharPositionInLine };
                    	}
                    
                    }
                    break;
                case 3 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:573:4: t= DISCARD
                    {
                    	t = (IToken)input.LT(1);
                    	Match(input,DISCARD,FOLLOW_DISCARD_in_jump_stmt3017); if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	  node = new JumpStatement{ Jump = JumpType.Discard, Line =t.Line, Column = t.CharPositionInLine };
                    	}
                    
                    }
                    break;
                case 4 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:574:4: t= RETURN (e= expression )?
                    {
                    	t = (IToken)input.LT(1);
                    	Match(input,RETURN,FOLLOW_RETURN_in_jump_stmt3027); if (failed) return node;
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:574:13: (e= expression )?
                    	int alt81 = 2;
                    	int LA81_0 = input.LA(1);
                    	
                    	if ( (LA81_0 == NOT || (LA81_0 >= INCREMENT && LA81_0 <= DECREMENT) || LA81_0 == LPAREN || LA81_0 == LBRACE || (LA81_0 >= ID && LA81_0 <= NUMBER) || LA81_0 == BOOL_CONSTANT) )
                    	{
                    	    alt81 = 1;
                    	}
                    	switch (alt81) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:574:14: e= expression
                    	        {
                    	        	PushFollow(FOLLOW_expression_in_jump_stmt3032);
                    	        	e = expression();
                    	        	followingStackPointer_--;
                    	        	if (failed) return node;
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	if ( backtracking == 0 ) 
                    	{
                    	  node = new JumpStatement{ Jump = JumpType.Return, ReturnExp = e, Line =t.Line, Column = t.CharPositionInLine };
                    	}
                    
                    }
                    break;
            
            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return node;
    }
    // $ANTLR end jump_stmt

    // $ANTLR start synpred1
    public void synpred1_fragment() //throws RecognitionException
    {   
        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:537:12: ( ELSE statement )
        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:537:13: ELSE statement
        {
        	Match(input,ELSE,FOLLOW_ELSE_in_synpred12815); if (failed) return ;
        	PushFollow(FOLLOW_statement_in_synpred12817);
        	statement();
        	followingStackPointer_--;
        	if (failed) return ;
        
        }
    }
    // $ANTLR end synpred1

   	public bool synpred1() 
   	{
   	    backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred1_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !failed;
   	    input.Rewind(start);
   	    backtracking--;
   	    failed = false;
   	    return success;
   	}


	private void InitializeCyclicDFAs()
	{
	}

 

    public static readonly BitSet FOLLOW_declaration_in_program588 = new BitSet(new ulong[]{0x0E40C00001000000UL});
    public static readonly BitSet FOLLOW_declaration_in_program595 = new BitSet(new ulong[]{0x0E40C00001000000UL});
    public static readonly BitSet FOLLOW_EOF_in_program607 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_UNIFORM_in_declaration627 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_STATIC_in_declaration633 = new BitSet(new ulong[]{0x0800200000000000UL});
    public static readonly BitSet FOLLOW_CONST_in_declaration638 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_type_ref_in_declaration645 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_ID_in_declaration649 = new BitSet(new ulong[]{0x0000000041C00000UL});
    public static readonly BitSet FOLLOW_variable_declaration_in_declaration653 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_attributes_in_declaration668 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_type_ref_in_declaration672 = new BitSet(new ulong[]{0x0800000001000000UL});
    public static readonly BitSet FOLLOW_fixed_array_in_declaration677 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_ID_in_declaration684 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_function_declaration_in_declaration688 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_type_ref_in_declaration701 = new BitSet(new ulong[]{0x0800000001000000UL});
    public static readonly BitSet FOLLOW_ID_in_declaration706 = new BitSet(new ulong[]{0x0000000045C00000UL});
    public static readonly BitSet FOLLOW_variable_declaration_in_declaration711 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_declaration_in_declaration728 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_fixed_array_in_declaration744 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_ID_in_declaration748 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_function_declaration_in_declaration753 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_struct_declaration_in_declaration769 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_cbuffer_declaration_in_declaration782 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_attribute_in_attributes802 = new BitSet(new ulong[]{0x0000000001000002UL});
    public static readonly BitSet FOLLOW_fixed_array_in_variable_declaration826 = new BitSet(new ulong[]{0x0000000040C00000UL});
    public static readonly BitSet FOLLOW_semantic_in_variable_declaration838 = new BitSet(new ulong[]{0x0000000040C00000UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_variable_declaration848 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_variable_declaration852 = new BitSet(new ulong[]{0x0000000000C00000UL});
    public static readonly BitSet FOLLOW_packoffset_modifier_in_variable_declaration864 = new BitSet(new ulong[]{0x0000000000C00000UL});
    public static readonly BitSet FOLLOW_register_modifier_in_variable_declaration875 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_variable_declaration883 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LBRACKET_in_fixed_array898 = new BitSet(new ulong[]{0x1000000000000000UL});
    public static readonly BitSet FOLLOW_NUMBER_in_fixed_array902 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_RBRACKET_in_fixed_array906 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ID_in_type_ref926 = new BitSet(new ulong[]{0x0000000000000042UL});
    public static readonly BitSet FOLLOW_LESS_in_type_ref930 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_type_ref_in_type_ref934 = new BitSet(new ulong[]{0x0000000000200100UL});
    public static readonly BitSet FOLLOW_COMMA_in_type_ref939 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_type_ref_in_type_ref944 = new BitSet(new ulong[]{0x0000000000200100UL});
    public static readonly BitSet FOLLOW_GREATER_in_type_ref949 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_COLON_in_semantic967 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_ID_in_semantic969 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LBRACE_in_constructor990 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_constructor994 = new BitSet(new ulong[]{0x0000000020200000UL});
    public static readonly BitSet FOLLOW_COMMA_in_constructor998 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_constructor1002 = new BitSet(new ulong[]{0x0000000020200000UL});
    public static readonly BitSet FOLLOW_RBRACE_in_constructor1007 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_COLON_in_register_modifier1026 = new BitSet(new ulong[]{0x0080000000000000UL});
    public static readonly BitSet FOLLOW_REG_in_register_modifier1029 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_LPAREN_in_register_modifier1031 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_lvalue_in_register_modifier1035 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_register_modifier1037 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_COLON_in_packoffset_modifier1052 = new BitSet(new ulong[]{0x0100000000000000UL});
    public static readonly BitSet FOLLOW_PACK_in_packoffset_modifier1054 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_LPAREN_in_packoffset_modifier1056 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_lvalue_in_packoffset_modifier1060 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_packoffset_modifier1062 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_CBUFFER_in_cbuffer_declaration1086 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_TBUFFER_in_cbuffer_declaration1091 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_ID_in_cbuffer_declaration1102 = new BitSet(new ulong[]{0x0000000010000000UL});
    public static readonly BitSet FOLLOW_LBRACE_in_cbuffer_declaration1105 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_type_ref_in_cbuffer_declaration1120 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_ID_in_cbuffer_declaration1124 = new BitSet(new ulong[]{0x0000000041C00000UL});
    public static readonly BitSet FOLLOW_variable_declaration_in_cbuffer_declaration1128 = new BitSet(new ulong[]{0x0800000020000000UL});
    public static readonly BitSet FOLLOW_RBRACE_in_cbuffer_declaration1152 = new BitSet(new ulong[]{0x0000000000400002UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_cbuffer_declaration1154 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STRUCT_in_struct_declaration1181 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_ID_in_struct_declaration1183 = new BitSet(new ulong[]{0x0000000010000000UL});
    public static readonly BitSet FOLLOW_LBRACE_in_struct_declaration1186 = new BitSet(new ulong[]{0x0807000000000000UL});
    public static readonly BitSet FOLLOW_member_declaration_in_struct_declaration1191 = new BitSet(new ulong[]{0x0807000020000000UL});
    public static readonly BitSet FOLLOW_RBRACE_in_struct_declaration1196 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_struct_declaration1198 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_interpolation_in_member_declaration1219 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_type_ref_in_member_declaration1229 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_ID_in_member_declaration1234 = new BitSet(new ulong[]{0x0000000001C00000UL});
    public static readonly BitSet FOLLOW_fixed_array_in_member_declaration1244 = new BitSet(new ulong[]{0x0000000000C00000UL});
    public static readonly BitSet FOLLOW_semantic_in_member_declaration1252 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_member_declaration1257 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_CENTROID_in_interpolation1270 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LINEAR_in_interpolation1277 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NOINTERP_in_interpolation1285 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LPAREN_in_function_declaration1300 = new BitSet(new ulong[]{0x0838400008000000UL});
    public static readonly BitSet FOLLOW_parameters_in_function_declaration1305 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_function_declaration1310 = new BitSet(new ulong[]{0x0000000010800000UL});
    public static readonly BitSet FOLLOW_semantic_in_function_declaration1317 = new BitSet(new ulong[]{0x0000000010000000UL});
    public static readonly BitSet FOLLOW_block_statement_in_function_declaration1325 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LBRACKET_in_attribute1343 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_ID_in_attribute1345 = new BitSet(new ulong[]{0x000000000A000000UL});
    public static readonly BitSet FOLLOW_attribute_parameters_in_attribute1351 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_RBRACKET_in_attribute1355 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_RPAREN_in_attribute_parameters1372 = new BitSet(new ulong[]{0x7800000000000000UL});
    public static readonly BitSet FOLLOW_attribute_expresion_in_attribute_parameters1377 = new BitSet(new ulong[]{0x0000000004200000UL});
    public static readonly BitSet FOLLOW_COMMA_in_attribute_parameters1382 = new BitSet(new ulong[]{0x7800000000000000UL});
    public static readonly BitSet FOLLOW_attribute_expresion_in_attribute_parameters1386 = new BitSet(new ulong[]{0x0000000004200000UL});
    public static readonly BitSet FOLLOW_LPAREN_in_attribute_parameters1392 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_constant_exp_in_attribute_expresion1409 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ID_in_attribute_expresion1418 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STRING_LITERAL_in_attribute_expresion1427 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_parameter_declaration_in_parameters1444 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_COMMA_in_parameters1448 = new BitSet(new ulong[]{0x0838400000000000UL});
    public static readonly BitSet FOLLOW_parameter_declaration_in_parameters1452 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_parameter_qualifier_in_parameter_declaration1473 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_type_ref_in_parameter_declaration1484 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_ID_in_parameter_declaration1492 = new BitSet(new ulong[]{0x0000000041800002UL});
    public static readonly BitSet FOLLOW_fixed_array_in_parameter_declaration1502 = new BitSet(new ulong[]{0x0000000040800002UL});
    public static readonly BitSet FOLLOW_semantic_in_parameter_declaration1512 = new BitSet(new ulong[]{0x0000000040000002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_parameter_declaration1518 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_parameter_declaration1522 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IN_in_parameter_qualifier1539 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_OUT_in_parameter_qualifier1546 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_INOUT_in_parameter_qualifier1553 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_UNIFORM_in_parameter_qualifier1560 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LBRACE_in_block_statement1579 = new BitSet(new ulong[]{0x08003FD031014000UL});
    public static readonly BitSet FOLLOW_statement_in_block_statement1584 = new BitSet(new ulong[]{0x08003FD031014000UL});
    public static readonly BitSet FOLLOW_RBRACE_in_block_statement1589 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_or_expr_in_expression1610 = new BitSet(new ulong[]{0x0000000800000002UL});
    public static readonly BitSet FOLLOW_QUESTION_in_expression1613 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_expression1617 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COLON_in_expression1619 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_expression1623 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_xor_expr_in_or_expr1646 = new BitSet(new ulong[]{0x0000000000020002UL});
    public static readonly BitSet FOLLOW_OR_in_or_expr1651 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_xor_expr_in_or_expr1655 = new BitSet(new ulong[]{0x0000000000020002UL});
    public static readonly BitSet FOLLOW_and_expr_in_xor_expr1680 = new BitSet(new ulong[]{0x0000000000080002UL});
    public static readonly BitSet FOLLOW_XOR_in_xor_expr1685 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_and_expr_in_xor_expr1690 = new BitSet(new ulong[]{0x0000000000080002UL});
    public static readonly BitSet FOLLOW_rel_exp_in_and_expr1712 = new BitSet(new ulong[]{0x0000000000040002UL});
    public static readonly BitSet FOLLOW_AND_in_and_expr1717 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_rel_exp_in_and_expr1721 = new BitSet(new ulong[]{0x0000000000040002UL});
    public static readonly BitSet FOLLOW_add_expr_in_rel_exp1744 = new BitSet(new ulong[]{0x00000000000003F2UL});
    public static readonly BitSet FOLLOW_EQUAL_in_rel_exp1759 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_add_expr_in_rel_exp1764 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NEQUAL_in_rel_exp1779 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_add_expr_in_rel_exp1784 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LESS_in_rel_exp1801 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_add_expr_in_rel_exp1808 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LEQUAL_in_rel_exp1823 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_add_expr_in_rel_exp1828 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_GREATER_in_rel_exp1843 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_add_expr_in_rel_exp1847 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_GEQUAL_in_rel_exp1862 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_add_expr_in_rel_exp1867 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_mul_expr_in_add_expr1899 = new BitSet(new ulong[]{0x000000000000A002UL});
    public static readonly BitSet FOLLOW_ADD_in_add_expr1905 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_SUB_in_add_expr1912 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_mul_expr_in_add_expr1933 = new BitSet(new ulong[]{0x000000000000A002UL});
    public static readonly BitSet FOLLOW_unary_expr_in_mul_expr1973 = new BitSet(new ulong[]{0x0000000000001802UL});
    public static readonly BitSet FOLLOW_MUL_in_mul_expr1979 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_DIV_in_mul_expr1986 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_unary_expr_in_mul_expr1995 = new BitSet(new ulong[]{0x0000000000001802UL});
    public static readonly BitSet FOLLOW_increment_in_unary_expr2018 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NOT_in_unary_expr2026 = new BitSet(new ulong[]{0x4800000004000000UL});
    public static readonly BitSet FOLLOW_lvalue_in_unary_expr2031 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_parent_exp_in_unary_expr2037 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_BOOL_CONSTANT_in_unary_expr2046 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUB_in_unary_expr2060 = new BitSet(new ulong[]{0x5800000004000000UL});
    public static readonly BitSet FOLLOW_lvalue_in_unary_expr2065 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_parent_exp_in_unary_expr2071 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_constant_exp_in_unary_expr2078 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_parent_exp_in_unary_expr2090 = new BitSet(new ulong[]{0x5800000004000002UL});
    public static readonly BitSet FOLLOW_lvalue_in_unary_expr2098 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_parent_exp_in_unary_expr2104 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_constant_exp_in_unary_expr2110 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_constructor_in_unary_expr2123 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_constant_exp_in_unary_expr2131 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LPAREN_in_parent_exp2147 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_parent_exp2151 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_parent_exp2155 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_lvalue_in_increment2175 = new BitSet(new ulong[]{0x0000000000014002UL});
    public static readonly BitSet FOLLOW_INCREMENT_in_increment2181 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DECREMENT_in_increment2196 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_INCREMENT_in_increment2210 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_DECREMENT_in_increment2224 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_lvalue_in_increment2232 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primary_exp_in_lvalue2258 = new BitSet(new ulong[]{0x0000000001100002UL});
    public static readonly BitSet FOLLOW_DOT_in_lvalue2262 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_ID_in_lvalue2267 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_LPAREN_in_lvalue2269 = new BitSet(new ulong[]{0x580000001C01C400UL});
    public static readonly BitSet FOLLOW_argument_list_in_lvalue2274 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_lvalue2278 = new BitSet(new ulong[]{0x0000000001100002UL});
    public static readonly BitSet FOLLOW_ID_in_lvalue2291 = new BitSet(new ulong[]{0x0000000001100002UL});
    public static readonly BitSet FOLLOW_LBRACKET_in_lvalue2311 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_lvalue2315 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_RBRACKET_in_lvalue2317 = new BitSet(new ulong[]{0x0000000001100002UL});
    public static readonly BitSet FOLLOW_ID_in_primary_exp2351 = new BitSet(new ulong[]{0x0000000004000002UL});
    public static readonly BitSet FOLLOW_LPAREN_in_primary_exp2354 = new BitSet(new ulong[]{0x580000001C01C400UL});
    public static readonly BitSet FOLLOW_argument_list_in_primary_exp2359 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_primary_exp2363 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ID_in_argument_list2389 = new BitSet(new ulong[]{0x0000000040000000UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_argument_list2391 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_argument_list2397 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_COMMA_in_argument_list2406 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_ID_in_argument_list2411 = new BitSet(new ulong[]{0x0000000040000000UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_argument_list2413 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_argument_list2419 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_BOOL_CONSTANT_in_constant_exp2447 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NUMBER_in_constant_exp2458 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_lvalue_statement_in_statement2477 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_statement2479 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_local_declarations_in_statement2487 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_statement2489 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_attribute_in_statement2500 = new BitSet(new ulong[]{0x000001D000000000UL});
    public static readonly BitSet FOLLOW_selection_stmt_in_statement2508 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_iteration_stmt_in_statement2515 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_block_statement_in_statement2525 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_jump_stmt_in_statement2534 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_statement2536 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_INCREMENT_in_lvalue_statement2562 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_DECREMENT_in_lvalue_statement2569 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_lvalue_in_lvalue_statement2574 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_lvalue_in_lvalue_statement2582 = new BitSet(new ulong[]{0x00000007C0014002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_lvalue_statement2590 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_ADDASSIGN_in_lvalue_statement2596 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_SUBASSIGN_in_lvalue_statement2604 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_MULASSIGN_in_lvalue_statement2613 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_DIVASSIGN_in_lvalue_statement2621 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_lvalue_statement2632 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_INCREMENT_in_lvalue_statement2646 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DECREMENT_in_lvalue_statement2664 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_CONST_in_local_declarations2687 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_type_ref_in_local_declarations2693 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_local_declaration_in_local_declarations2697 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_COMMA_in_local_declarations2704 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FOLLOW_local_declaration_in_local_declarations2708 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_ID_in_local_declaration2737 = new BitSet(new ulong[]{0x0000000041000002UL});
    public static readonly BitSet FOLLOW_fixed_array_in_local_declaration2743 = new BitSet(new ulong[]{0x0000000040000002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_local_declaration2748 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_local_declaration2752 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IF_in_selection_stmt2779 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_LPAREN_in_selection_stmt2781 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_selection_stmt2785 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_selection_stmt2787 = new BitSet(new ulong[]{0x08003FD011014000UL});
    public static readonly BitSet FOLLOW_statement_in_selection_stmt2791 = new BitSet(new ulong[]{0x0000002000000002UL});
    public static readonly BitSet FOLLOW_ELSE_in_selection_stmt2820 = new BitSet(new ulong[]{0x08003FD011014000UL});
    public static readonly BitSet FOLLOW_statement_in_selection_stmt2824 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_WHILE_in_iteration_stmt2846 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_LPAREN_in_iteration_stmt2848 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_iteration_stmt2852 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_iteration_stmt2854 = new BitSet(new ulong[]{0x08003FD011014000UL});
    public static readonly BitSet FOLLOW_statement_in_iteration_stmt2858 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DO_in_iteration_stmt2866 = new BitSet(new ulong[]{0x08003FD011014000UL});
    public static readonly BitSet FOLLOW_statement_in_iteration_stmt2870 = new BitSet(new ulong[]{0x0000004000000000UL});
    public static readonly BitSet FOLLOW_WHILE_in_iteration_stmt2872 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_LPAREN_in_iteration_stmt2874 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_iteration_stmt2878 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_iteration_stmt2880 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FOR_in_iteration_stmt2888 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_LPAREN_in_iteration_stmt2890 = new BitSet(new ulong[]{0x0800200000414000UL});
    public static readonly BitSet FOLLOW_for_ini_in_iteration_stmt2895 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_iteration_stmt2899 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FOLLOW_expression_in_iteration_stmt2903 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_iteration_stmt2905 = new BitSet(new ulong[]{0x0800000008014000UL});
    public static readonly BitSet FOLLOW_lvalue_statements_in_iteration_stmt2910 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_iteration_stmt2914 = new BitSet(new ulong[]{0x08003FD011014000UL});
    public static readonly BitSet FOLLOW_statement_in_iteration_stmt2918 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_lvalue_statement_in_lvalue_statements2938 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_COMMA_in_lvalue_statements2942 = new BitSet(new ulong[]{0x0800000000014000UL});
    public static readonly BitSet FOLLOW_lvalue_statement_in_lvalue_statements2946 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_local_declarations_in_for_ini2969 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_lvalue_statements_in_for_ini2981 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_BREAK_in_jump_stmt2999 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_CONTINUE_in_jump_stmt3008 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DISCARD_in_jump_stmt3017 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_RETURN_in_jump_stmt3027 = new BitSet(new ulong[]{0x580000001401C402UL});
    public static readonly BitSet FOLLOW_expression_in_jump_stmt3032 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ELSE_in_synpred12815 = new BitSet(new ulong[]{0x08003FD011014000UL});
    public static readonly BitSet FOLLOW_statement_in_synpred12817 = new BitSet(new ulong[]{0x0000000000000002UL});

}
}