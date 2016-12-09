grammar bvh;
options{
	language='CSharp';
	//namespace = 'Igneel.Importers.BVH';	
}

tokens{
	COLON=     ':';		
	LCURLY=    '{';
	RCURLY=    '}';
	DOT=	   '.';		
	MINUS = '-';
	HEIRARCHY = 'HIERARCHY';
	ROOT      = 'ROOT';
	JOINT     = 'JOINT';
	OFFSET    = 'OFFSET';
	CHANNELS  = 'CHANNELS';
	MOTION    = 'MOTION';
	FRAMES    = 'Frames';
	FRAME_TIME= 'Frame Time';
	XPOSITION = 'Xposition';
	YPOSITION = 'Yposition';
	ZPOSITION = 'Zposition';
	ZROTATION = 'Zrotation';
	XROTATION = 'Xrotation';
	YROTATION = 'Yrotation';
	END_SITE  = 'End Site';
}
@header{
using System.Collections.Generic;	
}

@lexer::namespace {
	Igneel.Importers.BVH
}

@parser::namespace {
	Igneel.Importers.BVH
}


fragment
NL	:'\r\n'
	|'\n'
	|'\r'
	;
	
WS :(' '|'\t'| NL ){$channel=HIDDEN;}
  ;
       
fragment		
DIGIT	:('0'..'9')
	;	

NUMBER	:  MINUS? DIGIT+ (DOT DIGIT+)? ('e'  MINUS? DIGIT+)?
	;	
	
fragment
LETTER :('a'..'z')|('A'..'Z')
       ;
       
ID	: ('_'|LETTER)(DIGIT|LETTER|'_')*
	;
	
fragment
OctalEscape
    :    ('0'..'3') ('0'..'7') ('0'..'7')
    |    ('0'..'7') ('0'..'7')
    |    ('0'..'7')
    ;
    

document returns [BvhDocument doc  = new BvhDocument()]	
	: HEIRARCHY
  	  root = heirarchy[doc]{ doc.Root = root; }
	  m = motion{ doc.Motion = m; }
	;

motion returns[BvhMotion m  =new BvhMotion()]
	: MOTION 
	  FRAMES COLON frames=NUMBER {m.FrameCount = int.Parse( frames.Text ); }
	  FRAME_TIME COLON time=NUMBER {m.FrameTime = float.Parse( time.Text ); }
  	  values[m.Data]
	;
values	[List<float>data]
	: ( num=NUMBER { data.Add(float.Parse(num.Text)); })+
	;

heirarchy [BvhDocument d]
   returns[BvhNode node = new BvhNode()]   	 
	  @init{
	  	d.Nodes.Add(node);
	  }	 
	: (ROOT{node.IsRoot = true;} | JOINT) ID {node.Name = $ID.Text;}
 	  LCURLY
	   v=offsets {node.Offset = (Vector3)v;}
 	   ch=channels {node.Channels = ch.ToArray();}
 	   ( (child=heirarchy[d]{ node.Nodes.Add(child); })+ |
 	     end=endsite{ node.End = end; })
 	  RCURLY
	;

endsite returns	[EndSite end = new EndSite()]
	: END_SITE 
 	  LCURLY
	  v=offsets{ end.Offset = (Vector3)v; }
	  RCURLY
	;

channels returns[List<FODChannel>list]	
	: CHANNELS len=NUMBER { list = new List<FODChannel>(int.Parse(len.Text)); }
	(
	    XPOSITION {list.Add(FODChannel.XPosition); } |
	    YPOSITION {list.Add(FODChannel.YPosition); } | 
	    ZPOSITION {list.Add(FODChannel.ZPosition); } |
	    ZROTATION {list.Add(FODChannel.ZRotation); }| 
	    XROTATION {list.Add(FODChannel.XRotation); }| 
	    YROTATION {list.Add(FODChannel.YRotation); }
	)+ 	
	;

offsets	returns[Vector3? v = new Vector3()]
	: OFFSET x =NUMBER  y=NUMBER z=NUMBER
	{
	  v = new Vector3(float.Parse(x.Text),float.Parse(y.Text),float.Parse(z.Text));
	}
	;
