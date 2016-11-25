lexer grammar bvh;
options {
  language=CSharp;

}
@namespace {
	Igneel.Importers.BVH
}

COLON : ':' ;
LCURLY : '{' ;
RCURLY : '}' ;
DOT : '.' ;
MINUS : '-' ;
HEIRARCHY : 'HIERARCHY' ;
ROOT : 'ROOT' ;
JOINT : 'JOINT' ;
OFFSET : 'OFFSET' ;
CHANNELS : 'CHANNELS' ;
MOTION : 'MOTION' ;
FRAMES : 'Frames' ;
FRAME_TIME : 'Frame Time' ;
XPOSITION : 'Xposition' ;
YPOSITION : 'Yposition' ;
ZPOSITION : 'Zposition' ;
ZROTATION : 'Zrotation' ;
XROTATION : 'Xrotation' ;
YROTATION : 'Yrotation' ;
END_SITE : 'End Site' ;

// $ANTLR src "F:\Projects\Igneel\Igneel\Importers\BVH\bvh.g" 42
fragment
NL	:'\r\n'
	|'\n'
	|'\r'
	;
	
// $ANTLR src "F:\Projects\Igneel\Igneel\Importers\BVH\bvh.g" 48
WS :(' '|'\t'| NL ){$channel=HIDDEN;}
  ;
       
// $ANTLR src "F:\Projects\Igneel\Igneel\Importers\BVH\bvh.g" 51
fragment		
DIGIT	:('0'..'9')
	;	

// $ANTLR src "F:\Projects\Igneel\Igneel\Importers\BVH\bvh.g" 55
NUMBER	:  MINUS? DIGIT+ (DOT DIGIT+)? ('e'  MINUS? DIGIT+)?
	;	
	
// $ANTLR src "F:\Projects\Igneel\Igneel\Importers\BVH\bvh.g" 58
fragment
LETTER :('a'..'z')|('A'..'Z')
       ;
       
// $ANTLR src "F:\Projects\Igneel\Igneel\Importers\BVH\bvh.g" 62
ID	: ('_'|LETTER)(DIGIT|LETTER|'_')*
	;
	
// $ANTLR src "F:\Projects\Igneel\Igneel\Importers\BVH\bvh.g" 65
fragment
OctalEscape
    :    ('0'..'3') ('0'..'7') ('0'..'7')
    |    ('0'..'7') ('0'..'7')
    |    ('0'..'7')
    ;
    

