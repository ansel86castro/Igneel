lexer grammar HLSL;
options {
  language=CSharp;

}
@namespace {
	Igneel.Compiling.Parser
}

EQUAL : '==' ;
NEQUAL : '!=' ;
LESS : '<' ;
LEQUAL : '<=' ;
GREATER : '>' ;
GEQUAL : '>=' ;
NOT : '!' ;
MUL : '*' ;
DIV : '/' ;
ADD : '+' ;
INCREMENT : '++' ;
SUB : '-' ;
DECREMENT : '--' ;
OR : '||' ;
AND : '&&' ;
XOR : '^^' ;
DOT : '.' ;
COMMA : ',' ;
SEMICOLON : ';' ;
COLON : ':' ;
LBRACKET : '[' ;
RBRACKET : ']' ;
LPAREN : '(' ;
RPAREN : ')' ;
LBRACE : '{' ;
RBRACE : '}' ;
ASSIGN : '=' ;
ADDASSIGN : '+=' ;
SUBASSIGN : '-=' ;
MULASSIGN : '*=' ;
DIVASSIGN : '/=' ;
QUESTION : '?' ;
IF : 'if' ;
ELSE : 'else' ;
WHILE : 'while' ;
DO : 'do' ;
FOR : 'for' ;
BREAK : 'break' ;
CONTINUE : 'continue' ;
RETURN : 'return' ;
DISCARD : 'discard' ;
CONST : 'const' ;
UNIFORM : 'uniform' ;
STATIC : 'static' ;
CENTROID : 'centroid' ;
LINEAR : 'linear' ;
NOINTERP : 'nointerpolation' ;
IN : 'in' ;
OUT : 'out' ;
INOUT : 'inout' ;
STRUCT : 'struct' ;
REG : 'register' ;
PACK : 'packoffset' ;
CBUFFER : 'cbuffer' ;
TBUFFER : 'tbuffer' ;

// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 592
fragment ZERO
	: '0'
	;
	
// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 596
fragment NON_ZERO_DIGIT
	:	OCTAL_DIGIT |'8'|'9'
	;
	
// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 600
fragment DIGIT
	:	ZERO
	|	NON_ZERO_DIGIT
	;

// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 605
fragment OCTAL_CONSTANT
	:	ZERO (OCTAL_DIGIT)+
	;

// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 609
fragment OCTAL_DIGIT
	:	('1'..'7')
	;

// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 613
fragment HEX_CONSTANT
	:	ZERO ('x'|'X') (HEX_DIGIT)+
	;

// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 617
fragment HEX_DIGIT
	:	DIGIT|'a'..'f'|'A'..'F'
	;

//FLOAT_CONSTANT
 //   :   f=FRACTIONAL_CONSTANT (e=EXPONENT)? (FLOAT_SUFFIX)? {$text=f.Text+((e!=null)?e.Text:"");}
 //   ;

//fragment FRACTIONAL_CONSTANT
//	: 	DIGIT_SEQUENCE DOT (DIGIT_SEQUENCE)?
//	|	DOT DIGIT_SEQUENCE
//	;

// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 630
NUMBER	: m=DIGIT_SEQUENCE (DOT f=DIGIT_SEQUENCE)? (e=EXPONENT)? (FLOAT_SUFFIX)?
	{
	  string value = m.Text;
	  if(f!=null)
	  	value += "."+f.Text;		  	
	  if(e!=null)
	  	value += e.Text;
    	  $text = value;
	}
	;

// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 641
fragment	
DIGIT_SEQUENCE
	:	DIGIT+
	;

// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 646
fragment
EXPONENT 
	: ('e'|'E') ( ADD | SUB)? DIGIT_SEQUENCE 
	;
// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 650
fragment
FLOAT_SUFFIX
	: 'f' | 'F'
	;
	
// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 655
BOOL_CONSTANT
	: 'true'
	| 'false'
	;

// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 660
ID  
    :	(LETTER | '_') (LETTER | DIGIT | '_')*
    ;
     
// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 664
fragment LETTER
	:   ('a'..'z'|'A'..'Z')
	;	


// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 669
COMMENT
    :   '//' ~('\n'|'\r')* ('\r'? '\n'| EOF) {$channel=HIDDEN;}
    |   '/*' ( options {greedy=false;} : . )* '*/' {$channel=HIDDEN;}
    ;

// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 674
WS  :   (' '| '\t' | '\r' | '\n' ) {$channel=HIDDEN;}
    ;
// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 676
STRING_LITERAL
    :  '"' ( EscapeSequence | ~('\\'|'"') )* '"'
    ;
    
// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 680
fragment
EscapeSequence
    :   '\\' ('b'|'t'|'n'|'f'|'r'|'\"'|'\''|'\\')
    |   OctalEscape
    ;
    // $ANTLR src "E:\Projects\Igneel\HLSLCompiler\HLSL.g" 685
fragment
OctalEscape
    :   '\\' ('0'..'3') ('0'..'7') ('0'..'7')
    |   '\\' ('0'..'7') ('0'..'7')
    |   '\\' ('0'..'7')
    ;

