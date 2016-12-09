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


public class HlslParser : Parser 
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

    public const int Exponent = 71;
    public const int FloatSuffix = 72;
    public const int While = 38;
    public const int Letter = 73;
    public const int Const = 45;
    public const int Lbrace = 28;
    public const int Do = 39;
    public const int For = 40;
    public const int Sub = 15;
    public const int Uniform = 46;
    public const int Not = 10;
    public const int Id = 59;
    public const int And = 18;
    public const int Eof = -1;
    public const int Subassign = 32;
    public const int OctalDigit = 64;
    public const int Break = 41;
    public const int Lparen = 26;
    public const int Zero = 63;
    public const int If = 36;
    public const int Inout = 53;
    public const int Lbracket = 24;
    public const int Lequal = 7;
    public const int Rparen = 27;
    public const int Linear = 49;
    public const int StringLiteral = 61;
    public const int Greater = 8;
    public const int In = 51;
    public const int BoolConstant = 62;
    public const int Continue = 42;
    public const int Comma = 21;
    public const int Gequal = 9;
    public const int Equal = 4;
    public const int Less = 6;
    public const int Return = 43;
    public const int Nointerp = 50;
    public const int Digit = 66;
    public const int Rbracket = 25;
    public const int Comment = 74;
    public const int Dot = 20;
    public const int Add = 13;
    public const int Tbuffer = 58;
    public const int Nequal = 5;
    public const int Xor = 19;
    public const int Mulassign = 33;
    public const int Divassign = 34;
    public const int Rbrace = 29;
    public const int NonZeroDigit = 65;
    public const int Static = 47;
    public const int Else = 37;
    public const int Number = 60;
    public const int HexDigit = 68;
    public const int Struct = 54;
    public const int Semicolon = 22;
    public const int HexConstant = 69;
    public const int Reg = 55;
    public const int Mul = 11;
    public const int Decrement = 16;
    public const int Colon = 23;
    public const int Increment = 14;
    public const int Ws = 75;
    public const int Discard = 44;
    public const int Question = 35;
    public const int Centroid = 48;
    public const int Addassign = 31;
    public const int Out = 52;
    public const int Cbuffer = 57;
    public const int DigitSequence = 70;
    public const int Or = 17;
    public const int Assign = 30;
    public const int OctalConstant = 67;
    public const int Div = 12;
    public const int Pack = 56;
    public const int OctalEscape = 77;
    public const int EscapeSequence = 76;
    
    
        public HlslParser(ITokenStream input) 
    		: base(input)
    	{
    		InitializeCyclicDfAs();
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

    
    Scope _scope;
    int _line,_column;
    
    List<RecognitionException> _errors = new List<RecognitionException>();
    
    public Scope GlobalScope{ get { return _scope; } set{ _scope = value; }}
    public List<RecognitionException> Errors { get { return _errors; } }
    
     public override void ReportError(RecognitionException e)
     {
                _errors.Add(e);
                base.ReportError(e);
     }
    	 


    
    // $ANTLR start program
    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:103:1: program returns [ProgramFile file = new ProgramFile()] : d= declaration (d= declaration )* EOF ;
    public HlslProgram Program() // throws RecognitionException [1]
    {   

        HlslProgram file =  new HlslProgram();
    
        Declaration d = null;
        
    
         
        	List<Declaration> decs = new List<Declaration>();
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:107:2: (d= declaration (d= declaration )* EOF )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:107:3: d= declaration (d= declaration )* EOF
            {
            	PushFollow(FollowDeclarationInProgram588);
            	d = Declaration();
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
            	    int la10 = input.LA(1);
            	    
            	    if ( (la10 == Lbracket || (la10 >= Uniform && la10 <= Static) || la10 == Struct || (la10 >= Cbuffer && la10 <= Id)) )
            	    {
            	        alt1 = 1;
            	    }
            	    
            	
            	    switch (alt1) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:107:35: d= declaration
            			    {
            			    	PushFollow(FollowDeclarationInProgram595);
            			    	d = Declaration();
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
            	Match(input,Eof,FollowEofInProgram607); if (failed) return file;
            
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
    public Declaration Declaration() // throws RecognitionException [1]
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
            case Uniform:
            case Static:
            	{
                alt7 = 1;
                }
                break;
            case Lbracket:
            	{
                alt7 = 2;
                }
                break;
            case Id:
            	{
                alt7 = 3;
                }
                break;
            case Struct:
            	{
                alt7 = 4;
                }
                break;
            case Cbuffer:
            case Tbuffer:
            	{
                alt7 = 5;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return node;}
            	    NoViableAltException nvaeD7S0 =
            	        new NoViableAltException("113:1: declaration returns [Declaration node] : ( (uni= UNIFORM | st= STATIC (con= CONST )? ) type= type_ref name= ID v= variable_declaration[type] | attr= attributes type= type_ref (e= fixed_array )? name= ID f= function_declaration | type= type_ref (name= ID (v= variable_declaration[type] | f= function_declaration ) | e= fixed_array name= ID f= function_declaration ) | s= struct_declaration | cb= cbuffer_declaration );", 7, 0, input);
            
            	    throw nvaeD7S0;
            }
            
            switch (alt7) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:115:2: (uni= UNIFORM | st= STATIC (con= CONST )? ) type= type_ref name= ID v= variable_declaration[type]
                    {
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:115:2: (uni= UNIFORM | st= STATIC (con= CONST )? )
                    	int alt3 = 2;
                    	int la30 = input.LA(1);
                    	
                    	if ( (la30 == Uniform) )
                    	{
                    	    alt3 = 1;
                    	}
                    	else if ( (la30 == Static) )
                    	{
                    	    alt3 = 2;
                    	}
                    	else 
                    	{
                    	    if ( backtracking > 0 ) {failed = true; return node;}
                    	    NoViableAltException nvaeD3S0 =
                    	        new NoViableAltException("115:2: (uni= UNIFORM | st= STATIC (con= CONST )? )", 3, 0, input);
                    	
                    	    throw nvaeD3S0;
                    	}
                    	switch (alt3) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:115:4: uni= UNIFORM
                    	        {
                    	        	uni = (IToken)input.LT(1);
                    	        	Match(input,Uniform,FollowUniformInDeclaration627); if (failed) return node;
                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:115:18: st= STATIC (con= CONST )?
                    	        {
                    	        	st = (IToken)input.LT(1);
                    	        	Match(input,Static,FollowStaticInDeclaration633); if (failed) return node;
                    	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:115:28: (con= CONST )?
                    	        	int alt2 = 2;
                    	        	int la20 = input.LA(1);
                    	        	
                    	        	if ( (la20 == Const) )
                    	        	{
                    	        	    alt2 = 1;
                    	        	}
                    	        	switch (alt2) 
                    	        	{
                    	        	    case 1 :
                    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:115:29: con= CONST
                    	        	        {
                    	        	        	con = (IToken)input.LT(1);
                    	        	        	Match(input,Const,FollowConstInDeclaration638); if (failed) return node;
                    	        	        
                    	        	        }
                    	        	        break;
                    	        	
                    	        	}

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	PushFollow(FollowTypeRefInDeclaration645);
                    	type = type_ref();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	name = (IToken)input.LT(1);
                    	Match(input,Id,FollowIdInDeclaration649); if (failed) return node;
                    	PushFollow(FollowVariableDeclarationInDeclaration653);
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
                    	PushFollow(FollowAttributesInDeclaration668);
                    	attr = Attributes();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	PushFollow(FollowTypeRefInDeclaration672);
                    	type = type_ref();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:128:34: (e= fixed_array )?
                    	int alt4 = 2;
                    	int la40 = input.LA(1);
                    	
                    	if ( (la40 == Lbracket) )
                    	{
                    	    alt4 = 1;
                    	}
                    	switch (alt4) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:128:35: e= fixed_array
                    	        {
                    	        	PushFollow(FollowFixedArrayInDeclaration677);
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
                    	Match(input,Id,FollowIdInDeclaration684); if (failed) return node;
                    	PushFollow(FollowFunctionDeclarationInDeclaration688);
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
                    	PushFollow(FollowTypeRefInDeclaration701);
                    	type = type_ref();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:137:18: (name= ID (v= variable_declaration[type] | f= function_declaration ) | e= fixed_array name= ID f= function_declaration )
                    	int alt6 = 2;
                    	int la60 = input.LA(1);
                    	
                    	if ( (la60 == Id) )
                    	{
                    	    alt6 = 1;
                    	}
                    	else if ( (la60 == Lbracket) )
                    	{
                    	    alt6 = 2;
                    	}
                    	else 
                    	{
                    	    if ( backtracking > 0 ) {failed = true; return node;}
                    	    NoViableAltException nvaeD6S0 =
                    	        new NoViableAltException("137:18: (name= ID (v= variable_declaration[type] | f= function_declaration ) | e= fixed_array name= ID f= function_declaration )", 6, 0, input);
                    	
                    	    throw nvaeD6S0;
                    	}
                    	switch (alt6) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:137:19: name= ID (v= variable_declaration[type] | f= function_declaration )
                    	        {
                    	        	name = (IToken)input.LT(1);
                    	        	Match(input,Id,FollowIdInDeclaration706); if (failed) return node;
                    	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:137:27: (v= variable_declaration[type] | f= function_declaration )
                    	        	int alt5 = 2;
                    	        	int la50 = input.LA(1);
                    	        	
                    	        	if ( ((la50 >= Semicolon && la50 <= Lbracket) || la50 == Assign) )
                    	        	{
                    	        	    alt5 = 1;
                    	        	}
                    	        	else if ( (la50 == Lparen) )
                    	        	{
                    	        	    alt5 = 2;
                    	        	}
                    	        	else 
                    	        	{
                    	        	    if ( backtracking > 0 ) {failed = true; return node;}
                    	        	    NoViableAltException nvaeD5S0 =
                    	        	        new NoViableAltException("137:27: (v= variable_declaration[type] | f= function_declaration )", 5, 0, input);
                    	        	
                    	        	    throw nvaeD5S0;
                    	        	}
                    	        	switch (alt5) 
                    	        	{
                    	        	    case 1 :
                    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:137:28: v= variable_declaration[type]
                    	        	        {
                    	        	        	PushFollow(FollowVariableDeclarationInDeclaration711);
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
                    	        	        	PushFollow(FollowFunctionDeclarationInDeclaration728);
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
                    	        	PushFollow(FollowFixedArrayInDeclaration744);
                    	        	e = fixed_array();
                    	        	followingStackPointer_--;
                    	        	if (failed) return node;
                    	        	name = (IToken)input.LT(1);
                    	        	Match(input,Id,FollowIdInDeclaration748); if (failed) return node;
                    	        	PushFollow(FollowFunctionDeclarationInDeclaration753);
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
                    	PushFollow(FollowStructDeclarationInDeclaration769);
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
                    	PushFollow(FollowCbufferDeclarationInDeclaration782);
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
    public List<AttributeDeclaration> Attributes() // throws RecognitionException [1]
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
            	    int la80 = input.LA(1);
            	    
            	    if ( (la80 == Lbracket) )
            	    {
            	        alt8 = 1;
            	    }
            	    
            	
            	    switch (alt8) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:161:5: a= attribute
            			    {
            			    	PushFollow(FollowAttributeInAttributes802);
            			    	a = Attribute();
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
            	int la90 = input.LA(1);
            	
            	if ( (la90 == Lbracket) )
            	{
            	    alt9 = 1;
            	}
            	switch (alt9) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:166:4: elements= fixed_array
            	        {
            	        	PushFollow(FollowFixedArrayInVariableDeclaration826);
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
            	int la100 = input.LA(1);
            	
            	if ( (la100 == Colon) )
            	{
            	    int la101 = input.LA(2);
            	    
            	    if ( (la101 == Id) )
            	    {
            	        alt10 = 1;
            	    }
            	}
            	switch (alt10) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:167:5: sem= semantic
            	        {
            	        	PushFollow(FollowSemanticInVariableDeclaration838);
            	        	sem = Semantic();
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
            	int la110 = input.LA(1);
            	
            	if ( (la110 == Assign) )
            	{
            	    alt11 = 1;
            	}
            	switch (alt11) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:168:5: ASSIGN v= expression
            	        {
            	        	Match(input,Assign,FollowAssignInVariableDeclaration848); if (failed) return node;
            	        	PushFollow(FollowExpressionInVariableDeclaration852);
            	        	v = Expression();
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
            	int la120 = input.LA(1);
            	
            	if ( (la120 == Colon) )
            	{
            	    int la121 = input.LA(2);
            	    
            	    if ( (la121 == Pack) )
            	    {
            	        alt12 = 1;
            	    }
            	}
            	switch (alt12) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:169:5: packoffset= packoffset_modifier
            	        {
            	        	PushFollow(FollowPackoffsetModifierInVariableDeclaration864);
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
            	int la130 = input.LA(1);
            	
            	if ( (la130 == Colon) )
            	{
            	    alt13 = 1;
            	}
            	switch (alt13) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:170:5: reg= register_modifier
            	        {
            	        	PushFollow(FollowRegisterModifierInVariableDeclaration875);
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

            	Match(input,Semicolon,FollowSemicolonInVariableDeclaration883); if (failed) return node;
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
            	Match(input,Lbracket,FollowLbracketInFixedArray898); if (failed) return elements;
            	d = (IToken)input.LT(1);
            	Match(input,Number,FollowNumberInFixedArray902); if (failed) return elements;
            	if ( backtracking == 0 ) 
            	{
            	   elements = int.Parse(d.Text);  
            	}
            	Match(input,Rbracket,FollowRbracketInFixedArray906); if (failed) return elements;
            
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
            	Match(input,Id,FollowIdInTypeRef926); if (failed) return type;
            	if ( backtracking == 0 ) 
            	{
            	  type.Name = id.Text;
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:181:32: ( LESS p= type_ref ( ( COMMA ) p= type_ref )* GREATER )?
            	int alt15 = 2;
            	int la150 = input.LA(1);
            	
            	if ( (la150 == Less) )
            	{
            	    alt15 = 1;
            	}
            	switch (alt15) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:181:33: LESS p= type_ref ( ( COMMA ) p= type_ref )* GREATER
            	        {
            	        	Match(input,Less,FollowLessInTypeRef930); if (failed) return type;
            	        	PushFollow(FollowTypeRefInTypeRef934);
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
            	        	    int la140 = input.LA(1);
            	        	    
            	        	    if ( (la140 == Comma) )
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
            	        			    		Match(input,Comma,FollowCommaInTypeRef939); if (failed) return type;
            	        			    	
            	        			    	}

            	        			    	PushFollow(FollowTypeRefInTypeRef944);
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

            	        	Match(input,Greater,FollowGreaterInTypeRef949); if (failed) return type;
            	        
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
    public string Semantic() // throws RecognitionException [1]
    {   

        string s = null;
    
        IToken id1 = null;
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:189:2: ( COLON ID )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:189:4: COLON ID
            {
            	Match(input,Colon,FollowColonInSemantic967); if (failed) return s;
            	id1 = (IToken)input.LT(1);
            	Match(input,Id,FollowIdInSemantic969); if (failed) return s;
            	if ( backtracking == 0 ) 
            	{
            	   s=id1.Text; 
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
    public VariableInitializer Constructor() // throws RecognitionException [1]
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
            	Match(input,Lbrace,FollowLbraceInConstructor990); if (failed) return ini;
            	PushFollow(FollowExpressionInConstructor994);
            	e = Expression();
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
            	    int la160 = input.LA(1);
            	    
            	    if ( (la160 == Comma) )
            	    {
            	        alt16 = 1;
            	    }
            	    
            	
            	    switch (alt16) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:195:41: COMMA e= expression
            			    {
            			    	Match(input,Comma,FollowCommaInConstructor998); if (failed) return ini;
            			    	PushFollow(FollowExpressionInConstructor1002);
            			    	e = Expression();
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

            	Match(input,Rbrace,FollowRbraceInConstructor1007); if (failed) return ini;
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
    
        IToken colon2 = null;
        Expression v = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:207:2: ( COLON REG LPAREN v= lvalue RPAREN )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:207:4: COLON REG LPAREN v= lvalue RPAREN
            {
            	colon2 = (IToken)input.LT(1);
            	Match(input,Colon,FollowColonInRegisterModifier1026); if (failed) return r;
            	Match(input,Reg,FollowRegInRegisterModifier1029); if (failed) return r;
            	Match(input,Lparen,FollowLparenInRegisterModifier1031); if (failed) return r;
            	PushFollow(FollowLvalueInRegisterModifier1035);
            	v = Lvalue();
            	followingStackPointer_--;
            	if (failed) return r;
            	Match(input,Rparen,FollowRparenInRegisterModifier1037); if (failed) return r;
            	if ( backtracking == 0 ) 
            	{
            	  r = new RegisterDeclaration(v, colon2.Line, colon2.CharPositionInLine); 
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
    
        IToken colon3 = null;
        Expression v = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:210:2: ( COLON PACK LPAREN v= lvalue RPAREN )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:210:4: COLON PACK LPAREN v= lvalue RPAREN
            {
            	colon3 = (IToken)input.LT(1);
            	Match(input,Colon,FollowColonInPackoffsetModifier1052); if (failed) return p;
            	Match(input,Pack,FollowPackInPackoffsetModifier1054); if (failed) return p;
            	Match(input,Lparen,FollowLparenInPackoffsetModifier1056); if (failed) return p;
            	PushFollow(FollowLvalueInPackoffsetModifier1060);
            	v = Lvalue();
            	followingStackPointer_--;
            	if (failed) return p;
            	Match(input,Rparen,FollowRparenInPackoffsetModifier1062); if (failed) return p;
            	if ( backtracking == 0 ) 
            	{
            	  p = new PackOffsetDeclaration(v, colon3.Line, colon3.CharPositionInLine); 
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
            	int la170 = input.LA(1);
            	
            	if ( (la170 == Cbuffer) )
            	{
            	    alt17 = 1;
            	}
            	else if ( (la170 == Tbuffer) )
            	{
            	    alt17 = 2;
            	}
            	else 
            	{
            	    if ( backtracking > 0 ) {failed = true; return cb;}
            	    NoViableAltException nvaeD17S0 =
            	        new NoViableAltException("217:5: (c= CBUFFER | c= TBUFFER )", 17, 0, input);
            	
            	    throw nvaeD17S0;
            	}
            	switch (alt17) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:217:6: c= CBUFFER
            	        {
            	        	c = (IToken)input.LT(1);
            	        	Match(input,Cbuffer,FollowCbufferInCbufferDeclaration1086); if (failed) return cb;
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
            	        	Match(input,Tbuffer,FollowTbufferInCbufferDeclaration1091); if (failed) return cb;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	   cb.Type = BufferType.TBuffer; 
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	name = (IToken)input.LT(1);
            	Match(input,Id,FollowIdInCbufferDeclaration1102); if (failed) return cb;
            	if ( backtracking == 0 ) 
            	{
            	  cb.Name= name.Text;
            	}
            	Match(input,Lbrace,FollowLbraceInCbufferDeclaration1105); if (failed) return cb;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:218:42: (type= type_ref name= ID d= variable_declaration[type] )+
            	int cnt18 = 0;
            	do 
            	{
            	    int alt18 = 2;
            	    int la180 = input.LA(1);
            	    
            	    if ( (la180 == Id) )
            	    {
            	        alt18 = 1;
            	    }
            	    
            	
            	    switch (alt18) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:219:9: type= type_ref name= ID d= variable_declaration[type]
            			    {
            			    	PushFollow(FollowTypeRefInCbufferDeclaration1120);
            			    	type = type_ref();
            			    	followingStackPointer_--;
            			    	if (failed) return cb;
            			    	name = (IToken)input.LT(1);
            			    	Match(input,Id,FollowIdInCbufferDeclaration1124); if (failed) return cb;
            			    	PushFollow(FollowVariableDeclarationInCbufferDeclaration1128);
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

            	Match(input,Rbrace,FollowRbraceInCbufferDeclaration1152); if (failed) return cb;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:226:15: ( SEMICOLON )?
            	int alt19 = 2;
            	int la190 = input.LA(1);
            	
            	if ( (la190 == Semicolon) )
            	{
            	    alt19 = 1;
            	}
            	switch (alt19) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:226:15: SEMICOLON
            	        {
            	        	Match(input,Semicolon,FollowSemicolonInCbufferDeclaration1154); if (failed) return cb;
            	        
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
    
        IToken id4 = null;
        IToken struct5 = null;
        StructMemberDeclaration m = null;
        
    
        
        	List<StructMemberDeclaration> members = new List<StructMemberDeclaration>();
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:238:2: ( STRUCT ID LBRACE (m= member_declaration )+ RBRACE SEMICOLON )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:238:4: STRUCT ID LBRACE (m= member_declaration )+ RBRACE SEMICOLON
            {
            	struct5 = (IToken)input.LT(1);
            	Match(input,Struct,FollowStructInStructDeclaration1181); if (failed) return dec;
            	id4 = (IToken)input.LT(1);
            	Match(input,Id,FollowIdInStructDeclaration1183); if (failed) return dec;
            	if ( backtracking == 0 ) 
            	{
            	   dec.Name = id4.Text; 
            	}
            	Match(input,Lbrace,FollowLbraceInStructDeclaration1186); if (failed) return dec;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:238:45: (m= member_declaration )+
            	int cnt20 = 0;
            	do 
            	{
            	    int alt20 = 2;
            	    int la200 = input.LA(1);
            	    
            	    if ( ((la200 >= Centroid && la200 <= Nointerp) || la200 == Id) )
            	    {
            	        alt20 = 1;
            	    }
            	    
            	
            	    switch (alt20) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:238:46: m= member_declaration
            			    {
            			    	PushFollow(FollowMemberDeclarationInStructDeclaration1191);
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

            	Match(input,Rbrace,FollowRbraceInStructDeclaration1196); if (failed) return dec;
            	Match(input,Semicolon,FollowSemicolonInStructDeclaration1198); if (failed) return dec;
            	if ( backtracking == 0 ) 
            	{
            	  
            	  		dec.Members = members.ToArray();
            	  		dec.Line = struct5.Line;
            	  	     	dec.Column = struct5.CharPositionInLine;		
            	  	
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
            	int la210 = input.LA(1);
            	
            	if ( ((la210 >= Centroid && la210 <= Nointerp)) )
            	{
            	    alt21 = 1;
            	}
            	switch (alt21) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:247:6: i= interpolation
            	        {
            	        	PushFollow(FollowInterpolationInMemberDeclaration1219);
            	        	i = Interpolation();
            	        	followingStackPointer_--;
            	        	if (failed) return m;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  m.Interpolation=(InterpolationMode)i;
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	PushFollow(FollowTypeRefInMemberDeclaration1229);
            	type = type_ref();
            	followingStackPointer_--;
            	if (failed) return m;
            	if ( backtracking == 0 ) 
            	{
            	  m.TypeInfo=type;
            	}
            	name = (IToken)input.LT(1);
            	Match(input,Id,FollowIdInMemberDeclaration1234); if (failed) return m;
            	if ( backtracking == 0 ) 
            	{
            	  m.Name=name.Text; m.Line=name.Line; m.Column = name.CharPositionInLine; 
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:249:5: (e= fixed_array )?
            	int alt22 = 2;
            	int la220 = input.LA(1);
            	
            	if ( (la220 == Lbracket) )
            	{
            	    alt22 = 1;
            	}
            	switch (alt22) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:249:6: e= fixed_array
            	        {
            	        	PushFollow(FollowFixedArrayInMemberDeclaration1244);
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
            	int la230 = input.LA(1);
            	
            	if ( (la230 == Colon) )
            	{
            	    alt23 = 1;
            	}
            	switch (alt23) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:249:43: s= semantic
            	        {
            	        	PushFollow(FollowSemanticInMemberDeclaration1252);
            	        	s = Semantic();
            	        	followingStackPointer_--;
            	        	if (failed) return m;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  m.Semantic=s;
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	Match(input,Semicolon,FollowSemicolonInMemberDeclaration1257); if (failed) return m;
            
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
    public InterpolationMode? Interpolation() // throws RecognitionException [1]
    {   

        InterpolationMode? i = null;
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:253:2: ( CENTROID | LINEAR | NOINTERP )
            int alt24 = 3;
            switch ( input.LA(1) ) 
            {
            case Centroid:
            	{
                alt24 = 1;
                }
                break;
            case Linear:
            	{
                alt24 = 2;
                }
                break;
            case Nointerp:
            	{
                alt24 = 3;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return i;}
            	    NoViableAltException nvaeD24S0 =
            	        new NoViableAltException("252:1: interpolation returns [InterpolationMode? i] : ( CENTROID | LINEAR | NOINTERP );", 24, 0, input);
            
            	    throw nvaeD24S0;
            }
            
            switch (alt24) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:253:3: CENTROID
                    {
                    	Match(input,Centroid,FollowCentroidInInterpolation1270); if (failed) return i;
                    	if ( backtracking == 0 ) 
                    	{
                    	  i = InterpolationMode.Centroid;
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:254:4: LINEAR
                    {
                    	Match(input,Linear,FollowLinearInInterpolation1277); if (failed) return i;
                    	if ( backtracking == 0 ) 
                    	{
                    	  i = InterpolationMode.Linear;
                    	}
                    
                    }
                    break;
                case 3 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:255:4: NOINTERP
                    {
                    	Match(input,Nointerp,FollowNointerpInInterpolation1285); if (failed) return i;
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
            	Match(input,Lparen,FollowLparenInFunctionDeclaration1300); if (failed) return dec;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:259:10: (p= parameters )?
            	int alt25 = 2;
            	int la250 = input.LA(1);
            	
            	if ( (la250 == Uniform || (la250 >= In && la250 <= Inout) || la250 == Id) )
            	{
            	    alt25 = 1;
            	}
            	switch (alt25) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:259:11: p= parameters
            	        {
            	        	PushFollow(FollowParametersInFunctionDeclaration1305);
            	        	p = Parameters();
            	        	followingStackPointer_--;
            	        	if (failed) return dec;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  dec.Parameters=p.ToArray();
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	Match(input,Rparen,FollowRparenInFunctionDeclaration1310); if (failed) return dec;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:260:2: (s= semantic )?
            	int alt26 = 2;
            	int la260 = input.LA(1);
            	
            	if ( (la260 == Colon) )
            	{
            	    alt26 = 1;
            	}
            	switch (alt26) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:260:3: s= semantic
            	        {
            	        	PushFollow(FollowSemanticInFunctionDeclaration1317);
            	        	s = Semantic();
            	        	followingStackPointer_--;
            	        	if (failed) return dec;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	  dec.ReturnSemantic = s;
            	        	}
            	        
            	        }
            	        break;
            	
            	}

            	PushFollow(FollowBlockStatementInFunctionDeclaration1325);
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
    public AttributeDeclaration Attribute() // throws RecognitionException [1]
    {   

        AttributeDeclaration attr =  new AttributeDeclaration();
    
        IToken id6 = null;
        List<Expression> args = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:265:2: ( LBRACKET ID (args= attribute_parameters )? RBRACKET )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:265:6: LBRACKET ID (args= attribute_parameters )? RBRACKET
            {
            	Match(input,Lbracket,FollowLbracketInAttribute1343); if (failed) return attr;
            	id6 = (IToken)input.LT(1);
            	Match(input,Id,FollowIdInAttribute1345); if (failed) return attr;
            	if ( backtracking == 0 ) 
            	{
            	  attr.Name = id6.Text;
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:265:41: (args= attribute_parameters )?
            	int alt27 = 2;
            	int la270 = input.LA(1);
            	
            	if ( (la270 == Rparen) )
            	{
            	    alt27 = 1;
            	}
            	switch (alt27) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:265:42: args= attribute_parameters
            	        {
            	        	PushFollow(FollowAttributeParametersInAttribute1351);
            	        	args = attribute_parameters();
            	        	followingStackPointer_--;
            	        	if (failed) return attr;
            	        
            	        }
            	        break;
            	
            	}

            	Match(input,Rbracket,FollowRbracketInAttribute1355); if (failed) return attr;
            	if ( backtracking == 0 ) 
            	{
            	  
            	  		attr.Arguments = args!=null?args.ToArray():null;
            	  		attr.Line = id6.Line;
            	  		attr.Column = id6.CharPositionInLine;
            	  	
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
            		Match(input,Rparen,FollowRparenInAttributeParameters1372); if (failed) return exp;
            		// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:273:12: (e= attribute_expresion )
            		// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:273:13: e= attribute_expresion
            		{
            			PushFollow(FollowAttributeExpresionInAttributeParameters1377);
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
            		    int la280 = input.LA(1);
            		    
            		    if ( (la280 == Comma) )
            		    {
            		        alt28 = 1;
            		    }
            		    
            		
            		    switch (alt28) 
            			{
            				case 1 :
            				    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:273:50: COMMA e= attribute_expresion
            				    {
            				    	Match(input,Comma,FollowCommaInAttributeParameters1382); if (failed) return exp;
            				    	PushFollow(FollowAttributeExpresionInAttributeParameters1386);
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

            		Match(input,Lparen,FollowLparenInAttributeParameters1392); if (failed) return exp;
            	
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
            case Number:
            case BoolConstant:
            	{
                alt29 = 1;
                }
                break;
            case Id:
            	{
                alt29 = 2;
                }
                break;
            case StringLiteral:
            	{
                alt29 = 3;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return lit;}
            	    NoViableAltException nvaeD29S0 =
            	        new NoViableAltException("275:1: attribute_expresion returns [LiteralExpression lit] : (e= constant_exp | t= ID | t= STRING_LITERAL );", 29, 0, input);
            
            	    throw nvaeD29S0;
            }
            
            switch (alt29) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:276:5: e= constant_exp
                    {
                    	PushFollow(FollowConstantExpInAttributeExpresion1409);
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
                    	Match(input,Id,FollowIdInAttributeExpresion1418); if (failed) return lit;
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
                    	Match(input,StringLiteral,FollowStringLiteralInAttributeExpresion1427); if (failed) return lit;
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
    public List<ParameterDeclaration> Parameters() // throws RecognitionException [1]
    {   

        List<ParameterDeclaration> l = new List<ParameterDeclaration>();
    
        ParameterDeclaration p = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:281:2: (p= parameter_declaration ( COMMA p= parameter_declaration )* )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:281:4: p= parameter_declaration ( COMMA p= parameter_declaration )*
            {
            	PushFollow(FollowParameterDeclarationInParameters1444);
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
            	    int la300 = input.LA(1);
            	    
            	    if ( (la300 == Comma) )
            	    {
            	        alt30 = 1;
            	    }
            	    
            	
            	    switch (alt30) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:281:40: COMMA p= parameter_declaration
            			    {
            			    	Match(input,Comma,FollowCommaInParameters1448); if (failed) return l;
            			    	PushFollow(FollowParameterDeclarationInParameters1452);
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
            	int la310 = input.LA(1);
            	
            	if ( (la310 == Uniform || (la310 >= In && la310 <= Inout)) )
            	{
            	    alt31 = 1;
            	}
            	switch (alt31) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:285:4: q= parameter_qualifier
            	        {
            	        	PushFollow(FollowParameterQualifierInParameterDeclaration1473);
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

            	PushFollow(FollowTypeRefInParameterDeclaration1484);
            	type = type_ref();
            	followingStackPointer_--;
            	if (failed) return p;
            	if ( backtracking == 0 ) 
            	{
            	  p.TypeInfo = type;
            	}
            	name = (IToken)input.LT(1);
            	Match(input,Id,FollowIdInParameterDeclaration1492); if (failed) return p;
            	if ( backtracking == 0 ) 
            	{
            	    p.Name = name.Text; 
            	  		   p.Line=name.Line; 
            	  		   p.Column=name.CharPositionInLine; 
            	  		
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:291:2: (e= fixed_array )?
            	int alt32 = 2;
            	int la320 = input.LA(1);
            	
            	if ( (la320 == Lbracket) )
            	{
            	    alt32 = 1;
            	}
            	switch (alt32) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:291:3: e= fixed_array
            	        {
            	        	PushFollow(FollowFixedArrayInParameterDeclaration1502);
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
            	int la330 = input.LA(1);
            	
            	if ( (la330 == Colon) )
            	{
            	    alt33 = 1;
            	}
            	switch (alt33) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:292:3: s= semantic
            	        {
            	        	PushFollow(FollowSemanticInParameterDeclaration1512);
            	        	s = Semantic();
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
            	int la340 = input.LA(1);
            	
            	if ( (la340 == Assign) )
            	{
            	    alt34 = 1;
            	}
            	switch (alt34) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:292:34: ASSIGN v= expression
            	        {
            	        	Match(input,Assign,FollowAssignInParameterDeclaration1518); if (failed) return p;
            	        	PushFollow(FollowExpressionInParameterDeclaration1522);
            	        	v = Expression();
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
            case In:
            	{
                alt35 = 1;
                }
                break;
            case Out:
            	{
                alt35 = 2;
                }
                break;
            case Inout:
            	{
                alt35 = 3;
                }
                break;
            case Uniform:
            	{
                alt35 = 4;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return q;}
            	    NoViableAltException nvaeD35S0 =
            	        new NoViableAltException("295:1: parameter_qualifier returns [ParamQualifier? q] : ( IN | OUT | INOUT | UNIFORM );", 35, 0, input);
            
            	    throw nvaeD35S0;
            }
            
            switch (alt35) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:296:4: IN
                    {
                    	Match(input,In,FollowInInParameterQualifier1539); if (failed) return q;
                    	if ( backtracking == 0 ) 
                    	{
                    	   q = ParamQualifier.In; 
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:297:4: OUT
                    {
                    	Match(input,Out,FollowOutInParameterQualifier1546); if (failed) return q;
                    	if ( backtracking == 0 ) 
                    	{
                    	   q = ParamQualifier.Out; 
                    	}
                    
                    }
                    break;
                case 3 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:298:4: INOUT
                    {
                    	Match(input,Inout,FollowInoutInParameterQualifier1553); if (failed) return q;
                    	if ( backtracking == 0 ) 
                    	{
                    	   q = ParamQualifier.InOut; 
                    	}
                    
                    }
                    break;
                case 4 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:299:4: UNIFORM
                    {
                    	Match(input,Uniform,FollowUniformInParameterQualifier1560); if (failed) return q;
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
    
        IToken lbrace7 = null;
        Statement s = null;
        
    
         var l = new List<Statement>(); 
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:303:2: ( LBRACE (s= statement )* RBRACE )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:303:4: LBRACE (s= statement )* RBRACE
            {
            	lbrace7 = (IToken)input.LT(1);
            	Match(input,Lbrace,FollowLbraceInBlockStatement1579); if (failed) return b;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:303:11: (s= statement )*
            	do 
            	{
            	    int alt36 = 2;
            	    int la360 = input.LA(1);
            	    
            	    if ( (la360 == Increment || la360 == Decrement || la360 == Lbracket || la360 == Lbrace || la360 == If || (la360 >= While && la360 <= Const) || la360 == Id) )
            	    {
            	        alt36 = 1;
            	    }
            	    
            	
            	    switch (alt36) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:303:12: s= statement
            			    {
            			    	PushFollow(FollowStatementInBlockStatement1584);
            			    	s = Statement();
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

            	Match(input,Rbrace,FollowRbraceInBlockStatement1589); if (failed) return b;
            	if ( backtracking == 0 ) 
            	{
            	   
            	  	 	b.Statements = l.ToArray();
            	  	 	b.Line = lbrace7.Line;
            	  	 	b.Column = lbrace7.CharPositionInLine;
            	  	 
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
    public Expression Expression() // throws RecognitionException [1]
    {   

        Expression n = null;
    
        IToken question8 = null;
        Expression e = null;

        Expression v = null;

        Expression f = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:313:2: (e= or_expr ( QUESTION v= expression COLON f= expression )? )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:313:4: e= or_expr ( QUESTION v= expression COLON f= expression )?
            {
            	PushFollow(FollowOrExprInExpression1610);
            	e = or_expr();
            	followingStackPointer_--;
            	if (failed) return n;
            	if ( backtracking == 0 ) 
            	{
            	  n=e;
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:313:19: ( QUESTION v= expression COLON f= expression )?
            	int alt37 = 2;
            	int la370 = input.LA(1);
            	
            	if ( (la370 == Question) )
            	{
            	    alt37 = 1;
            	}
            	switch (alt37) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:313:20: QUESTION v= expression COLON f= expression
            	        {
            	        	question8 = (IToken)input.LT(1);
            	        	Match(input,Question,FollowQuestionInExpression1613); if (failed) return n;
            	        	PushFollow(FollowExpressionInExpression1617);
            	        	v = Expression();
            	        	followingStackPointer_--;
            	        	if (failed) return n;
            	        	Match(input,Colon,FollowColonInExpression1619); if (failed) return n;
            	        	PushFollow(FollowExpressionInExpression1623);
            	        	f = Expression();
            	        	followingStackPointer_--;
            	        	if (failed) return n;
            	        	if ( backtracking == 0 ) 
            	        	{
            	        	   
            	        	  			n = new TernaryExpression(e,v,f)
            	        	  			{
            	        	  			  Line = question8.Line,
            	        	  			  Column = question8.CharPositionInLine
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
            	PushFollow(FollowXorExprInOrExpr1646);
            	e = xor_expr();
            	followingStackPointer_--;
            	if (failed) return n;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:324:15: (o= OR r= xor_expr )*
            	do 
            	{
            	    int alt38 = 2;
            	    int la380 = input.LA(1);
            	    
            	    if ( (la380 == Or) )
            	    {
            	        alt38 = 1;
            	    }
            	    
            	
            	    switch (alt38) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:324:16: o= OR r= xor_expr
            			    {
            			    	o = (IToken)input.LT(1);
            			    	Match(input,Or,FollowOrInOrExpr1651); if (failed) return n;
            			    	PushFollow(FollowXorExprInOrExpr1655);
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
            	PushFollow(FollowAndExprInXorExpr1680);
            	e = and_expr();
            	followingStackPointer_--;
            	if (failed) return n;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:334:15: (x= XOR r= and_expr )*
            	do 
            	{
            	    int alt39 = 2;
            	    int la390 = input.LA(1);
            	    
            	    if ( (la390 == Xor) )
            	    {
            	        alt39 = 1;
            	    }
            	    
            	
            	    switch (alt39) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:334:16: x= XOR r= and_expr
            			    {
            			    	x = (IToken)input.LT(1);
            			    	Match(input,Xor,FollowXorInXorExpr1685); if (failed) return n;
            			    	PushFollow(FollowAndExprInXorExpr1690);
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
            	PushFollow(FollowRelExpInAndExpr1712);
            	e = rel_exp();
            	followingStackPointer_--;
            	if (failed) return n;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:346:13: (a= AND r= rel_exp )*
            	do 
            	{
            	    int alt40 = 2;
            	    int la400 = input.LA(1);
            	    
            	    if ( (la400 == And) )
            	    {
            	        alt40 = 1;
            	    }
            	    
            	
            	    switch (alt40) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:346:14: a= AND r= rel_exp
            			    {
            			    	a = (IToken)input.LT(1);
            			    	Match(input,And,FollowAndInAndExpr1717); if (failed) return n;
            			    	PushFollow(FollowRelExpInAndExpr1721);
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
            	PushFollow(FollowAddExprInRelExp1744);
            	e = add_expr();
            	followingStackPointer_--;
            	if (failed) return n;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:356:15: (t= EQUAL r= add_expr | t= NEQUAL r= add_expr | t= LESS r= add_expr | t= LEQUAL r= add_expr | t= GREATER r= add_expr | t= GEQUAL r= add_expr )?
            	int alt41 = 7;
            	switch ( input.LA(1) ) 
            	{
            	    case Equal:
            	    	{
            	        alt41 = 1;
            	        }
            	        break;
            	    case Nequal:
            	    	{
            	        alt41 = 2;
            	        }
            	        break;
            	    case Less:
            	    	{
            	        alt41 = 3;
            	        }
            	        break;
            	    case Lequal:
            	    	{
            	        alt41 = 4;
            	        }
            	        break;
            	    case Greater:
            	    	{
            	        alt41 = 5;
            	        }
            	        break;
            	    case Gequal:
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
            	        	Match(input,Equal,FollowEqualInRelExp1759); if (failed) return n;
            	        	PushFollow(FollowAddExprInRelExp1764);
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
            	        	Match(input,Nequal,FollowNequalInRelExp1779); if (failed) return n;
            	        	PushFollow(FollowAddExprInRelExp1784);
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
            	        	Match(input,Less,FollowLessInRelExp1801); if (failed) return n;
            	        	PushFollow(FollowAddExprInRelExp1808);
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
            	        	Match(input,Lequal,FollowLequalInRelExp1823); if (failed) return n;
            	        	PushFollow(FollowAddExprInRelExp1828);
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
            	        	Match(input,Greater,FollowGreaterInRelExp1843); if (failed) return n;
            	        	PushFollow(FollowAddExprInRelExp1847);
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
            	        	Match(input,Gequal,FollowGequalInRelExp1862); if (failed) return n;
            	        	PushFollow(FollowAddExprInRelExp1867);
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
            	PushFollow(FollowMulExprInAddExpr1899);
            	e = mul_expr();
            	followingStackPointer_--;
            	if (failed) return n;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:368:14: ( (t= ADD | t= SUB ) r= mul_expr )*
            	do 
            	{
            	    int alt43 = 2;
            	    int la430 = input.LA(1);
            	    
            	    if ( (la430 == Add || la430 == Sub) )
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
            			    	int la420 = input.LA(1);
            			    	
            			    	if ( (la420 == Add) )
            			    	{
            			    	    alt42 = 1;
            			    	}
            			    	else if ( (la420 == Sub) )
            			    	{
            			    	    alt42 = 2;
            			    	}
            			    	else 
            			    	{
            			    	    if ( backtracking > 0 ) {failed = true; return n;}
            			    	    NoViableAltException nvaeD42S0 =
            			    	        new NoViableAltException("368:15: (t= ADD | t= SUB )", 42, 0, input);
            			    	
            			    	    throw nvaeD42S0;
            			    	}
            			    	switch (alt42) 
            			    	{
            			    	    case 1 :
            			    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:368:16: t= ADD
            			    	        {
            			    	        	t = (IToken)input.LT(1);
            			    	        	Match(input,Add,FollowAddInAddExpr1905); if (failed) return n;
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
            			    	        	Match(input,Sub,FollowSubInAddExpr1912); if (failed) return n;
            			    	        	if ( backtracking == 0 ) 
            			    	        	{
            			    	        	   op = BinaryOperator.Substraction;
            			    	        	}
            			    	        
            			    	        }
            			    	        break;
            			    	
            			    	}

            			    	PushFollow(FollowMulExprInAddExpr1933);
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
            	PushFollow(FollowUnaryExprInMulExpr1973);
            	e = unary_expr();
            	followingStackPointer_--;
            	if (failed) return n;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:375:16: ( (t= MUL | t= DIV ) r= unary_expr )*
            	do 
            	{
            	    int alt45 = 2;
            	    int la450 = input.LA(1);
            	    
            	    if ( ((la450 >= Mul && la450 <= Div)) )
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
            			    	int la440 = input.LA(1);
            			    	
            			    	if ( (la440 == Mul) )
            			    	{
            			    	    alt44 = 1;
            			    	}
            			    	else if ( (la440 == Div) )
            			    	{
            			    	    alt44 = 2;
            			    	}
            			    	else 
            			    	{
            			    	    if ( backtracking > 0 ) {failed = true; return n;}
            			    	    NoViableAltException nvaeD44S0 =
            			    	        new NoViableAltException("375:17: (t= MUL | t= DIV )", 44, 0, input);
            			    	
            			    	    throw nvaeD44S0;
            			    	}
            			    	switch (alt44) 
            			    	{
            			    	    case 1 :
            			    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:375:18: t= MUL
            			    	        {
            			    	        	t = (IToken)input.LT(1);
            			    	        	Match(input,Mul,FollowMulInMulExpr1979); if (failed) return n;
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
            			    	        	Match(input,Div,FollowDivInMulExpr1986); if (failed) return n;
            			    	        	if ( backtracking == 0 ) 
            			    	        	{
            			    	        	   op =BinaryOperator.Division; 
            			    	        	}
            			    	        
            			    	        }
            			    	        break;
            			    	
            			    	}

            			    	PushFollow(FollowUnaryExprInMulExpr1995);
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
            case Increment:
            case Decrement:
            case Id:
            	{
                alt50 = 1;
                }
                break;
            case Not:
            	{
                alt50 = 2;
                }
                break;
            case Sub:
            	{
                alt50 = 3;
                }
                break;
            case Lparen:
            	{
                alt50 = 4;
                }
                break;
            case Lbrace:
            	{
                alt50 = 5;
                }
                break;
            case Number:
            case BoolConstant:
            	{
                alt50 = 6;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return n;}
            	    NoViableAltException nvaeD50S0 =
            	        new NoViableAltException("378:1: unary_expr returns [Expression n] : (e= increment | t= NOT (v= lvalue | v= parent_exp | b= BOOL_CONSTANT ) | t= SUB (v= lvalue | v= parent_exp | v= constant_exp ) | c= parent_exp ( (v= lvalue | v= parent_exp | v= constant_exp ) )? | cons= constructor | lit= constant_exp );", 50, 0, input);
            
            	    throw nvaeD50S0;
            }
            
            switch (alt50) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:380:2: e= increment
                    {
                    	PushFollow(FollowIncrementInUnaryExpr2018);
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
                    	Match(input,Not,FollowNotInUnaryExpr2026); if (failed) return n;
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:381:10: (v= lvalue | v= parent_exp | b= BOOL_CONSTANT )
                    	int alt46 = 3;
                    	switch ( input.LA(1) ) 
                    	{
                    	case Id:
                    		{
                    	    alt46 = 1;
                    	    }
                    	    break;
                    	case Lparen:
                    		{
                    	    alt46 = 2;
                    	    }
                    	    break;
                    	case BoolConstant:
                    		{
                    	    alt46 = 3;
                    	    }
                    	    break;
                    		default:
                    		    if ( backtracking > 0 ) {failed = true; return n;}
                    		    NoViableAltException nvaeD46S0 =
                    		        new NoViableAltException("381:10: (v= lvalue | v= parent_exp | b= BOOL_CONSTANT )", 46, 0, input);
                    	
                    		    throw nvaeD46S0;
                    	}
                    	
                    	switch (alt46) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:381:11: v= lvalue
                    	        {
                    	        	PushFollow(FollowLvalueInUnaryExpr2031);
                    	        	v = Lvalue();
                    	        	followingStackPointer_--;
                    	        	if (failed) return n;
                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:381:22: v= parent_exp
                    	        {
                    	        	PushFollow(FollowParentExpInUnaryExpr2037);
                    	        	v = parent_exp();
                    	        	followingStackPointer_--;
                    	        	if (failed) return n;
                    	        
                    	        }
                    	        break;
                    	    case 3 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:382:5: b= BOOL_CONSTANT
                    	        {
                    	        	b = (IToken)input.LT(1);
                    	        	Match(input,BoolConstant,FollowBoolConstantInUnaryExpr2046); if (failed) return n;
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
                    	Match(input,Sub,FollowSubInUnaryExpr2060); if (failed) return n;
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:386:10: (v= lvalue | v= parent_exp | v= constant_exp )
                    	int alt47 = 3;
                    	switch ( input.LA(1) ) 
                    	{
                    	case Id:
                    		{
                    	    alt47 = 1;
                    	    }
                    	    break;
                    	case Lparen:
                    		{
                    	    alt47 = 2;
                    	    }
                    	    break;
                    	case Number:
                    	case BoolConstant:
                    		{
                    	    alt47 = 3;
                    	    }
                    	    break;
                    		default:
                    		    if ( backtracking > 0 ) {failed = true; return n;}
                    		    NoViableAltException nvaeD47S0 =
                    		        new NoViableAltException("386:10: (v= lvalue | v= parent_exp | v= constant_exp )", 47, 0, input);
                    	
                    		    throw nvaeD47S0;
                    	}
                    	
                    	switch (alt47) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:386:11: v= lvalue
                    	        {
                    	        	PushFollow(FollowLvalueInUnaryExpr2065);
                    	        	v = Lvalue();
                    	        	followingStackPointer_--;
                    	        	if (failed) return n;
                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:386:22: v= parent_exp
                    	        {
                    	        	PushFollow(FollowParentExpInUnaryExpr2071);
                    	        	v = parent_exp();
                    	        	followingStackPointer_--;
                    	        	if (failed) return n;
                    	        
                    	        }
                    	        break;
                    	    case 3 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:386:37: v= constant_exp
                    	        {
                    	        	PushFollow(FollowConstantExpInUnaryExpr2078);
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
                    	PushFollow(FollowParentExpInUnaryExpr2090);
                    	c = parent_exp();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	if ( backtracking == 0 ) 
                    	{
                    	  n=c;
                    	}
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:387:24: ( (v= lvalue | v= parent_exp | v= constant_exp ) )?
                    	int alt49 = 2;
                    	int la490 = input.LA(1);
                    	
                    	if ( (la490 == Lparen || (la490 >= Id && la490 <= Number) || la490 == BoolConstant) )
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
                    	        	case Id:
                    	        		{
                    	        	    alt48 = 1;
                    	        	    }
                    	        	    break;
                    	        	case Lparen:
                    	        		{
                    	        	    alt48 = 2;
                    	        	    }
                    	        	    break;
                    	        	case Number:
                    	        	case BoolConstant:
                    	        		{
                    	        	    alt48 = 3;
                    	        	    }
                    	        	    break;
                    	        		default:
                    	        		    if ( backtracking > 0 ) {failed = true; return n;}
                    	        		    NoViableAltException nvaeD48S0 =
                    	        		        new NoViableAltException("387:25: (v= lvalue | v= parent_exp | v= constant_exp )", 48, 0, input);
                    	        	
                    	        		    throw nvaeD48S0;
                    	        	}
                    	        	
                    	        	switch (alt48) 
                    	        	{
                    	        	    case 1 :
                    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:387:26: v= lvalue
                    	        	        {
                    	        	        	PushFollow(FollowLvalueInUnaryExpr2098);
                    	        	        	v = Lvalue();
                    	        	        	followingStackPointer_--;
                    	        	        	if (failed) return n;
                    	        	        
                    	        	        }
                    	        	        break;
                    	        	    case 2 :
                    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:387:37: v= parent_exp
                    	        	        {
                    	        	        	PushFollow(FollowParentExpInUnaryExpr2104);
                    	        	        	v = parent_exp();
                    	        	        	followingStackPointer_--;
                    	        	        	if (failed) return n;
                    	        	        
                    	        	        }
                    	        	        break;
                    	        	    case 3 :
                    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:387:52: v= constant_exp
                    	        	        {
                    	        	        	PushFollow(FollowConstantExpInUnaryExpr2110);
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
                    	PushFollow(FollowConstructorInUnaryExpr2123);
                    	cons = Constructor();
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
                    	PushFollow(FollowConstantExpInUnaryExpr2131);
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
    
        IToken lparen9 = null;
        Expression c = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:393:2: ( LPAREN c= expression RPAREN )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:393:4: LPAREN c= expression RPAREN
            {
            	lparen9 = (IToken)input.LT(1);
            	Match(input,Lparen,FollowLparenInParentExp2147); if (failed) return n;
            	PushFollow(FollowExpressionInParentExp2151);
            	c = Expression();
            	followingStackPointer_--;
            	if (failed) return n;
            	if ( backtracking == 0 ) 
            	{
            	   n = new ParenEncloseExpression(c, lparen9.Line, lparen9.CharPositionInLine); 
            	}
            	Match(input,Rparen,FollowRparenInParentExp2155); if (failed) return n;
            
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
            int la530 = input.LA(1);
            
            if ( (la530 == Id) )
            {
                alt53 = 1;
            }
            else if ( (la530 == Increment || la530 == Decrement) )
            {
                alt53 = 2;
            }
            else 
            {
                if ( backtracking > 0 ) {failed = true; return n;}
                NoViableAltException nvaeD53S0 =
                    new NoViableAltException("395:1: increment returns [Expression n] : (e= lvalue (t= INCREMENT | t= DECREMENT )? | (t= INCREMENT | t= DECREMENT ) e= lvalue );", 53, 0, input);
            
                throw nvaeD53S0;
            }
            switch (alt53) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:397:4: e= lvalue (t= INCREMENT | t= DECREMENT )?
                    {
                    	PushFollow(FollowLvalueInIncrement2175);
                    	e = Lvalue();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	if ( backtracking == 0 ) 
                    	{
                    	   n=e; 
                    	}
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:397:21: (t= INCREMENT | t= DECREMENT )?
                    	int alt51 = 3;
                    	int la510 = input.LA(1);
                    	
                    	if ( (la510 == Increment) )
                    	{
                    	    alt51 = 1;
                    	}
                    	else if ( (la510 == Decrement) )
                    	{
                    	    alt51 = 2;
                    	}
                    	switch (alt51) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:397:22: t= INCREMENT
                    	        {
                    	        	t = (IToken)input.LT(1);
                    	        	Match(input,Increment,FollowIncrementInIncrement2181); if (failed) return n;
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
                    	        	Match(input,Decrement,FollowDecrementInIncrement2196); if (failed) return n;
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
                    	int la520 = input.LA(1);
                    	
                    	if ( (la520 == Increment) )
                    	{
                    	    alt52 = 1;
                    	}
                    	else if ( (la520 == Decrement) )
                    	{
                    	    alt52 = 2;
                    	}
                    	else 
                    	{
                    	    if ( backtracking > 0 ) {failed = true; return n;}
                    	    NoViableAltException nvaeD52S0 =
                    	        new NoViableAltException("399:6: (t= INCREMENT | t= DECREMENT )", 52, 0, input);
                    	
                    	    throw nvaeD52S0;
                    	}
                    	switch (alt52) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:399:8: t= INCREMENT
                    	        {
                    	        	t = (IToken)input.LT(1);
                    	        	Match(input,Increment,FollowIncrementInIncrement2210); if (failed) return n;
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
                    	        	Match(input,Decrement,FollowDecrementInIncrement2224); if (failed) return n;
                    	        	if ( backtracking == 0 ) 
                    	        	{
                    	        	   op = UnaryOperator.PreDec;
                    	        	}
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	PushFollow(FollowLvalueInIncrement2232);
                    	e = Lvalue();
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
    public Expression Lvalue() // throws RecognitionException [1]
    {   

        Expression n = null;
    
        IToken m = null;
        IToken lbracket10 = null;
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
            	PushFollow(FollowPrimaryExpInLvalue2258);
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
            	    int la570 = input.LA(1);
            	    
            	    if ( (la570 == Dot || la570 == Lbracket) )
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
            			    	int la560 = input.LA(1);
            			    	
            			    	if ( (la560 == Dot) )
            			    	{
            			    	    alt56 = 1;
            			    	}
            			    	else if ( (la560 == Lbracket) )
            			    	{
            			    	    alt56 = 2;
            			    	}
            			    	else 
            			    	{
            			    	    if ( backtracking > 0 ) {failed = true; return n;}
            			    	    NoViableAltException nvaeD56S0 =
            			    	        new NoViableAltException("408:28: ( DOT (m= ID LPAREN (args= argument_list )? RPAREN | m= ID ) | LBRACKET indexer= expression RBRACKET )", 56, 0, input);
            			    	
            			    	    throw nvaeD56S0;
            			    	}
            			    	switch (alt56) 
            			    	{
            			    	    case 1 :
            			    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:29: DOT (m= ID LPAREN (args= argument_list )? RPAREN | m= ID )
            			    	        {
            			    	        	Match(input,Dot,FollowDotInLvalue2262); if (failed) return n;
            			    	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:33: (m= ID LPAREN (args= argument_list )? RPAREN | m= ID )
            			    	        	int alt55 = 2;
            			    	        	int la550 = input.LA(1);
            			    	        	
            			    	        	if ( (la550 == Id) )
            			    	        	{
            			    	        	    int la551 = input.LA(2);
            			    	        	    
            			    	        	    if ( (la551 == Lparen) )
            			    	        	    {
            			    	        	        alt55 = 1;
            			    	        	    }
            			    	        	    else if ( ((la551 >= Equal && la551 <= Gequal) || (la551 >= Mul && la551 <= Rbracket) || la551 == Rparen || (la551 >= Rbrace && la551 <= Question)) )
            			    	        	    {
            			    	        	        alt55 = 2;
            			    	        	    }
            			    	        	    else 
            			    	        	    {
            			    	        	        if ( backtracking > 0 ) {failed = true; return n;}
            			    	        	        NoViableAltException nvaeD55S1 =
            			    	        	            new NoViableAltException("408:33: (m= ID LPAREN (args= argument_list )? RPAREN | m= ID )", 55, 1, input);
            			    	        	    
            			    	        	        throw nvaeD55S1;
            			    	        	    }
            			    	        	}
            			    	        	else 
            			    	        	{
            			    	        	    if ( backtracking > 0 ) {failed = true; return n;}
            			    	        	    NoViableAltException nvaeD55S0 =
            			    	        	        new NoViableAltException("408:33: (m= ID LPAREN (args= argument_list )? RPAREN | m= ID )", 55, 0, input);
            			    	        	
            			    	        	    throw nvaeD55S0;
            			    	        	}
            			    	        	switch (alt55) 
            			    	        	{
            			    	        	    case 1 :
            			    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:34: m= ID LPAREN (args= argument_list )? RPAREN
            			    	        	        {
            			    	        	        	m = (IToken)input.LT(1);
            			    	        	        	Match(input,Id,FollowIdInLvalue2267); if (failed) return n;
            			    	        	        	Match(input,Lparen,FollowLparenInLvalue2269); if (failed) return n;
            			    	        	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:46: (args= argument_list )?
            			    	        	        	int alt54 = 2;
            			    	        	        	int la540 = input.LA(1);
            			    	        	        	
            			    	        	        	if ( (la540 == Not || (la540 >= Increment && la540 <= Decrement) || la540 == Lparen || la540 == Lbrace || (la540 >= Id && la540 <= Number) || la540 == BoolConstant) )
            			    	        	        	{
            			    	        	        	    alt54 = 1;
            			    	        	        	}
            			    	        	        	switch (alt54) 
            			    	        	        	{
            			    	        	        	    case 1 :
            			    	        	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:408:47: args= argument_list
            			    	        	        	        {
            			    	        	        	        	PushFollow(FollowArgumentListInLvalue2274);
            			    	        	        	        	args = argument_list();
            			    	        	        	        	followingStackPointer_--;
            			    	        	        	        	if (failed) return n;
            			    	        	        	        
            			    	        	        	        }
            			    	        	        	        break;
            			    	        	        	
            			    	        	        	}

            			    	        	        	Match(input,Rparen,FollowRparenInLvalue2278); if (failed) return n;
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
            			    	        	        	Match(input,Id,FollowIdInLvalue2291); if (failed) return n;
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
            			    	        	lbracket10 = (IToken)input.LT(1);
            			    	        	Match(input,Lbracket,FollowLbracketInLvalue2311); if (failed) return n;
            			    	        	PushFollow(FollowExpressionInLvalue2315);
            			    	        	indexer = Expression();
            			    	        	followingStackPointer_--;
            			    	        	if (failed) return n;
            			    	        	Match(input,Rbracket,FollowRbracketInLvalue2317); if (failed) return n;
            			    	        	if ( backtracking == 0 ) 
            			    	        	{
            			    	        	   
            			    	        	  				          n = new IndexArrayExpression
            			    	        	  						{
            			    	        	  							Left = n,
            			    	        	  							Indexer = indexer,
            			    	        	  							Line = lbracket10.Line,
            			    	        	  							Column =lbracket10.CharPositionInLine
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
            	Match(input,Id,FollowIdInPrimaryExp2351); if (failed) return e;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:439:8: ( LPAREN (args= argument_list )? RPAREN )?
            	int alt59 = 2;
            	int la590 = input.LA(1);
            	
            	if ( (la590 == Lparen) )
            	{
            	    alt59 = 1;
            	}
            	switch (alt59) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:439:9: LPAREN (args= argument_list )? RPAREN
            	        {
            	        	Match(input,Lparen,FollowLparenInPrimaryExp2354); if (failed) return e;
            	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:439:16: (args= argument_list )?
            	        	int alt58 = 2;
            	        	int la580 = input.LA(1);
            	        	
            	        	if ( (la580 == Not || (la580 >= Increment && la580 <= Decrement) || la580 == Lparen || la580 == Lbrace || (la580 >= Id && la580 <= Number) || la580 == BoolConstant) )
            	        	{
            	        	    alt58 = 1;
            	        	}
            	        	switch (alt58) 
            	        	{
            	        	    case 1 :
            	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:439:17: args= argument_list
            	        	        {
            	        	        	PushFollow(FollowArgumentListInPrimaryExp2359);
            	        	        	args = argument_list();
            	        	        	followingStackPointer_--;
            	        	        	if (failed) return e;
            	        	        
            	        	        }
            	        	        break;
            	        	
            	        	}

            	        	Match(input,Rparen,FollowRparenInPrimaryExp2363); if (failed) return e;
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
            	int la600 = input.LA(1);
            	
            	if ( (la600 == Id) )
            	{
            	    int la601 = input.LA(2);
            	    
            	    if ( (la601 == Assign) )
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
            	        	Match(input,Id,FollowIdInArgumentList2389); if (failed) return l;
            	        	Match(input,Assign,FollowAssignInArgumentList2391); if (failed) return l;
            	        
            	        }
            	        break;
            	
            	}

            	PushFollow(FollowExpressionInArgumentList2397);
            	v = Expression();
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
            	    int la620 = input.LA(1);
            	    
            	    if ( (la620 == Comma) )
            	    {
            	        alt62 = 1;
            	    }
            	    
            	
            	    switch (alt62) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:464:5: COMMA (n= ID ASSIGN )? v= expression
            			    {
            			    	Match(input,Comma,FollowCommaInArgumentList2406); if (failed) return l;
            			    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:464:11: (n= ID ASSIGN )?
            			    	int alt61 = 2;
            			    	int la610 = input.LA(1);
            			    	
            			    	if ( (la610 == Id) )
            			    	{
            			    	    int la611 = input.LA(2);
            			    	    
            			    	    if ( (la611 == Assign) )
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
            			    	        	Match(input,Id,FollowIdInArgumentList2411); if (failed) return l;
            			    	        	Match(input,Assign,FollowAssignInArgumentList2413); if (failed) return l;
            			    	        
            			    	        }
            			    	        break;
            			    	
            			    	}

            			    	PushFollow(FollowExpressionInArgumentList2419);
            			    	v = Expression();
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
            int la630 = input.LA(1);
            
            if ( (la630 == BoolConstant) )
            {
                alt63 = 1;
            }
            else if ( (la630 == Number) )
            {
                alt63 = 2;
            }
            else 
            {
                if ( backtracking > 0 ) {failed = true; return l;}
                NoViableAltException nvaeD63S0 =
                    new NoViableAltException("467:1: constant_exp returns [Expression l] : (v= BOOL_CONSTANT | t= NUMBER );", 63, 0, input);
            
                throw nvaeD63S0;
            }
            switch (alt63) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:471:3: v= BOOL_CONSTANT
                    {
                    	v = (IToken)input.LT(1);
                    	Match(input,BoolConstant,FollowBoolConstantInConstantExp2447); if (failed) return l;
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
                    	Match(input,Number,FollowNumberInConstantExp2458); if (failed) return l;
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
    public Statement Statement() // throws RecognitionException [1]
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
            case Increment:
            case Decrement:
            	{
                alt66 = 1;
                }
                break;
            case Id:
            	{
                int la663 = input.LA(2);
                
                if ( (la663 == Increment || la663 == Decrement || la663 == Dot || la663 == Semicolon || la663 == Lbracket || la663 == Lparen || (la663 >= Assign && la663 <= Divassign)) )
                {
                    alt66 = 1;
                }
                else if ( (la663 == Less || la663 == Id) )
                {
                    alt66 = 2;
                }
                else 
                {
                    if ( backtracking > 0 ) {failed = true; return node;}
                    NoViableAltException nvaeD66S3 =
                        new NoViableAltException("482:1: statement returns [Statement node] : (e= lvalue_statement SEMICOLON | list= local_declarations SEMICOLON | (attr= attribute )? (n= selection_stmt | n= iteration_stmt ) | n= block_statement | n= jump_stmt SEMICOLON );", 66, 3, input);
                
                    throw nvaeD66S3;
                }
                }
                break;
            case Const:
            	{
                alt66 = 2;
                }
                break;
            case Lbracket:
            case If:
            case While:
            case Do:
            case For:
            	{
                alt66 = 3;
                }
                break;
            case Lbrace:
            	{
                alt66 = 4;
                }
                break;
            case Break:
            case Continue:
            case Return:
            case Discard:
            	{
                alt66 = 5;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return node;}
            	    NoViableAltException nvaeD66S0 =
            	        new NoViableAltException("482:1: statement returns [Statement node] : (e= lvalue_statement SEMICOLON | list= local_declarations SEMICOLON | (attr= attribute )? (n= selection_stmt | n= iteration_stmt ) | n= block_statement | n= jump_stmt SEMICOLON );", 66, 0, input);
            
            	    throw nvaeD66S0;
            }
            
            switch (alt66) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:483:3: e= lvalue_statement SEMICOLON
                    {
                    	PushFollow(FollowLvalueStatementInStatement2477);
                    	e = lvalue_statement();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	Match(input,Semicolon,FollowSemicolonInStatement2479); if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	  node=new LValueStatement(e);
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:484:3: list= local_declarations SEMICOLON
                    {
                    	PushFollow(FollowLocalDeclarationsInStatement2487);
                    	list = local_declarations();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	Match(input,Semicolon,FollowSemicolonInStatement2489); if (failed) return node;
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
                    	int la640 = input.LA(1);
                    	
                    	if ( (la640 == Lbracket) )
                    	{
                    	    alt64 = 1;
                    	}
                    	switch (alt64) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:491:4: attr= attribute
                    	        {
                    	        	PushFollow(FollowAttributeInStatement2500);
                    	        	attr = Attribute();
                    	        	followingStackPointer_--;
                    	        	if (failed) return node;
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:491:21: (n= selection_stmt | n= iteration_stmt )
                    	int alt65 = 2;
                    	int la650 = input.LA(1);
                    	
                    	if ( (la650 == If) )
                    	{
                    	    alt65 = 1;
                    	}
                    	else if ( ((la650 >= While && la650 <= For)) )
                    	{
                    	    alt65 = 2;
                    	}
                    	else 
                    	{
                    	    if ( backtracking > 0 ) {failed = true; return node;}
                    	    NoViableAltException nvaeD65S0 =
                    	        new NoViableAltException("491:21: (n= selection_stmt | n= iteration_stmt )", 65, 0, input);
                    	
                    	    throw nvaeD65S0;
                    	}
                    	switch (alt65) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:491:22: n= selection_stmt
                    	        {
                    	        	PushFollow(FollowSelectionStmtInStatement2508);
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
                    	        	PushFollow(FollowIterationStmtInStatement2515);
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
                    	PushFollow(FollowBlockStatementInStatement2525);
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
                    	PushFollow(FollowJumpStmtInStatement2534);
                    	n = jump_stmt();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	Match(input,Semicolon,FollowSemicolonInStatement2536); if (failed) return node;
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
            int la700 = input.LA(1);
            
            if ( (la700 == Increment || la700 == Decrement) )
            {
                alt70 = 1;
            }
            else if ( (la700 == Id) )
            {
                alt70 = 2;
            }
            else 
            {
                if ( backtracking > 0 ) {failed = true; return node;}
                NoViableAltException nvaeD70S0 =
                    new NoViableAltException("496:1: lvalue_statement returns [Expression node] : ( (inc= INCREMENT | t= DECREMENT ) n= lvalue | n= lvalue ( ( (t= ASSIGN | t= ADDASSIGN | t= SUBASSIGN | t= MULASSIGN | t= DIVASSIGN ) e= expression ) | t= INCREMENT | t= DECREMENT )? );", 70, 0, input);
            
                throw nvaeD70S0;
            }
            switch (alt70) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:499:2: (inc= INCREMENT | t= DECREMENT ) n= lvalue
                    {
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:499:2: (inc= INCREMENT | t= DECREMENT )
                    	int alt67 = 2;
                    	int la670 = input.LA(1);
                    	
                    	if ( (la670 == Increment) )
                    	{
                    	    alt67 = 1;
                    	}
                    	else if ( (la670 == Decrement) )
                    	{
                    	    alt67 = 2;
                    	}
                    	else 
                    	{
                    	    if ( backtracking > 0 ) {failed = true; return node;}
                    	    NoViableAltException nvaeD67S0 =
                    	        new NoViableAltException("499:2: (inc= INCREMENT | t= DECREMENT )", 67, 0, input);
                    	
                    	    throw nvaeD67S0;
                    	}
                    	switch (alt67) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:499:3: inc= INCREMENT
                    	        {
                    	        	inc = (IToken)input.LT(1);
                    	        	Match(input,Increment,FollowIncrementInLvalueStatement2562); if (failed) return node;
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
                    	        	Match(input,Decrement,FollowDecrementInLvalueStatement2569); if (failed) return node;
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	PushFollow(FollowLvalueInLvalueStatement2574);
                    	n = Lvalue();
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
                    	PushFollow(FollowLvalueInLvalueStatement2582);
                    	n = Lvalue();
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
                    	    case Assign:
                    	    case Addassign:
                    	    case Subassign:
                    	    case Mulassign:
                    	    case Divassign:
                    	    	{
                    	        alt69 = 1;
                    	        }
                    	        break;
                    	    case Increment:
                    	    	{
                    	        alt69 = 2;
                    	        }
                    	        break;
                    	    case Decrement:
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
                    	        		case Assign:
                    	        			{
                    	        		    alt68 = 1;
                    	        		    }
                    	        		    break;
                    	        		case Addassign:
                    	        			{
                    	        		    alt68 = 2;
                    	        		    }
                    	        		    break;
                    	        		case Subassign:
                    	        			{
                    	        		    alt68 = 3;
                    	        		    }
                    	        		    break;
                    	        		case Mulassign:
                    	        			{
                    	        		    alt68 = 4;
                    	        		    }
                    	        		    break;
                    	        		case Divassign:
                    	        			{
                    	        		    alt68 = 5;
                    	        		    }
                    	        		    break;
                    	        			default:
                    	        			    if ( backtracking > 0 ) {failed = true; return node;}
                    	        			    NoViableAltException nvaeD68S0 =
                    	        			        new NoViableAltException("500:26: (t= ASSIGN | t= ADDASSIGN | t= SUBASSIGN | t= MULASSIGN | t= DIVASSIGN )", 68, 0, input);
                    	        		
                    	        			    throw nvaeD68S0;
                    	        		}
                    	        		
                    	        		switch (alt68) 
                    	        		{
                    	        		    case 1 :
                    	        		        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:500:27: t= ASSIGN
                    	        		        {
                    	        		        	t = (IToken)input.LT(1);
                    	        		        	Match(input,Assign,FollowAssignInLvalueStatement2590); if (failed) return node;
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
                    	        		        	Match(input,Addassign,FollowAddassignInLvalueStatement2596); if (failed) return node;
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
                    	        		        	Match(input,Subassign,FollowSubassignInLvalueStatement2604); if (failed) return node;
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
                    	        		        	Match(input,Mulassign,FollowMulassignInLvalueStatement2613); if (failed) return node;
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
                    	        		        	Match(input,Divassign,FollowDivassignInLvalueStatement2621); if (failed) return node;
                    	        		        	if ( backtracking == 0 ) 
                    	        		        	{
                    	        		        	   op = AssignOp.DivAssign;
                    	        		        	}
                    	        		        
                    	        		        }
                    	        		        break;
                    	        		
                    	        		}

                    	        		PushFollow(FollowExpressionInLvalueStatement2632);
                    	        		e = Expression();
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
                    	        	Match(input,Increment,FollowIncrementInLvalueStatement2646); if (failed) return node;
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
                    	        	Match(input,Decrement,FollowDecrementInLvalueStatement2664); if (failed) return node;
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
            	int la710 = input.LA(1);
            	
            	if ( (la710 == Const) )
            	{
            	    alt71 = 1;
            	}
            	switch (alt71) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:509:4: c= CONST
            	        {
            	        	c = (IToken)input.LT(1);
            	        	Match(input,Const,FollowConstInLocalDeclarations2687); if (failed) return list;
            	        
            	        }
            	        break;
            	
            	}

            	PushFollow(FollowTypeRefInLocalDeclarations2693);
            	type = type_ref();
            	followingStackPointer_--;
            	if (failed) return list;
            	PushFollow(FollowLocalDeclarationInLocalDeclarations2697);
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
            	    int la720 = input.LA(1);
            	    
            	    if ( (la720 == Comma) )
            	    {
            	        alt72 = 1;
            	    }
            	    
            	
            	    switch (alt72) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:515:4: COMMA l= local_declaration
            			    {
            			    	Match(input,Comma,FollowCommaInLocalDeclarations2704); if (failed) return list;
            			    	PushFollow(FollowLocalDeclarationInLocalDeclarations2708);
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
            	Match(input,Id,FollowIdInLocalDeclaration2737); if (failed) return n;
            	if ( backtracking == 0 ) 
            	{
            	  n.Name = id.Text;
            	}
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:525:29: (elements= fixed_array )?
            	int alt73 = 2;
            	int la730 = input.LA(1);
            	
            	if ( (la730 == Lbracket) )
            	{
            	    alt73 = 1;
            	}
            	switch (alt73) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:525:30: elements= fixed_array
            	        {
            	        	PushFollow(FollowFixedArrayInLocalDeclaration2743);
            	        	elements = fixed_array();
            	        	followingStackPointer_--;
            	        	if (failed) return n;
            	        
            	        }
            	        break;
            	
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:525:53: ( ASSIGN e= expression )?
            	int alt74 = 2;
            	int la740 = input.LA(1);
            	
            	if ( (la740 == Assign) )
            	{
            	    alt74 = 1;
            	}
            	switch (alt74) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:525:54: ASSIGN e= expression
            	        {
            	        	Match(input,Assign,FollowAssignInLocalDeclaration2748); if (failed) return n;
            	        	PushFollow(FollowExpressionInLocalDeclaration2752);
            	        	e = Expression();
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
            	Match(input,If,FollowIfInSelectionStmt2779); if (failed) return n;
            	Match(input,Lparen,FollowLparenInSelectionStmt2781); if (failed) return n;
            	PushFollow(FollowExpressionInSelectionStmt2785);
            	c = Expression();
            	followingStackPointer_--;
            	if (failed) return n;
            	Match(input,Rparen,FollowRparenInSelectionStmt2787); if (failed) return n;
            	PushFollow(FollowStatementInSelectionStmt2791);
            	v = Statement();
            	followingStackPointer_--;
            	if (failed) return n;
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:537:11: ( ( ELSE statement )=> ELSE f= statement )?
            	int alt75 = 2;
            	int la750 = input.LA(1);
            	
            	if ( (la750 == Else) )
            	{
            	    int la751 = input.LA(2);
            	    
            	    if ( (Synpred1()) )
            	    {
            	        alt75 = 1;
            	    }
            	}
            	switch (alt75) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:537:12: ( ELSE statement )=> ELSE f= statement
            	        {
            	        	Match(input,Else,FollowElseInSelectionStmt2820); if (failed) return n;
            	        	PushFollow(FollowStatementInSelectionStmt2824);
            	        	f = Statement();
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

        List<AstNode> ini = null;

        List<AstNode> list = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:541:2: (t= WHILE LPAREN c= expression RPAREN b= statement | t= DO b= statement WHILE LPAREN c= expression RPAREN | t= FOR LPAREN (ini= for_ini )? SEMICOLON c= expression SEMICOLON (list= lvalue_statements )? RPAREN b= statement )
            int alt78 = 3;
            switch ( input.LA(1) ) 
            {
            case While:
            	{
                alt78 = 1;
                }
                break;
            case Do:
            	{
                alt78 = 2;
                }
                break;
            case For:
            	{
                alt78 = 3;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return n;}
            	    NoViableAltException nvaeD78S0 =
            	        new NoViableAltException("540:1: iteration_stmt returns [LoopStatement n] : (t= WHILE LPAREN c= expression RPAREN b= statement | t= DO b= statement WHILE LPAREN c= expression RPAREN | t= FOR LPAREN (ini= for_ini )? SEMICOLON c= expression SEMICOLON (list= lvalue_statements )? RPAREN b= statement );", 78, 0, input);
            
            	    throw nvaeD78S0;
            }
            
            switch (alt78) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:542:2: t= WHILE LPAREN c= expression RPAREN b= statement
                    {
                    	t = (IToken)input.LT(1);
                    	Match(input,While,FollowWhileInIterationStmt2846); if (failed) return n;
                    	Match(input,Lparen,FollowLparenInIterationStmt2848); if (failed) return n;
                    	PushFollow(FollowExpressionInIterationStmt2852);
                    	c = Expression();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	Match(input,Rparen,FollowRparenInIterationStmt2854); if (failed) return n;
                    	PushFollow(FollowStatementInIterationStmt2858);
                    	b = Statement();
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
                    	Match(input,Do,FollowDoInIterationStmt2866); if (failed) return n;
                    	PushFollow(FollowStatementInIterationStmt2870);
                    	b = Statement();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	Match(input,While,FollowWhileInIterationStmt2872); if (failed) return n;
                    	Match(input,Lparen,FollowLparenInIterationStmt2874); if (failed) return n;
                    	PushFollow(FollowExpressionInIterationStmt2878);
                    	c = Expression();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	Match(input,Rparen,FollowRparenInIterationStmt2880); if (failed) return n;
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
                    	Match(input,For,FollowForInIterationStmt2888); if (failed) return n;
                    	Match(input,Lparen,FollowLparenInIterationStmt2890); if (failed) return n;
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:544:16: (ini= for_ini )?
                    	int alt76 = 2;
                    	int la760 = input.LA(1);
                    	
                    	if ( (la760 == Increment || la760 == Decrement || la760 == Const || la760 == Id) )
                    	{
                    	    alt76 = 1;
                    	}
                    	switch (alt76) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:544:17: ini= for_ini
                    	        {
                    	        	PushFollow(FollowForIniInIterationStmt2895);
                    	        	ini = for_ini();
                    	        	followingStackPointer_--;
                    	        	if (failed) return n;
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	Match(input,Semicolon,FollowSemicolonInIterationStmt2899); if (failed) return n;
                    	PushFollow(FollowExpressionInIterationStmt2903);
                    	c = Expression();
                    	followingStackPointer_--;
                    	if (failed) return n;
                    	Match(input,Semicolon,FollowSemicolonInIterationStmt2905); if (failed) return n;
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:544:64: (list= lvalue_statements )?
                    	int alt77 = 2;
                    	int la770 = input.LA(1);
                    	
                    	if ( (la770 == Increment || la770 == Decrement || la770 == Id) )
                    	{
                    	    alt77 = 1;
                    	}
                    	switch (alt77) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:544:65: list= lvalue_statements
                    	        {
                    	        	PushFollow(FollowLvalueStatementsInIterationStmt2910);
                    	        	list = lvalue_statements();
                    	        	followingStackPointer_--;
                    	        	if (failed) return n;
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	Match(input,Rparen,FollowRparenInIterationStmt2914); if (failed) return n;
                    	PushFollow(FollowStatementInIterationStmt2918);
                    	b = Statement();
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
    public List<AstNode> lvalue_statements() // throws RecognitionException [1]
    {   

        List<AstNode> list =  new List<AstNode>();
    
        Expression n = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:557:2: (n= lvalue_statement ( COMMA n= lvalue_statement )* )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:557:4: n= lvalue_statement ( COMMA n= lvalue_statement )*
            {
            	PushFollow(FollowLvalueStatementInLvalueStatements2938);
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
            	    int la790 = input.LA(1);
            	    
            	    if ( (la790 == Comma) )
            	    {
            	        alt79 = 1;
            	    }
            	    
            	
            	    switch (alt79) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:557:38: COMMA n= lvalue_statement
            			    {
            			    	Match(input,Comma,FollowCommaInLvalueStatements2942); if (failed) return list;
            			    	PushFollow(FollowLvalueStatementInLvalueStatements2946);
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
    public List<AstNode> for_ini() // throws RecognitionException [1]
    {   

        List<AstNode> node = null;
    
        List<LocalDeclaration> l = null;

        List<AstNode> n = null;
        
    
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:560:2: (l= local_declarations | n= lvalue_statements )
            int alt80 = 2;
            switch ( input.LA(1) ) 
            {
            case Const:
            	{
                alt80 = 1;
                }
                break;
            case Id:
            	{
                int la802 = input.LA(2);
                
                if ( (la802 == Less || la802 == Id) )
                {
                    alt80 = 1;
                }
                else if ( (la802 == Increment || la802 == Decrement || (la802 >= Dot && la802 <= Semicolon) || la802 == Lbracket || la802 == Lparen || (la802 >= Assign && la802 <= Divassign)) )
                {
                    alt80 = 2;
                }
                else 
                {
                    if ( backtracking > 0 ) {failed = true; return node;}
                    NoViableAltException nvaeD80S2 =
                        new NoViableAltException("559:1: for_ini returns [List<ASTNode> node] : (l= local_declarations | n= lvalue_statements );", 80, 2, input);
                
                    throw nvaeD80S2;
                }
                }
                break;
            case Increment:
            case Decrement:
            	{
                alt80 = 2;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return node;}
            	    NoViableAltException nvaeD80S0 =
            	        new NoViableAltException("559:1: for_ini returns [List<ASTNode> node] : (l= local_declarations | n= lvalue_statements );", 80, 0, input);
            
            	    throw nvaeD80S0;
            }
            
            switch (alt80) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:561:4: l= local_declarations
                    {
                    	PushFollow(FollowLocalDeclarationsInForIni2969);
                    	l = local_declarations();
                    	followingStackPointer_--;
                    	if (failed) return node;
                    	if ( backtracking == 0 ) 
                    	{
                    	   
                    	  	  	node = new List<AstNode>();
                    	  	  	foreach (var item in l)
                    	  	  		node.Add(item);
                    	  	  
                    	}
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:567:4: n= lvalue_statements
                    {
                    	PushFollow(FollowLvalueStatementsInForIni2981);
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
            case Break:
            	{
                alt82 = 1;
                }
                break;
            case Continue:
            	{
                alt82 = 2;
                }
                break;
            case Discard:
            	{
                alt82 = 3;
                }
                break;
            case Return:
            	{
                alt82 = 4;
                }
                break;
            	default:
            	    if ( backtracking > 0 ) {failed = true; return node;}
            	    NoViableAltException nvaeD82S0 =
            	        new NoViableAltException("570:1: jump_stmt returns [Statement node] : (t= BREAK | t= CONTINUE | t= DISCARD | t= RETURN (e= expression )? );", 82, 0, input);
            
            	    throw nvaeD82S0;
            }
            
            switch (alt82) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:571:4: t= BREAK
                    {
                    	t = (IToken)input.LT(1);
                    	Match(input,Break,FollowBreakInJumpStmt2999); if (failed) return node;
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
                    	Match(input,Continue,FollowContinueInJumpStmt3008); if (failed) return node;
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
                    	Match(input,Discard,FollowDiscardInJumpStmt3017); if (failed) return node;
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
                    	Match(input,Return,FollowReturnInJumpStmt3027); if (failed) return node;
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:574:13: (e= expression )?
                    	int alt81 = 2;
                    	int la810 = input.LA(1);
                    	
                    	if ( (la810 == Not || (la810 >= Increment && la810 <= Decrement) || la810 == Lparen || la810 == Lbrace || (la810 >= Id && la810 <= Number) || la810 == BoolConstant) )
                    	{
                    	    alt81 = 1;
                    	}
                    	switch (alt81) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:574:14: e= expression
                    	        {
                    	        	PushFollow(FollowExpressionInJumpStmt3032);
                    	        	e = Expression();
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
        	Match(input,Else,FollowElseInSynpred12815); if (failed) return ;
        	PushFollow(FollowStatementInSynpred12817);
        	Statement();
        	followingStackPointer_--;
        	if (failed) return ;
        
        }
    }
    // $ANTLR end synpred1

   	public bool Synpred1() 
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


	private void InitializeCyclicDfAs()
	{
	}

 

    public static readonly BitSet FollowDeclarationInProgram588 = new BitSet(new ulong[]{0x0E40C00001000000UL});
    public static readonly BitSet FollowDeclarationInProgram595 = new BitSet(new ulong[]{0x0E40C00001000000UL});
    public static readonly BitSet FollowEofInProgram607 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowUniformInDeclaration627 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowStaticInDeclaration633 = new BitSet(new ulong[]{0x0800200000000000UL});
    public static readonly BitSet FollowConstInDeclaration638 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowTypeRefInDeclaration645 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowIdInDeclaration649 = new BitSet(new ulong[]{0x0000000041C00000UL});
    public static readonly BitSet FollowVariableDeclarationInDeclaration653 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowAttributesInDeclaration668 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowTypeRefInDeclaration672 = new BitSet(new ulong[]{0x0800000001000000UL});
    public static readonly BitSet FollowFixedArrayInDeclaration677 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowIdInDeclaration684 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FollowFunctionDeclarationInDeclaration688 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowTypeRefInDeclaration701 = new BitSet(new ulong[]{0x0800000001000000UL});
    public static readonly BitSet FollowIdInDeclaration706 = new BitSet(new ulong[]{0x0000000045C00000UL});
    public static readonly BitSet FollowVariableDeclarationInDeclaration711 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowFunctionDeclarationInDeclaration728 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowFixedArrayInDeclaration744 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowIdInDeclaration748 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FollowFunctionDeclarationInDeclaration753 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowStructDeclarationInDeclaration769 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowCbufferDeclarationInDeclaration782 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowAttributeInAttributes802 = new BitSet(new ulong[]{0x0000000001000002UL});
    public static readonly BitSet FollowFixedArrayInVariableDeclaration826 = new BitSet(new ulong[]{0x0000000040C00000UL});
    public static readonly BitSet FollowSemanticInVariableDeclaration838 = new BitSet(new ulong[]{0x0000000040C00000UL});
    public static readonly BitSet FollowAssignInVariableDeclaration848 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInVariableDeclaration852 = new BitSet(new ulong[]{0x0000000000C00000UL});
    public static readonly BitSet FollowPackoffsetModifierInVariableDeclaration864 = new BitSet(new ulong[]{0x0000000000C00000UL});
    public static readonly BitSet FollowRegisterModifierInVariableDeclaration875 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FollowSemicolonInVariableDeclaration883 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLbracketInFixedArray898 = new BitSet(new ulong[]{0x1000000000000000UL});
    public static readonly BitSet FollowNumberInFixedArray902 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FollowRbracketInFixedArray906 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowIdInTypeRef926 = new BitSet(new ulong[]{0x0000000000000042UL});
    public static readonly BitSet FollowLessInTypeRef930 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowTypeRefInTypeRef934 = new BitSet(new ulong[]{0x0000000000200100UL});
    public static readonly BitSet FollowCommaInTypeRef939 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowTypeRefInTypeRef944 = new BitSet(new ulong[]{0x0000000000200100UL});
    public static readonly BitSet FollowGreaterInTypeRef949 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowColonInSemantic967 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowIdInSemantic969 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLbraceInConstructor990 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInConstructor994 = new BitSet(new ulong[]{0x0000000020200000UL});
    public static readonly BitSet FollowCommaInConstructor998 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInConstructor1002 = new BitSet(new ulong[]{0x0000000020200000UL});
    public static readonly BitSet FollowRbraceInConstructor1007 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowColonInRegisterModifier1026 = new BitSet(new ulong[]{0x0080000000000000UL});
    public static readonly BitSet FollowRegInRegisterModifier1029 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FollowLparenInRegisterModifier1031 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowLvalueInRegisterModifier1035 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FollowRparenInRegisterModifier1037 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowColonInPackoffsetModifier1052 = new BitSet(new ulong[]{0x0100000000000000UL});
    public static readonly BitSet FollowPackInPackoffsetModifier1054 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FollowLparenInPackoffsetModifier1056 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowLvalueInPackoffsetModifier1060 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FollowRparenInPackoffsetModifier1062 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowCbufferInCbufferDeclaration1086 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowTbufferInCbufferDeclaration1091 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowIdInCbufferDeclaration1102 = new BitSet(new ulong[]{0x0000000010000000UL});
    public static readonly BitSet FollowLbraceInCbufferDeclaration1105 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowTypeRefInCbufferDeclaration1120 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowIdInCbufferDeclaration1124 = new BitSet(new ulong[]{0x0000000041C00000UL});
    public static readonly BitSet FollowVariableDeclarationInCbufferDeclaration1128 = new BitSet(new ulong[]{0x0800000020000000UL});
    public static readonly BitSet FollowRbraceInCbufferDeclaration1152 = new BitSet(new ulong[]{0x0000000000400002UL});
    public static readonly BitSet FollowSemicolonInCbufferDeclaration1154 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowStructInStructDeclaration1181 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowIdInStructDeclaration1183 = new BitSet(new ulong[]{0x0000000010000000UL});
    public static readonly BitSet FollowLbraceInStructDeclaration1186 = new BitSet(new ulong[]{0x0807000000000000UL});
    public static readonly BitSet FollowMemberDeclarationInStructDeclaration1191 = new BitSet(new ulong[]{0x0807000020000000UL});
    public static readonly BitSet FollowRbraceInStructDeclaration1196 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FollowSemicolonInStructDeclaration1198 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowInterpolationInMemberDeclaration1219 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowTypeRefInMemberDeclaration1229 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowIdInMemberDeclaration1234 = new BitSet(new ulong[]{0x0000000001C00000UL});
    public static readonly BitSet FollowFixedArrayInMemberDeclaration1244 = new BitSet(new ulong[]{0x0000000000C00000UL});
    public static readonly BitSet FollowSemanticInMemberDeclaration1252 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FollowSemicolonInMemberDeclaration1257 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowCentroidInInterpolation1270 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLinearInInterpolation1277 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowNointerpInInterpolation1285 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLparenInFunctionDeclaration1300 = new BitSet(new ulong[]{0x0838400008000000UL});
    public static readonly BitSet FollowParametersInFunctionDeclaration1305 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FollowRparenInFunctionDeclaration1310 = new BitSet(new ulong[]{0x0000000010800000UL});
    public static readonly BitSet FollowSemanticInFunctionDeclaration1317 = new BitSet(new ulong[]{0x0000000010000000UL});
    public static readonly BitSet FollowBlockStatementInFunctionDeclaration1325 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLbracketInAttribute1343 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowIdInAttribute1345 = new BitSet(new ulong[]{0x000000000A000000UL});
    public static readonly BitSet FollowAttributeParametersInAttribute1351 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FollowRbracketInAttribute1355 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowRparenInAttributeParameters1372 = new BitSet(new ulong[]{0x7800000000000000UL});
    public static readonly BitSet FollowAttributeExpresionInAttributeParameters1377 = new BitSet(new ulong[]{0x0000000004200000UL});
    public static readonly BitSet FollowCommaInAttributeParameters1382 = new BitSet(new ulong[]{0x7800000000000000UL});
    public static readonly BitSet FollowAttributeExpresionInAttributeParameters1386 = new BitSet(new ulong[]{0x0000000004200000UL});
    public static readonly BitSet FollowLparenInAttributeParameters1392 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowConstantExpInAttributeExpresion1409 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowIdInAttributeExpresion1418 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowStringLiteralInAttributeExpresion1427 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowParameterDeclarationInParameters1444 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FollowCommaInParameters1448 = new BitSet(new ulong[]{0x0838400000000000UL});
    public static readonly BitSet FollowParameterDeclarationInParameters1452 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FollowParameterQualifierInParameterDeclaration1473 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowTypeRefInParameterDeclaration1484 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowIdInParameterDeclaration1492 = new BitSet(new ulong[]{0x0000000041800002UL});
    public static readonly BitSet FollowFixedArrayInParameterDeclaration1502 = new BitSet(new ulong[]{0x0000000040800002UL});
    public static readonly BitSet FollowSemanticInParameterDeclaration1512 = new BitSet(new ulong[]{0x0000000040000002UL});
    public static readonly BitSet FollowAssignInParameterDeclaration1518 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInParameterDeclaration1522 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowInInParameterQualifier1539 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowOutInParameterQualifier1546 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowInoutInParameterQualifier1553 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowUniformInParameterQualifier1560 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLbraceInBlockStatement1579 = new BitSet(new ulong[]{0x08003FD031014000UL});
    public static readonly BitSet FollowStatementInBlockStatement1584 = new BitSet(new ulong[]{0x08003FD031014000UL});
    public static readonly BitSet FollowRbraceInBlockStatement1589 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowOrExprInExpression1610 = new BitSet(new ulong[]{0x0000000800000002UL});
    public static readonly BitSet FollowQuestionInExpression1613 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInExpression1617 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FollowColonInExpression1619 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInExpression1623 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowXorExprInOrExpr1646 = new BitSet(new ulong[]{0x0000000000020002UL});
    public static readonly BitSet FollowOrInOrExpr1651 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowXorExprInOrExpr1655 = new BitSet(new ulong[]{0x0000000000020002UL});
    public static readonly BitSet FollowAndExprInXorExpr1680 = new BitSet(new ulong[]{0x0000000000080002UL});
    public static readonly BitSet FollowXorInXorExpr1685 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowAndExprInXorExpr1690 = new BitSet(new ulong[]{0x0000000000080002UL});
    public static readonly BitSet FollowRelExpInAndExpr1712 = new BitSet(new ulong[]{0x0000000000040002UL});
    public static readonly BitSet FollowAndInAndExpr1717 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowRelExpInAndExpr1721 = new BitSet(new ulong[]{0x0000000000040002UL});
    public static readonly BitSet FollowAddExprInRelExp1744 = new BitSet(new ulong[]{0x00000000000003F2UL});
    public static readonly BitSet FollowEqualInRelExp1759 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowAddExprInRelExp1764 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowNequalInRelExp1779 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowAddExprInRelExp1784 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLessInRelExp1801 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowAddExprInRelExp1808 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLequalInRelExp1823 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowAddExprInRelExp1828 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowGreaterInRelExp1843 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowAddExprInRelExp1847 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowGequalInRelExp1862 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowAddExprInRelExp1867 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowMulExprInAddExpr1899 = new BitSet(new ulong[]{0x000000000000A002UL});
    public static readonly BitSet FollowAddInAddExpr1905 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowSubInAddExpr1912 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowMulExprInAddExpr1933 = new BitSet(new ulong[]{0x000000000000A002UL});
    public static readonly BitSet FollowUnaryExprInMulExpr1973 = new BitSet(new ulong[]{0x0000000000001802UL});
    public static readonly BitSet FollowMulInMulExpr1979 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowDivInMulExpr1986 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowUnaryExprInMulExpr1995 = new BitSet(new ulong[]{0x0000000000001802UL});
    public static readonly BitSet FollowIncrementInUnaryExpr2018 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowNotInUnaryExpr2026 = new BitSet(new ulong[]{0x4800000004000000UL});
    public static readonly BitSet FollowLvalueInUnaryExpr2031 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowParentExpInUnaryExpr2037 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowBoolConstantInUnaryExpr2046 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowSubInUnaryExpr2060 = new BitSet(new ulong[]{0x5800000004000000UL});
    public static readonly BitSet FollowLvalueInUnaryExpr2065 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowParentExpInUnaryExpr2071 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowConstantExpInUnaryExpr2078 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowParentExpInUnaryExpr2090 = new BitSet(new ulong[]{0x5800000004000002UL});
    public static readonly BitSet FollowLvalueInUnaryExpr2098 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowParentExpInUnaryExpr2104 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowConstantExpInUnaryExpr2110 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowConstructorInUnaryExpr2123 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowConstantExpInUnaryExpr2131 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLparenInParentExp2147 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInParentExp2151 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FollowRparenInParentExp2155 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLvalueInIncrement2175 = new BitSet(new ulong[]{0x0000000000014002UL});
    public static readonly BitSet FollowIncrementInIncrement2181 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowDecrementInIncrement2196 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowIncrementInIncrement2210 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowDecrementInIncrement2224 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowLvalueInIncrement2232 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowPrimaryExpInLvalue2258 = new BitSet(new ulong[]{0x0000000001100002UL});
    public static readonly BitSet FollowDotInLvalue2262 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowIdInLvalue2267 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FollowLparenInLvalue2269 = new BitSet(new ulong[]{0x580000001C01C400UL});
    public static readonly BitSet FollowArgumentListInLvalue2274 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FollowRparenInLvalue2278 = new BitSet(new ulong[]{0x0000000001100002UL});
    public static readonly BitSet FollowIdInLvalue2291 = new BitSet(new ulong[]{0x0000000001100002UL});
    public static readonly BitSet FollowLbracketInLvalue2311 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInLvalue2315 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FollowRbracketInLvalue2317 = new BitSet(new ulong[]{0x0000000001100002UL});
    public static readonly BitSet FollowIdInPrimaryExp2351 = new BitSet(new ulong[]{0x0000000004000002UL});
    public static readonly BitSet FollowLparenInPrimaryExp2354 = new BitSet(new ulong[]{0x580000001C01C400UL});
    public static readonly BitSet FollowArgumentListInPrimaryExp2359 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FollowRparenInPrimaryExp2363 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowIdInArgumentList2389 = new BitSet(new ulong[]{0x0000000040000000UL});
    public static readonly BitSet FollowAssignInArgumentList2391 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInArgumentList2397 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FollowCommaInArgumentList2406 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowIdInArgumentList2411 = new BitSet(new ulong[]{0x0000000040000000UL});
    public static readonly BitSet FollowAssignInArgumentList2413 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInArgumentList2419 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FollowBoolConstantInConstantExp2447 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowNumberInConstantExp2458 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLvalueStatementInStatement2477 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FollowSemicolonInStatement2479 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLocalDeclarationsInStatement2487 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FollowSemicolonInStatement2489 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowAttributeInStatement2500 = new BitSet(new ulong[]{0x000001D000000000UL});
    public static readonly BitSet FollowSelectionStmtInStatement2508 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowIterationStmtInStatement2515 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowBlockStatementInStatement2525 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowJumpStmtInStatement2534 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FollowSemicolonInStatement2536 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowIncrementInLvalueStatement2562 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowDecrementInLvalueStatement2569 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowLvalueInLvalueStatement2574 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLvalueInLvalueStatement2582 = new BitSet(new ulong[]{0x00000007C0014002UL});
    public static readonly BitSet FollowAssignInLvalueStatement2590 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowAddassignInLvalueStatement2596 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowSubassignInLvalueStatement2604 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowMulassignInLvalueStatement2613 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowDivassignInLvalueStatement2621 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInLvalueStatement2632 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowIncrementInLvalueStatement2646 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowDecrementInLvalueStatement2664 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowConstInLocalDeclarations2687 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowTypeRefInLocalDeclarations2693 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowLocalDeclarationInLocalDeclarations2697 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FollowCommaInLocalDeclarations2704 = new BitSet(new ulong[]{0x0800000000000000UL});
    public static readonly BitSet FollowLocalDeclarationInLocalDeclarations2708 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FollowIdInLocalDeclaration2737 = new BitSet(new ulong[]{0x0000000041000002UL});
    public static readonly BitSet FollowFixedArrayInLocalDeclaration2743 = new BitSet(new ulong[]{0x0000000040000002UL});
    public static readonly BitSet FollowAssignInLocalDeclaration2748 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInLocalDeclaration2752 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowIfInSelectionStmt2779 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FollowLparenInSelectionStmt2781 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInSelectionStmt2785 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FollowRparenInSelectionStmt2787 = new BitSet(new ulong[]{0x08003FD011014000UL});
    public static readonly BitSet FollowStatementInSelectionStmt2791 = new BitSet(new ulong[]{0x0000002000000002UL});
    public static readonly BitSet FollowElseInSelectionStmt2820 = new BitSet(new ulong[]{0x08003FD011014000UL});
    public static readonly BitSet FollowStatementInSelectionStmt2824 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowWhileInIterationStmt2846 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FollowLparenInIterationStmt2848 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInIterationStmt2852 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FollowRparenInIterationStmt2854 = new BitSet(new ulong[]{0x08003FD011014000UL});
    public static readonly BitSet FollowStatementInIterationStmt2858 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowDoInIterationStmt2866 = new BitSet(new ulong[]{0x08003FD011014000UL});
    public static readonly BitSet FollowStatementInIterationStmt2870 = new BitSet(new ulong[]{0x0000004000000000UL});
    public static readonly BitSet FollowWhileInIterationStmt2872 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FollowLparenInIterationStmt2874 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInIterationStmt2878 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FollowRparenInIterationStmt2880 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowForInIterationStmt2888 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FollowLparenInIterationStmt2890 = new BitSet(new ulong[]{0x0800200000414000UL});
    public static readonly BitSet FollowForIniInIterationStmt2895 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FollowSemicolonInIterationStmt2899 = new BitSet(new ulong[]{0x580000001401C400UL});
    public static readonly BitSet FollowExpressionInIterationStmt2903 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FollowSemicolonInIterationStmt2905 = new BitSet(new ulong[]{0x0800000008014000UL});
    public static readonly BitSet FollowLvalueStatementsInIterationStmt2910 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FollowRparenInIterationStmt2914 = new BitSet(new ulong[]{0x08003FD011014000UL});
    public static readonly BitSet FollowStatementInIterationStmt2918 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLvalueStatementInLvalueStatements2938 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FollowCommaInLvalueStatements2942 = new BitSet(new ulong[]{0x0800000000014000UL});
    public static readonly BitSet FollowLvalueStatementInLvalueStatements2946 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FollowLocalDeclarationsInForIni2969 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowLvalueStatementsInForIni2981 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowBreakInJumpStmt2999 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowContinueInJumpStmt3008 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowDiscardInJumpStmt3017 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowReturnInJumpStmt3027 = new BitSet(new ulong[]{0x580000001401C402UL});
    public static readonly BitSet FollowExpressionInJumpStmt3032 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FollowElseInSynpred12815 = new BitSet(new ulong[]{0x08003FD011014000UL});
    public static readonly BitSet FollowStatementInSynpred12817 = new BitSet(new ulong[]{0x0000000000000002UL});

}
}