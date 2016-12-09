// $ANTLR 3.0.1 F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g 2014-09-21 00:29:20
namespace 
	Igneel.Importers.BVH

{

using System.Collections.Generic;	


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;



public class bvhParser : Parser 
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"COLON", 
		"LCURLY", 
		"RCURLY", 
		"DOT", 
		"MINUS", 
		"HEIRARCHY", 
		"ROOT", 
		"JOINT", 
		"OFFSET", 
		"CHANNELS", 
		"MOTION", 
		"FRAMES", 
		"FRAME_TIME", 
		"XPOSITION", 
		"YPOSITION", 
		"ZPOSITION", 
		"ZROTATION", 
		"XROTATION", 
		"YROTATION", 
		"END_SITE", 
		"NL", 
		"WS", 
		"DIGIT", 
		"NUMBER", 
		"LETTER", 
		"ID", 
		"OctalEscape"
    };

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
    public const int DIGIT = 26;
    public const int NL = 24;
    public const int DOT = 7;
    public const int OctalEscape = 30;
    
    
        public bvhParser(ITokenStream input) 
    		: base(input)
    	{
    		InitializeCyclicDFAs();
        }
        

    override public string[] TokenNames
	{
		get { return tokenNames; }
	}

    override public string GrammarFileName
	{
		get { return "F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g"; }
	}


    
    // $ANTLR start document
    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:73:1: document returns [BvhDocument doc = new BvhDocument()] : HEIRARCHY root= heirarchy[doc] m= motion ;
    public BvhDocument document() // throws RecognitionException [1]
    {   

        BvhDocument doc =  new BvhDocument();
    
        BvhNode root = null;

        BvhMotion m = null;
        
    
        try 
    	{
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:74:2: ( HEIRARCHY root= heirarchy[doc] m= motion )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:74:4: HEIRARCHY root= heirarchy[doc] m= motion
            {
            	Match(input,HEIRARCHY,FOLLOW_HEIRARCHY_in_document485); 
            	PushFollow(FOLLOW_heirarchy_in_document496);
            	root = heirarchy(doc);
            	followingStackPointer_--;

            	 doc.Root = root; 
            	PushFollow(FOLLOW_motion_in_document507);
            	m = motion();
            	followingStackPointer_--;

            	 doc.Motion = m; 
            
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
        return doc;
    }
    // $ANTLR end document

    
    // $ANTLR start motion
    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:79:1: motion returns [BvhMotion m =new BvhMotion()] : MOTION FRAMES COLON frames= NUMBER FRAME_TIME COLON time= NUMBER values[m.Data] ;
    public BvhMotion motion() // throws RecognitionException [1]
    {   

        BvhMotion m = new BvhMotion();
    
        IToken frames = null;
        IToken time = null;
    
        try 
    	{
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:80:2: ( MOTION FRAMES COLON frames= NUMBER FRAME_TIME COLON time= NUMBER values[m.Data] )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:80:4: MOTION FRAMES COLON frames= NUMBER FRAME_TIME COLON time= NUMBER values[m.Data]
            {
            	Match(input,MOTION,FOLLOW_MOTION_in_motion522); 
            	Match(input,FRAMES,FOLLOW_FRAMES_in_motion528); 
            	Match(input,COLON,FOLLOW_COLON_in_motion530); 
            	frames = (IToken)input.LT(1);
            	Match(input,NUMBER,FOLLOW_NUMBER_in_motion534); 
            	m.FrameCount = int.Parse( frames.Text ); 
            	Match(input,FRAME_TIME,FOLLOW_FRAME_TIME_in_motion541); 
            	Match(input,COLON,FOLLOW_COLON_in_motion543); 
            	time = (IToken)input.LT(1);
            	Match(input,NUMBER,FOLLOW_NUMBER_in_motion547); 
            	m.FrameTime = float.Parse( time.Text ); 
            	PushFollow(FOLLOW_values_in_motion556);
            	values(m.Data);
            	followingStackPointer_--;

            
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
    // $ANTLR end motion

    
    // $ANTLR start values
    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:85:1: values[List<float>data] : (num= NUMBER )+ ;
    public void values(List<float>data) // throws RecognitionException [1]
    {   
        IToken num = null;
    
        try 
    	{
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:86:2: ( (num= NUMBER )+ )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:86:4: (num= NUMBER )+
            {
            	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:86:4: (num= NUMBER )+
            	int cnt1 = 0;
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);
            	    
            	    if ( (LA1_0 == NUMBER) )
            	    {
            	        alt1 = 1;
            	    }
            	    
            	
            	    switch (alt1) 
            		{
            			case 1 :
            			    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:86:6: num= NUMBER
            			    {
            			    	num = (IToken)input.LT(1);
            			    	Match(input,NUMBER,FOLLOW_NUMBER_in_values573); 
            			    	 data.Add(float.Parse(num.Text)); 
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt1 >= 1 ) goto loop1;
            		            EarlyExitException eee =
            		                new EarlyExitException(1, input);
            		            throw eee;
            	    }
            	    cnt1++;
            	} while (true);
            	
            	loop1:
            		;	// Stops C# compiler whinging that label 'loop1' has no statements

            
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
        return ;
    }
    // $ANTLR end values

    
    // $ANTLR start heirarchy
    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:89:1: heirarchy[BvhDocument d] returns [BvhNode node = new BvhNode()] : ( ROOT | JOINT ) ID LCURLY v= offsets ch= channels ( (child= heirarchy[d] )+ | end= endsite ) RCURLY ;
    public BvhNode heirarchy(BvhDocument d) // throws RecognitionException [1]
    {   

        BvhNode node =  new BvhNode();
    
        IToken ID1 = null;
        Vector3? v = null;

        List<FODChannel> ch = null;

        BvhNode child = null;

        EndSite end = null;
        
    
        
        	  	d.Nodes.Add(node);
        	  
        try 
    	{
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:94:2: ( ( ROOT | JOINT ) ID LCURLY v= offsets ch= channels ( (child= heirarchy[d] )+ | end= endsite ) RCURLY )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:94:4: ( ROOT | JOINT ) ID LCURLY v= offsets ch= channels ( (child= heirarchy[d] )+ | end= endsite ) RCURLY
            {
            	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:94:4: ( ROOT | JOINT )
            	int alt2 = 2;
            	int LA2_0 = input.LA(1);
            	
            	if ( (LA2_0 == ROOT) )
            	{
            	    alt2 = 1;
            	}
            	else if ( (LA2_0 == JOINT) )
            	{
            	    alt2 = 2;
            	}
            	else 
            	{
            	    NoViableAltException nvae_d2s0 =
            	        new NoViableAltException("94:4: ( ROOT | JOINT )", 2, 0, input);
            	
            	    throw nvae_d2s0;
            	}
            	switch (alt2) 
            	{
            	    case 1 :
            	        // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:94:5: ROOT
            	        {
            	        	Match(input,ROOT,FOLLOW_ROOT_in_heirarchy611); 
            	        	node.IsRoot = true;
            	        
            	        }
            	        break;
            	    case 2 :
            	        // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:94:33: JOINT
            	        {
            	        	Match(input,JOINT,FOLLOW_JOINT_in_heirarchy616); 
            	        
            	        }
            	        break;
            	
            	}

            	ID1 = (IToken)input.LT(1);
            	Match(input,ID,FOLLOW_ID_in_heirarchy619); 
            	node.Name = ID1.Text;
            	Match(input,LCURLY,FOLLOW_LCURLY_in_heirarchy627); 
            	PushFollow(FOLLOW_offsets_in_heirarchy635);
            	v = offsets();
            	followingStackPointer_--;

            	node.Offset = (Vector3)v;
            	PushFollow(FOLLOW_channels_in_heirarchy646);
            	ch = channels();
            	followingStackPointer_--;

            	node.Channels = ch.ToArray();
            	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:98:6: ( (child= heirarchy[d] )+ | end= endsite )
            	int alt4 = 2;
            	int LA4_0 = input.LA(1);
            	
            	if ( ((LA4_0 >= ROOT && LA4_0 <= JOINT)) )
            	{
            	    alt4 = 1;
            	}
            	else if ( (LA4_0 == END_SITE) )
            	{
            	    alt4 = 2;
            	}
            	else 
            	{
            	    NoViableAltException nvae_d4s0 =
            	        new NoViableAltException("98:6: ( (child= heirarchy[d] )+ | end= endsite )", 4, 0, input);
            	
            	    throw nvae_d4s0;
            	}
            	switch (alt4) 
            	{
            	    case 1 :
            	        // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:98:8: (child= heirarchy[d] )+
            	        {
            	        	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:98:8: (child= heirarchy[d] )+
            	        	int cnt3 = 0;
            	        	do 
            	        	{
            	        	    int alt3 = 2;
            	        	    int LA3_0 = input.LA(1);
            	        	    
            	        	    if ( ((LA3_0 >= ROOT && LA3_0 <= JOINT)) )
            	        	    {
            	        	        alt3 = 1;
            	        	    }
            	        	    
            	        	
            	        	    switch (alt3) 
            	        		{
            	        			case 1 :
            	        			    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:98:9: child= heirarchy[d]
            	        			    {
            	        			    	PushFollow(FOLLOW_heirarchy_in_heirarchy660);
            	        			    	child = heirarchy(d);
            	        			    	followingStackPointer_--;

            	        			    	 node.Nodes.Add(child); 
            	        			    
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
            	        break;
            	    case 2 :
            	        // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:99:8: end= endsite
            	        {
            	        	PushFollow(FOLLOW_endsite_in_heirarchy677);
            	        	end = endsite();
            	        	followingStackPointer_--;

            	        	 node.End = end; 
            	        
            	        }
            	        break;
            	
            	}

            	Match(input,RCURLY,FOLLOW_RCURLY_in_heirarchy685); 
            
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
    // $ANTLR end heirarchy

    
    // $ANTLR start endsite
    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:103:1: endsite returns [EndSite end = new EndSite()] : END_SITE LCURLY v= offsets RCURLY ;
    public EndSite endsite() // throws RecognitionException [1]
    {   

        EndSite end =  new EndSite();
    
        Vector3? v = null;
        
    
        try 
    	{
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:104:2: ( END_SITE LCURLY v= offsets RCURLY )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:104:4: END_SITE LCURLY v= offsets RCURLY
            {
            	Match(input,END_SITE,FOLLOW_END_SITE_in_endsite700); 
            	Match(input,LCURLY,FOLLOW_LCURLY_in_endsite707); 
            	PushFollow(FOLLOW_offsets_in_endsite714);
            	v = offsets();
            	followingStackPointer_--;

            	 end.Offset = (Vector3)v; 
            	Match(input,RCURLY,FOLLOW_RCURLY_in_endsite720); 
            
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
        return end;
    }
    // $ANTLR end endsite

    
    // $ANTLR start channels
    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:110:1: channels returns [List<FODChannel>list] : CHANNELS len= NUMBER ( XPOSITION | YPOSITION | ZPOSITION | ZROTATION | XROTATION | YROTATION )+ ;
    public List<FODChannel> channels() // throws RecognitionException [1]
    {   

        List<FODChannel> list = null;
    
        IToken len = null;
    
        try 
    	{
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:111:2: ( CHANNELS len= NUMBER ( XPOSITION | YPOSITION | ZPOSITION | ZROTATION | XROTATION | YROTATION )+ )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:111:4: CHANNELS len= NUMBER ( XPOSITION | YPOSITION | ZPOSITION | ZROTATION | XROTATION | YROTATION )+
            {
            	Match(input,CHANNELS,FOLLOW_CHANNELS_in_channels735); 
            	len = (IToken)input.LT(1);
            	Match(input,NUMBER,FOLLOW_NUMBER_in_channels739); 
            	 list = new List<FODChannel>(int.Parse(len.Text)); 
            	// F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:112:2: ( XPOSITION | YPOSITION | ZPOSITION | ZROTATION | XROTATION | YROTATION )+
            	int cnt5 = 0;
            	do 
            	{
            	    int alt5 = 7;
            	    switch ( input.LA(1) ) 
            	    {
            	    case XPOSITION:
            	    	{
            	        alt5 = 1;
            	        }
            	        break;
            	    case YPOSITION:
            	    	{
            	        alt5 = 2;
            	        }
            	        break;
            	    case ZPOSITION:
            	    	{
            	        alt5 = 3;
            	        }
            	        break;
            	    case ZROTATION:
            	    	{
            	        alt5 = 4;
            	        }
            	        break;
            	    case XROTATION:
            	    	{
            	        alt5 = 5;
            	        }
            	        break;
            	    case YROTATION:
            	    	{
            	        alt5 = 6;
            	        }
            	        break;
            	    
            	    }
            	
            	    switch (alt5) 
            		{
            			case 1 :
            			    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:113:6: XPOSITION
            			    {
            			    	Match(input,XPOSITION,FOLLOW_XPOSITION_in_channels751); 
            			    	list.Add(FODChannel.XPosition); 
            			    
            			    }
            			    break;
            			case 2 :
            			    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:114:6: YPOSITION
            			    {
            			    	Match(input,YPOSITION,FOLLOW_YPOSITION_in_channels762); 
            			    	list.Add(FODChannel.YPosition); 
            			    
            			    }
            			    break;
            			case 3 :
            			    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:115:6: ZPOSITION
            			    {
            			    	Match(input,ZPOSITION,FOLLOW_ZPOSITION_in_channels774); 
            			    	list.Add(FODChannel.ZPosition); 
            			    
            			    }
            			    break;
            			case 4 :
            			    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:116:6: ZROTATION
            			    {
            			    	Match(input,ZROTATION,FOLLOW_ZROTATION_in_channels785); 
            			    	list.Add(FODChannel.ZRotation); 
            			    
            			    }
            			    break;
            			case 5 :
            			    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:117:6: XROTATION
            			    {
            			    	Match(input,XROTATION,FOLLOW_XROTATION_in_channels796); 
            			    	list.Add(FODChannel.XRotation); 
            			    
            			    }
            			    break;
            			case 6 :
            			    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:118:6: YROTATION
            			    {
            			    	Match(input,YROTATION,FOLLOW_YROTATION_in_channels807); 
            			    	list.Add(FODChannel.YRotation); 
            			    
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
    // $ANTLR end channels

    
    // $ANTLR start offsets
    // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:122:1: offsets returns [Vector3? v = new Vector3()] : OFFSET x= NUMBER y= NUMBER z= NUMBER ;
    public Vector3? offsets() // throws RecognitionException [1]
    {   

        Vector3? v =  new Vector3();
    
        IToken x = null;
        IToken y = null;
        IToken z = null;
    
        try 
    	{
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:123:2: ( OFFSET x= NUMBER y= NUMBER z= NUMBER )
            // F:\\Projects\\Igneel\\Igneel\\Importers\\BVH\\bvh.g:123:4: OFFSET x= NUMBER y= NUMBER z= NUMBER
            {
            	Match(input,OFFSET,FOLLOW_OFFSET_in_offsets829); 
            	x = (IToken)input.LT(1);
            	Match(input,NUMBER,FOLLOW_NUMBER_in_offsets834); 
            	y = (IToken)input.LT(1);
            	Match(input,NUMBER,FOLLOW_NUMBER_in_offsets839); 
            	z = (IToken)input.LT(1);
            	Match(input,NUMBER,FOLLOW_NUMBER_in_offsets843); 
            	
            		  v = new Vector3(float.Parse(x.Text),float.Parse(y.Text),float.Parse(z.Text));
            		
            
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
        return v;
    }
    // $ANTLR end offsets


	private void InitializeCyclicDFAs()
	{
	}

 

    public static readonly BitSet FOLLOW_HEIRARCHY_in_document485 = new BitSet(new ulong[]{0x0000000000000C00UL});
    public static readonly BitSet FOLLOW_heirarchy_in_document496 = new BitSet(new ulong[]{0x0000000000004000UL});
    public static readonly BitSet FOLLOW_motion_in_document507 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MOTION_in_motion522 = new BitSet(new ulong[]{0x0000000000008000UL});
    public static readonly BitSet FOLLOW_FRAMES_in_motion528 = new BitSet(new ulong[]{0x0000000000000010UL});
    public static readonly BitSet FOLLOW_COLON_in_motion530 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_NUMBER_in_motion534 = new BitSet(new ulong[]{0x0000000000010000UL});
    public static readonly BitSet FOLLOW_FRAME_TIME_in_motion541 = new BitSet(new ulong[]{0x0000000000000010UL});
    public static readonly BitSet FOLLOW_COLON_in_motion543 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_NUMBER_in_motion547 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_values_in_motion556 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NUMBER_in_values573 = new BitSet(new ulong[]{0x0000000008000002UL});
    public static readonly BitSet FOLLOW_ROOT_in_heirarchy611 = new BitSet(new ulong[]{0x0000000020000000UL});
    public static readonly BitSet FOLLOW_JOINT_in_heirarchy616 = new BitSet(new ulong[]{0x0000000020000000UL});
    public static readonly BitSet FOLLOW_ID_in_heirarchy619 = new BitSet(new ulong[]{0x0000000000000020UL});
    public static readonly BitSet FOLLOW_LCURLY_in_heirarchy627 = new BitSet(new ulong[]{0x0000000000001000UL});
    public static readonly BitSet FOLLOW_offsets_in_heirarchy635 = new BitSet(new ulong[]{0x0000000000002000UL});
    public static readonly BitSet FOLLOW_channels_in_heirarchy646 = new BitSet(new ulong[]{0x0000000000800C00UL});
    public static readonly BitSet FOLLOW_heirarchy_in_heirarchy660 = new BitSet(new ulong[]{0x0000000000000C40UL});
    public static readonly BitSet FOLLOW_endsite_in_heirarchy677 = new BitSet(new ulong[]{0x0000000000000040UL});
    public static readonly BitSet FOLLOW_RCURLY_in_heirarchy685 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_END_SITE_in_endsite700 = new BitSet(new ulong[]{0x0000000000000020UL});
    public static readonly BitSet FOLLOW_LCURLY_in_endsite707 = new BitSet(new ulong[]{0x0000000000001000UL});
    public static readonly BitSet FOLLOW_offsets_in_endsite714 = new BitSet(new ulong[]{0x0000000000000040UL});
    public static readonly BitSet FOLLOW_RCURLY_in_endsite720 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_CHANNELS_in_channels735 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_NUMBER_in_channels739 = new BitSet(new ulong[]{0x00000000007E0000UL});
    public static readonly BitSet FOLLOW_XPOSITION_in_channels751 = new BitSet(new ulong[]{0x00000000007E0002UL});
    public static readonly BitSet FOLLOW_YPOSITION_in_channels762 = new BitSet(new ulong[]{0x00000000007E0002UL});
    public static readonly BitSet FOLLOW_ZPOSITION_in_channels774 = new BitSet(new ulong[]{0x00000000007E0002UL});
    public static readonly BitSet FOLLOW_ZROTATION_in_channels785 = new BitSet(new ulong[]{0x00000000007E0002UL});
    public static readonly BitSet FOLLOW_XROTATION_in_channels796 = new BitSet(new ulong[]{0x00000000007E0002UL});
    public static readonly BitSet FOLLOW_YROTATION_in_channels807 = new BitSet(new ulong[]{0x00000000007E0002UL});
    public static readonly BitSet FOLLOW_OFFSET_in_offsets829 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_NUMBER_in_offsets834 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_NUMBER_in_offsets839 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_NUMBER_in_offsets843 = new BitSet(new ulong[]{0x0000000000000002UL});

}
}