lexer grammar preprocess;
options {
  language=CSharp;

}
@namespace {
	Igneel.Compiling.Parser
}

INCLUDE : 'include' ;
IFDEFINE : 'ifdef' ;
IFNDEFINE : 'ifndef' ;
ELIF : 'elif' ;
ENDIF : 'endif' ;
DEFINE : 'define' ;
UNDEFINE : 'undef' ;
NUMBER : '#' ;

// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\preprocess.g" 43
PREPROC	: NUMBER WS* (DEFINE WS+ ID WS+  TEXT 
		     | INCLUDE WS* INCLUDE_FILE) WS?  NL
	;

// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\preprocess.g" 47
fragment
WS  :   (' '| '\t')
    ;	
    
// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\preprocess.g" 51
fragment
NL :'\r' '\n'? | '\n'
   ;
   
// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\preprocess.g" 55
ID  
    :	(LETTER | '_') (LETTER | '0'..'9' | '_')*
    ;
    
// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\preprocess.g" 59
fragment 
LETTER
	:   ('a'..'z'|'A'..'Z')
	;

// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\preprocess.g" 64
fragment
TEXT	: ( options {greedy=false;} : . )+
	;   
	 
// $ANTLR src "E:\Projects\Igneel\HLSLCompiler\preprocess.g" 68
INCLUDE_FILE 	
	  : '<' ( ' ' | '!' | '#' ..';' | '=' | '?' .. '[' | ']' .. '\u00FF')+ '>'
     	|   '"' ( ' ' | '!' | '#' ..';' | '=' | '?' .. '[' | ']' .. '\u00FF')+ '"'
     	;  
