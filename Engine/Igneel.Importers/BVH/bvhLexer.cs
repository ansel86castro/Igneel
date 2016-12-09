// $ANTLR 3.0.1 F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g 2014-09-21 00:29:20
namespace 
	Igneel.Importers.BVH

{

using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;



public class bvhLexer : Lexer 
{
    public const int MOTION = 14;
    public const int FRAME_TIME = 16;
    public const int JOINT = 11;
    public const int LETTER = 28;
    public const int XPOSITION = 17;
    public const int NUMBER = 27;
    public const int XROTATION = 21;
    public const int YPOSITION = 18;
    public const int LCURLY = 5;
    public const int MINUS = 8;
    public const int HEIRARCHY = 9;
    public const int ID = 29;
    public const int Tokens = 31;
    public const int EOF = -1;
    public const int ROOT = 10;
    public const int COLON = 4;
    public const int WS = 25;
    public const int ZROTATION = 20;
    public const int YROTATION = 22;
    public const int OFFSET = 12;
    public const int RCURLY = 6;
    public const int FRAMES = 15;
    public const int END_SITE = 23;
    public const int ZPOSITION = 19;
    public const int CHANNELS = 13;
    public const int NL = 24;
    public const int DIGIT = 26;
    public const int DOT = 7;
    public const int OctalEscape = 30;

    public bvhLexer() 
    {
		InitializeCyclicDFAs();
    }
    public bvhLexer(ICharStream input) 
		: base(input)
	{
		InitializeCyclicDFAs();
    }
    
    override public string GrammarFileName
    {
    	get { return "F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g";} 
    }

    // $ANTLR start COLON 
    public void mCOLON() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = COLON;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:10:7: ( ':' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:10:9: ':'
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

    // $ANTLR start LCURLY 
    public void mLCURLY() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = LCURLY;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:11:8: ( '{' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:11:10: '{'
            {
            	Match('{'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end LCURLY

    // $ANTLR start RCURLY 
    public void mRCURLY() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = RCURLY;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:12:8: ( '}' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:12:10: '}'
            {
            	Match('}'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end RCURLY

    // $ANTLR start DOT 
    public void mDOT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = DOT;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:13:5: ( '.' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:13:7: '.'
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

    // $ANTLR start MINUS 
    public void mMINUS() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = MINUS;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:14:7: ( '-' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:14:9: '-'
            {
            	Match('-'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end MINUS

    // $ANTLR start HEIRARCHY 
    public void mHEIRARCHY() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = HEIRARCHY;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:15:11: ( 'HIERARCHY' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:15:13: 'HIERARCHY'
            {
            	Match("HIERARCHY"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end HEIRARCHY

    // $ANTLR start ROOT 
    public void mROOT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = ROOT;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:16:6: ( 'ROOT' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:16:8: 'ROOT'
            {
            	Match("ROOT"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end ROOT

    // $ANTLR start JOINT 
    public void mJOINT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = JOINT;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:17:7: ( 'JOINT' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:17:9: 'JOINT'
            {
            	Match("JOINT"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end JOINT

    // $ANTLR start OFFSET 
    public void mOFFSET() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = OFFSET;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:18:8: ( 'OFFSET' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:18:10: 'OFFSET'
            {
            	Match("OFFSET"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end OFFSET

    // $ANTLR start CHANNELS 
    public void mCHANNELS() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = CHANNELS;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:19:10: ( 'CHANNELS' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:19:12: 'CHANNELS'
            {
            	Match("CHANNELS"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end CHANNELS

    // $ANTLR start MOTION 
    public void mMOTION() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = MOTION;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:20:8: ( 'MOTION' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:20:10: 'MOTION'
            {
            	Match("MOTION"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end MOTION

    // $ANTLR start FRAMES 
    public void mFRAMES() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = FRAMES;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:21:8: ( 'Frames' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:21:10: 'Frames'
            {
            	Match("Frames"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end FRAMES

    // $ANTLR start FRAME_TIME 
    public void mFRAME_TIME() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = FRAME_TIME;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:22:12: ( 'Frame Time' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:22:14: 'Frame Time'
            {
            	Match("Frame Time"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end FRAME_TIME

    // $ANTLR start XPOSITION 
    public void mXPOSITION() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = XPOSITION;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:23:11: ( 'Xposition' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:23:13: 'Xposition'
            {
            	Match("Xposition"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end XPOSITION

    // $ANTLR start YPOSITION 
    public void mYPOSITION() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = YPOSITION;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:24:11: ( 'Yposition' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:24:13: 'Yposition'
            {
            	Match("Yposition"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end YPOSITION

    // $ANTLR start ZPOSITION 
    public void mZPOSITION() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = ZPOSITION;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:25:11: ( 'Zposition' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:25:13: 'Zposition'
            {
            	Match("Zposition"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end ZPOSITION

    // $ANTLR start ZROTATION 
    public void mZROTATION() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = ZROTATION;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:26:11: ( 'Zrotation' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:26:13: 'Zrotation'
            {
            	Match("Zrotation"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end ZROTATION

    // $ANTLR start XROTATION 
    public void mXROTATION() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = XROTATION;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:27:11: ( 'Xrotation' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:27:13: 'Xrotation'
            {
            	Match("Xrotation"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end XROTATION

    // $ANTLR start YROTATION 
    public void mYROTATION() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = YROTATION;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:28:11: ( 'Yrotation' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:28:13: 'Yrotation'
            {
            	Match("Yrotation"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end YROTATION

    // $ANTLR start END_SITE 
    public void mEND_SITE() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = END_SITE;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:29:10: ( 'End Site' )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:29:12: 'End Site'
            {
            	Match("End Site"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end END_SITE

    // $ANTLR start NL 
    public void mNL() // throws RecognitionException [2]
    {
        try 
    	{
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:43:4: ( '\\r\\n' | '\\n' | '\\r' )
            int alt1 = 3;
            int LA1_0 = input.LA(1);
            
            if ( (LA1_0 == '\r') )
            {
                int LA1_1 = input.LA(2);
                
                if ( (LA1_1 == '\n') )
                {
                    alt1 = 1;
                }
                else 
                {
                    alt1 = 3;}
            }
            else if ( (LA1_0 == '\n') )
            {
                alt1 = 2;
            }
            else 
            {
                NoViableAltException nvae_d1s0 =
                    new NoViableAltException("42:1: fragment NL : ( '\\r\\n' | '\\n' | '\\r' );", 1, 0, input);
            
                throw nvae_d1s0;
            }
            switch (alt1) 
            {
                case 1 :
                    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:43:5: '\\r\\n'
                    {
                    	Match("\r\n"); 

                    
                    }
                    break;
                case 2 :
                    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:44:3: '\\n'
                    {
                    	Match('\n'); 
                    
                    }
                    break;
                case 3 :
                    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:45:3: '\\r'
                    {
                    	Match('\r'); 
                    
                    }
                    break;
            
            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end NL

    // $ANTLR start WS 
    public void mWS() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = WS;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:48:4: ( ( ' ' | '\\t' | NL ) )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:48:5: ( ' ' | '\\t' | NL )
            {
            	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:48:5: ( ' ' | '\\t' | NL )
            	int alt2 = 3;
            	switch ( input.LA(1) ) 
            	{
            	case ' ':
            		{
            	    alt2 = 1;
            	    }
            	    break;
            	case '\t':
            		{
            	    alt2 = 2;
            	    }
            	    break;
            	case '\n':
            	case '\r':
            		{
            	    alt2 = 3;
            	    }
            	    break;
            		default:
            		    NoViableAltException nvae_d2s0 =
            		        new NoViableAltException("48:5: ( ' ' | '\\t' | NL )", 2, 0, input);
            	
            		    throw nvae_d2s0;
            	}
            	
            	switch (alt2) 
            	{
            	    case 1 :
            	        // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:48:6: ' '
            	        {
            	        	Match(' '); 
            	        
            	        }
            	        break;
            	    case 2 :
            	        // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:48:10: '\\t'
            	        {
            	        	Match('\t'); 
            	        
            	        }
            	        break;
            	    case 3 :
            	        // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:48:16: NL
            	        {
            	        	mNL(); 
            	        
            	        }
            	        break;
            	
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

    // $ANTLR start DIGIT 
    public void mDIGIT() // throws RecognitionException [2]
    {
        try 
    	{
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:52:7: ( ( '0' .. '9' ) )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:52:8: ( '0' .. '9' )
            {
            	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:52:8: ( '0' .. '9' )
            	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:52:9: '0' .. '9'
            	{
            		MatchRange('0','9'); 
            	
            	}

            
            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end DIGIT

    // $ANTLR start NUMBER 
    public void mNUMBER() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = NUMBER;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:8: ( ( MINUS )? ( DIGIT )+ ( DOT ( DIGIT )+ )? ( 'e' ( MINUS )? ( DIGIT )+ )? )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:11: ( MINUS )? ( DIGIT )+ ( DOT ( DIGIT )+ )? ( 'e' ( MINUS )? ( DIGIT )+ )?
            {
            	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:11: ( MINUS )?
            	int alt3 = 2;
            	int LA3_0 = input.LA(1);
            	
            	if ( (LA3_0 == '-') )
            	{
            	    alt3 = 1;
            	}
            	switch (alt3) 
            	{
            	    case 1 :
            	        // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:11: MINUS
            	        {
            	        	mMINUS(); 
            	        
            	        }
            	        break;
            	
            	}

            	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:18: ( DIGIT )+
            	int cnt4 = 0;
            	do 
            	{
            	    int alt4 = 2;
            	    int LA4_0 = input.LA(1);
            	    
            	    if ( ((LA4_0 >= '0' && LA4_0 <= '9')) )
            	    {
            	        alt4 = 1;
            	    }
            	    
            	
            	    switch (alt4) 
            		{
            			case 1 :
            			    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:18: DIGIT
            			    {
            			    	mDIGIT(); 
            			    
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

            	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:25: ( DOT ( DIGIT )+ )?
            	int alt6 = 2;
            	int LA6_0 = input.LA(1);
            	
            	if ( (LA6_0 == '.') )
            	{
            	    alt6 = 1;
            	}
            	switch (alt6) 
            	{
            	    case 1 :
            	        // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:26: DOT ( DIGIT )+
            	        {
            	        	mDOT(); 
            	        	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:30: ( DIGIT )+
            	        	int cnt5 = 0;
            	        	do 
            	        	{
            	        	    int alt5 = 2;
            	        	    int LA5_0 = input.LA(1);
            	        	    
            	        	    if ( ((LA5_0 >= '0' && LA5_0 <= '9')) )
            	        	    {
            	        	        alt5 = 1;
            	        	    }
            	        	    
            	        	
            	        	    switch (alt5) 
            	        		{
            	        			case 1 :
            	        			    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:30: DIGIT
            	        			    {
            	        			    	mDIGIT(); 
            	        			    
            	        			    }
            	        			    break;
            	        	
            	        			default:
            	        			    if ( cnt5 >= 1 ) goto loop5;
            	        		            EarlyExitException eee =
            	        		                new EarlyExitException(5, input);
            	        		            throw eee;
            	        	    }
            	        	    cnt5++;
            	        	} while (true);
            	        	
            	        	loop5:
            	        		;	// Stops C# compiler whinging that label 'loop5' has no statements

            	        
            	        }
            	        break;
            	
            	}

            	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:39: ( 'e' ( MINUS )? ( DIGIT )+ )?
            	int alt9 = 2;
            	int LA9_0 = input.LA(1);
            	
            	if ( (LA9_0 == 'e') )
            	{
            	    alt9 = 1;
            	}
            	switch (alt9) 
            	{
            	    case 1 :
            	        // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:40: 'e' ( MINUS )? ( DIGIT )+
            	        {
            	        	Match('e'); 
            	        	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:45: ( MINUS )?
            	        	int alt7 = 2;
            	        	int LA7_0 = input.LA(1);
            	        	
            	        	if ( (LA7_0 == '-') )
            	        	{
            	        	    alt7 = 1;
            	        	}
            	        	switch (alt7) 
            	        	{
            	        	    case 1 :
            	        	        // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:45: MINUS
            	        	        {
            	        	        	mMINUS(); 
            	        	        
            	        	        }
            	        	        break;
            	        	
            	        	}

            	        	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:52: ( DIGIT )+
            	        	int cnt8 = 0;
            	        	do 
            	        	{
            	        	    int alt8 = 2;
            	        	    int LA8_0 = input.LA(1);
            	        	    
            	        	    if ( ((LA8_0 >= '0' && LA8_0 <= '9')) )
            	        	    {
            	        	        alt8 = 1;
            	        	    }
            	        	    
            	        	
            	        	    switch (alt8) 
            	        		{
            	        			case 1 :
            	        			    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:55:52: DIGIT
            	        			    {
            	        			    	mDIGIT(); 
            	        			    
            	        			    }
            	        			    break;
            	        	
            	        			default:
            	        			    if ( cnt8 >= 1 ) goto loop8;
            	        		            EarlyExitException eee =
            	        		                new EarlyExitException(8, input);
            	        		            throw eee;
            	        	    }
            	        	    cnt8++;
            	        	} while (true);
            	        	
            	        	loop8:
            	        		;	// Stops C# compiler whinging that label 'loop8' has no statements

            	        
            	        }
            	        break;
            	
            	}

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end NUMBER

    // $ANTLR start LETTER 
    public void mLETTER() // throws RecognitionException [2]
    {
        try 
    	{
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:59:8: ( ( 'a' .. 'z' ) | ( 'A' .. 'Z' ) )
            int alt10 = 2;
            int LA10_0 = input.LA(1);
            
            if ( ((LA10_0 >= 'a' && LA10_0 <= 'z')) )
            {
                alt10 = 1;
            }
            else if ( ((LA10_0 >= 'A' && LA10_0 <= 'Z')) )
            {
                alt10 = 2;
            }
            else 
            {
                NoViableAltException nvae_d10s0 =
                    new NoViableAltException("58:1: fragment LETTER : ( ( 'a' .. 'z' ) | ( 'A' .. 'Z' ) );", 10, 0, input);
            
                throw nvae_d10s0;
            }
            switch (alt10) 
            {
                case 1 :
                    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:59:9: ( 'a' .. 'z' )
                    {
                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:59:9: ( 'a' .. 'z' )
                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:59:10: 'a' .. 'z'
                    	{
                    		MatchRange('a','z'); 
                    	
                    	}

                    
                    }
                    break;
                case 2 :
                    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:59:20: ( 'A' .. 'Z' )
                    {
                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:59:20: ( 'A' .. 'Z' )
                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:59:21: 'A' .. 'Z'
                    	{
                    		MatchRange('A','Z'); 
                    	
                    	}

                    
                    }
                    break;
            
            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end LETTER

    // $ANTLR start ID 
    public void mID() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = ID;
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:62:4: ( ( '_' | LETTER ) ( DIGIT | LETTER | '_' )* )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:62:6: ( '_' | LETTER ) ( DIGIT | LETTER | '_' )*
            {
            	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:62:6: ( '_' | LETTER )
            	int alt11 = 2;
            	int LA11_0 = input.LA(1);
            	
            	if ( (LA11_0 == '_') )
            	{
            	    alt11 = 1;
            	}
            	else if ( ((LA11_0 >= 'A' && LA11_0 <= 'Z') || (LA11_0 >= 'a' && LA11_0 <= 'z')) )
            	{
            	    alt11 = 2;
            	}
            	else 
            	{
            	    NoViableAltException nvae_d11s0 =
            	        new NoViableAltException("62:6: ( '_' | LETTER )", 11, 0, input);
            	
            	    throw nvae_d11s0;
            	}
            	switch (alt11) 
            	{
            	    case 1 :
            	        // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:62:7: '_'
            	        {
            	        	Match('_'); 
            	        
            	        }
            	        break;
            	    case 2 :
            	        // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:62:11: LETTER
            	        {
            	        	mLETTER(); 
            	        
            	        }
            	        break;
            	
            	}

            	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:62:18: ( DIGIT | LETTER | '_' )*
            	do 
            	{
            	    int alt12 = 4;
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
            	        alt12 = 1;
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
            			    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:62:19: DIGIT
            			    {
            			    	mDIGIT(); 
            			    
            			    }
            			    break;
            			case 2 :
            			    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:62:25: LETTER
            			    {
            			    	mLETTER(); 
            			    
            			    }
            			    break;
            			case 3 :
            			    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:62:32: '_'
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

    // $ANTLR start OctalEscape 
    public void mOctalEscape() // throws RecognitionException [2]
    {
        try 
    	{
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:67:5: ( ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' ) | ( '0' .. '7' ) ( '0' .. '7' ) | ( '0' .. '7' ) )
            int alt13 = 3;
            int LA13_0 = input.LA(1);
            
            if ( ((LA13_0 >= '0' && LA13_0 <= '3')) )
            {
                int LA13_1 = input.LA(2);
                
                if ( ((LA13_1 >= '0' && LA13_1 <= '7')) )
                {
                    int LA13_3 = input.LA(3);
                    
                    if ( ((LA13_3 >= '0' && LA13_3 <= '7')) )
                    {
                        alt13 = 1;
                    }
                    else 
                    {
                        alt13 = 2;}
                }
                else 
                {
                    alt13 = 3;}
            }
            else if ( ((LA13_0 >= '4' && LA13_0 <= '7')) )
            {
                int LA13_2 = input.LA(2);
                
                if ( ((LA13_2 >= '0' && LA13_2 <= '7')) )
                {
                    alt13 = 2;
                }
                else 
                {
                    alt13 = 3;}
            }
            else 
            {
                NoViableAltException nvae_d13s0 =
                    new NoViableAltException("65:1: fragment OctalEscape : ( ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' ) | ( '0' .. '7' ) ( '0' .. '7' ) | ( '0' .. '7' ) );", 13, 0, input);
            
                throw nvae_d13s0;
            }
            switch (alt13) 
            {
                case 1 :
                    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:67:10: ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' )
                    {
                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:67:10: ( '0' .. '3' )
                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:67:11: '0' .. '3'
                    	{
                    		MatchRange('0','3'); 
                    	
                    	}

                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:67:21: ( '0' .. '7' )
                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:67:22: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 
                    	
                    	}

                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:67:32: ( '0' .. '7' )
                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:67:33: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 
                    	
                    	}

                    
                    }
                    break;
                case 2 :
                    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:68:10: ( '0' .. '7' ) ( '0' .. '7' )
                    {
                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:68:10: ( '0' .. '7' )
                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:68:11: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 
                    	
                    	}

                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:68:21: ( '0' .. '7' )
                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:68:22: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 
                    	
                    	}

                    
                    }
                    break;
                case 3 :
                    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:69:10: ( '0' .. '7' )
                    {
                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:69:10: ( '0' .. '7' )
                    	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:69:11: '0' .. '7'
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
        // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:8: ( COLON | LCURLY | RCURLY | DOT | MINUS | HEIRARCHY | ROOT | JOINT | OFFSET | CHANNELS | MOTION | FRAMES | FRAME_TIME | XPOSITION | YPOSITION | ZPOSITION | ZROTATION | XROTATION | YROTATION | END_SITE | WS | NUMBER | ID )
        int alt14 = 23;
        switch ( input.LA(1) ) 
        {
        case ':':
        	{
            alt14 = 1;
            }
            break;
        case '{':
        	{
            alt14 = 2;
            }
            break;
        case '}':
        	{
            alt14 = 3;
            }
            break;
        case '.':
        	{
            alt14 = 4;
            }
            break;
        case '-':
        	{
            int LA14_5 = input.LA(2);
            
            if ( ((LA14_5 >= '0' && LA14_5 <= '9')) )
            {
                alt14 = 22;
            }
            else 
            {
                alt14 = 5;}
            }
            break;
        case 'H':
        	{
            int LA14_6 = input.LA(2);
            
            if ( (LA14_6 == 'I') )
            {
                int LA14_21 = input.LA(3);
                
                if ( (LA14_21 == 'E') )
                {
                    int LA14_35 = input.LA(4);
                    
                    if ( (LA14_35 == 'R') )
                    {
                        int LA14_49 = input.LA(5);
                        
                        if ( (LA14_49 == 'A') )
                        {
                            int LA14_63 = input.LA(6);
                            
                            if ( (LA14_63 == 'R') )
                            {
                                int LA14_76 = input.LA(7);
                                
                                if ( (LA14_76 == 'C') )
                                {
                                    int LA14_89 = input.LA(8);
                                    
                                    if ( (LA14_89 == 'H') )
                                    {
                                        int LA14_100 = input.LA(9);
                                        
                                        if ( (LA14_100 == 'Y') )
                                        {
                                            int LA14_108 = input.LA(10);
                                            
                                            if ( ((LA14_108 >= '0' && LA14_108 <= '9') || (LA14_108 >= 'A' && LA14_108 <= 'Z') || LA14_108 == '_' || (LA14_108 >= 'a' && LA14_108 <= 'z')) )
                                            {
                                                alt14 = 23;
                                            }
                                            else 
                                            {
                                                alt14 = 6;}
                                        }
                                        else 
                                        {
                                            alt14 = 23;}
                                    }
                                    else 
                                    {
                                        alt14 = 23;}
                                }
                                else 
                                {
                                    alt14 = 23;}
                            }
                            else 
                            {
                                alt14 = 23;}
                        }
                        else 
                        {
                            alt14 = 23;}
                    }
                    else 
                    {
                        alt14 = 23;}
                }
                else 
                {
                    alt14 = 23;}
            }
            else 
            {
                alt14 = 23;}
            }
            break;
        case 'R':
        	{
            int LA14_7 = input.LA(2);
            
            if ( (LA14_7 == 'O') )
            {
                int LA14_22 = input.LA(3);
                
                if ( (LA14_22 == 'O') )
                {
                    int LA14_36 = input.LA(4);
                    
                    if ( (LA14_36 == 'T') )
                    {
                        int LA14_50 = input.LA(5);
                        
                        if ( ((LA14_50 >= '0' && LA14_50 <= '9') || (LA14_50 >= 'A' && LA14_50 <= 'Z') || LA14_50 == '_' || (LA14_50 >= 'a' && LA14_50 <= 'z')) )
                        {
                            alt14 = 23;
                        }
                        else 
                        {
                            alt14 = 7;}
                    }
                    else 
                    {
                        alt14 = 23;}
                }
                else 
                {
                    alt14 = 23;}
            }
            else 
            {
                alt14 = 23;}
            }
            break;
        case 'J':
        	{
            int LA14_8 = input.LA(2);
            
            if ( (LA14_8 == 'O') )
            {
                int LA14_23 = input.LA(3);
                
                if ( (LA14_23 == 'I') )
                {
                    int LA14_37 = input.LA(4);
                    
                    if ( (LA14_37 == 'N') )
                    {
                        int LA14_51 = input.LA(5);
                        
                        if ( (LA14_51 == 'T') )
                        {
                            int LA14_65 = input.LA(6);
                            
                            if ( ((LA14_65 >= '0' && LA14_65 <= '9') || (LA14_65 >= 'A' && LA14_65 <= 'Z') || LA14_65 == '_' || (LA14_65 >= 'a' && LA14_65 <= 'z')) )
                            {
                                alt14 = 23;
                            }
                            else 
                            {
                                alt14 = 8;}
                        }
                        else 
                        {
                            alt14 = 23;}
                    }
                    else 
                    {
                        alt14 = 23;}
                }
                else 
                {
                    alt14 = 23;}
            }
            else 
            {
                alt14 = 23;}
            }
            break;
        case 'O':
        	{
            int LA14_9 = input.LA(2);
            
            if ( (LA14_9 == 'F') )
            {
                int LA14_24 = input.LA(3);
                
                if ( (LA14_24 == 'F') )
                {
                    int LA14_38 = input.LA(4);
                    
                    if ( (LA14_38 == 'S') )
                    {
                        int LA14_52 = input.LA(5);
                        
                        if ( (LA14_52 == 'E') )
                        {
                            int LA14_66 = input.LA(6);
                            
                            if ( (LA14_66 == 'T') )
                            {
                                int LA14_78 = input.LA(7);
                                
                                if ( ((LA14_78 >= '0' && LA14_78 <= '9') || (LA14_78 >= 'A' && LA14_78 <= 'Z') || LA14_78 == '_' || (LA14_78 >= 'a' && LA14_78 <= 'z')) )
                                {
                                    alt14 = 23;
                                }
                                else 
                                {
                                    alt14 = 9;}
                            }
                            else 
                            {
                                alt14 = 23;}
                        }
                        else 
                        {
                            alt14 = 23;}
                    }
                    else 
                    {
                        alt14 = 23;}
                }
                else 
                {
                    alt14 = 23;}
            }
            else 
            {
                alt14 = 23;}
            }
            break;
        case 'C':
        	{
            int LA14_10 = input.LA(2);
            
            if ( (LA14_10 == 'H') )
            {
                int LA14_25 = input.LA(3);
                
                if ( (LA14_25 == 'A') )
                {
                    int LA14_39 = input.LA(4);
                    
                    if ( (LA14_39 == 'N') )
                    {
                        int LA14_53 = input.LA(5);
                        
                        if ( (LA14_53 == 'N') )
                        {
                            int LA14_67 = input.LA(6);
                            
                            if ( (LA14_67 == 'E') )
                            {
                                int LA14_79 = input.LA(7);
                                
                                if ( (LA14_79 == 'L') )
                                {
                                    int LA14_91 = input.LA(8);
                                    
                                    if ( (LA14_91 == 'S') )
                                    {
                                        int LA14_101 = input.LA(9);
                                        
                                        if ( ((LA14_101 >= '0' && LA14_101 <= '9') || (LA14_101 >= 'A' && LA14_101 <= 'Z') || LA14_101 == '_' || (LA14_101 >= 'a' && LA14_101 <= 'z')) )
                                        {
                                            alt14 = 23;
                                        }
                                        else 
                                        {
                                            alt14 = 10;}
                                    }
                                    else 
                                    {
                                        alt14 = 23;}
                                }
                                else 
                                {
                                    alt14 = 23;}
                            }
                            else 
                            {
                                alt14 = 23;}
                        }
                        else 
                        {
                            alt14 = 23;}
                    }
                    else 
                    {
                        alt14 = 23;}
                }
                else 
                {
                    alt14 = 23;}
            }
            else 
            {
                alt14 = 23;}
            }
            break;
        case 'M':
        	{
            int LA14_11 = input.LA(2);
            
            if ( (LA14_11 == 'O') )
            {
                int LA14_26 = input.LA(3);
                
                if ( (LA14_26 == 'T') )
                {
                    int LA14_40 = input.LA(4);
                    
                    if ( (LA14_40 == 'I') )
                    {
                        int LA14_54 = input.LA(5);
                        
                        if ( (LA14_54 == 'O') )
                        {
                            int LA14_68 = input.LA(6);
                            
                            if ( (LA14_68 == 'N') )
                            {
                                int LA14_80 = input.LA(7);
                                
                                if ( ((LA14_80 >= '0' && LA14_80 <= '9') || (LA14_80 >= 'A' && LA14_80 <= 'Z') || LA14_80 == '_' || (LA14_80 >= 'a' && LA14_80 <= 'z')) )
                                {
                                    alt14 = 23;
                                }
                                else 
                                {
                                    alt14 = 11;}
                            }
                            else 
                            {
                                alt14 = 23;}
                        }
                        else 
                        {
                            alt14 = 23;}
                    }
                    else 
                    {
                        alt14 = 23;}
                }
                else 
                {
                    alt14 = 23;}
            }
            else 
            {
                alt14 = 23;}
            }
            break;
        case 'F':
        	{
            int LA14_12 = input.LA(2);
            
            if ( (LA14_12 == 'r') )
            {
                int LA14_27 = input.LA(3);
                
                if ( (LA14_27 == 'a') )
                {
                    int LA14_41 = input.LA(4);
                    
                    if ( (LA14_41 == 'm') )
                    {
                        int LA14_55 = input.LA(5);
                        
                        if ( (LA14_55 == 'e') )
                        {
                            switch ( input.LA(6) ) 
                            {
                            case ' ':
                            	{
                                alt14 = 13;
                                }
                                break;
                            case 's':
                            	{
                                int LA14_82 = input.LA(7);
                                
                                if ( ((LA14_82 >= '0' && LA14_82 <= '9') || (LA14_82 >= 'A' && LA14_82 <= 'Z') || LA14_82 == '_' || (LA14_82 >= 'a' && LA14_82 <= 'z')) )
                                {
                                    alt14 = 23;
                                }
                                else 
                                {
                                    alt14 = 12;}
                                }
                                break;
                            	default:
                                	alt14 = 23;
                                	break;}
                        
                        }
                        else 
                        {
                            alt14 = 23;}
                    }
                    else 
                    {
                        alt14 = 23;}
                }
                else 
                {
                    alt14 = 23;}
            }
            else 
            {
                alt14 = 23;}
            }
            break;
        case 'X':
        	{
            switch ( input.LA(2) ) 
            {
            case 'r':
            	{
                int LA14_28 = input.LA(3);
                
                if ( (LA14_28 == 'o') )
                {
                    int LA14_42 = input.LA(4);
                    
                    if ( (LA14_42 == 't') )
                    {
                        int LA14_56 = input.LA(5);
                        
                        if ( (LA14_56 == 'a') )
                        {
                            int LA14_70 = input.LA(6);
                            
                            if ( (LA14_70 == 't') )
                            {
                                int LA14_83 = input.LA(7);
                                
                                if ( (LA14_83 == 'i') )
                                {
                                    int LA14_94 = input.LA(8);
                                    
                                    if ( (LA14_94 == 'o') )
                                    {
                                        int LA14_102 = input.LA(9);
                                        
                                        if ( (LA14_102 == 'n') )
                                        {
                                            int LA14_110 = input.LA(10);
                                            
                                            if ( ((LA14_110 >= '0' && LA14_110 <= '9') || (LA14_110 >= 'A' && LA14_110 <= 'Z') || LA14_110 == '_' || (LA14_110 >= 'a' && LA14_110 <= 'z')) )
                                            {
                                                alt14 = 23;
                                            }
                                            else 
                                            {
                                                alt14 = 18;}
                                        }
                                        else 
                                        {
                                            alt14 = 23;}
                                    }
                                    else 
                                    {
                                        alt14 = 23;}
                                }
                                else 
                                {
                                    alt14 = 23;}
                            }
                            else 
                            {
                                alt14 = 23;}
                        }
                        else 
                        {
                            alt14 = 23;}
                    }
                    else 
                    {
                        alt14 = 23;}
                }
                else 
                {
                    alt14 = 23;}
                }
                break;
            case 'p':
            	{
                int LA14_29 = input.LA(3);
                
                if ( (LA14_29 == 'o') )
                {
                    int LA14_43 = input.LA(4);
                    
                    if ( (LA14_43 == 's') )
                    {
                        int LA14_57 = input.LA(5);
                        
                        if ( (LA14_57 == 'i') )
                        {
                            int LA14_71 = input.LA(6);
                            
                            if ( (LA14_71 == 't') )
                            {
                                int LA14_84 = input.LA(7);
                                
                                if ( (LA14_84 == 'i') )
                                {
                                    int LA14_95 = input.LA(8);
                                    
                                    if ( (LA14_95 == 'o') )
                                    {
                                        int LA14_103 = input.LA(9);
                                        
                                        if ( (LA14_103 == 'n') )
                                        {
                                            int LA14_111 = input.LA(10);
                                            
                                            if ( ((LA14_111 >= '0' && LA14_111 <= '9') || (LA14_111 >= 'A' && LA14_111 <= 'Z') || LA14_111 == '_' || (LA14_111 >= 'a' && LA14_111 <= 'z')) )
                                            {
                                                alt14 = 23;
                                            }
                                            else 
                                            {
                                                alt14 = 14;}
                                        }
                                        else 
                                        {
                                            alt14 = 23;}
                                    }
                                    else 
                                    {
                                        alt14 = 23;}
                                }
                                else 
                                {
                                    alt14 = 23;}
                            }
                            else 
                            {
                                alt14 = 23;}
                        }
                        else 
                        {
                            alt14 = 23;}
                    }
                    else 
                    {
                        alt14 = 23;}
                }
                else 
                {
                    alt14 = 23;}
                }
                break;
            	default:
                	alt14 = 23;
                	break;}
        
            }
            break;
        case 'Y':
        	{
            switch ( input.LA(2) ) 
            {
            case 'p':
            	{
                int LA14_30 = input.LA(3);
                
                if ( (LA14_30 == 'o') )
                {
                    int LA14_44 = input.LA(4);
                    
                    if ( (LA14_44 == 's') )
                    {
                        int LA14_58 = input.LA(5);
                        
                        if ( (LA14_58 == 'i') )
                        {
                            int LA14_72 = input.LA(6);
                            
                            if ( (LA14_72 == 't') )
                            {
                                int LA14_85 = input.LA(7);
                                
                                if ( (LA14_85 == 'i') )
                                {
                                    int LA14_96 = input.LA(8);
                                    
                                    if ( (LA14_96 == 'o') )
                                    {
                                        int LA14_104 = input.LA(9);
                                        
                                        if ( (LA14_104 == 'n') )
                                        {
                                            int LA14_112 = input.LA(10);
                                            
                                            if ( ((LA14_112 >= '0' && LA14_112 <= '9') || (LA14_112 >= 'A' && LA14_112 <= 'Z') || LA14_112 == '_' || (LA14_112 >= 'a' && LA14_112 <= 'z')) )
                                            {
                                                alt14 = 23;
                                            }
                                            else 
                                            {
                                                alt14 = 15;}
                                        }
                                        else 
                                        {
                                            alt14 = 23;}
                                    }
                                    else 
                                    {
                                        alt14 = 23;}
                                }
                                else 
                                {
                                    alt14 = 23;}
                            }
                            else 
                            {
                                alt14 = 23;}
                        }
                        else 
                        {
                            alt14 = 23;}
                    }
                    else 
                    {
                        alt14 = 23;}
                }
                else 
                {
                    alt14 = 23;}
                }
                break;
            case 'r':
            	{
                int LA14_31 = input.LA(3);
                
                if ( (LA14_31 == 'o') )
                {
                    int LA14_45 = input.LA(4);
                    
                    if ( (LA14_45 == 't') )
                    {
                        int LA14_59 = input.LA(5);
                        
                        if ( (LA14_59 == 'a') )
                        {
                            int LA14_73 = input.LA(6);
                            
                            if ( (LA14_73 == 't') )
                            {
                                int LA14_86 = input.LA(7);
                                
                                if ( (LA14_86 == 'i') )
                                {
                                    int LA14_97 = input.LA(8);
                                    
                                    if ( (LA14_97 == 'o') )
                                    {
                                        int LA14_105 = input.LA(9);
                                        
                                        if ( (LA14_105 == 'n') )
                                        {
                                            int LA14_113 = input.LA(10);
                                            
                                            if ( ((LA14_113 >= '0' && LA14_113 <= '9') || (LA14_113 >= 'A' && LA14_113 <= 'Z') || LA14_113 == '_' || (LA14_113 >= 'a' && LA14_113 <= 'z')) )
                                            {
                                                alt14 = 23;
                                            }
                                            else 
                                            {
                                                alt14 = 19;}
                                        }
                                        else 
                                        {
                                            alt14 = 23;}
                                    }
                                    else 
                                    {
                                        alt14 = 23;}
                                }
                                else 
                                {
                                    alt14 = 23;}
                            }
                            else 
                            {
                                alt14 = 23;}
                        }
                        else 
                        {
                            alt14 = 23;}
                    }
                    else 
                    {
                        alt14 = 23;}
                }
                else 
                {
                    alt14 = 23;}
                }
                break;
            	default:
                	alt14 = 23;
                	break;}
        
            }
            break;
        case 'Z':
        	{
            switch ( input.LA(2) ) 
            {
            case 'r':
            	{
                int LA14_32 = input.LA(3);
                
                if ( (LA14_32 == 'o') )
                {
                    int LA14_46 = input.LA(4);
                    
                    if ( (LA14_46 == 't') )
                    {
                        int LA14_60 = input.LA(5);
                        
                        if ( (LA14_60 == 'a') )
                        {
                            int LA14_74 = input.LA(6);
                            
                            if ( (LA14_74 == 't') )
                            {
                                int LA14_87 = input.LA(7);
                                
                                if ( (LA14_87 == 'i') )
                                {
                                    int LA14_98 = input.LA(8);
                                    
                                    if ( (LA14_98 == 'o') )
                                    {
                                        int LA14_106 = input.LA(9);
                                        
                                        if ( (LA14_106 == 'n') )
                                        {
                                            int LA14_114 = input.LA(10);
                                            
                                            if ( ((LA14_114 >= '0' && LA14_114 <= '9') || (LA14_114 >= 'A' && LA14_114 <= 'Z') || LA14_114 == '_' || (LA14_114 >= 'a' && LA14_114 <= 'z')) )
                                            {
                                                alt14 = 23;
                                            }
                                            else 
                                            {
                                                alt14 = 17;}
                                        }
                                        else 
                                        {
                                            alt14 = 23;}
                                    }
                                    else 
                                    {
                                        alt14 = 23;}
                                }
                                else 
                                {
                                    alt14 = 23;}
                            }
                            else 
                            {
                                alt14 = 23;}
                        }
                        else 
                        {
                            alt14 = 23;}
                    }
                    else 
                    {
                        alt14 = 23;}
                }
                else 
                {
                    alt14 = 23;}
                }
                break;
            case 'p':
            	{
                int LA14_33 = input.LA(3);
                
                if ( (LA14_33 == 'o') )
                {
                    int LA14_47 = input.LA(4);
                    
                    if ( (LA14_47 == 's') )
                    {
                        int LA14_61 = input.LA(5);
                        
                        if ( (LA14_61 == 'i') )
                        {
                            int LA14_75 = input.LA(6);
                            
                            if ( (LA14_75 == 't') )
                            {
                                int LA14_88 = input.LA(7);
                                
                                if ( (LA14_88 == 'i') )
                                {
                                    int LA14_99 = input.LA(8);
                                    
                                    if ( (LA14_99 == 'o') )
                                    {
                                        int LA14_107 = input.LA(9);
                                        
                                        if ( (LA14_107 == 'n') )
                                        {
                                            int LA14_115 = input.LA(10);
                                            
                                            if ( ((LA14_115 >= '0' && LA14_115 <= '9') || (LA14_115 >= 'A' && LA14_115 <= 'Z') || LA14_115 == '_' || (LA14_115 >= 'a' && LA14_115 <= 'z')) )
                                            {
                                                alt14 = 23;
                                            }
                                            else 
                                            {
                                                alt14 = 16;}
                                        }
                                        else 
                                        {
                                            alt14 = 23;}
                                    }
                                    else 
                                    {
                                        alt14 = 23;}
                                }
                                else 
                                {
                                    alt14 = 23;}
                            }
                            else 
                            {
                                alt14 = 23;}
                        }
                        else 
                        {
                            alt14 = 23;}
                    }
                    else 
                    {
                        alt14 = 23;}
                }
                else 
                {
                    alt14 = 23;}
                }
                break;
            	default:
                	alt14 = 23;
                	break;}
        
            }
            break;
        case 'E':
        	{
            int LA14_16 = input.LA(2);
            
            if ( (LA14_16 == 'n') )
            {
                int LA14_34 = input.LA(3);
                
                if ( (LA14_34 == 'd') )
                {
                    int LA14_48 = input.LA(4);
                    
                    if ( (LA14_48 == ' ') )
                    {
                        alt14 = 20;
                    }
                    else 
                    {
                        alt14 = 23;}
                }
                else 
                {
                    alt14 = 23;}
            }
            else 
            {
                alt14 = 23;}
            }
            break;
        case '\t':
        case '\n':
        case '\r':
        case ' ':
        	{
            alt14 = 21;
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
            alt14 = 22;
            }
            break;
        case 'A':
        case 'B':
        case 'D':
        case 'G':
        case 'I':
        case 'K':
        case 'L':
        case 'N':
        case 'P':
        case 'Q':
        case 'S':
        case 'T':
        case 'U':
        case 'V':
        case 'W':
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
            alt14 = 23;
            }
            break;
        	default:
        	    NoViableAltException nvae_d14s0 =
        	        new NoViableAltException("1:1: Tokens : ( COLON | LCURLY | RCURLY | DOT | MINUS | HEIRARCHY | ROOT | JOINT | OFFSET | CHANNELS | MOTION | FRAMES | FRAME_TIME | XPOSITION | YPOSITION | ZPOSITION | ZROTATION | XROTATION | YROTATION | END_SITE | WS | NUMBER | ID );", 14, 0, input);
        
        	    throw nvae_d14s0;
        }
        
        switch (alt14) 
        {
            case 1 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:10: COLON
                {
                	mCOLON(); 
                
                }
                break;
            case 2 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:16: LCURLY
                {
                	mLCURLY(); 
                
                }
                break;
            case 3 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:23: RCURLY
                {
                	mRCURLY(); 
                
                }
                break;
            case 4 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:30: DOT
                {
                	mDOT(); 
                
                }
                break;
            case 5 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:34: MINUS
                {
                	mMINUS(); 
                
                }
                break;
            case 6 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:40: HEIRARCHY
                {
                	mHEIRARCHY(); 
                
                }
                break;
            case 7 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:50: ROOT
                {
                	mROOT(); 
                
                }
                break;
            case 8 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:55: JOINT
                {
                	mJOINT(); 
                
                }
                break;
            case 9 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:61: OFFSET
                {
                	mOFFSET(); 
                
                }
                break;
            case 10 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:68: CHANNELS
                {
                	mCHANNELS(); 
                
                }
                break;
            case 11 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:77: MOTION
                {
                	mMOTION(); 
                
                }
                break;
            case 12 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:84: FRAMES
                {
                	mFRAMES(); 
                
                }
                break;
            case 13 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:91: FRAME_TIME
                {
                	mFRAME_TIME(); 
                
                }
                break;
            case 14 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:102: XPOSITION
                {
                	mXPOSITION(); 
                
                }
                break;
            case 15 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:112: YPOSITION
                {
                	mYPOSITION(); 
                
                }
                break;
            case 16 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:122: ZPOSITION
                {
                	mZPOSITION(); 
                
                }
                break;
            case 17 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:132: ZROTATION
                {
                	mZROTATION(); 
                
                }
                break;
            case 18 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:142: XROTATION
                {
                	mXROTATION(); 
                
                }
                break;
            case 19 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:152: YROTATION
                {
                	mYROTATION(); 
                
                }
                break;
            case 20 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:162: END_SITE
                {
                	mEND_SITE(); 
                
                }
                break;
            case 21 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:171: WS
                {
                	mWS(); 
                
                }
                break;
            case 22 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:174: NUMBER
                {
                	mNUMBER(); 
                
                }
                break;
            case 23 :
                // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:1:181: ID
                {
                	mID(); 
                
                }
                break;
        
        }
    
    }


	private void InitializeCyclicDFAs()
	{
	}

 
    
}
}