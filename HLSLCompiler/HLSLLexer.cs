// $ANTLR 3.0.1 E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g 2015-03-08 14:43:04
namespace 
	Igneel.Compiling.Parser

{

using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;



public class HLSLLexer : Lexer 
{
    public const int EXPONENT = 71;
    public const int FLOAT_SUFFIX = 72;
    public const int WHILE = 38;
    public const int LETTER = 73;
    public const int CONST = 45;
    public const int LBRACE = 28;
    public const int FOR = 40;
    public const int DO = 39;
    public const int SUB = 15;
    public const int NOT = 10;
    public const int UNIFORM = 46;
    public const int AND = 18;
    public const int ID = 59;
    public const int SUBASSIGN = 32;
    public const int EOF = -1;
    public const int BREAK = 41;
    public const int OCTAL_DIGIT = 64;
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
    public const int NEQUAL = 5;
    public const int TBUFFER = 58;
    public const int DIVASSIGN = 34;
    public const int MULASSIGN = 33;
    public const int XOR = 19;
    public const int RBRACE = 29;
    public const int NON_ZERO_DIGIT = 65;
    public const int STATIC = 47;
    public const int ELSE = 37;
    public const int NUMBER = 60;
    public const int HEX_DIGIT = 68;
    public const int STRUCT = 54;
    public const int SEMICOLON = 22;
    public const int HEX_CONSTANT = 69;
    public const int Tokens = 78;
    public const int REG = 55;
    public const int MUL = 11;
    public const int DECREMENT = 16;
    public const int COLON = 23;
    public const int INCREMENT = 14;
    public const int WS = 75;
    public const int QUESTION = 35;
    public const int DISCARD = 44;
    public const int ADDASSIGN = 31;
    public const int CENTROID = 48;
    public const int OUT = 52;
    public const int CBUFFER = 57;
    public const int DIGIT_SEQUENCE = 70;
    public const int OR = 17;
    public const int ASSIGN = 30;
    public const int OCTAL_CONSTANT = 67;
    public const int DIV = 12;
    public const int PACK = 56;
    public const int EscapeSequence = 76;
    public const int OctalEscape = 77;

    public HLSLLexer() 
    {
		InitializeCyclicDFAs();
    }
    public HLSLLexer(ICharStream input) 
		: base(input)
	{
		InitializeCyclicDFAs();
    }
    
    override public string GrammarFileName
    {
    	get { return "E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g";} 
    }

    // $ANTLR start EQUAL 
    public void mEQUAL() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = EQUAL;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:10:7: ( '==' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:10:9: '=='
            {
            	Match("=="); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end EQUAL

    // $ANTLR start NEQUAL 
    public void mNEQUAL() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = NEQUAL;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:11:8: ( '!=' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:11:10: '!='
            {
            	Match("!="); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end NEQUAL

    // $ANTLR start LESS 
    public void mLESS() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = LESS;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:12:6: ( '<' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:12:8: '<'
            {
            	Match('<'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end LESS

    // $ANTLR start LEQUAL 
    public void mLEQUAL() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = LEQUAL;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:13:8: ( '<=' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:13:10: '<='
            {
            	Match("<="); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end LEQUAL

    // $ANTLR start GREATER 
    public void mGREATER() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = GREATER;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:14:9: ( '>' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:14:11: '>'
            {
            	Match('>'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end GREATER

    // $ANTLR start GEQUAL 
    public void mGEQUAL() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = GEQUAL;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:15:8: ( '>=' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:15:10: '>='
            {
            	Match(">="); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end GEQUAL

    // $ANTLR start NOT 
    public void mNOT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = NOT;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:16:5: ( '!' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:16:7: '!'
            {
            	Match('!'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end NOT

    // $ANTLR start MUL 
    public void mMUL() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = MUL;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:17:5: ( '*' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:17:7: '*'
            {
            	Match('*'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end MUL

    // $ANTLR start DIV 
    public void mDIV() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = DIV;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:18:5: ( '/' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:18:7: '/'
            {
            	Match('/'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end DIV

    // $ANTLR start ADD 
    public void mADD() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = ADD;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:19:5: ( '+' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:19:7: '+'
            {
            	Match('+'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end ADD

    // $ANTLR start INCREMENT 
    public void mINCREMENT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = INCREMENT;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:20:11: ( '++' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:20:13: '++'
            {
            	Match("++"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end INCREMENT

    // $ANTLR start SUB 
    public void mSUB() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = SUB;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:21:5: ( '-' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:21:7: '-'
            {
            	Match('-'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end SUB

    // $ANTLR start DECREMENT 
    public void mDECREMENT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = DECREMENT;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:22:11: ( '--' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:22:13: '--'
            {
            	Match("--"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end DECREMENT

    // $ANTLR start OR 
    public void mOR() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = OR;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:23:4: ( '||' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:23:6: '||'
            {
            	Match("||"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end OR

    // $ANTLR start AND 
    public void mAND() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = AND;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:24:5: ( '&&' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:24:7: '&&'
            {
            	Match("&&"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end AND

    // $ANTLR start XOR 
    public void mXOR() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = XOR;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:25:5: ( '^^' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:25:7: '^^'
            {
            	Match("^^"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end XOR

    // $ANTLR start DOT 
    public void mDOT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = DOT;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:26:5: ( '.' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:26:7: '.'
            {
            	Match('.'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end DOT

    // $ANTLR start COMMA 
    public void mCOMMA() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = COMMA;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:27:7: ( ',' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:27:9: ','
            {
            	Match(','); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end COMMA

    // $ANTLR start SEMICOLON 
    public void mSEMICOLON() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = SEMICOLON;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:28:11: ( ';' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:28:13: ';'
            {
            	Match(';'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end SEMICOLON

    // $ANTLR start COLON 
    public void mCOLON() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = COLON;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:29:7: ( ':' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:29:9: ':'
            {
            	Match(':'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end COLON

    // $ANTLR start LBRACKET 
    public void mLBRACKET() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = LBRACKET;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:30:10: ( '[' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:30:12: '['
            {
            	Match('['); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end LBRACKET

    // $ANTLR start RBRACKET 
    public void mRBRACKET() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = RBRACKET;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:31:10: ( ']' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:31:12: ']'
            {
            	Match(']'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end RBRACKET

    // $ANTLR start LPAREN 
    public void mLPAREN() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = LPAREN;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:32:8: ( '(' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:32:10: '('
            {
            	Match('('); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end LPAREN

    // $ANTLR start RPAREN 
    public void mRPAREN() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = RPAREN;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:33:8: ( ')' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:33:10: ')'
            {
            	Match(')'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end RPAREN

    // $ANTLR start LBRACE 
    public void mLBRACE() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = LBRACE;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:34:8: ( '{' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:34:10: '{'
            {
            	Match('{'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end LBRACE

    // $ANTLR start RBRACE 
    public void mRBRACE() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = RBRACE;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:35:8: ( '}' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:35:10: '}'
            {
            	Match('}'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end RBRACE

    // $ANTLR start ASSIGN 
    public void mASSIGN() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = ASSIGN;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:36:8: ( '=' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:36:10: '='
            {
            	Match('='); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end ASSIGN

    // $ANTLR start ADDASSIGN 
    public void mADDASSIGN() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = ADDASSIGN;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:37:11: ( '+=' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:37:13: '+='
            {
            	Match("+="); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end ADDASSIGN

    // $ANTLR start SUBASSIGN 
    public void mSUBASSIGN() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = SUBASSIGN;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:38:11: ( '-=' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:38:13: '-='
            {
            	Match("-="); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end SUBASSIGN

    // $ANTLR start MULASSIGN 
    public void mMULASSIGN() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = MULASSIGN;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:39:11: ( '*=' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:39:13: '*='
            {
            	Match("*="); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end MULASSIGN

    // $ANTLR start DIVASSIGN 
    public void mDIVASSIGN() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = DIVASSIGN;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:40:11: ( '/=' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:40:13: '/='
            {
            	Match("/="); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end DIVASSIGN

    // $ANTLR start QUESTION 
    public void mQUESTION() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = QUESTION;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:41:10: ( '?' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:41:12: '?'
            {
            	Match('?'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end QUESTION

    // $ANTLR start IF 
    public void mIF() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = IF;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:42:4: ( 'if' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:42:6: 'if'
            {
            	Match("if"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end IF

    // $ANTLR start ELSE 
    public void mELSE() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = ELSE;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:43:6: ( 'else' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:43:8: 'else'
            {
            	Match("else"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end ELSE

    // $ANTLR start WHILE 
    public void mWHILE() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = WHILE;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:44:7: ( 'while' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:44:9: 'while'
            {
            	Match("while"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end WHILE

    // $ANTLR start DO 
    public void mDO() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = DO;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:45:4: ( 'do' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:45:6: 'do'
            {
            	Match("do"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end DO

    // $ANTLR start FOR 
    public void mFOR() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = FOR;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:46:5: ( 'for' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:46:7: 'for'
            {
            	Match("for"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end FOR

    // $ANTLR start BREAK 
    public void mBREAK() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = BREAK;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:47:7: ( 'break' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:47:9: 'break'
            {
            	Match("break"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end BREAK

    // $ANTLR start CONTINUE 
    public void mCONTINUE() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = CONTINUE;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:48:10: ( 'continue' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:48:12: 'continue'
            {
            	Match("continue"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end CONTINUE

    // $ANTLR start RETURN 
    public void mRETURN() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = RETURN;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:49:8: ( 'return' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:49:10: 'return'
            {
            	Match("return"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end RETURN

    // $ANTLR start DISCARD 
    public void mDISCARD() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = DISCARD;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:50:9: ( 'discard' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:50:11: 'discard'
            {
            	Match("discard"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end DISCARD

    // $ANTLR start CONST 
    public void mCONST() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = CONST;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:51:7: ( 'const' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:51:9: 'const'
            {
            	Match("const"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end CONST

    // $ANTLR start UNIFORM 
    public void mUNIFORM() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = UNIFORM;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:52:9: ( 'uniform' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:52:11: 'uniform'
            {
            	Match("uniform"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end UNIFORM

    // $ANTLR start STATIC 
    public void mSTATIC() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = STATIC;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:53:8: ( 'static' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:53:10: 'static'
            {
            	Match("static"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end STATIC

    // $ANTLR start CENTROID 
    public void mCENTROID() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = CENTROID;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:54:10: ( 'centroid' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:54:12: 'centroid'
            {
            	Match("centroid"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end CENTROID

    // $ANTLR start LINEAR 
    public void mLINEAR() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = LINEAR;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:55:8: ( 'linear' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:55:10: 'linear'
            {
            	Match("linear"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end LINEAR

    // $ANTLR start NOINTERP 
    public void mNOINTERP() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = NOINTERP;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:56:10: ( 'nointerpolation' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:56:12: 'nointerpolation'
            {
            	Match("nointerpolation"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end NOINTERP

    // $ANTLR start IN 
    public void mIN() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = IN;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:57:4: ( 'in' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:57:6: 'in'
            {
            	Match("in"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end IN

    // $ANTLR start OUT 
    public void mOUT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = OUT;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:58:5: ( 'out' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:58:7: 'out'
            {
            	Match("out"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end OUT

    // $ANTLR start INOUT 
    public void mINOUT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = INOUT;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:59:7: ( 'inout' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:59:9: 'inout'
            {
            	Match("inout"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end INOUT

    // $ANTLR start STRUCT 
    public void mSTRUCT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = STRUCT;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:60:8: ( 'struct' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:60:10: 'struct'
            {
            	Match("struct"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end STRUCT

    // $ANTLR start REG 
    public void mREG() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = REG;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:61:5: ( 'register' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:61:7: 'register'
            {
            	Match("register"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end REG

    // $ANTLR start PACK 
    public void mPACK() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = PACK;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:62:6: ( 'packoffset' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:62:8: 'packoffset'
            {
            	Match("packoffset"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end PACK

    // $ANTLR start CBUFFER 
    public void mCBUFFER() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = CBUFFER;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:63:9: ( 'cbuffer' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:63:11: 'cbuffer'
            {
            	Match("cbuffer"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end CBUFFER

    // $ANTLR start TBUFFER 
    public void mTBUFFER() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = TBUFFER;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:64:9: ( 'tbuffer' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:64:11: 'tbuffer'
            {
            	Match("tbuffer"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end TBUFFER

    // $ANTLR start ZERO 
    public void mZERO() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:593:2: ( '0' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:593:4: '0'
            {
            	Match('0'); 
            
            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end ZERO

    // $ANTLR start NON_ZERO_DIGIT 
    public void mNON_ZERO_DIGIT() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:597:2: ( OCTAL_DIGIT | '8' | '9' )
            int alt1 = 3;
            switch ( input.LA(1) ) 
            {
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            	{
                alt1 = 1;
                }
                break;
            case '8':
            	{
                alt1 = 2;
                }
                break;
            case '9':
            	{
                alt1 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d1s0 =
            	        new NoViableAltException("596:10: fragment NON_ZERO_DIGIT : ( OCTAL_DIGIT | '8' | '9' );", 1, 0, input);
            
            	    throw nvae_d1s0;
            }
            
            switch (alt1) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:597:4: OCTAL_DIGIT
                    {
                    	mOCTAL_DIGIT(); 
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:597:17: '8'
                    {
                    	Match('8'); 
                    
                    }
                    break;
                case 3 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:597:21: '9'
                    {
                    	Match('9'); 
                    
                    }
                    break;
            
            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end NON_ZERO_DIGIT

    // $ANTLR start DIGIT 
    public void mDIGIT() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:601:2: ( ZERO | NON_ZERO_DIGIT )
            int alt2 = 2;
            int LA2_0 = input.LA(1);
            
            if ( (LA2_0 == '0') )
            {
                alt2 = 1;
            }
            else if ( ((LA2_0 >= '1' && LA2_0 <= '9')) )
            {
                alt2 = 2;
            }
            else 
            {
                NoViableAltException nvae_d2s0 =
                    new NoViableAltException("600:10: fragment DIGIT : ( ZERO | NON_ZERO_DIGIT );", 2, 0, input);
            
                throw nvae_d2s0;
            }
            switch (alt2) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:601:4: ZERO
                    {
                    	mZERO(); 
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:602:4: NON_ZERO_DIGIT
                    {
                    	mNON_ZERO_DIGIT(); 
                    
                    }
                    break;
            
            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end DIGIT

    // $ANTLR start OCTAL_CONSTANT 
    public void mOCTAL_CONSTANT() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:606:2: ( ZERO ( OCTAL_DIGIT )+ )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:606:4: ZERO ( OCTAL_DIGIT )+
            {
            	mZERO(); 
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:606:9: ( OCTAL_DIGIT )+
            	int cnt3 = 0;
            	do 
            	{
            	    int alt3 = 2;
            	    int LA3_0 = input.LA(1);
            	    
            	    if ( ((LA3_0 >= '1' && LA3_0 <= '7')) )
            	    {
            	        alt3 = 1;
            	    }
            	    
            	
            	    switch (alt3) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:606:10: OCTAL_DIGIT
            			    {
            			    	mOCTAL_DIGIT(); 
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt3 >= 1 ) goto loop3;
            		            EarlyExitException eee =
            		                new EarlyExitException(3, input);
            		            throw eee;
            	    }
            	    cnt3++;
            	} while (true);
            	
            	loop3:
            		;	// Stops C# compiler whinging that label 'loop3' has no statements

            
            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end OCTAL_CONSTANT

    // $ANTLR start OCTAL_DIGIT 
    public void mOCTAL_DIGIT() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:610:2: ( ( '1' .. '7' ) )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:610:4: ( '1' .. '7' )
            {
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:610:4: ( '1' .. '7' )
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:610:5: '1' .. '7'
            	{
            		MatchRange('1','7'); 
            	
            	}

            
            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end OCTAL_DIGIT

    // $ANTLR start HEX_CONSTANT 
    public void mHEX_CONSTANT() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:614:2: ( ZERO ( 'x' | 'X' ) ( HEX_DIGIT )+ )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:614:4: ZERO ( 'x' | 'X' ) ( HEX_DIGIT )+
            {
            	mZERO(); 
            	if ( input.LA(1) == 'X' || input.LA(1) == 'x' ) 
            	{
            	    input.Consume();
            	
            	}
            	else 
            	{
            	    MismatchedSetException mse =
            	        new MismatchedSetException(null,input);
            	    Recover(mse);    throw mse;
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:614:19: ( HEX_DIGIT )+
            	int cnt4 = 0;
            	do 
            	{
            	    int alt4 = 2;
            	    int LA4_0 = input.LA(1);
            	    
            	    if ( ((LA4_0 >= '0' && LA4_0 <= '9') || (LA4_0 >= 'A' && LA4_0 <= 'F') || (LA4_0 >= 'a' && LA4_0 <= 'f')) )
            	    {
            	        alt4 = 1;
            	    }
            	    
            	
            	    switch (alt4) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:614:20: HEX_DIGIT
            			    {
            			    	mHEX_DIGIT(); 
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt4 >= 1 ) goto loop4;
            		            EarlyExitException eee =
            		                new EarlyExitException(4, input);
            		            throw eee;
            	    }
            	    cnt4++;
            	} while (true);
            	
            	loop4:
            		;	// Stops C# compiler whinging that label 'loop4' has no statements

            
            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end HEX_CONSTANT

    // $ANTLR start HEX_DIGIT 
    public void mHEX_DIGIT() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:618:2: ( DIGIT | 'a' .. 'f' | 'A' .. 'F' )
            int alt5 = 3;
            switch ( input.LA(1) ) 
            {
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
            	{
                alt5 = 1;
                }
                break;
            case 'a':
            case 'b':
            case 'c':
            case 'd':
            case 'e':
            case 'f':
            	{
                alt5 = 2;
                }
                break;
            case 'A':
            case 'B':
            case 'C':
            case 'D':
            case 'E':
            case 'F':
            	{
                alt5 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d5s0 =
            	        new NoViableAltException("617:10: fragment HEX_DIGIT : ( DIGIT | 'a' .. 'f' | 'A' .. 'F' );", 5, 0, input);
            
            	    throw nvae_d5s0;
            }
            
            switch (alt5) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:618:4: DIGIT
                    {
                    	mDIGIT(); 
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:618:10: 'a' .. 'f'
                    {
                    	MatchRange('a','f'); 
                    
                    }
                    break;
                case 3 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:618:19: 'A' .. 'F'
                    {
                    	MatchRange('A','F'); 
                    
                    }
                    break;
            
            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end HEX_DIGIT

    // $ANTLR start NUMBER 
    public void mNUMBER() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = NUMBER;
            Token m = null;
            Token f = null;
            Token e = null;
    
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:630:8: (m= DIGIT_SEQUENCE ( DOT f= DIGIT_SEQUENCE )? (e= EXPONENT )? ( FLOAT_SUFFIX )? )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:630:10: m= DIGIT_SEQUENCE ( DOT f= DIGIT_SEQUENCE )? (e= EXPONENT )? ( FLOAT_SUFFIX )?
            {
            	int mStart622 = CharIndex;
            	mDIGIT_SEQUENCE(); 
            	m = new CommonToken(input, Token.INVALID_TOKEN_TYPE, Token.DEFAULT_CHANNEL, mStart622, CharIndex-1);
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:630:27: ( DOT f= DIGIT_SEQUENCE )?
            	int alt6 = 2;
            	int LA6_0 = input.LA(1);
            	
            	if ( (LA6_0 == '.') )
            	{
            	    alt6 = 1;
            	}
            	switch (alt6) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:630:28: DOT f= DIGIT_SEQUENCE
            	        {
            	        	mDOT(); 
            	        	int fStart629 = CharIndex;
            	        	mDIGIT_SEQUENCE(); 
            	        	f = new CommonToken(input, Token.INVALID_TOKEN_TYPE, Token.DEFAULT_CHANNEL, fStart629, CharIndex-1);
            	        
            	        }
            	        break;
            	
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:630:51: (e= EXPONENT )?
            	int alt7 = 2;
            	int LA7_0 = input.LA(1);
            	
            	if ( (LA7_0 == 'E' || LA7_0 == 'e') )
            	{
            	    alt7 = 1;
            	}
            	switch (alt7) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:630:52: e= EXPONENT
            	        {
            	        	int eStart636 = CharIndex;
            	        	mEXPONENT(); 
            	        	e = new CommonToken(input, Token.INVALID_TOKEN_TYPE, Token.DEFAULT_CHANNEL, eStart636, CharIndex-1);
            	        
            	        }
            	        break;
            	
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:630:65: ( FLOAT_SUFFIX )?
            	int alt8 = 2;
            	int LA8_0 = input.LA(1);
            	
            	if ( (LA8_0 == 'F' || LA8_0 == 'f') )
            	{
            	    alt8 = 1;
            	}
            	switch (alt8) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:630:66: FLOAT_SUFFIX
            	        {
            	        	mFLOAT_SUFFIX(); 
            	        
            	        }
            	        break;
            	
            	}

            	
            		  string value = m.Text;
            		  if(f!=null)
            		  	value += "."+f.Text;		  	
            		  if(e!=null)
            		  	value += e.Text;
            	    	  Text = value;
            		
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end NUMBER

    // $ANTLR start DIGIT_SEQUENCE 
    public void mDIGIT_SEQUENCE() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:643:2: ( ( DIGIT )+ )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:643:4: ( DIGIT )+
            {
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:643:4: ( DIGIT )+
            	int cnt9 = 0;
            	do 
            	{
            	    int alt9 = 2;
            	    int LA9_0 = input.LA(1);
            	    
            	    if ( ((LA9_0 >= '0' && LA9_0 <= '9')) )
            	    {
            	        alt9 = 1;
            	    }
            	    
            	
            	    switch (alt9) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:643:4: DIGIT
            			    {
            			    	mDIGIT(); 
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt9 >= 1 ) goto loop9;
            		            EarlyExitException eee =
            		                new EarlyExitException(9, input);
            		            throw eee;
            	    }
            	    cnt9++;
            	} while (true);
            	
            	loop9:
            		;	// Stops C# compiler whinging that label 'loop9' has no statements

            
            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end DIGIT_SEQUENCE

    // $ANTLR start EXPONENT 
    public void mEXPONENT() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:648:2: ( ( 'e' | 'E' ) ( ADD | SUB )? DIGIT_SEQUENCE )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:648:4: ( 'e' | 'E' ) ( ADD | SUB )? DIGIT_SEQUENCE
            {
            	if ( input.LA(1) == 'E' || input.LA(1) == 'e' ) 
            	{
            	    input.Consume();
            	
            	}
            	else 
            	{
            	    MismatchedSetException mse =
            	        new MismatchedSetException(null,input);
            	    Recover(mse);    throw mse;
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:648:14: ( ADD | SUB )?
            	int alt10 = 2;
            	int LA10_0 = input.LA(1);
            	
            	if ( (LA10_0 == '+' || LA10_0 == '-') )
            	{
            	    alt10 = 1;
            	}
            	switch (alt10) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:
            	        {
            	        	if ( input.LA(1) == '+' || input.LA(1) == '-' ) 
            	        	{
            	        	    input.Consume();
            	        	
            	        	}
            	        	else 
            	        	{
            	        	    MismatchedSetException mse =
            	        	        new MismatchedSetException(null,input);
            	        	    Recover(mse);    throw mse;
            	        	}

            	        
            	        }
            	        break;
            	
            	}

            	mDIGIT_SEQUENCE(); 
            
            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end EXPONENT

    // $ANTLR start FLOAT_SUFFIX 
    public void mFLOAT_SUFFIX() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:652:2: ( 'f' | 'F' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:
            {
            	if ( input.LA(1) == 'F' || input.LA(1) == 'f' ) 
            	{
            	    input.Consume();
            	
            	}
            	else 
            	{
            	    MismatchedSetException mse =
            	        new MismatchedSetException(null,input);
            	    Recover(mse);    throw mse;
            	}

            
            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end FLOAT_SUFFIX

    // $ANTLR start BOOL_CONSTANT 
    public void mBOOL_CONSTANT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = BOOL_CONSTANT;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:656:2: ( 'true' | 'false' )
            int alt11 = 2;
            int LA11_0 = input.LA(1);
            
            if ( (LA11_0 == 't') )
            {
                alt11 = 1;
            }
            else if ( (LA11_0 == 'f') )
            {
                alt11 = 2;
            }
            else 
            {
                NoViableAltException nvae_d11s0 =
                    new NoViableAltException("655:1: BOOL_CONSTANT : ( 'true' | 'false' );", 11, 0, input);
            
                throw nvae_d11s0;
            }
            switch (alt11) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:656:4: 'true'
                    {
                    	Match("true"); 

                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:657:4: 'false'
                    {
                    	Match("false"); 

                    
                    }
                    break;
            
            }
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end BOOL_CONSTANT

    // $ANTLR start ID 
    public void mID() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = ID;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:661:5: ( ( LETTER | '_' ) ( LETTER | DIGIT | '_' )* )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:661:7: ( LETTER | '_' ) ( LETTER | DIGIT | '_' )*
            {
            	if ( (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') ) 
            	{
            	    input.Consume();
            	
            	}
            	else 
            	{
            	    MismatchedSetException mse =
            	        new MismatchedSetException(null,input);
            	    Recover(mse);    throw mse;
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:661:22: ( LETTER | DIGIT | '_' )*
            	do 
            	{
            	    int alt12 = 4;
            	    switch ( input.LA(1) ) 
            	    {
            	    case 'A':
            	    case 'B':
            	    case 'C':
            	    case 'D':
            	    case 'E':
            	    case 'F':
            	    case 'G':
            	    case 'H':
            	    case 'I':
            	    case 'J':
            	    case 'K':
            	    case 'L':
            	    case 'M':
            	    case 'N':
            	    case 'O':
            	    case 'P':
            	    case 'Q':
            	    case 'R':
            	    case 'S':
            	    case 'T':
            	    case 'U':
            	    case 'V':
            	    case 'W':
            	    case 'X':
            	    case 'Y':
            	    case 'Z':
            	    case 'a':
            	    case 'b':
            	    case 'c':
            	    case 'd':
            	    case 'e':
            	    case 'f':
            	    case 'g':
            	    case 'h':
            	    case 'i':
            	    case 'j':
            	    case 'k':
            	    case 'l':
            	    case 'm':
            	    case 'n':
            	    case 'o':
            	    case 'p':
            	    case 'q':
            	    case 'r':
            	    case 's':
            	    case 't':
            	    case 'u':
            	    case 'v':
            	    case 'w':
            	    case 'x':
            	    case 'y':
            	    case 'z':
            	    	{
            	        alt12 = 1;
            	        }
            	        break;
            	    case '0':
            	    case '1':
            	    case '2':
            	    case '3':
            	    case '4':
            	    case '5':
            	    case '6':
            	    case '7':
            	    case '8':
            	    case '9':
            	    	{
            	        alt12 = 2;
            	        }
            	        break;
            	    case '_':
            	    	{
            	        alt12 = 3;
            	        }
            	        break;
            	    
            	    }
            	
            	    switch (alt12) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:661:23: LETTER
            			    {
            			    	mLETTER(); 
            			    
            			    }
            			    break;
            			case 2 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:661:32: DIGIT
            			    {
            			    	mDIGIT(); 
            			    
            			    }
            			    break;
            			case 3 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:661:40: '_'
            			    {
            			    	Match('_'); 
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop12;
            	    }
            	} while (true);
            	
            	loop12:
            		;	// Stops C# compiler whinging that label 'loop12' has no statements

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end ID

    // $ANTLR start LETTER 
    public void mLETTER() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:665:2: ( ( 'a' .. 'z' | 'A' .. 'Z' ) )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:665:6: ( 'a' .. 'z' | 'A' .. 'Z' )
            {
            	if ( (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || (input.LA(1) >= 'a' && input.LA(1) <= 'z') ) 
            	{
            	    input.Consume();
            	
            	}
            	else 
            	{
            	    MismatchedSetException mse =
            	        new MismatchedSetException(null,input);
            	    Recover(mse);    throw mse;
            	}

            
            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end LETTER

    // $ANTLR start COMMENT 
    public void mCOMMENT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = COMMENT;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:670:5: ( '//' (~ ( '\\n' | '\\r' ) )* ( ( '\\r' )? '\\n' | EOF ) | '/*' ( options {greedy=false; } : . )* '*/' )
            int alt17 = 2;
            int LA17_0 = input.LA(1);
            
            if ( (LA17_0 == '/') )
            {
                int LA17_1 = input.LA(2);
                
                if ( (LA17_1 == '*') )
                {
                    alt17 = 2;
                }
                else if ( (LA17_1 == '/') )
                {
                    alt17 = 1;
                }
                else 
                {
                    NoViableAltException nvae_d17s1 =
                        new NoViableAltException("669:1: COMMENT : ( '//' (~ ( '\\n' | '\\r' ) )* ( ( '\\r' )? '\\n' | EOF ) | '/*' ( options {greedy=false; } : . )* '*/' );", 17, 1, input);
                
                    throw nvae_d17s1;
                }
            }
            else 
            {
                NoViableAltException nvae_d17s0 =
                    new NoViableAltException("669:1: COMMENT : ( '//' (~ ( '\\n' | '\\r' ) )* ( ( '\\r' )? '\\n' | EOF ) | '/*' ( options {greedy=false; } : . )* '*/' );", 17, 0, input);
            
                throw nvae_d17s0;
            }
            switch (alt17) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:670:9: '//' (~ ( '\\n' | '\\r' ) )* ( ( '\\r' )? '\\n' | EOF )
                    {
                    	Match("//"); 

                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:670:14: (~ ( '\\n' | '\\r' ) )*
                    	do 
                    	{
                    	    int alt13 = 2;
                    	    int LA13_0 = input.LA(1);
                    	    
                    	    if ( ((LA13_0 >= '\u0000' && LA13_0 <= '\t') || (LA13_0 >= '\u000B' && LA13_0 <= '\f') || (LA13_0 >= '\u000E' && LA13_0 <= '\uFFFE')) )
                    	    {
                    	        alt13 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt13) 
                    		{
                    			case 1 :
                    			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:670:14: ~ ( '\\n' | '\\r' )
                    			    {
                    			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || (input.LA(1) >= '\u000E' && input.LA(1) <= '\uFFFE') ) 
                    			    	{
                    			    	    input.Consume();
                    			    	
                    			    	}
                    			    	else 
                    			    	{
                    			    	    MismatchedSetException mse =
                    			    	        new MismatchedSetException(null,input);
                    			    	    Recover(mse);    throw mse;
                    			    	}

                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop13;
                    	    }
                    	} while (true);
                    	
                    	loop13:
                    		;	// Stops C# compiler whinging that label 'loop13' has no statements

                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:670:28: ( ( '\\r' )? '\\n' | EOF )
                    	int alt15 = 2;
                    	int LA15_0 = input.LA(1);
                    	
                    	if ( (LA15_0 == '\n' || LA15_0 == '\r') )
                    	{
                    	    alt15 = 1;
                    	}
                    	else 
                    	{
                    	    alt15 = 2;}
                    	switch (alt15) 
                    	{
                    	    case 1 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:670:29: ( '\\r' )? '\\n'
                    	        {
                    	        	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:670:29: ( '\\r' )?
                    	        	int alt14 = 2;
                    	        	int LA14_0 = input.LA(1);
                    	        	
                    	        	if ( (LA14_0 == '\r') )
                    	        	{
                    	        	    alt14 = 1;
                    	        	}
                    	        	switch (alt14) 
                    	        	{
                    	        	    case 1 :
                    	        	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:670:29: '\\r'
                    	        	        {
                    	        	        	Match('\r'); 
                    	        	        
                    	        	        }
                    	        	        break;
                    	        	
                    	        	}

                    	        	Match('\n'); 
                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:670:41: EOF
                    	        {
                    	        	Match(EOF); 
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	channel=HIDDEN;
                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:671:9: '/*' ( options {greedy=false; } : . )* '*/'
                    {
                    	Match("/*"); 

                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:671:14: ( options {greedy=false; } : . )*
                    	do 
                    	{
                    	    int alt16 = 2;
                    	    int LA16_0 = input.LA(1);
                    	    
                    	    if ( (LA16_0 == '*') )
                    	    {
                    	        int LA16_1 = input.LA(2);
                    	        
                    	        if ( (LA16_1 == '/') )
                    	        {
                    	            alt16 = 2;
                    	        }
                    	        else if ( ((LA16_1 >= '\u0000' && LA16_1 <= '.') || (LA16_1 >= '0' && LA16_1 <= '\uFFFE')) )
                    	        {
                    	            alt16 = 1;
                    	        }
                    	        
                    	    
                    	    }
                    	    else if ( ((LA16_0 >= '\u0000' && LA16_0 <= ')') || (LA16_0 >= '+' && LA16_0 <= '\uFFFE')) )
                    	    {
                    	        alt16 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt16) 
                    		{
                    			case 1 :
                    			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:671:42: .
                    			    {
                    			    	MatchAny(); 
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop16;
                    	    }
                    	} while (true);
                    	
                    	loop16:
                    		;	// Stops C# compiler whinging that label 'loop16' has no statements

                    	Match("*/"); 

                    	channel=HIDDEN;
                    
                    }
                    break;
            
            }
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end COMMENT

    // $ANTLR start WS 
    public void mWS() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = WS;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:674:5: ( ( ' ' | '\\t' | '\\r' | '\\n' ) )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:674:9: ( ' ' | '\\t' | '\\r' | '\\n' )
            {
            	if ( (input.LA(1) >= '\t' && input.LA(1) <= '\n') || input.LA(1) == '\r' || input.LA(1) == ' ' ) 
            	{
            	    input.Consume();
            	
            	}
            	else 
            	{
            	    MismatchedSetException mse =
            	        new MismatchedSetException(null,input);
            	    Recover(mse);    throw mse;
            	}

            	channel=HIDDEN;
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end WS

    // $ANTLR start STRING_LITERAL 
    public void mSTRING_LITERAL() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = STRING_LITERAL;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:677:5: ( '\"' ( EscapeSequence | ~ ( '\\\\' | '\"' ) )* '\"' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:677:8: '\"' ( EscapeSequence | ~ ( '\\\\' | '\"' ) )* '\"'
            {
            	Match('\"'); 
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:677:12: ( EscapeSequence | ~ ( '\\\\' | '\"' ) )*
            	do 
            	{
            	    int alt18 = 3;
            	    int LA18_0 = input.LA(1);
            	    
            	    if ( (LA18_0 == '\\') )
            	    {
            	        alt18 = 1;
            	    }
            	    else if ( ((LA18_0 >= '\u0000' && LA18_0 <= '!') || (LA18_0 >= '#' && LA18_0 <= '[') || (LA18_0 >= ']' && LA18_0 <= '\uFFFE')) )
            	    {
            	        alt18 = 2;
            	    }
            	    
            	
            	    switch (alt18) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:677:14: EscapeSequence
            			    {
            			    	mEscapeSequence(); 
            			    
            			    }
            			    break;
            			case 2 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:677:31: ~ ( '\\\\' | '\"' )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '!') || (input.LA(1) >= '#' && input.LA(1) <= '[') || (input.LA(1) >= ']' && input.LA(1) <= '\uFFFE') ) 
            			    	{
            			    	    input.Consume();
            			    	
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse =
            			    	        new MismatchedSetException(null,input);
            			    	    Recover(mse);    throw mse;
            			    	}

            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop18;
            	    }
            	} while (true);
            	
            	loop18:
            		;	// Stops C# compiler whinging that label 'loop18' has no statements

            	Match('\"'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end STRING_LITERAL

    // $ANTLR start EscapeSequence 
    public void mEscapeSequence() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:682:5: ( '\\\\' ( 'b' | 't' | 'n' | 'f' | 'r' | '\\\"' | '\\'' | '\\\\' ) | OctalEscape )
            int alt19 = 2;
            int LA19_0 = input.LA(1);
            
            if ( (LA19_0 == '\\') )
            {
                int LA19_1 = input.LA(2);
                
                if ( (LA19_1 == '\"' || LA19_1 == '\'' || LA19_1 == '\\' || LA19_1 == 'b' || LA19_1 == 'f' || LA19_1 == 'n' || LA19_1 == 'r' || LA19_1 == 't') )
                {
                    alt19 = 1;
                }
                else if ( ((LA19_1 >= '0' && LA19_1 <= '7')) )
                {
                    alt19 = 2;
                }
                else 
                {
                    NoViableAltException nvae_d19s1 =
                        new NoViableAltException("680:1: fragment EscapeSequence : ( '\\\\' ( 'b' | 't' | 'n' | 'f' | 'r' | '\\\"' | '\\'' | '\\\\' ) | OctalEscape );", 19, 1, input);
                
                    throw nvae_d19s1;
                }
            }
            else 
            {
                NoViableAltException nvae_d19s0 =
                    new NoViableAltException("680:1: fragment EscapeSequence : ( '\\\\' ( 'b' | 't' | 'n' | 'f' | 'r' | '\\\"' | '\\'' | '\\\\' ) | OctalEscape );", 19, 0, input);
            
                throw nvae_d19s0;
            }
            switch (alt19) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:682:9: '\\\\' ( 'b' | 't' | 'n' | 'f' | 'r' | '\\\"' | '\\'' | '\\\\' )
                    {
                    	Match('\\'); 
                    	if ( input.LA(1) == '\"' || input.LA(1) == '\'' || input.LA(1) == '\\' || input.LA(1) == 'b' || input.LA(1) == 'f' || input.LA(1) == 'n' || input.LA(1) == 'r' || input.LA(1) == 't' ) 
                    	{
                    	    input.Consume();
                    	
                    	}
                    	else 
                    	{
                    	    MismatchedSetException mse =
                    	        new MismatchedSetException(null,input);
                    	    Recover(mse);    throw mse;
                    	}

                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:683:9: OctalEscape
                    {
                    	mOctalEscape(); 
                    
                    }
                    break;
            
            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end EscapeSequence

    // $ANTLR start OctalEscape 
    public void mOctalEscape() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:687:5: ( '\\\\' ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) )
            int alt20 = 3;
            int LA20_0 = input.LA(1);
            
            if ( (LA20_0 == '\\') )
            {
                int LA20_1 = input.LA(2);
                
                if ( ((LA20_1 >= '0' && LA20_1 <= '3')) )
                {
                    int LA20_2 = input.LA(3);
                    
                    if ( ((LA20_2 >= '0' && LA20_2 <= '7')) )
                    {
                        int LA20_4 = input.LA(4);
                        
                        if ( ((LA20_4 >= '0' && LA20_4 <= '7')) )
                        {
                            alt20 = 1;
                        }
                        else 
                        {
                            alt20 = 2;}
                    }
                    else 
                    {
                        alt20 = 3;}
                }
                else if ( ((LA20_1 >= '4' && LA20_1 <= '7')) )
                {
                    int LA20_3 = input.LA(3);
                    
                    if ( ((LA20_3 >= '0' && LA20_3 <= '7')) )
                    {
                        alt20 = 2;
                    }
                    else 
                    {
                        alt20 = 3;}
                }
                else 
                {
                    NoViableAltException nvae_d20s1 =
                        new NoViableAltException("685:1: fragment OctalEscape : ( '\\\\' ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) );", 20, 1, input);
                
                    throw nvae_d20s1;
                }
            }
            else 
            {
                NoViableAltException nvae_d20s0 =
                    new NoViableAltException("685:1: fragment OctalEscape : ( '\\\\' ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) );", 20, 0, input);
            
                throw nvae_d20s0;
            }
            switch (alt20) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:687:9: '\\\\' ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' )
                    {
                    	Match('\\'); 
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:687:14: ( '0' .. '3' )
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:687:15: '0' .. '3'
                    	{
                    		MatchRange('0','3'); 
                    	
                    	}

                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:687:25: ( '0' .. '7' )
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:687:26: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 
                    	
                    	}

                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:687:36: ( '0' .. '7' )
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:687:37: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 
                    	
                    	}

                    
                    }
                    break;
                case 2 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:688:9: '\\\\' ( '0' .. '7' ) ( '0' .. '7' )
                    {
                    	Match('\\'); 
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:688:14: ( '0' .. '7' )
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:688:15: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 
                    	
                    	}

                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:688:25: ( '0' .. '7' )
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:688:26: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 
                    	
                    	}

                    
                    }
                    break;
                case 3 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:689:9: '\\\\' ( '0' .. '7' )
                    {
                    	Match('\\'); 
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:689:14: ( '0' .. '7' )
                    	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:689:15: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 
                    	
                    	}

                    
                    }
                    break;
            
            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end OctalEscape

    override public void mTokens() // throws RecognitionException 
    {
        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:8: ( EQUAL | NEQUAL | LESS | LEQUAL | GREATER | GEQUAL | NOT | MUL | DIV | ADD | INCREMENT | SUB | DECREMENT | OR | AND | XOR | DOT | COMMA | SEMICOLON | COLON | LBRACKET | RBRACKET | LPAREN | RPAREN | LBRACE | RBRACE | ASSIGN | ADDASSIGN | SUBASSIGN | MULASSIGN | DIVASSIGN | QUESTION | IF | ELSE | WHILE | DO | FOR | BREAK | CONTINUE | RETURN | DISCARD | CONST | UNIFORM | STATIC | CENTROID | LINEAR | NOINTERP | IN | OUT | INOUT | STRUCT | REG | PACK | CBUFFER | TBUFFER | NUMBER | BOOL_CONSTANT | ID | COMMENT | WS | STRING_LITERAL )
        int alt21 = 61;
        switch ( input.LA(1) ) 
        {
        case '=':
        	{
            int LA21_1 = input.LA(2);
            
            if ( (LA21_1 == '=') )
            {
                alt21 = 1;
            }
            else 
            {
                alt21 = 27;}
            }
            break;
        case '!':
        	{
            int LA21_2 = input.LA(2);
            
            if ( (LA21_2 == '=') )
            {
                alt21 = 2;
            }
            else 
            {
                alt21 = 7;}
            }
            break;
        case '<':
        	{
            int LA21_3 = input.LA(2);
            
            if ( (LA21_3 == '=') )
            {
                alt21 = 4;
            }
            else 
            {
                alt21 = 3;}
            }
            break;
        case '>':
        	{
            int LA21_4 = input.LA(2);
            
            if ( (LA21_4 == '=') )
            {
                alt21 = 6;
            }
            else 
            {
                alt21 = 5;}
            }
            break;
        case '*':
        	{
            int LA21_5 = input.LA(2);
            
            if ( (LA21_5 == '=') )
            {
                alt21 = 30;
            }
            else 
            {
                alt21 = 8;}
            }
            break;
        case '/':
        	{
            switch ( input.LA(2) ) 
            {
            case '*':
            case '/':
            	{
                alt21 = 59;
                }
                break;
            case '=':
            	{
                alt21 = 31;
                }
                break;
            	default:
                	alt21 = 9;
                	break;}
        
            }
            break;
        case '+':
        	{
            switch ( input.LA(2) ) 
            {
            case '+':
            	{
                alt21 = 11;
                }
                break;
            case '=':
            	{
                alt21 = 28;
                }
                break;
            	default:
                	alt21 = 10;
                	break;}
        
            }
            break;
        case '-':
        	{
            switch ( input.LA(2) ) 
            {
            case '-':
            	{
                alt21 = 13;
                }
                break;
            case '=':
            	{
                alt21 = 29;
                }
                break;
            	default:
                	alt21 = 12;
                	break;}
        
            }
            break;
        case '|':
        	{
            alt21 = 14;
            }
            break;
        case '&':
        	{
            alt21 = 15;
            }
            break;
        case '^':
        	{
            alt21 = 16;
            }
            break;
        case '.':
        	{
            alt21 = 17;
            }
            break;
        case ',':
        	{
            alt21 = 18;
            }
            break;
        case ';':
        	{
            alt21 = 19;
            }
            break;
        case ':':
        	{
            alt21 = 20;
            }
            break;
        case '[':
        	{
            alt21 = 21;
            }
            break;
        case ']':
        	{
            alt21 = 22;
            }
            break;
        case '(':
        	{
            alt21 = 23;
            }
            break;
        case ')':
        	{
            alt21 = 24;
            }
            break;
        case '{':
        	{
            alt21 = 25;
            }
            break;
        case '}':
        	{
            alt21 = 26;
            }
            break;
        case '?':
        	{
            alt21 = 32;
            }
            break;
        case 'i':
        	{
            switch ( input.LA(2) ) 
            {
            case 'n':
            	{
                switch ( input.LA(3) ) 
                {
                case 'o':
                	{
                    int LA21_82 = input.LA(4);
                    
                    if ( (LA21_82 == 'u') )
                    {
                        int LA21_106 = input.LA(5);
                        
                        if ( (LA21_106 == 't') )
                        {
                            int LA21_128 = input.LA(6);
                            
                            if ( ((LA21_128 >= '0' && LA21_128 <= '9') || (LA21_128 >= 'A' && LA21_128 <= 'Z') || LA21_128 == '_' || (LA21_128 >= 'a' && LA21_128 <= 'z')) )
                            {
                                alt21 = 58;
                            }
                            else 
                            {
                                alt21 = 50;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                    }
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                case '_':
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                	{
                    alt21 = 58;
                    }
                    break;
                	default:
                    	alt21 = 48;
                    	break;}
            
                }
                break;
            case 'f':
            	{
                int LA21_62 = input.LA(3);
                
                if ( ((LA21_62 >= '0' && LA21_62 <= '9') || (LA21_62 >= 'A' && LA21_62 <= 'Z') || LA21_62 == '_' || (LA21_62 >= 'a' && LA21_62 <= 'z')) )
                {
                    alt21 = 58;
                }
                else 
                {
                    alt21 = 33;}
                }
                break;
            	default:
                	alt21 = 58;
                	break;}
        
            }
            break;
        case 'e':
        	{
            int LA21_24 = input.LA(2);
            
            if ( (LA21_24 == 'l') )
            {
                int LA21_63 = input.LA(3);
                
                if ( (LA21_63 == 's') )
                {
                    int LA21_85 = input.LA(4);
                    
                    if ( (LA21_85 == 'e') )
                    {
                        int LA21_107 = input.LA(5);
                        
                        if ( ((LA21_107 >= '0' && LA21_107 <= '9') || (LA21_107 >= 'A' && LA21_107 <= 'Z') || LA21_107 == '_' || (LA21_107 >= 'a' && LA21_107 <= 'z')) )
                        {
                            alt21 = 58;
                        }
                        else 
                        {
                            alt21 = 34;}
                    }
                    else 
                    {
                        alt21 = 58;}
                }
                else 
                {
                    alt21 = 58;}
            }
            else 
            {
                alt21 = 58;}
            }
            break;
        case 'w':
        	{
            int LA21_25 = input.LA(2);
            
            if ( (LA21_25 == 'h') )
            {
                int LA21_64 = input.LA(3);
                
                if ( (LA21_64 == 'i') )
                {
                    int LA21_86 = input.LA(4);
                    
                    if ( (LA21_86 == 'l') )
                    {
                        int LA21_108 = input.LA(5);
                        
                        if ( (LA21_108 == 'e') )
                        {
                            int LA21_130 = input.LA(6);
                            
                            if ( ((LA21_130 >= '0' && LA21_130 <= '9') || (LA21_130 >= 'A' && LA21_130 <= 'Z') || LA21_130 == '_' || (LA21_130 >= 'a' && LA21_130 <= 'z')) )
                            {
                                alt21 = 58;
                            }
                            else 
                            {
                                alt21 = 35;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                }
                else 
                {
                    alt21 = 58;}
            }
            else 
            {
                alt21 = 58;}
            }
            break;
        case 'd':
        	{
            switch ( input.LA(2) ) 
            {
            case 'i':
            	{
                int LA21_65 = input.LA(3);
                
                if ( (LA21_65 == 's') )
                {
                    int LA21_87 = input.LA(4);
                    
                    if ( (LA21_87 == 'c') )
                    {
                        int LA21_109 = input.LA(5);
                        
                        if ( (LA21_109 == 'a') )
                        {
                            int LA21_131 = input.LA(6);
                            
                            if ( (LA21_131 == 'r') )
                            {
                                int LA21_150 = input.LA(7);
                                
                                if ( (LA21_150 == 'd') )
                                {
                                    int LA21_165 = input.LA(8);
                                    
                                    if ( ((LA21_165 >= '0' && LA21_165 <= '9') || (LA21_165 >= 'A' && LA21_165 <= 'Z') || LA21_165 == '_' || (LA21_165 >= 'a' && LA21_165 <= 'z')) )
                                    {
                                        alt21 = 58;
                                    }
                                    else 
                                    {
                                        alt21 = 41;}
                                }
                                else 
                                {
                                    alt21 = 58;}
                            }
                            else 
                            {
                                alt21 = 58;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                }
                else 
                {
                    alt21 = 58;}
                }
                break;
            case 'o':
            	{
                int LA21_66 = input.LA(3);
                
                if ( ((LA21_66 >= '0' && LA21_66 <= '9') || (LA21_66 >= 'A' && LA21_66 <= 'Z') || LA21_66 == '_' || (LA21_66 >= 'a' && LA21_66 <= 'z')) )
                {
                    alt21 = 58;
                }
                else 
                {
                    alt21 = 36;}
                }
                break;
            	default:
                	alt21 = 58;
                	break;}
        
            }
            break;
        case 'f':
        	{
            switch ( input.LA(2) ) 
            {
            case 'o':
            	{
                int LA21_67 = input.LA(3);
                
                if ( (LA21_67 == 'r') )
                {
                    int LA21_89 = input.LA(4);
                    
                    if ( ((LA21_89 >= '0' && LA21_89 <= '9') || (LA21_89 >= 'A' && LA21_89 <= 'Z') || LA21_89 == '_' || (LA21_89 >= 'a' && LA21_89 <= 'z')) )
                    {
                        alt21 = 58;
                    }
                    else 
                    {
                        alt21 = 37;}
                }
                else 
                {
                    alt21 = 58;}
                }
                break;
            case 'a':
            	{
                int LA21_68 = input.LA(3);
                
                if ( (LA21_68 == 'l') )
                {
                    int LA21_90 = input.LA(4);
                    
                    if ( (LA21_90 == 's') )
                    {
                        int LA21_111 = input.LA(5);
                        
                        if ( (LA21_111 == 'e') )
                        {
                            int LA21_132 = input.LA(6);
                            
                            if ( ((LA21_132 >= '0' && LA21_132 <= '9') || (LA21_132 >= 'A' && LA21_132 <= 'Z') || LA21_132 == '_' || (LA21_132 >= 'a' && LA21_132 <= 'z')) )
                            {
                                alt21 = 58;
                            }
                            else 
                            {
                                alt21 = 57;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                }
                else 
                {
                    alt21 = 58;}
                }
                break;
            	default:
                	alt21 = 58;
                	break;}
        
            }
            break;
        case 'b':
        	{
            int LA21_28 = input.LA(2);
            
            if ( (LA21_28 == 'r') )
            {
                int LA21_69 = input.LA(3);
                
                if ( (LA21_69 == 'e') )
                {
                    int LA21_91 = input.LA(4);
                    
                    if ( (LA21_91 == 'a') )
                    {
                        int LA21_112 = input.LA(5);
                        
                        if ( (LA21_112 == 'k') )
                        {
                            int LA21_133 = input.LA(6);
                            
                            if ( ((LA21_133 >= '0' && LA21_133 <= '9') || (LA21_133 >= 'A' && LA21_133 <= 'Z') || LA21_133 == '_' || (LA21_133 >= 'a' && LA21_133 <= 'z')) )
                            {
                                alt21 = 58;
                            }
                            else 
                            {
                                alt21 = 38;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                }
                else 
                {
                    alt21 = 58;}
            }
            else 
            {
                alt21 = 58;}
            }
            break;
        case 'c':
        	{
            switch ( input.LA(2) ) 
            {
            case 'o':
            	{
                int LA21_70 = input.LA(3);
                
                if ( (LA21_70 == 'n') )
                {
                    switch ( input.LA(4) ) 
                    {
                    case 's':
                    	{
                        int LA21_113 = input.LA(5);
                        
                        if ( (LA21_113 == 't') )
                        {
                            int LA21_134 = input.LA(6);
                            
                            if ( ((LA21_134 >= '0' && LA21_134 <= '9') || (LA21_134 >= 'A' && LA21_134 <= 'Z') || LA21_134 == '_' || (LA21_134 >= 'a' && LA21_134 <= 'z')) )
                            {
                                alt21 = 58;
                            }
                            else 
                            {
                                alt21 = 42;}
                        }
                        else 
                        {
                            alt21 = 58;}
                        }
                        break;
                    case 't':
                    	{
                        int LA21_114 = input.LA(5);
                        
                        if ( (LA21_114 == 'i') )
                        {
                            int LA21_135 = input.LA(6);
                            
                            if ( (LA21_135 == 'n') )
                            {
                                int LA21_153 = input.LA(7);
                                
                                if ( (LA21_153 == 'u') )
                                {
                                    int LA21_166 = input.LA(8);
                                    
                                    if ( (LA21_166 == 'e') )
                                    {
                                        int LA21_179 = input.LA(9);
                                        
                                        if ( ((LA21_179 >= '0' && LA21_179 <= '9') || (LA21_179 >= 'A' && LA21_179 <= 'Z') || LA21_179 == '_' || (LA21_179 >= 'a' && LA21_179 <= 'z')) )
                                        {
                                            alt21 = 58;
                                        }
                                        else 
                                        {
                                            alt21 = 39;}
                                    }
                                    else 
                                    {
                                        alt21 = 58;}
                                }
                                else 
                                {
                                    alt21 = 58;}
                            }
                            else 
                            {
                                alt21 = 58;}
                        }
                        else 
                        {
                            alt21 = 58;}
                        }
                        break;
                    	default:
                        	alt21 = 58;
                        	break;}
                
                }
                else 
                {
                    alt21 = 58;}
                }
                break;
            case 'e':
            	{
                int LA21_71 = input.LA(3);
                
                if ( (LA21_71 == 'n') )
                {
                    int LA21_93 = input.LA(4);
                    
                    if ( (LA21_93 == 't') )
                    {
                        int LA21_115 = input.LA(5);
                        
                        if ( (LA21_115 == 'r') )
                        {
                            int LA21_136 = input.LA(6);
                            
                            if ( (LA21_136 == 'o') )
                            {
                                int LA21_154 = input.LA(7);
                                
                                if ( (LA21_154 == 'i') )
                                {
                                    int LA21_167 = input.LA(8);
                                    
                                    if ( (LA21_167 == 'd') )
                                    {
                                        int LA21_180 = input.LA(9);
                                        
                                        if ( ((LA21_180 >= '0' && LA21_180 <= '9') || (LA21_180 >= 'A' && LA21_180 <= 'Z') || LA21_180 == '_' || (LA21_180 >= 'a' && LA21_180 <= 'z')) )
                                        {
                                            alt21 = 58;
                                        }
                                        else 
                                        {
                                            alt21 = 45;}
                                    }
                                    else 
                                    {
                                        alt21 = 58;}
                                }
                                else 
                                {
                                    alt21 = 58;}
                            }
                            else 
                            {
                                alt21 = 58;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                }
                else 
                {
                    alt21 = 58;}
                }
                break;
            case 'b':
            	{
                int LA21_72 = input.LA(3);
                
                if ( (LA21_72 == 'u') )
                {
                    int LA21_94 = input.LA(4);
                    
                    if ( (LA21_94 == 'f') )
                    {
                        int LA21_116 = input.LA(5);
                        
                        if ( (LA21_116 == 'f') )
                        {
                            int LA21_137 = input.LA(6);
                            
                            if ( (LA21_137 == 'e') )
                            {
                                int LA21_155 = input.LA(7);
                                
                                if ( (LA21_155 == 'r') )
                                {
                                    int LA21_168 = input.LA(8);
                                    
                                    if ( ((LA21_168 >= '0' && LA21_168 <= '9') || (LA21_168 >= 'A' && LA21_168 <= 'Z') || LA21_168 == '_' || (LA21_168 >= 'a' && LA21_168 <= 'z')) )
                                    {
                                        alt21 = 58;
                                    }
                                    else 
                                    {
                                        alt21 = 54;}
                                }
                                else 
                                {
                                    alt21 = 58;}
                            }
                            else 
                            {
                                alt21 = 58;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                }
                else 
                {
                    alt21 = 58;}
                }
                break;
            	default:
                	alt21 = 58;
                	break;}
        
            }
            break;
        case 'r':
        	{
            int LA21_30 = input.LA(2);
            
            if ( (LA21_30 == 'e') )
            {
                switch ( input.LA(3) ) 
                {
                case 'g':
                	{
                    int LA21_95 = input.LA(4);
                    
                    if ( (LA21_95 == 'i') )
                    {
                        int LA21_117 = input.LA(5);
                        
                        if ( (LA21_117 == 's') )
                        {
                            int LA21_138 = input.LA(6);
                            
                            if ( (LA21_138 == 't') )
                            {
                                int LA21_156 = input.LA(7);
                                
                                if ( (LA21_156 == 'e') )
                                {
                                    int LA21_169 = input.LA(8);
                                    
                                    if ( (LA21_169 == 'r') )
                                    {
                                        int LA21_182 = input.LA(9);
                                        
                                        if ( ((LA21_182 >= '0' && LA21_182 <= '9') || (LA21_182 >= 'A' && LA21_182 <= 'Z') || LA21_182 == '_' || (LA21_182 >= 'a' && LA21_182 <= 'z')) )
                                        {
                                            alt21 = 58;
                                        }
                                        else 
                                        {
                                            alt21 = 52;}
                                    }
                                    else 
                                    {
                                        alt21 = 58;}
                                }
                                else 
                                {
                                    alt21 = 58;}
                            }
                            else 
                            {
                                alt21 = 58;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                    }
                    break;
                case 't':
                	{
                    int LA21_96 = input.LA(4);
                    
                    if ( (LA21_96 == 'u') )
                    {
                        int LA21_118 = input.LA(5);
                        
                        if ( (LA21_118 == 'r') )
                        {
                            int LA21_139 = input.LA(6);
                            
                            if ( (LA21_139 == 'n') )
                            {
                                int LA21_157 = input.LA(7);
                                
                                if ( ((LA21_157 >= '0' && LA21_157 <= '9') || (LA21_157 >= 'A' && LA21_157 <= 'Z') || LA21_157 == '_' || (LA21_157 >= 'a' && LA21_157 <= 'z')) )
                                {
                                    alt21 = 58;
                                }
                                else 
                                {
                                    alt21 = 40;}
                            }
                            else 
                            {
                                alt21 = 58;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                    }
                    break;
                	default:
                    	alt21 = 58;
                    	break;}
            
            }
            else 
            {
                alt21 = 58;}
            }
            break;
        case 'u':
        	{
            int LA21_31 = input.LA(2);
            
            if ( (LA21_31 == 'n') )
            {
                int LA21_74 = input.LA(3);
                
                if ( (LA21_74 == 'i') )
                {
                    int LA21_97 = input.LA(4);
                    
                    if ( (LA21_97 == 'f') )
                    {
                        int LA21_119 = input.LA(5);
                        
                        if ( (LA21_119 == 'o') )
                        {
                            int LA21_140 = input.LA(6);
                            
                            if ( (LA21_140 == 'r') )
                            {
                                int LA21_158 = input.LA(7);
                                
                                if ( (LA21_158 == 'm') )
                                {
                                    int LA21_171 = input.LA(8);
                                    
                                    if ( ((LA21_171 >= '0' && LA21_171 <= '9') || (LA21_171 >= 'A' && LA21_171 <= 'Z') || LA21_171 == '_' || (LA21_171 >= 'a' && LA21_171 <= 'z')) )
                                    {
                                        alt21 = 58;
                                    }
                                    else 
                                    {
                                        alt21 = 43;}
                                }
                                else 
                                {
                                    alt21 = 58;}
                            }
                            else 
                            {
                                alt21 = 58;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                }
                else 
                {
                    alt21 = 58;}
            }
            else 
            {
                alt21 = 58;}
            }
            break;
        case 's':
        	{
            int LA21_32 = input.LA(2);
            
            if ( (LA21_32 == 't') )
            {
                switch ( input.LA(3) ) 
                {
                case 'a':
                	{
                    int LA21_98 = input.LA(4);
                    
                    if ( (LA21_98 == 't') )
                    {
                        int LA21_120 = input.LA(5);
                        
                        if ( (LA21_120 == 'i') )
                        {
                            int LA21_141 = input.LA(6);
                            
                            if ( (LA21_141 == 'c') )
                            {
                                int LA21_159 = input.LA(7);
                                
                                if ( ((LA21_159 >= '0' && LA21_159 <= '9') || (LA21_159 >= 'A' && LA21_159 <= 'Z') || LA21_159 == '_' || (LA21_159 >= 'a' && LA21_159 <= 'z')) )
                                {
                                    alt21 = 58;
                                }
                                else 
                                {
                                    alt21 = 44;}
                            }
                            else 
                            {
                                alt21 = 58;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                    }
                    break;
                case 'r':
                	{
                    int LA21_99 = input.LA(4);
                    
                    if ( (LA21_99 == 'u') )
                    {
                        int LA21_121 = input.LA(5);
                        
                        if ( (LA21_121 == 'c') )
                        {
                            int LA21_142 = input.LA(6);
                            
                            if ( (LA21_142 == 't') )
                            {
                                int LA21_160 = input.LA(7);
                                
                                if ( ((LA21_160 >= '0' && LA21_160 <= '9') || (LA21_160 >= 'A' && LA21_160 <= 'Z') || LA21_160 == '_' || (LA21_160 >= 'a' && LA21_160 <= 'z')) )
                                {
                                    alt21 = 58;
                                }
                                else 
                                {
                                    alt21 = 51;}
                            }
                            else 
                            {
                                alt21 = 58;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                    }
                    break;
                	default:
                    	alt21 = 58;
                    	break;}
            
            }
            else 
            {
                alt21 = 58;}
            }
            break;
        case 'l':
        	{
            int LA21_33 = input.LA(2);
            
            if ( (LA21_33 == 'i') )
            {
                int LA21_76 = input.LA(3);
                
                if ( (LA21_76 == 'n') )
                {
                    int LA21_100 = input.LA(4);
                    
                    if ( (LA21_100 == 'e') )
                    {
                        int LA21_122 = input.LA(5);
                        
                        if ( (LA21_122 == 'a') )
                        {
                            int LA21_143 = input.LA(6);
                            
                            if ( (LA21_143 == 'r') )
                            {
                                int LA21_161 = input.LA(7);
                                
                                if ( ((LA21_161 >= '0' && LA21_161 <= '9') || (LA21_161 >= 'A' && LA21_161 <= 'Z') || LA21_161 == '_' || (LA21_161 >= 'a' && LA21_161 <= 'z')) )
                                {
                                    alt21 = 58;
                                }
                                else 
                                {
                                    alt21 = 46;}
                            }
                            else 
                            {
                                alt21 = 58;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                }
                else 
                {
                    alt21 = 58;}
            }
            else 
            {
                alt21 = 58;}
            }
            break;
        case 'n':
        	{
            int LA21_34 = input.LA(2);
            
            if ( (LA21_34 == 'o') )
            {
                int LA21_77 = input.LA(3);
                
                if ( (LA21_77 == 'i') )
                {
                    int LA21_101 = input.LA(4);
                    
                    if ( (LA21_101 == 'n') )
                    {
                        int LA21_123 = input.LA(5);
                        
                        if ( (LA21_123 == 't') )
                        {
                            int LA21_144 = input.LA(6);
                            
                            if ( (LA21_144 == 'e') )
                            {
                                int LA21_162 = input.LA(7);
                                
                                if ( (LA21_162 == 'r') )
                                {
                                    int LA21_175 = input.LA(8);
                                    
                                    if ( (LA21_175 == 'p') )
                                    {
                                        int LA21_184 = input.LA(9);
                                        
                                        if ( (LA21_184 == 'o') )
                                        {
                                            int LA21_190 = input.LA(10);
                                            
                                            if ( (LA21_190 == 'l') )
                                            {
                                                int LA21_192 = input.LA(11);
                                                
                                                if ( (LA21_192 == 'a') )
                                                {
                                                    int LA21_194 = input.LA(12);
                                                    
                                                    if ( (LA21_194 == 't') )
                                                    {
                                                        int LA21_196 = input.LA(13);
                                                        
                                                        if ( (LA21_196 == 'i') )
                                                        {
                                                            int LA21_197 = input.LA(14);
                                                            
                                                            if ( (LA21_197 == 'o') )
                                                            {
                                                                int LA21_198 = input.LA(15);
                                                                
                                                                if ( (LA21_198 == 'n') )
                                                                {
                                                                    int LA21_199 = input.LA(16);
                                                                    
                                                                    if ( ((LA21_199 >= '0' && LA21_199 <= '9') || (LA21_199 >= 'A' && LA21_199 <= 'Z') || LA21_199 == '_' || (LA21_199 >= 'a' && LA21_199 <= 'z')) )
                                                                    {
                                                                        alt21 = 58;
                                                                    }
                                                                    else 
                                                                    {
                                                                        alt21 = 47;}
                                                                }
                                                                else 
                                                                {
                                                                    alt21 = 58;}
                                                            }
                                                            else 
                                                            {
                                                                alt21 = 58;}
                                                        }
                                                        else 
                                                        {
                                                            alt21 = 58;}
                                                    }
                                                    else 
                                                    {
                                                        alt21 = 58;}
                                                }
                                                else 
                                                {
                                                    alt21 = 58;}
                                            }
                                            else 
                                            {
                                                alt21 = 58;}
                                        }
                                        else 
                                        {
                                            alt21 = 58;}
                                    }
                                    else 
                                    {
                                        alt21 = 58;}
                                }
                                else 
                                {
                                    alt21 = 58;}
                            }
                            else 
                            {
                                alt21 = 58;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                }
                else 
                {
                    alt21 = 58;}
            }
            else 
            {
                alt21 = 58;}
            }
            break;
        case 'o':
        	{
            int LA21_35 = input.LA(2);
            
            if ( (LA21_35 == 'u') )
            {
                int LA21_78 = input.LA(3);
                
                if ( (LA21_78 == 't') )
                {
                    int LA21_102 = input.LA(4);
                    
                    if ( ((LA21_102 >= '0' && LA21_102 <= '9') || (LA21_102 >= 'A' && LA21_102 <= 'Z') || LA21_102 == '_' || (LA21_102 >= 'a' && LA21_102 <= 'z')) )
                    {
                        alt21 = 58;
                    }
                    else 
                    {
                        alt21 = 49;}
                }
                else 
                {
                    alt21 = 58;}
            }
            else 
            {
                alt21 = 58;}
            }
            break;
        case 'p':
        	{
            int LA21_36 = input.LA(2);
            
            if ( (LA21_36 == 'a') )
            {
                int LA21_79 = input.LA(3);
                
                if ( (LA21_79 == 'c') )
                {
                    int LA21_103 = input.LA(4);
                    
                    if ( (LA21_103 == 'k') )
                    {
                        int LA21_125 = input.LA(5);
                        
                        if ( (LA21_125 == 'o') )
                        {
                            int LA21_145 = input.LA(6);
                            
                            if ( (LA21_145 == 'f') )
                            {
                                int LA21_163 = input.LA(7);
                                
                                if ( (LA21_163 == 'f') )
                                {
                                    int LA21_176 = input.LA(8);
                                    
                                    if ( (LA21_176 == 's') )
                                    {
                                        int LA21_185 = input.LA(9);
                                        
                                        if ( (LA21_185 == 'e') )
                                        {
                                            int LA21_191 = input.LA(10);
                                            
                                            if ( (LA21_191 == 't') )
                                            {
                                                int LA21_193 = input.LA(11);
                                                
                                                if ( ((LA21_193 >= '0' && LA21_193 <= '9') || (LA21_193 >= 'A' && LA21_193 <= 'Z') || LA21_193 == '_' || (LA21_193 >= 'a' && LA21_193 <= 'z')) )
                                                {
                                                    alt21 = 58;
                                                }
                                                else 
                                                {
                                                    alt21 = 53;}
                                            }
                                            else 
                                            {
                                                alt21 = 58;}
                                        }
                                        else 
                                        {
                                            alt21 = 58;}
                                    }
                                    else 
                                    {
                                        alt21 = 58;}
                                }
                                else 
                                {
                                    alt21 = 58;}
                            }
                            else 
                            {
                                alt21 = 58;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                }
                else 
                {
                    alt21 = 58;}
            }
            else 
            {
                alt21 = 58;}
            }
            break;
        case 't':
        	{
            switch ( input.LA(2) ) 
            {
            case 'b':
            	{
                int LA21_80 = input.LA(3);
                
                if ( (LA21_80 == 'u') )
                {
                    int LA21_104 = input.LA(4);
                    
                    if ( (LA21_104 == 'f') )
                    {
                        int LA21_126 = input.LA(5);
                        
                        if ( (LA21_126 == 'f') )
                        {
                            int LA21_146 = input.LA(6);
                            
                            if ( (LA21_146 == 'e') )
                            {
                                int LA21_164 = input.LA(7);
                                
                                if ( (LA21_164 == 'r') )
                                {
                                    int LA21_177 = input.LA(8);
                                    
                                    if ( ((LA21_177 >= '0' && LA21_177 <= '9') || (LA21_177 >= 'A' && LA21_177 <= 'Z') || LA21_177 == '_' || (LA21_177 >= 'a' && LA21_177 <= 'z')) )
                                    {
                                        alt21 = 58;
                                    }
                                    else 
                                    {
                                        alt21 = 55;}
                                }
                                else 
                                {
                                    alt21 = 58;}
                            }
                            else 
                            {
                                alt21 = 58;}
                        }
                        else 
                        {
                            alt21 = 58;}
                    }
                    else 
                    {
                        alt21 = 58;}
                }
                else 
                {
                    alt21 = 58;}
                }
                break;
            case 'r':
            	{
                int LA21_81 = input.LA(3);
                
                if ( (LA21_81 == 'u') )
                {
                    int LA21_105 = input.LA(4);
                    
                    if ( (LA21_105 == 'e') )
                    {
                        int LA21_127 = input.LA(5);
                        
                        if ( ((LA21_127 >= '0' && LA21_127 <= '9') || (LA21_127 >= 'A' && LA21_127 <= 'Z') || LA21_127 == '_' || (LA21_127 >= 'a' && LA21_127 <= 'z')) )
                        {
                            alt21 = 58;
                        }
                        else 
                        {
                            alt21 = 57;}
                    }
                    else 
                    {
                        alt21 = 58;}
                }
                else 
                {
                    alt21 = 58;}
                }
                break;
            	default:
                	alt21 = 58;
                	break;}
        
            }
            break;
        case '0':
        case '1':
        case '2':
        case '3':
        case '4':
        case '5':
        case '6':
        case '7':
        case '8':
        case '9':
        	{
            alt21 = 56;
            }
            break;
        case 'A':
        case 'B':
        case 'C':
        case 'D':
        case 'E':
        case 'F':
        case 'G':
        case 'H':
        case 'I':
        case 'J':
        case 'K':
        case 'L':
        case 'M':
        case 'N':
        case 'O':
        case 'P':
        case 'Q':
        case 'R':
        case 'S':
        case 'T':
        case 'U':
        case 'V':
        case 'W':
        case 'X':
        case 'Y':
        case 'Z':
        case '_':
        case 'a':
        case 'g':
        case 'h':
        case 'j':
        case 'k':
        case 'm':
        case 'q':
        case 'v':
        case 'x':
        case 'y':
        case 'z':
        	{
            alt21 = 58;
            }
            break;
        case '\t':
        case '\n':
        case '\r':
        case ' ':
        	{
            alt21 = 60;
            }
            break;
        case '\"':
        	{
            alt21 = 61;
            }
            break;
        	default:
        	    NoViableAltException nvae_d21s0 =
        	        new NoViableAltException("1:1: Tokens : ( EQUAL | NEQUAL | LESS | LEQUAL | GREATER | GEQUAL | NOT | MUL | DIV | ADD | INCREMENT | SUB | DECREMENT | OR | AND | XOR | DOT | COMMA | SEMICOLON | COLON | LBRACKET | RBRACKET | LPAREN | RPAREN | LBRACE | RBRACE | ASSIGN | ADDASSIGN | SUBASSIGN | MULASSIGN | DIVASSIGN | QUESTION | IF | ELSE | WHILE | DO | FOR | BREAK | CONTINUE | RETURN | DISCARD | CONST | UNIFORM | STATIC | CENTROID | LINEAR | NOINTERP | IN | OUT | INOUT | STRUCT | REG | PACK | CBUFFER | TBUFFER | NUMBER | BOOL_CONSTANT | ID | COMMENT | WS | STRING_LITERAL );", 21, 0, input);
        
        	    throw nvae_d21s0;
        }
        
        switch (alt21) 
        {
            case 1 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:10: EQUAL
                {
                	mEQUAL(); 
                
                }
                break;
            case 2 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:16: NEQUAL
                {
                	mNEQUAL(); 
                
                }
                break;
            case 3 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:23: LESS
                {
                	mLESS(); 
                
                }
                break;
            case 4 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:28: LEQUAL
                {
                	mLEQUAL(); 
                
                }
                break;
            case 5 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:35: GREATER
                {
                	mGREATER(); 
                
                }
                break;
            case 6 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:43: GEQUAL
                {
                	mGEQUAL(); 
                
                }
                break;
            case 7 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:50: NOT
                {
                	mNOT(); 
                
                }
                break;
            case 8 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:54: MUL
                {
                	mMUL(); 
                
                }
                break;
            case 9 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:58: DIV
                {
                	mDIV(); 
                
                }
                break;
            case 10 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:62: ADD
                {
                	mADD(); 
                
                }
                break;
            case 11 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:66: INCREMENT
                {
                	mINCREMENT(); 
                
                }
                break;
            case 12 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:76: SUB
                {
                	mSUB(); 
                
                }
                break;
            case 13 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:80: DECREMENT
                {
                	mDECREMENT(); 
                
                }
                break;
            case 14 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:90: OR
                {
                	mOR(); 
                
                }
                break;
            case 15 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:93: AND
                {
                	mAND(); 
                
                }
                break;
            case 16 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:97: XOR
                {
                	mXOR(); 
                
                }
                break;
            case 17 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:101: DOT
                {
                	mDOT(); 
                
                }
                break;
            case 18 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:105: COMMA
                {
                	mCOMMA(); 
                
                }
                break;
            case 19 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:111: SEMICOLON
                {
                	mSEMICOLON(); 
                
                }
                break;
            case 20 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:121: COLON
                {
                	mCOLON(); 
                
                }
                break;
            case 21 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:127: LBRACKET
                {
                	mLBRACKET(); 
                
                }
                break;
            case 22 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:136: RBRACKET
                {
                	mRBRACKET(); 
                
                }
                break;
            case 23 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:145: LPAREN
                {
                	mLPAREN(); 
                
                }
                break;
            case 24 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:152: RPAREN
                {
                	mRPAREN(); 
                
                }
                break;
            case 25 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:159: LBRACE
                {
                	mLBRACE(); 
                
                }
                break;
            case 26 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:166: RBRACE
                {
                	mRBRACE(); 
                
                }
                break;
            case 27 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:173: ASSIGN
                {
                	mASSIGN(); 
                
                }
                break;
            case 28 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:180: ADDASSIGN
                {
                	mADDASSIGN(); 
                
                }
                break;
            case 29 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:190: SUBASSIGN
                {
                	mSUBASSIGN(); 
                
                }
                break;
            case 30 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:200: MULASSIGN
                {
                	mMULASSIGN(); 
                
                }
                break;
            case 31 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:210: DIVASSIGN
                {
                	mDIVASSIGN(); 
                
                }
                break;
            case 32 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:220: QUESTION
                {
                	mQUESTION(); 
                
                }
                break;
            case 33 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:229: IF
                {
                	mIF(); 
                
                }
                break;
            case 34 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:232: ELSE
                {
                	mELSE(); 
                
                }
                break;
            case 35 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:237: WHILE
                {
                	mWHILE(); 
                
                }
                break;
            case 36 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:243: DO
                {
                	mDO(); 
                
                }
                break;
            case 37 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:246: FOR
                {
                	mFOR(); 
                
                }
                break;
            case 38 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:250: BREAK
                {
                	mBREAK(); 
                
                }
                break;
            case 39 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:256: CONTINUE
                {
                	mCONTINUE(); 
                
                }
                break;
            case 40 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:265: RETURN
                {
                	mRETURN(); 
                
                }
                break;
            case 41 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:272: DISCARD
                {
                	mDISCARD(); 
                
                }
                break;
            case 42 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:280: CONST
                {
                	mCONST(); 
                
                }
                break;
            case 43 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:286: UNIFORM
                {
                	mUNIFORM(); 
                
                }
                break;
            case 44 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:294: STATIC
                {
                	mSTATIC(); 
                
                }
                break;
            case 45 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:301: CENTROID
                {
                	mCENTROID(); 
                
                }
                break;
            case 46 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:310: LINEAR
                {
                	mLINEAR(); 
                
                }
                break;
            case 47 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:317: NOINTERP
                {
                	mNOINTERP(); 
                
                }
                break;
            case 48 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:326: IN
                {
                	mIN(); 
                
                }
                break;
            case 49 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:329: OUT
                {
                	mOUT(); 
                
                }
                break;
            case 50 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:333: INOUT
                {
                	mINOUT(); 
                
                }
                break;
            case 51 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:339: STRUCT
                {
                	mSTRUCT(); 
                
                }
                break;
            case 52 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:346: REG
                {
                	mREG(); 
                
                }
                break;
            case 53 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:350: PACK
                {
                	mPACK(); 
                
                }
                break;
            case 54 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:355: CBUFFER
                {
                	mCBUFFER(); 
                
                }
                break;
            case 55 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:363: TBUFFER
                {
                	mTBUFFER(); 
                
                }
                break;
            case 56 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:371: NUMBER
                {
                	mNUMBER(); 
                
                }
                break;
            case 57 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:378: BOOL_CONSTANT
                {
                	mBOOL_CONSTANT(); 
                
                }
                break;
            case 58 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:392: ID
                {
                	mID(); 
                
                }
                break;
            case 59 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:395: COMMENT
                {
                	mCOMMENT(); 
                
                }
                break;
            case 60 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:403: WS
                {
                	mWS(); 
                
                }
                break;
            case 61 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:406: STRING_LITERAL
                {
                	mSTRING_LITERAL(); 
                
                }
                break;
        
        }
    
    }


	private void InitializeCyclicDFAs()
	{
	}

 
    
}
}