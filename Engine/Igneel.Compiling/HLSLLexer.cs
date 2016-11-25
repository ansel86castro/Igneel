// $ANTLR 3.0.1 E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g 2015-03-08 14:43:04
namespace 
	Igneel.Compiling.Parser

{

using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;



public class HlslLexer : Lexer 
{
    public const int Exponent = 71;
    public const int FloatSuffix = 72;
    public const int While = 38;
    public const int Letter = 73;
    public const int Const = 45;
    public const int Lbrace = 28;
    public const int For = 40;
    public const int Do = 39;
    public const int Sub = 15;
    public const int Not = 10;
    public const int Uniform = 46;
    public const int And = 18;
    public const int Id = 59;
    public const int Subassign = 32;
    public const int Eof = -1;
    public const int Break = 41;
    public const int OctalDigit = 64;
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
    public const int Nequal = 5;
    public const int Tbuffer = 58;
    public const int Divassign = 34;
    public const int Mulassign = 33;
    public const int Xor = 19;
    public const int Rbrace = 29;
    public const int NonZeroDigit = 65;
    public const int Static = 47;
    public const int Else = 37;
    public const int Number = 60;
    public const int HexDigit = 68;
    public const int Struct = 54;
    public const int Semicolon = 22;
    public const int HexConstant = 69;
    public const int Tokens = 78;
    public const int Reg = 55;
    public const int Mul = 11;
    public const int Decrement = 16;
    public const int Colon = 23;
    public const int Increment = 14;
    public const int Ws = 75;
    public const int Question = 35;
    public const int Discard = 44;
    public const int Addassign = 31;
    public const int Centroid = 48;
    public const int Out = 52;
    public const int Cbuffer = 57;
    public const int DigitSequence = 70;
    public const int Or = 17;
    public const int Assign = 30;
    public const int OctalConstant = 67;
    public const int Div = 12;
    public const int Pack = 56;
    public const int EscapeSequence = 76;
    public const int OctalEscape = 77;

    public HlslLexer() 
    {
		InitializeCyclicDfAs();
    }
    public HlslLexer(ICharStream input) 
		: base(input)
	{
		InitializeCyclicDfAs();
    }
    
    override public string GrammarFileName
    {
    	get { return "E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g";} 
    }

    // $ANTLR start EQUAL 
    public void MEqual() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Equal;
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
    public void MNequal() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Nequal;
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
    public void MLess() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Less;
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
    public void MLequal() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Lequal;
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
    public void MGreater() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Greater;
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
    public void MGequal() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Gequal;
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
    public void MNot() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Not;
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
    public void MMul() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Mul;
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
    public void MDiv() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Div;
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
    public void MAdd() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Add;
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
    public void MIncrement() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Increment;
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
    public void MSub() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Sub;
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
    public void MDecrement() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Decrement;
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
    public void MOr() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Or;
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
    public void MAnd() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = And;
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
    public void MXor() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Xor;
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
    public void MDot() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Dot;
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
    public void MComma() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Comma;
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
    public void MSemicolon() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Semicolon;
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
    public void MColon() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Colon;
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
    public void MLbracket() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Lbracket;
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
    public void MRbracket() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Rbracket;
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
    public void MLparen() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Lparen;
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
    public void MRparen() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Rparen;
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
    public void MLbrace() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Lbrace;
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
    public void MRbrace() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Rbrace;
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
    public void MAssign() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Assign;
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
    public void MAddassign() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Addassign;
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
    public void MSubassign() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Subassign;
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
    public void MMulassign() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Mulassign;
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
    public void MDivassign() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Divassign;
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
    public void MQuestion() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Question;
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
    public void MIf() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = If;
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
    public void MElse() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Else;
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
    public void MWhile() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = While;
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
    public void MDo() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Do;
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
    public void MFor() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = For;
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
    public void MBreak() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Break;
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
    public void MContinue() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Continue;
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
    public void MReturn() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Return;
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
    public void MDiscard() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Discard;
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
    public void MConst() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Const;
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
    public void MUniform() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Uniform;
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
    public void MStatic() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Static;
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
    public void MCentroid() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Centroid;
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
    public void MLinear() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Linear;
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
    public void MNointerp() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Nointerp;
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
    public void MIn() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = In;
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
    public void MOut() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Out;
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
    public void MInout() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Inout;
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
    public void MStruct() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Struct;
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
    public void MReg() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Reg;
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
    public void MPack() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Pack;
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
    public void MCbuffer() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Cbuffer;
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
    public void MTbuffer() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Tbuffer;
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
    public void MZero() // throws RecognitionException [2]
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
            	    NoViableAltException nvaeD1S0 =
            	        new NoViableAltException("596:10: fragment NON_ZERO_DIGIT : ( OCTAL_DIGIT | '8' | '9' );", 1, 0, input);
            
            	    throw nvaeD1S0;
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
    public void MDigit() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:601:2: ( ZERO | NON_ZERO_DIGIT )
            int alt2 = 2;
            int la20 = input.LA(1);
            
            if ( (la20 == '0') )
            {
                alt2 = 1;
            }
            else if ( ((la20 >= '1' && la20 <= '9')) )
            {
                alt2 = 2;
            }
            else 
            {
                NoViableAltException nvaeD2S0 =
                    new NoViableAltException("600:10: fragment DIGIT : ( ZERO | NON_ZERO_DIGIT );", 2, 0, input);
            
                throw nvaeD2S0;
            }
            switch (alt2) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:601:4: ZERO
                    {
                    	MZero(); 
                    
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
            	MZero(); 
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:606:9: ( OCTAL_DIGIT )+
            	int cnt3 = 0;
            	do 
            	{
            	    int alt3 = 2;
            	    int la30 = input.LA(1);
            	    
            	    if ( ((la30 >= '1' && la30 <= '7')) )
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
            	MZero(); 
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
            	    int la40 = input.LA(1);
            	    
            	    if ( ((la40 >= '0' && la40 <= '9') || (la40 >= 'A' && la40 <= 'F') || (la40 >= 'a' && la40 <= 'f')) )
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
            	    NoViableAltException nvaeD5S0 =
            	        new NoViableAltException("617:10: fragment HEX_DIGIT : ( DIGIT | 'a' .. 'f' | 'A' .. 'F' );", 5, 0, input);
            
            	    throw nvaeD5S0;
            }
            
            switch (alt5) 
            {
                case 1 :
                    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:618:4: DIGIT
                    {
                    	MDigit(); 
                    
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
    public void MNumber() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Number;
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
            	int la60 = input.LA(1);
            	
            	if ( (la60 == '.') )
            	{
            	    alt6 = 1;
            	}
            	switch (alt6) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:630:28: DOT f= DIGIT_SEQUENCE
            	        {
            	        	MDot(); 
            	        	int fStart629 = CharIndex;
            	        	mDIGIT_SEQUENCE(); 
            	        	f = new CommonToken(input, Token.INVALID_TOKEN_TYPE, Token.DEFAULT_CHANNEL, fStart629, CharIndex-1);
            	        
            	        }
            	        break;
            	
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:630:51: (e= EXPONENT )?
            	int alt7 = 2;
            	int la70 = input.LA(1);
            	
            	if ( (la70 == 'E' || la70 == 'e') )
            	{
            	    alt7 = 1;
            	}
            	switch (alt7) 
            	{
            	    case 1 :
            	        // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:630:52: e= EXPONENT
            	        {
            	        	int eStart636 = CharIndex;
            	        	MExponent(); 
            	        	e = new CommonToken(input, Token.INVALID_TOKEN_TYPE, Token.DEFAULT_CHANNEL, eStart636, CharIndex-1);
            	        
            	        }
            	        break;
            	
            	}

            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:630:65: ( FLOAT_SUFFIX )?
            	int alt8 = 2;
            	int la80 = input.LA(1);
            	
            	if ( (la80 == 'F' || la80 == 'f') )
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
            	    int la90 = input.LA(1);
            	    
            	    if ( ((la90 >= '0' && la90 <= '9')) )
            	    {
            	        alt9 = 1;
            	    }
            	    
            	
            	    switch (alt9) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:643:4: DIGIT
            			    {
            			    	MDigit(); 
            			    
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
    public void MExponent() // throws RecognitionException [2]
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
            	int la100 = input.LA(1);
            	
            	if ( (la100 == '+' || la100 == '-') )
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
            int _type = BoolConstant;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:656:2: ( 'true' | 'false' )
            int alt11 = 2;
            int la110 = input.LA(1);
            
            if ( (la110 == 't') )
            {
                alt11 = 1;
            }
            else if ( (la110 == 'f') )
            {
                alt11 = 2;
            }
            else 
            {
                NoViableAltException nvaeD11S0 =
                    new NoViableAltException("655:1: BOOL_CONSTANT : ( 'true' | 'false' );", 11, 0, input);
            
                throw nvaeD11S0;
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
    public void MId() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Id;
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
            			    	MLetter(); 
            			    
            			    }
            			    break;
            			case 2 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:661:32: DIGIT
            			    {
            			    	MDigit(); 
            			    
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
    public void MLetter() // throws RecognitionException [2]
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
    public void MComment() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Comment;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:670:5: ( '//' (~ ( '\\n' | '\\r' ) )* ( ( '\\r' )? '\\n' | EOF ) | '/*' ( options {greedy=false; } : . )* '*/' )
            int alt17 = 2;
            int la170 = input.LA(1);
            
            if ( (la170 == '/') )
            {
                int la171 = input.LA(2);
                
                if ( (la171 == '*') )
                {
                    alt17 = 2;
                }
                else if ( (la171 == '/') )
                {
                    alt17 = 1;
                }
                else 
                {
                    NoViableAltException nvaeD17S1 =
                        new NoViableAltException("669:1: COMMENT : ( '//' (~ ( '\\n' | '\\r' ) )* ( ( '\\r' )? '\\n' | EOF ) | '/*' ( options {greedy=false; } : . )* '*/' );", 17, 1, input);
                
                    throw nvaeD17S1;
                }
            }
            else 
            {
                NoViableAltException nvaeD17S0 =
                    new NoViableAltException("669:1: COMMENT : ( '//' (~ ( '\\n' | '\\r' ) )* ( ( '\\r' )? '\\n' | EOF ) | '/*' ( options {greedy=false; } : . )* '*/' );", 17, 0, input);
            
                throw nvaeD17S0;
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
                    	    int la130 = input.LA(1);
                    	    
                    	    if ( ((la130 >= '\u0000' && la130 <= '\t') || (la130 >= '\u000B' && la130 <= '\f') || (la130 >= '\u000E' && la130 <= '\uFFFE')) )
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
                    	int la150 = input.LA(1);
                    	
                    	if ( (la150 == '\n' || la150 == '\r') )
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
                    	        	int la140 = input.LA(1);
                    	        	
                    	        	if ( (la140 == '\r') )
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
                    	        	Match(Eof); 
                    	        
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
                    	    int la160 = input.LA(1);
                    	    
                    	    if ( (la160 == '*') )
                    	    {
                    	        int la161 = input.LA(2);
                    	        
                    	        if ( (la161 == '/') )
                    	        {
                    	            alt16 = 2;
                    	        }
                    	        else if ( ((la161 >= '\u0000' && la161 <= '.') || (la161 >= '0' && la161 <= '\uFFFE')) )
                    	        {
                    	            alt16 = 1;
                    	        }
                    	        
                    	    
                    	    }
                    	    else if ( ((la160 >= '\u0000' && la160 <= ')') || (la160 >= '+' && la160 <= '\uFFFE')) )
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
    public void MWs() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = Ws;
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
            int _type = StringLiteral;
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:677:5: ( '\"' ( EscapeSequence | ~ ( '\\\\' | '\"' ) )* '\"' )
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:677:8: '\"' ( EscapeSequence | ~ ( '\\\\' | '\"' ) )* '\"'
            {
            	Match('\"'); 
            	// E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:677:12: ( EscapeSequence | ~ ( '\\\\' | '\"' ) )*
            	do 
            	{
            	    int alt18 = 3;
            	    int la180 = input.LA(1);
            	    
            	    if ( (la180 == '\\') )
            	    {
            	        alt18 = 1;
            	    }
            	    else if ( ((la180 >= '\u0000' && la180 <= '!') || (la180 >= '#' && la180 <= '[') || (la180 >= ']' && la180 <= '\uFFFE')) )
            	    {
            	        alt18 = 2;
            	    }
            	    
            	
            	    switch (alt18) 
            		{
            			case 1 :
            			    // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:677:14: EscapeSequence
            			    {
            			    	MEscapeSequence(); 
            			    
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
    public void MEscapeSequence() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:682:5: ( '\\\\' ( 'b' | 't' | 'n' | 'f' | 'r' | '\\\"' | '\\'' | '\\\\' ) | OctalEscape )
            int alt19 = 2;
            int la190 = input.LA(1);
            
            if ( (la190 == '\\') )
            {
                int la191 = input.LA(2);
                
                if ( (la191 == '\"' || la191 == '\'' || la191 == '\\' || la191 == 'b' || la191 == 'f' || la191 == 'n' || la191 == 'r' || la191 == 't') )
                {
                    alt19 = 1;
                }
                else if ( ((la191 >= '0' && la191 <= '7')) )
                {
                    alt19 = 2;
                }
                else 
                {
                    NoViableAltException nvaeD19S1 =
                        new NoViableAltException("680:1: fragment EscapeSequence : ( '\\\\' ( 'b' | 't' | 'n' | 'f' | 'r' | '\\\"' | '\\'' | '\\\\' ) | OctalEscape );", 19, 1, input);
                
                    throw nvaeD19S1;
                }
            }
            else 
            {
                NoViableAltException nvaeD19S0 =
                    new NoViableAltException("680:1: fragment EscapeSequence : ( '\\\\' ( 'b' | 't' | 'n' | 'f' | 'r' | '\\\"' | '\\'' | '\\\\' ) | OctalEscape );", 19, 0, input);
            
                throw nvaeD19S0;
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
                    	MOctalEscape(); 
                    
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
    public void MOctalEscape() // throws RecognitionException [2]
    {
        try 
    	{
            // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:687:5: ( '\\\\' ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) )
            int alt20 = 3;
            int la200 = input.LA(1);
            
            if ( (la200 == '\\') )
            {
                int la201 = input.LA(2);
                
                if ( ((la201 >= '0' && la201 <= '3')) )
                {
                    int la202 = input.LA(3);
                    
                    if ( ((la202 >= '0' && la202 <= '7')) )
                    {
                        int la204 = input.LA(4);
                        
                        if ( ((la204 >= '0' && la204 <= '7')) )
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
                else if ( ((la201 >= '4' && la201 <= '7')) )
                {
                    int la203 = input.LA(3);
                    
                    if ( ((la203 >= '0' && la203 <= '7')) )
                    {
                        alt20 = 2;
                    }
                    else 
                    {
                        alt20 = 3;}
                }
                else 
                {
                    NoViableAltException nvaeD20S1 =
                        new NoViableAltException("685:1: fragment OctalEscape : ( '\\\\' ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) );", 20, 1, input);
                
                    throw nvaeD20S1;
                }
            }
            else 
            {
                NoViableAltException nvaeD20S0 =
                    new NoViableAltException("685:1: fragment OctalEscape : ( '\\\\' ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) );", 20, 0, input);
            
                throw nvaeD20S0;
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
            int la211 = input.LA(2);
            
            if ( (la211 == '=') )
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
            int la212 = input.LA(2);
            
            if ( (la212 == '=') )
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
            int la213 = input.LA(2);
            
            if ( (la213 == '=') )
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
            int la214 = input.LA(2);
            
            if ( (la214 == '=') )
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
            int la215 = input.LA(2);
            
            if ( (la215 == '=') )
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
                    int la2182 = input.LA(4);
                    
                    if ( (la2182 == 'u') )
                    {
                        int la21106 = input.LA(5);
                        
                        if ( (la21106 == 't') )
                        {
                            int la21128 = input.LA(6);
                            
                            if ( ((la21128 >= '0' && la21128 <= '9') || (la21128 >= 'A' && la21128 <= 'Z') || la21128 == '_' || (la21128 >= 'a' && la21128 <= 'z')) )
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
                int la2162 = input.LA(3);
                
                if ( ((la2162 >= '0' && la2162 <= '9') || (la2162 >= 'A' && la2162 <= 'Z') || la2162 == '_' || (la2162 >= 'a' && la2162 <= 'z')) )
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
            int la2124 = input.LA(2);
            
            if ( (la2124 == 'l') )
            {
                int la2163 = input.LA(3);
                
                if ( (la2163 == 's') )
                {
                    int la2185 = input.LA(4);
                    
                    if ( (la2185 == 'e') )
                    {
                        int la21107 = input.LA(5);
                        
                        if ( ((la21107 >= '0' && la21107 <= '9') || (la21107 >= 'A' && la21107 <= 'Z') || la21107 == '_' || (la21107 >= 'a' && la21107 <= 'z')) )
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
            int la2125 = input.LA(2);
            
            if ( (la2125 == 'h') )
            {
                int la2164 = input.LA(3);
                
                if ( (la2164 == 'i') )
                {
                    int la2186 = input.LA(4);
                    
                    if ( (la2186 == 'l') )
                    {
                        int la21108 = input.LA(5);
                        
                        if ( (la21108 == 'e') )
                        {
                            int la21130 = input.LA(6);
                            
                            if ( ((la21130 >= '0' && la21130 <= '9') || (la21130 >= 'A' && la21130 <= 'Z') || la21130 == '_' || (la21130 >= 'a' && la21130 <= 'z')) )
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
                int la2165 = input.LA(3);
                
                if ( (la2165 == 's') )
                {
                    int la2187 = input.LA(4);
                    
                    if ( (la2187 == 'c') )
                    {
                        int la21109 = input.LA(5);
                        
                        if ( (la21109 == 'a') )
                        {
                            int la21131 = input.LA(6);
                            
                            if ( (la21131 == 'r') )
                            {
                                int la21150 = input.LA(7);
                                
                                if ( (la21150 == 'd') )
                                {
                                    int la21165 = input.LA(8);
                                    
                                    if ( ((la21165 >= '0' && la21165 <= '9') || (la21165 >= 'A' && la21165 <= 'Z') || la21165 == '_' || (la21165 >= 'a' && la21165 <= 'z')) )
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
                int la2166 = input.LA(3);
                
                if ( ((la2166 >= '0' && la2166 <= '9') || (la2166 >= 'A' && la2166 <= 'Z') || la2166 == '_' || (la2166 >= 'a' && la2166 <= 'z')) )
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
                int la2167 = input.LA(3);
                
                if ( (la2167 == 'r') )
                {
                    int la2189 = input.LA(4);
                    
                    if ( ((la2189 >= '0' && la2189 <= '9') || (la2189 >= 'A' && la2189 <= 'Z') || la2189 == '_' || (la2189 >= 'a' && la2189 <= 'z')) )
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
                int la2168 = input.LA(3);
                
                if ( (la2168 == 'l') )
                {
                    int la2190 = input.LA(4);
                    
                    if ( (la2190 == 's') )
                    {
                        int la21111 = input.LA(5);
                        
                        if ( (la21111 == 'e') )
                        {
                            int la21132 = input.LA(6);
                            
                            if ( ((la21132 >= '0' && la21132 <= '9') || (la21132 >= 'A' && la21132 <= 'Z') || la21132 == '_' || (la21132 >= 'a' && la21132 <= 'z')) )
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
            int la2128 = input.LA(2);
            
            if ( (la2128 == 'r') )
            {
                int la2169 = input.LA(3);
                
                if ( (la2169 == 'e') )
                {
                    int la2191 = input.LA(4);
                    
                    if ( (la2191 == 'a') )
                    {
                        int la21112 = input.LA(5);
                        
                        if ( (la21112 == 'k') )
                        {
                            int la21133 = input.LA(6);
                            
                            if ( ((la21133 >= '0' && la21133 <= '9') || (la21133 >= 'A' && la21133 <= 'Z') || la21133 == '_' || (la21133 >= 'a' && la21133 <= 'z')) )
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
                int la2170 = input.LA(3);
                
                if ( (la2170 == 'n') )
                {
                    switch ( input.LA(4) ) 
                    {
                    case 's':
                    	{
                        int la21113 = input.LA(5);
                        
                        if ( (la21113 == 't') )
                        {
                            int la21134 = input.LA(6);
                            
                            if ( ((la21134 >= '0' && la21134 <= '9') || (la21134 >= 'A' && la21134 <= 'Z') || la21134 == '_' || (la21134 >= 'a' && la21134 <= 'z')) )
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
                        int la21114 = input.LA(5);
                        
                        if ( (la21114 == 'i') )
                        {
                            int la21135 = input.LA(6);
                            
                            if ( (la21135 == 'n') )
                            {
                                int la21153 = input.LA(7);
                                
                                if ( (la21153 == 'u') )
                                {
                                    int la21166 = input.LA(8);
                                    
                                    if ( (la21166 == 'e') )
                                    {
                                        int la21179 = input.LA(9);
                                        
                                        if ( ((la21179 >= '0' && la21179 <= '9') || (la21179 >= 'A' && la21179 <= 'Z') || la21179 == '_' || (la21179 >= 'a' && la21179 <= 'z')) )
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
                int la2171 = input.LA(3);
                
                if ( (la2171 == 'n') )
                {
                    int la2193 = input.LA(4);
                    
                    if ( (la2193 == 't') )
                    {
                        int la21115 = input.LA(5);
                        
                        if ( (la21115 == 'r') )
                        {
                            int la21136 = input.LA(6);
                            
                            if ( (la21136 == 'o') )
                            {
                                int la21154 = input.LA(7);
                                
                                if ( (la21154 == 'i') )
                                {
                                    int la21167 = input.LA(8);
                                    
                                    if ( (la21167 == 'd') )
                                    {
                                        int la21180 = input.LA(9);
                                        
                                        if ( ((la21180 >= '0' && la21180 <= '9') || (la21180 >= 'A' && la21180 <= 'Z') || la21180 == '_' || (la21180 >= 'a' && la21180 <= 'z')) )
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
                int la2172 = input.LA(3);
                
                if ( (la2172 == 'u') )
                {
                    int la2194 = input.LA(4);
                    
                    if ( (la2194 == 'f') )
                    {
                        int la21116 = input.LA(5);
                        
                        if ( (la21116 == 'f') )
                        {
                            int la21137 = input.LA(6);
                            
                            if ( (la21137 == 'e') )
                            {
                                int la21155 = input.LA(7);
                                
                                if ( (la21155 == 'r') )
                                {
                                    int la21168 = input.LA(8);
                                    
                                    if ( ((la21168 >= '0' && la21168 <= '9') || (la21168 >= 'A' && la21168 <= 'Z') || la21168 == '_' || (la21168 >= 'a' && la21168 <= 'z')) )
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
            int la2130 = input.LA(2);
            
            if ( (la2130 == 'e') )
            {
                switch ( input.LA(3) ) 
                {
                case 'g':
                	{
                    int la2195 = input.LA(4);
                    
                    if ( (la2195 == 'i') )
                    {
                        int la21117 = input.LA(5);
                        
                        if ( (la21117 == 's') )
                        {
                            int la21138 = input.LA(6);
                            
                            if ( (la21138 == 't') )
                            {
                                int la21156 = input.LA(7);
                                
                                if ( (la21156 == 'e') )
                                {
                                    int la21169 = input.LA(8);
                                    
                                    if ( (la21169 == 'r') )
                                    {
                                        int la21182 = input.LA(9);
                                        
                                        if ( ((la21182 >= '0' && la21182 <= '9') || (la21182 >= 'A' && la21182 <= 'Z') || la21182 == '_' || (la21182 >= 'a' && la21182 <= 'z')) )
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
                    int la2196 = input.LA(4);
                    
                    if ( (la2196 == 'u') )
                    {
                        int la21118 = input.LA(5);
                        
                        if ( (la21118 == 'r') )
                        {
                            int la21139 = input.LA(6);
                            
                            if ( (la21139 == 'n') )
                            {
                                int la21157 = input.LA(7);
                                
                                if ( ((la21157 >= '0' && la21157 <= '9') || (la21157 >= 'A' && la21157 <= 'Z') || la21157 == '_' || (la21157 >= 'a' && la21157 <= 'z')) )
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
            int la2131 = input.LA(2);
            
            if ( (la2131 == 'n') )
            {
                int la2174 = input.LA(3);
                
                if ( (la2174 == 'i') )
                {
                    int la2197 = input.LA(4);
                    
                    if ( (la2197 == 'f') )
                    {
                        int la21119 = input.LA(5);
                        
                        if ( (la21119 == 'o') )
                        {
                            int la21140 = input.LA(6);
                            
                            if ( (la21140 == 'r') )
                            {
                                int la21158 = input.LA(7);
                                
                                if ( (la21158 == 'm') )
                                {
                                    int la21171 = input.LA(8);
                                    
                                    if ( ((la21171 >= '0' && la21171 <= '9') || (la21171 >= 'A' && la21171 <= 'Z') || la21171 == '_' || (la21171 >= 'a' && la21171 <= 'z')) )
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
            int la2132 = input.LA(2);
            
            if ( (la2132 == 't') )
            {
                switch ( input.LA(3) ) 
                {
                case 'a':
                	{
                    int la2198 = input.LA(4);
                    
                    if ( (la2198 == 't') )
                    {
                        int la21120 = input.LA(5);
                        
                        if ( (la21120 == 'i') )
                        {
                            int la21141 = input.LA(6);
                            
                            if ( (la21141 == 'c') )
                            {
                                int la21159 = input.LA(7);
                                
                                if ( ((la21159 >= '0' && la21159 <= '9') || (la21159 >= 'A' && la21159 <= 'Z') || la21159 == '_' || (la21159 >= 'a' && la21159 <= 'z')) )
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
                    int la2199 = input.LA(4);
                    
                    if ( (la2199 == 'u') )
                    {
                        int la21121 = input.LA(5);
                        
                        if ( (la21121 == 'c') )
                        {
                            int la21142 = input.LA(6);
                            
                            if ( (la21142 == 't') )
                            {
                                int la21160 = input.LA(7);
                                
                                if ( ((la21160 >= '0' && la21160 <= '9') || (la21160 >= 'A' && la21160 <= 'Z') || la21160 == '_' || (la21160 >= 'a' && la21160 <= 'z')) )
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
            int la2133 = input.LA(2);
            
            if ( (la2133 == 'i') )
            {
                int la2176 = input.LA(3);
                
                if ( (la2176 == 'n') )
                {
                    int la21100 = input.LA(4);
                    
                    if ( (la21100 == 'e') )
                    {
                        int la21122 = input.LA(5);
                        
                        if ( (la21122 == 'a') )
                        {
                            int la21143 = input.LA(6);
                            
                            if ( (la21143 == 'r') )
                            {
                                int la21161 = input.LA(7);
                                
                                if ( ((la21161 >= '0' && la21161 <= '9') || (la21161 >= 'A' && la21161 <= 'Z') || la21161 == '_' || (la21161 >= 'a' && la21161 <= 'z')) )
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
            int la2134 = input.LA(2);
            
            if ( (la2134 == 'o') )
            {
                int la2177 = input.LA(3);
                
                if ( (la2177 == 'i') )
                {
                    int la21101 = input.LA(4);
                    
                    if ( (la21101 == 'n') )
                    {
                        int la21123 = input.LA(5);
                        
                        if ( (la21123 == 't') )
                        {
                            int la21144 = input.LA(6);
                            
                            if ( (la21144 == 'e') )
                            {
                                int la21162 = input.LA(7);
                                
                                if ( (la21162 == 'r') )
                                {
                                    int la21175 = input.LA(8);
                                    
                                    if ( (la21175 == 'p') )
                                    {
                                        int la21184 = input.LA(9);
                                        
                                        if ( (la21184 == 'o') )
                                        {
                                            int la21190 = input.LA(10);
                                            
                                            if ( (la21190 == 'l') )
                                            {
                                                int la21192 = input.LA(11);
                                                
                                                if ( (la21192 == 'a') )
                                                {
                                                    int la21194 = input.LA(12);
                                                    
                                                    if ( (la21194 == 't') )
                                                    {
                                                        int la21196 = input.LA(13);
                                                        
                                                        if ( (la21196 == 'i') )
                                                        {
                                                            int la21197 = input.LA(14);
                                                            
                                                            if ( (la21197 == 'o') )
                                                            {
                                                                int la21198 = input.LA(15);
                                                                
                                                                if ( (la21198 == 'n') )
                                                                {
                                                                    int la21199 = input.LA(16);
                                                                    
                                                                    if ( ((la21199 >= '0' && la21199 <= '9') || (la21199 >= 'A' && la21199 <= 'Z') || la21199 == '_' || (la21199 >= 'a' && la21199 <= 'z')) )
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
            int la2135 = input.LA(2);
            
            if ( (la2135 == 'u') )
            {
                int la2178 = input.LA(3);
                
                if ( (la2178 == 't') )
                {
                    int la21102 = input.LA(4);
                    
                    if ( ((la21102 >= '0' && la21102 <= '9') || (la21102 >= 'A' && la21102 <= 'Z') || la21102 == '_' || (la21102 >= 'a' && la21102 <= 'z')) )
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
            int la2136 = input.LA(2);
            
            if ( (la2136 == 'a') )
            {
                int la2179 = input.LA(3);
                
                if ( (la2179 == 'c') )
                {
                    int la21103 = input.LA(4);
                    
                    if ( (la21103 == 'k') )
                    {
                        int la21125 = input.LA(5);
                        
                        if ( (la21125 == 'o') )
                        {
                            int la21145 = input.LA(6);
                            
                            if ( (la21145 == 'f') )
                            {
                                int la21163 = input.LA(7);
                                
                                if ( (la21163 == 'f') )
                                {
                                    int la21176 = input.LA(8);
                                    
                                    if ( (la21176 == 's') )
                                    {
                                        int la21185 = input.LA(9);
                                        
                                        if ( (la21185 == 'e') )
                                        {
                                            int la21191 = input.LA(10);
                                            
                                            if ( (la21191 == 't') )
                                            {
                                                int la21193 = input.LA(11);
                                                
                                                if ( ((la21193 >= '0' && la21193 <= '9') || (la21193 >= 'A' && la21193 <= 'Z') || la21193 == '_' || (la21193 >= 'a' && la21193 <= 'z')) )
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
                int la2180 = input.LA(3);
                
                if ( (la2180 == 'u') )
                {
                    int la21104 = input.LA(4);
                    
                    if ( (la21104 == 'f') )
                    {
                        int la21126 = input.LA(5);
                        
                        if ( (la21126 == 'f') )
                        {
                            int la21146 = input.LA(6);
                            
                            if ( (la21146 == 'e') )
                            {
                                int la21164 = input.LA(7);
                                
                                if ( (la21164 == 'r') )
                                {
                                    int la21177 = input.LA(8);
                                    
                                    if ( ((la21177 >= '0' && la21177 <= '9') || (la21177 >= 'A' && la21177 <= 'Z') || la21177 == '_' || (la21177 >= 'a' && la21177 <= 'z')) )
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
                int la2181 = input.LA(3);
                
                if ( (la2181 == 'u') )
                {
                    int la21105 = input.LA(4);
                    
                    if ( (la21105 == 'e') )
                    {
                        int la21127 = input.LA(5);
                        
                        if ( ((la21127 >= '0' && la21127 <= '9') || (la21127 >= 'A' && la21127 <= 'Z') || la21127 == '_' || (la21127 >= 'a' && la21127 <= 'z')) )
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
        	    NoViableAltException nvaeD21S0 =
        	        new NoViableAltException("1:1: Tokens : ( EQUAL | NEQUAL | LESS | LEQUAL | GREATER | GEQUAL | NOT | MUL | DIV | ADD | INCREMENT | SUB | DECREMENT | OR | AND | XOR | DOT | COMMA | SEMICOLON | COLON | LBRACKET | RBRACKET | LPAREN | RPAREN | LBRACE | RBRACE | ASSIGN | ADDASSIGN | SUBASSIGN | MULASSIGN | DIVASSIGN | QUESTION | IF | ELSE | WHILE | DO | FOR | BREAK | CONTINUE | RETURN | DISCARD | CONST | UNIFORM | STATIC | CENTROID | LINEAR | NOINTERP | IN | OUT | INOUT | STRUCT | REG | PACK | CBUFFER | TBUFFER | NUMBER | BOOL_CONSTANT | ID | COMMENT | WS | STRING_LITERAL );", 21, 0, input);
        
        	    throw nvaeD21S0;
        }
        
        switch (alt21) 
        {
            case 1 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:10: EQUAL
                {
                	MEqual(); 
                
                }
                break;
            case 2 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:16: NEQUAL
                {
                	MNequal(); 
                
                }
                break;
            case 3 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:23: LESS
                {
                	MLess(); 
                
                }
                break;
            case 4 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:28: LEQUAL
                {
                	MLequal(); 
                
                }
                break;
            case 5 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:35: GREATER
                {
                	MGreater(); 
                
                }
                break;
            case 6 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:43: GEQUAL
                {
                	MGequal(); 
                
                }
                break;
            case 7 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:50: NOT
                {
                	MNot(); 
                
                }
                break;
            case 8 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:54: MUL
                {
                	MMul(); 
                
                }
                break;
            case 9 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:58: DIV
                {
                	MDiv(); 
                
                }
                break;
            case 10 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:62: ADD
                {
                	MAdd(); 
                
                }
                break;
            case 11 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:66: INCREMENT
                {
                	MIncrement(); 
                
                }
                break;
            case 12 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:76: SUB
                {
                	MSub(); 
                
                }
                break;
            case 13 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:80: DECREMENT
                {
                	MDecrement(); 
                
                }
                break;
            case 14 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:90: OR
                {
                	MOr(); 
                
                }
                break;
            case 15 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:93: AND
                {
                	MAnd(); 
                
                }
                break;
            case 16 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:97: XOR
                {
                	MXor(); 
                
                }
                break;
            case 17 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:101: DOT
                {
                	MDot(); 
                
                }
                break;
            case 18 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:105: COMMA
                {
                	MComma(); 
                
                }
                break;
            case 19 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:111: SEMICOLON
                {
                	MSemicolon(); 
                
                }
                break;
            case 20 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:121: COLON
                {
                	MColon(); 
                
                }
                break;
            case 21 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:127: LBRACKET
                {
                	MLbracket(); 
                
                }
                break;
            case 22 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:136: RBRACKET
                {
                	MRbracket(); 
                
                }
                break;
            case 23 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:145: LPAREN
                {
                	MLparen(); 
                
                }
                break;
            case 24 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:152: RPAREN
                {
                	MRparen(); 
                
                }
                break;
            case 25 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:159: LBRACE
                {
                	MLbrace(); 
                
                }
                break;
            case 26 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:166: RBRACE
                {
                	MRbrace(); 
                
                }
                break;
            case 27 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:173: ASSIGN
                {
                	MAssign(); 
                
                }
                break;
            case 28 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:180: ADDASSIGN
                {
                	MAddassign(); 
                
                }
                break;
            case 29 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:190: SUBASSIGN
                {
                	MSubassign(); 
                
                }
                break;
            case 30 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:200: MULASSIGN
                {
                	MMulassign(); 
                
                }
                break;
            case 31 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:210: DIVASSIGN
                {
                	MDivassign(); 
                
                }
                break;
            case 32 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:220: QUESTION
                {
                	MQuestion(); 
                
                }
                break;
            case 33 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:229: IF
                {
                	MIf(); 
                
                }
                break;
            case 34 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:232: ELSE
                {
                	MElse(); 
                
                }
                break;
            case 35 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:237: WHILE
                {
                	MWhile(); 
                
                }
                break;
            case 36 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:243: DO
                {
                	MDo(); 
                
                }
                break;
            case 37 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:246: FOR
                {
                	MFor(); 
                
                }
                break;
            case 38 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:250: BREAK
                {
                	MBreak(); 
                
                }
                break;
            case 39 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:256: CONTINUE
                {
                	MContinue(); 
                
                }
                break;
            case 40 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:265: RETURN
                {
                	MReturn(); 
                
                }
                break;
            case 41 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:272: DISCARD
                {
                	MDiscard(); 
                
                }
                break;
            case 42 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:280: CONST
                {
                	MConst(); 
                
                }
                break;
            case 43 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:286: UNIFORM
                {
                	MUniform(); 
                
                }
                break;
            case 44 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:294: STATIC
                {
                	MStatic(); 
                
                }
                break;
            case 45 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:301: CENTROID
                {
                	MCentroid(); 
                
                }
                break;
            case 46 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:310: LINEAR
                {
                	MLinear(); 
                
                }
                break;
            case 47 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:317: NOINTERP
                {
                	MNointerp(); 
                
                }
                break;
            case 48 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:326: IN
                {
                	MIn(); 
                
                }
                break;
            case 49 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:329: OUT
                {
                	MOut(); 
                
                }
                break;
            case 50 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:333: INOUT
                {
                	MInout(); 
                
                }
                break;
            case 51 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:339: STRUCT
                {
                	MStruct(); 
                
                }
                break;
            case 52 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:346: REG
                {
                	MReg(); 
                
                }
                break;
            case 53 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:350: PACK
                {
                	MPack(); 
                
                }
                break;
            case 54 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:355: CBUFFER
                {
                	MCbuffer(); 
                
                }
                break;
            case 55 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:363: TBUFFER
                {
                	MTbuffer(); 
                
                }
                break;
            case 56 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:371: NUMBER
                {
                	MNumber(); 
                
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
                	MId(); 
                
                }
                break;
            case 59 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:395: COMMENT
                {
                	MComment(); 
                
                }
                break;
            case 60 :
                // E:\\Projects\\Igneel\\HLSLCompiler\\HLSL.g:1:403: WS
                {
                	MWs(); 
                
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


	private void InitializeCyclicDfAs()
	{
	}

 
    
}
}