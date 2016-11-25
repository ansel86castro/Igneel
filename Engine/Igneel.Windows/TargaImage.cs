﻿// ==========================================================
// TargaImage
//
// Design and implementation by
// - David Polomis (paloma_sw@cox.net)
//
//
// This source code, along with any associated files, is licensed under
// The Code Project Open License (CPOL) 1.02
// A copy of this license can be found in the CPOL.html file 
// which was downloaded with this source code
// or at http://www.codeproject.com/info/cpol10.aspx
//
// 
// COVERED CODE IS PROVIDED UNDER THIS LICENSE ON AN "AS IS" BASIS,
// WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED,
// INCLUDING, WITHOUT LIMITATION, WARRANTIES THAT THE COVERED CODE IS
// FREE OF DEFECTS, MERCHANTABLE, FIT FOR A PARTICULAR PURPOSE OR
// NON-INFRINGING. THE ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE
// OF THE COVERED CODE IS WITH YOU. SHOULD ANY COVERED CODE PROVE
// DEFECTIVE IN ANY RESPECT, YOU (NOT THE INITIAL DEVELOPER OR ANY
// OTHER CONTRIBUTOR) ASSUME THE COST OF ANY NECESSARY SERVICING,
// REPAIR OR CORRECTION. THIS DISCLAIMER OF WARRANTY CONSTITUTES AN
// ESSENTIAL PART OF THIS LICENSE. NO USE OF ANY COVERED CODE IS
// AUTHORIZED HEREUNDER EXCEPT UNDER THIS DISCLAIMER.
//
// Use at your own risk!
//
// ==========================================================


using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Igneel.Graphics
{
    internal static class TargaConstants
    {
        // constant byte lengths for various fields in the Targa format
        internal const int HeaderByteLength = 18;
        internal const int FooterByteLength = 26;
        internal const int FooterSignatureOffsetFromEnd = 18;
        internal const int FooterSignatureByteLength = 16;
        internal const int FooterReservedCharByteLength = 1;
        internal const int ExtensionAreaAuthorNameByteLength = 41;
        internal const int ExtensionAreaAuthorCommentsByteLength = 324;
        internal const int ExtensionAreaJobNameByteLength = 41;
        internal const int ExtensionAreaSoftwareIdByteLength = 41;
        internal const int ExtensionAreaSoftwareVersionLetterByteLength = 1;
        internal const int ExtensionAreaColorCorrectionTableValueLength = 256;
        internal const string TargaFooterAsciiSignature = "TRUEVISION-XFILE";
    }


    /// <summary>
    /// The Targa format of the file.
    /// </summary>
    public enum TgaFormat
    {
        /// <summary>
        /// UNKNOWN Targa Image format.
        /// </summary>
        UNKNOWN = 0,

        /// <summary>
        /// Original Targa Image format.
        /// </summary>
        /// <remarks>Targa Image does not have a Signature of ""TRUEVISION-XFILE"".</remarks>
        OriginalTga = 100,

        /// <summary>
        /// New Targa Image format
        /// </summary>
        /// <remarks>Targa Image has a TargaFooter with a Signature of ""TRUEVISION-XFILE"".</remarks>
        NewTga = 200
    }


    /// <summary>
    /// Indicates the type of color map, if any, included with the image file. 
    /// </summary>
    public enum ColorMapType : byte
    {
        /// <summary>
        /// No color map was included in the file.
        /// </summary>
        NoColorMap = 0,

        /// <summary>
        /// Color map was included in the file.
        /// </summary>
        ColorMapIncluded = 1
    }


    /// <summary>
    /// The type of image read from the file.
    /// </summary>
    public enum ImageType : byte
    {
        /// <summary>
        /// No image data was found in file.
        /// </summary>
        NoImageData = 0,

        /// <summary>
        /// Image is an uncompressed, indexed color-mapped image.
        /// </summary>
        UncompressedColorMapped = 1,

        /// <summary>
        /// Image is an uncompressed, RGB image.
        /// </summary>
        UncompressedTrueColor = 2,

        /// <summary>
        /// Image is an uncompressed, Greyscale image.
        /// </summary>
        UncompressedBlackAndWhite = 3,

        /// <summary>
        /// Image is a compressed, indexed color-mapped image.
        /// </summary>
        RunLengthEncodedColorMapped = 9,

        /// <summary>
        /// Image is a compressed, RGB image.
        /// </summary>
        RunLengthEncodedTrueColor = 10,

        /// <summary>
        /// Image is a compressed, Greyscale image.
        /// </summary>
        RunLengthEncodedBlackAndWhite = 11
    }


    /// <summary>
    /// The top-to-bottom ordering in which pixel data is transferred from the file to the screen.
    /// </summary>
    public enum VerticalTransferOrder 
    {
        /// <summary>
        /// UNKNOWN transfer order.
        /// </summary>
        UNKNOWN = -1,

        /// <summary>
        /// Transfer order of pixels is from the bottom to top.
        /// </summary>
        Bottom = 0,

        /// <summary>
        /// Transfer order of pixels is from the top to bottom.
        /// </summary>
        Top = 1
    }


    /// <summary>
    /// The left-to-right ordering in which pixel data is transferred from the file to the screen.
    /// </summary>
    public enum HorizontalTransferOrder 
    {
        /// <summary>
        /// UNKNOWN transfer order.
        /// </summary>
        UNKNOWN = -1,

        /// <summary>
        /// Transfer order of pixels is from the right to left.
        /// </summary>
        Right = 0,

        /// <summary>
        /// Transfer order of pixels is from the left to right.
        /// </summary>
        Left = 1
    }


    /// <summary>
    /// Screen destination of first pixel based on the VerticalTransferOrder and HorizontalTransferOrder.
    /// </summary>
    public enum FirstPixelDestination 
    {
        /// <summary>
        /// UNKNOWN first pixel destination.
        /// </summary>
        UNKNOWN = 0,

        /// <summary>
        /// First pixel destination is the top-left corner of the image.
        /// </summary>
        TopLeft = 1,

        /// <summary>
        /// First pixel destination is the top-right corner of the image.
        /// </summary>
        TopRight = 2,

        /// <summary>
        /// First pixel destination is the bottom-left corner of the image.
        /// </summary>
        BottomLeft = 3,

        /// <summary>
        /// First pixel destination is the bottom-right corner of the image.
        /// </summary>
        BottomRight = 4
    }


    /// <summary>
    /// The RLE packet type used in a RLE compressed image.
    /// </summary>
    public enum RlePacketType 
    {
        /// <summary>
        /// A raw RLE packet type.
        /// </summary>
        Raw = 0,

        /// <summary>
        /// A run-length RLE packet type.
        /// </summary>
        RunLength = 1
    }


    /// <summary>
    /// Reads and loads a Truevision TGA Format image file.
    /// </summary>
    public class TargaImage : IDisposable

    {
        private TargaHeader _objTargaHeader = null;
        private TargaExtensionArea _objTargaExtensionArea = null;
        private TargaFooter _objTargaFooter = null;
        private Bitmap _bmpTargaImage = null;
        private Bitmap _bmpImageThumbnail = null;
        private TgaFormat _eTgaFormat = TgaFormat.UNKNOWN;
        private string _strFileName = string.Empty;
        private int _intStride = 0;
        private int _intPadding = 0;
        private GCHandle _imageByteHandle;
        private GCHandle _thumbnailByteHandle;
        private System.Collections.Generic.List<System.Collections.Generic.List<byte>> _rows = new System.Collections.Generic.List<System.Collections.Generic.List<byte>>();
        private System.Collections.Generic.List<byte> _row = new System.Collections.Generic.List<byte>();
                        

        // Track whether Dispose has been called.
        private bool _disposed = false;


        /// <summary>
        /// Creates a new instance of the TargaImage object.
        /// </summary>
        public TargaImage()
        {
            this._objTargaFooter = new TargaFooter();
            this._objTargaHeader = new TargaHeader();
            this._objTargaExtensionArea = new TargaExtensionArea();
            this._bmpTargaImage = null;
            this._bmpImageThumbnail = null;
        }


        /// <summary>
        /// Gets a TargaHeader object that holds the Targa Header information of the loaded file.
        /// </summary>
        public TargaHeader Header
        {
            get { return this._objTargaHeader; }
        }


        /// <summary>
        /// Gets a TargaExtensionArea object that holds the Targa Extension Area information of the loaded file.
        /// </summary>
        public TargaExtensionArea ExtensionArea
        {
            get { return this._objTargaExtensionArea; }
        }


        /// <summary>
        /// Gets a TargaExtensionArea object that holds the Targa Footer information of the loaded file.
        /// </summary>
        public TargaFooter Footer
        {
            get { return this._objTargaFooter; }
        }


        /// <summary>
        /// Gets the Targa format of the loaded file.
        /// </summary>
        public TgaFormat Format
        {
            get { return this._eTgaFormat; }
        }


        /// <summary>
        /// Gets a Bitmap representation of the loaded file.
        /// </summary>
        public Bitmap Image
        {
            get { return this._bmpTargaImage; }
        }

        /// <summary>
        /// Gets the thumbnail of the loaded file if there is one in the file.
        /// </summary>
        public Bitmap Thumbnail
        {
            get { return this._bmpImageThumbnail; }
        }

        /// <summary>
        /// Gets the full path and filename of the loaded file.
        /// </summary>
        public string FileName
        {
            get { return this._strFileName; }
        }


        /// <summary>
        /// Gets the byte offset between the beginning of one scan line and the next. Used when loading the image into the Image Bitmap.
        /// </summary>
        /// <remarks>
        /// The memory allocated for Microsoft Bitmaps must be aligned on a 32bit boundary.
        /// The stride refers to the number of bytes allocated for one scanline of the bitmap.
        /// </remarks>
        public int Stride
        {
            get { return this._intStride; }
        }


        /// <summary>
        /// Gets the number of bytes used to pad each scan line to meet the Stride value. Used when loading the image into the Image Bitmap.
        /// </summary>
        /// <remarks>
        /// The memory allocated for Microsoft Bitmaps must be aligned on a 32bit boundary.
        /// The stride refers to the number of bytes allocated for one scanline of the bitmap.
        /// In your loop, you copy the pixels one scanline at a time and take into 
        /// consideration the amount of padding that occurs due to memory alignment.
        /// </remarks>
        public int Padding
        {
            get { return this._intPadding; }
        }


        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method 
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        /// <summary>
        /// TargaImage deconstructor.
        /// </summary>
        ~TargaImage()      
        {
           // Do not re-create Dispose clean-up code here.
           // Calling Dispose(false) is optimal in terms of
           // readability and maintainability.
           Dispose(false);
        }


        /// <summary>
        /// Creates a new instance of the TargaImage object with strFileName as the image loaded.
        /// </summary>
        public TargaImage(string strFileName) : this()
        {
            // make sure we have a .tga file
            if (System.IO.Path.GetExtension(strFileName).ToLower() == ".tga")
            {
                // make sure the file exists
                if (System.IO.File.Exists(strFileName) == true)
                {
                    this._strFileName = strFileName;
                    MemoryStream filestream = null;
                    BinaryReader binReader = null;
                    byte[] filebytes = null;

                    // load the file as an array of bytes
                    filebytes = System.IO.File.ReadAllBytes(this._strFileName);
                    if (filebytes != null && filebytes.Length > 0)
                    {
                        // create a seekable memory stream of the file bytes
                        using (filestream = new MemoryStream(filebytes))
                        {
                            if (filestream != null && filestream.Length > 0 && filestream.CanSeek == true)
                            {
                                // create a BinaryReader used to read the Targa file
                                using (binReader = new BinaryReader(filestream))
                                {
                                    this.LoadTgaFooterInfo(binReader);
                                    this.LoadTgaHeaderInfo(binReader);
                                    this.LoadTgaExtensionArea(binReader);
                                    this.LoadTgaImage(binReader);
                                }
                            }
                            else
                                throw new Exception(@"Error loading file, could not read file from disk.");
                        
                        }

                    }
                    else
                        throw new Exception(@"Error loading file, could not read file from disk.");

                }
                else
                    throw new Exception(@"Error loading file, could not find file '" + strFileName + "' on disk.");

            }
            else
                throw new Exception(@"Error loading file, file '" + strFileName + "' must have an extension of '.tga'.");

            
        }


        /// <summary>
        /// Loads the Targa Footer information from the file.
        /// </summary>
        /// <param name="binReader">A BinaryReader that points the loaded file byte stream.</param>
        private void LoadTgaFooterInfo(BinaryReader binReader)
        {

                if (binReader != null && binReader.BaseStream != null && binReader.BaseStream.Length > 0 && binReader.BaseStream.CanSeek == true)
                {
                    
                    try
                    {            
                        // set the cursor at the beginning of the signature string.
                        binReader.BaseStream.Seek((TargaConstants.FooterSignatureOffsetFromEnd * -1), SeekOrigin.End);

                        // read the signature bytes and convert to ascii string
                        string signature = System.Text.Encoding.ASCII.GetString(binReader.ReadBytes(TargaConstants.FooterSignatureByteLength)).TrimEnd('\0');

                        // do we have a proper signature
                        if (string.Compare(signature, TargaConstants.TargaFooterAsciiSignature) == 0)
                        {
                            // this is a NEW targa file.
                            // create the footer
                            this._eTgaFormat = TgaFormat.NewTga;

                            // set cursor to beginning of footer info
                            binReader.BaseStream.Seek((TargaConstants.FooterByteLength * -1), SeekOrigin.End);

                            // read the Extension Area Offset value
                            int extOffset = binReader.ReadInt32();

                            // read the Developer Directory Offset value
                            int devDirOff = binReader.ReadInt32();

                            // skip the signature we have already read it.
                            binReader.ReadBytes(TargaConstants.FooterSignatureByteLength);

                            // read the reserved character
                            string resChar = System.Text.Encoding.ASCII.GetString(binReader.ReadBytes(TargaConstants.FooterReservedCharByteLength)).TrimEnd('\0');

                            // set all values to our TargaFooter class
                            this._objTargaFooter.SetExtensionAreaOffset(extOffset);
                            this._objTargaFooter.SetDeveloperDirectoryOffset(devDirOff);
                            this._objTargaFooter.SetSignature(signature);
                            this._objTargaFooter.SetReservedCharacter(resChar);
                        }
                        else
                        {
                            // this is not an ORIGINAL targa file.
                            this._eTgaFormat = TgaFormat.OriginalTga;
                        }
                    }
                    catch ( Exception ex )
                    {
                        // clear all 
                        this.ClearAll();
                        throw ex;
                    }
                }
                else
                {
                    this.ClearAll();
                    throw new Exception(@"Error loading file, could not read file from disk.");
                }

           
        }


        /// <summary>
        /// Loads the Targa Header information from the file.
        /// </summary>
        /// <param name="binReader">A BinaryReader that points the loaded file byte stream.</param>
        private void LoadTgaHeaderInfo(BinaryReader binReader)
        {

            if (binReader != null && binReader.BaseStream != null && binReader.BaseStream.Length > 0 && binReader.BaseStream.CanSeek == true)
            {
                try
                {
                    // set the cursor at the beginning of the file.
                    binReader.BaseStream.Seek(0, SeekOrigin.Begin);

                    // read the header properties from the file
                    this._objTargaHeader.SetImageIdLength(binReader.ReadByte());
                    this._objTargaHeader.SetColorMapType((ColorMapType)binReader.ReadByte());
                    this._objTargaHeader.SetImageType((ImageType)binReader.ReadByte());

                    this._objTargaHeader.SetColorMapFirstEntryIndex(binReader.ReadInt16());
                    this._objTargaHeader.SetColorMapLength(binReader.ReadInt16());
                    this._objTargaHeader.SetColorMapEntrySize(binReader.ReadByte());

                    this._objTargaHeader.SetXOrigin(binReader.ReadInt16());
                    this._objTargaHeader.SetYOrigin(binReader.ReadInt16());
                    this._objTargaHeader.SetWidth(binReader.ReadInt16());
                    this._objTargaHeader.SetHeight(binReader.ReadInt16());

                    byte pixeldepth = binReader.ReadByte();
                    switch (pixeldepth)
                    {
                        case 8:
                        case 16:
                        case 24:
                        case 32:
                            this._objTargaHeader.SetPixelDepth(pixeldepth);
                            break;

                        default:
                            this.ClearAll();
                            throw new Exception("Targa Image only supports 8, 16, 24, or 32 bit pixel depths.");
                    }
                    

                    byte imageDescriptor = binReader.ReadByte();
                    this._objTargaHeader.SetAttributeBits((byte)Utilities.GetBits(imageDescriptor, 0, 4));

                    this._objTargaHeader.SetVerticalTransferOrder((VerticalTransferOrder)Utilities.GetBits(imageDescriptor, 5, 1));
                    this._objTargaHeader.SetHorizontalTransferOrder((HorizontalTransferOrder)Utilities.GetBits(imageDescriptor, 4, 1));

                    // load ImageID value if any
                    if (this._objTargaHeader.ImageIdLength > 0)
                    {
                        byte[] imageIdValueBytes = binReader.ReadBytes(this._objTargaHeader.ImageIdLength);
                        this._objTargaHeader.SetImageIdValue(System.Text.Encoding.ASCII.GetString(imageIdValueBytes).TrimEnd('\0'));
                    }
                }
                catch (Exception ex)
                {
                    this.ClearAll();
                    throw ex;
                }


                // load color map if it's included and/or needed
                // Only needed for UNCOMPRESSED_COLOR_MAPPED and RUN_LENGTH_ENCODED_COLOR_MAPPED
                // image types. If color map is included for other file types we can ignore it.
                if (this._objTargaHeader.ColorMapType == ColorMapType.ColorMapIncluded)
                {
                    if (this._objTargaHeader.ImageType == ImageType.UncompressedColorMapped || 
                        this._objTargaHeader.ImageType == ImageType.RunLengthEncodedColorMapped)
                    {
                        if (this._objTargaHeader.ColorMapLength > 0)
                        {
                            try
                            {
                                for (int i = 0; i < this._objTargaHeader.ColorMapLength; i++)
                                {
                                    int a = 0;
                                    int r = 0;
                                    int g = 0;
                                    int b = 0;

                                    // load each color map entry based on the ColorMapEntrySize value
                                    switch (this._objTargaHeader.ColorMapEntrySize)
                                    {
                                        case 15:
                                            byte[] color15 = binReader.ReadBytes(2);
                                            // remember that the bytes are stored in reverse oreder
                                            this._objTargaHeader.ColorMap.Add(Utilities.GetColorFrom2Bytes(color15[1], color15[0]));
                                            break;
                                        case 16:
                                            byte[] color16 = binReader.ReadBytes(2);
                                            // remember that the bytes are stored in reverse oreder
                                            this._objTargaHeader.ColorMap.Add(Utilities.GetColorFrom2Bytes(color16[1], color16[0]));
                                            break;
                                        case 24:
                                            b = Convert.ToInt32(binReader.ReadByte());
                                            g = Convert.ToInt32(binReader.ReadByte());
                                            r = Convert.ToInt32(binReader.ReadByte());
                                            this._objTargaHeader.ColorMap.Add(System.Drawing.Color.FromArgb(r, g, b));
                                            break;
                                        case 32:
                                            a = Convert.ToInt32(binReader.ReadByte());
                                            b = Convert.ToInt32(binReader.ReadByte());
                                            g = Convert.ToInt32(binReader.ReadByte());
                                            r = Convert.ToInt32(binReader.ReadByte());
                                            this._objTargaHeader.ColorMap.Add(System.Drawing.Color.FromArgb(a, r, g, b));
                                            break;
                                        default:
                                            this.ClearAll();
                                            throw new Exception("TargaImage only supports ColorMap Entry Sizes of 15, 16, 24 or 32 bits.");
                                            
                                    }


                                }
                            }
                            catch (Exception ex)
                            {
                                this.ClearAll();
                                throw ex;
                            }

                            

                        }
                        else
                        {
                            this.ClearAll();
                            throw new Exception("Image Type requires a Color Map and Color Map Length is zero.");
                        }
                    }


                }
                else
                {
                    if (this._objTargaHeader.ImageType == ImageType.UncompressedColorMapped || 
                        this._objTargaHeader.ImageType == ImageType.RunLengthEncodedColorMapped)
                    {
                        this.ClearAll();
                        throw new Exception("Image Type requires a Color Map and there was not a Color Map included in the file.");
                    }
                }


            }
            else
            {
                this.ClearAll();
                throw new Exception(@"Error loading file, could not read file from disk.");
            }
        }


        /// <summary>
        /// Loads the Targa Extension Area from the file, if it exists.
        /// </summary>
        /// <param name="binReader">A BinaryReader that points the loaded file byte stream.</param>
        private void LoadTgaExtensionArea(BinaryReader binReader)
        {

            if (binReader != null && binReader.BaseStream != null && binReader.BaseStream.Length > 0 && binReader.BaseStream.CanSeek == true)
            {
                // is there an Extension Area in file
                if (this._objTargaFooter.ExtensionAreaOffset > 0)
                {
                    try
                    {
                        // set the cursor at the beginning of the Extension Area using ExtensionAreaOffset.
                        binReader.BaseStream.Seek(this._objTargaFooter.ExtensionAreaOffset, SeekOrigin.Begin);

                        // load the extension area fields from the file

                        this._objTargaExtensionArea.SetExtensionSize((int)(binReader.ReadInt16()));
                        this._objTargaExtensionArea.SetAuthorName(System.Text.Encoding.ASCII.GetString(binReader.ReadBytes(TargaConstants.ExtensionAreaAuthorNameByteLength)).TrimEnd('\0'));
                        this._objTargaExtensionArea.SetAuthorComments(System.Text.Encoding.ASCII.GetString(binReader.ReadBytes(TargaConstants.ExtensionAreaAuthorCommentsByteLength)).TrimEnd('\0'));


                        // get the date/time stamp of the file
                        Int16 iMonth = binReader.ReadInt16();
                        Int16 iDay = binReader.ReadInt16();
                        Int16 iYear = binReader.ReadInt16();
                        Int16 iHour = binReader.ReadInt16();
                        Int16 iMinute = binReader.ReadInt16();
                        Int16 iSecond = binReader.ReadInt16();
                        DateTime dtstamp;
                        string strStamp = iMonth.ToString() + @"/" + iDay.ToString() + @"/" + iYear.ToString() + @" ";
                        strStamp += iHour.ToString() + @":" + iMinute.ToString() + @":" + iSecond.ToString();
                        if (DateTime.TryParse(strStamp, out dtstamp) == true)
                            this._objTargaExtensionArea.SetDateTimeStamp(dtstamp);


                        this._objTargaExtensionArea.SetJobName(System.Text.Encoding.ASCII.GetString(binReader.ReadBytes(TargaConstants.ExtensionAreaJobNameByteLength)).TrimEnd('\0'));


                        // get the job time of the file
                        iHour = binReader.ReadInt16();
                        iMinute = binReader.ReadInt16();
                        iSecond = binReader.ReadInt16();
                        TimeSpan ts = new TimeSpan((int)iHour, (int)iMinute, (int)iSecond);
                        this._objTargaExtensionArea.SetJobTime(ts);


                        this._objTargaExtensionArea.SetSoftwareId(System.Text.Encoding.ASCII.GetString(binReader.ReadBytes(TargaConstants.ExtensionAreaSoftwareIdByteLength)).TrimEnd('\0'));


                        // get the version number and letter from file
                        float iVersionNumber = (float)binReader.ReadInt16() / 100.0F;
                        string strVersionLetter = System.Text.Encoding.ASCII.GetString(binReader.ReadBytes(TargaConstants.ExtensionAreaSoftwareVersionLetterByteLength)).TrimEnd('\0');
                        
                        
                        this._objTargaExtensionArea.SetSoftwareId(iVersionNumber.ToString(@"F2") + strVersionLetter);


                        // get the color key of the file
                        int a = (int)binReader.ReadByte();
                        int r = (int)binReader.ReadByte();
                        int b = (int)binReader.ReadByte();
                        int g = (int)binReader.ReadByte();
                        this._objTargaExtensionArea.SetKeyColor(Color.FromArgb(a, r, g, b));


                        this._objTargaExtensionArea.SetPixelAspectRatioNumerator((int)binReader.ReadInt16());
                        this._objTargaExtensionArea.SetPixelAspectRatioDenominator((int)binReader.ReadInt16());
                        this._objTargaExtensionArea.SetGammaNumerator((int)binReader.ReadInt16());
                        this._objTargaExtensionArea.SetGammaDenominator((int)binReader.ReadInt16());
                        this._objTargaExtensionArea.SetColorCorrectionOffset(binReader.ReadInt32());
                        this._objTargaExtensionArea.SetPostageStampOffset(binReader.ReadInt32());
                        this._objTargaExtensionArea.SetScanLineOffset(binReader.ReadInt32());
                        this._objTargaExtensionArea.SetAttributesType((int)binReader.ReadByte());


                        // load Scan Line Table from file if any
                        if (this._objTargaExtensionArea.ScanLineOffset > 0)
                        {
                            binReader.BaseStream.Seek(this._objTargaExtensionArea.ScanLineOffset, SeekOrigin.Begin);
                            for (int i = 0; i < this._objTargaHeader.Height; i++)
                            {
                                this._objTargaExtensionArea.ScanLineTable.Add(binReader.ReadInt32());
                            }
                        }


                        // load Color Correction Table from file if any
                        if (this._objTargaExtensionArea.ColorCorrectionOffset > 0)
                        {
                            binReader.BaseStream.Seek(this._objTargaExtensionArea.ColorCorrectionOffset, SeekOrigin.Begin);
                            for (int i = 0; i < TargaConstants.ExtensionAreaColorCorrectionTableValueLength; i++)
                            {
                                a = (int)binReader.ReadInt16();
                                r = (int)binReader.ReadInt16();
                                b = (int)binReader.ReadInt16();
                                g = (int)binReader.ReadInt16();
                                this._objTargaExtensionArea.ColorCorrectionTable.Add(Color.FromArgb(a, r, g, b));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ClearAll();
                        throw ex;
                    }
                }
            }
            else
            {
                this.ClearAll();
                throw new Exception(@"Error loading file, could not read file from disk.");
            }
        }

        /// <summary>
        /// Reads the image data bytes from the file. Handles Uncompressed and RLE Compressed image data. 
        /// Uses FirstPixelDestination to properly align the image.
        /// </summary>
        /// <param name="binReader">A BinaryReader that points the loaded file byte stream.</param>
        /// <returns>An array of bytes representing the image data in the proper alignment.</returns>
        private byte[] LoadImageBytes(BinaryReader binReader)
        {

            // read the image data into a byte array
            // take into account stride has to be a multiple of 4
            // use padding to make sure multiple of 4    
        
            byte[] data = null;
            if (binReader != null && binReader.BaseStream != null && binReader.BaseStream.Length > 0 && binReader.BaseStream.CanSeek == true)
            {
                if (this._objTargaHeader.ImageDataOffset > 0)
                {
                    // padding bytes
                    byte[] padding = new byte[this._intPadding];
                    MemoryStream msData = null;
                                       
                    // seek to the beginning of the image data using the ImageDataOffset value
                    binReader.BaseStream.Seek(this._objTargaHeader.ImageDataOffset, SeekOrigin.Begin);
                   
                    
                    // get the size in bytes of each row in the image
                    int intImageRowByteSize = (int)this._objTargaHeader.Width * ((int)this._objTargaHeader.BytesPerPixel);

                    // get the size in bytes of the whole image
                    int intImageByteSize = intImageRowByteSize * (int)this._objTargaHeader.Height;
                                    
                    // is this a RLE compressed image type
                    if (this._objTargaHeader.ImageType == ImageType.RunLengthEncodedBlackAndWhite ||
                       this._objTargaHeader.ImageType == ImageType.RunLengthEncodedColorMapped ||
                       this._objTargaHeader.ImageType == ImageType.RunLengthEncodedTrueColor)
                    {

                        #region COMPRESSED
                        
                            // RLE Packet info
                            byte bRlePacket = 0;
                            int intRlePacketType = -1;
                            int intRlePixelCount = 0;
                            byte[] bRunLengthPixel = null;

                            // used to keep track of bytes read
                            int intImageBytesRead = 0;
                            int intImageRowBytesRead = 0;

                            // keep reading until we have the all image bytes
                            while (intImageBytesRead < intImageByteSize)
                            {
                                // get the RLE packet
                                bRlePacket = binReader.ReadByte();
                                intRlePacketType = Utilities.GetBits(bRlePacket, 7, 1);
                                intRlePixelCount = Utilities.GetBits(bRlePacket, 0, 7) + 1;

                                // check the RLE packet type
                                if ((RlePacketType)intRlePacketType == RlePacketType.RunLength)
                                {
                                    // get the pixel color data
                                    bRunLengthPixel = binReader.ReadBytes((int)this._objTargaHeader.BytesPerPixel);

                                    // add the number of pixels specified using the read pixel color
                                    for (int i = 0; i < intRlePixelCount; i++)
                                    {
                                        foreach (byte b in bRunLengthPixel)
                                            _row.Add(b);

                                        // increment the byte counts
                                        intImageRowBytesRead += bRunLengthPixel.Length;
                                        intImageBytesRead += bRunLengthPixel.Length;

                                        // if we have read a full image row
                                        // add the row to the row list and clear it
                                        // restart row byte count
                                        if (intImageRowBytesRead == intImageRowByteSize)
                                        {
                                            _rows.Add(_row);
                                            _row = new System.Collections.Generic.List<byte>();
                                            intImageRowBytesRead = 0;
                                            
                                        }
                                    }

                                }

                                else if ((RlePacketType)intRlePacketType == RlePacketType.Raw)
                                {
                                    // get the number of bytes to read based on the read pixel count
                                    int intBytesToRead = intRlePixelCount * (int)this._objTargaHeader.BytesPerPixel;

                                    // read each byte
                                    for (int i = 0;i < intBytesToRead;i++)
                                    {
                                        _row.Add(binReader.ReadByte());

                                        // increment the byte counts
                                        intImageBytesRead++;
                                        intImageRowBytesRead++;

                                        // if we have read a full image row
                                        // add the row to the row list and clear it
                                        // restart row byte count
                                        if (intImageRowBytesRead == intImageRowByteSize)
                                        {
                                            _rows.Add(_row);
                                            _row = new System.Collections.Generic.List<byte>();
                                            intImageRowBytesRead = 0;
                                        }

                                    }

                                }
                            }

                        #endregion

                    }

                    else
                    {
                        #region NON-COMPRESSED

                        // loop through each row in the image
                        for (int i = 0; i < (int)this._objTargaHeader.Height; i++)
                        {
                            // loop through each byte in the row
                            for (int j = 0; j < intImageRowByteSize; j++)
                            {
                                // add the byte to the row
                                _row.Add(binReader.ReadByte());
                            }

                            // add row to the list of rows
                            _rows.Add(_row);

                            // create a new row
                            _row = new System.Collections.Generic.List<byte>();
                        }

                        
                        #endregion
                    }

                    // flag that states whether or not to reverse the location of all rows.
                    bool blnRowsReverse = false;

                    // flag that states whether or not to reverse the bytes in each row.
                    bool blnEachRowReverse = false;

                    // use FirstPixelDestination to determine the alignment of the 
                    // image data byte
                    switch (this._objTargaHeader.FirstPixelDestination)
                    {
                        case FirstPixelDestination.TopLeft:
                            blnRowsReverse = false;
                            blnEachRowReverse = true;
                            break;

                        case FirstPixelDestination.TopRight:
                            blnRowsReverse = false;
                            blnEachRowReverse = false;
                            break;

                        case FirstPixelDestination.BottomLeft:
                            blnRowsReverse = true;
                            blnEachRowReverse = true;
                            break;

                        case FirstPixelDestination.BottomRight:
                        case FirstPixelDestination.UNKNOWN:
                            blnRowsReverse = true;
                            blnEachRowReverse = false;

                            break;
                    }

                    // write the bytes from each row into a memory stream and get the 
                    // resulting byte array
                    using (msData = new MemoryStream())
                    {

                        // do we reverse the rows in the row list.
                        if (blnRowsReverse == true)
                            _rows.Reverse();

                        // go through each row
                        for (int i = 0; i < _rows.Count; i++)
                        {
                            // do we reverse the bytes in the row
                            if (blnEachRowReverse == true)
                                _rows[i].Reverse();

                            // get the byte array for the row
                            byte[] brow = _rows[i].ToArray();

                            // write the row bytes and padding bytes to the memory streem
                            msData.Write(brow, 0, brow.Length);
                            msData.Write(padding, 0, padding.Length);
                        }
                        // get the image byte array
                        data = msData.ToArray(); 
                        
                        

                    }

                }
                else
                {
                    this.ClearAll();
                    throw new Exception(@"Error loading file, No image data in file.");
                }
            }
            else
            {
                this.ClearAll();
                throw new Exception(@"Error loading file, could not read file from disk.");
            }

            // return the image byte array
            return data;

        }

        /// <summary>
        /// Reads the image data bytes from the file and loads them into the Image Bitmap object.
        /// Also loads the color map, if any, into the Image Bitmap.
        /// </summary>
        /// <param name="binReader">A BinaryReader that points the loaded file byte stream.</param>
        private void LoadTgaImage(BinaryReader binReader)
        {
            //**************  NOTE  *******************
            // The memory allocated for Microsoft Bitmaps must be aligned on a 32bit boundary.
            // The stride refers to the number of bytes allocated for one scanline of the bitmap.
            // In your loop, you copy the pixels one scanline at a time and take into
            // consideration the amount of padding that occurs due to memory alignment.
            // calculate the stride, in bytes, of the image (32bit aligned width of each image row)
            this._intStride = (((int)this._objTargaHeader.Width * (int)this._objTargaHeader.PixelDepth + 31) & ~31) >> 3; // width in bytes

            // calculate the padding, in bytes, of the image 
            // number of bytes to add to make each row a 32bit aligned row
            // padding in bytes
            this._intPadding = this._intStride - ((((int)this._objTargaHeader.Width * (int)this._objTargaHeader.PixelDepth) + 7) / 8);

            // get the image data bytes
            byte[] bimagedata = this.LoadImageBytes(binReader);

            // since the Bitmap constructor requires a poiter to an array of image bytes
            // we have to pin down the memory used by the byte array and use the pointer 
            // of this pinned memory to create the Bitmap.
            // This tells the Garbage Collector to leave the memory alone and DO NOT touch it.
            this._imageByteHandle = GCHandle.Alloc(bimagedata, GCHandleType.Pinned);

            // make sure we don't have a phantom Bitmap
            if (this._bmpTargaImage != null)
            {
                this._bmpTargaImage.Dispose();
            }

            // make sure we don't have a phantom Thumbnail
            if (this._bmpImageThumbnail != null)
            {
                this._bmpImageThumbnail.Dispose();
            }


            // get the Pixel format to use with the Bitmap object
            PixelFormat pf = this.GetPixelFormat();


            // create a Bitmap object using the image Width, Height,
            // Stride, PixelFormat and the pointer to the pinned byte array.
            this._bmpTargaImage = new Bitmap((int)this._objTargaHeader.Width,
                                            (int)this._objTargaHeader.Height,
                                            this._intStride,
                                            pf,
                                            this._imageByteHandle.AddrOfPinnedObject());


            this.LoadThumbnail(binReader, pf);



            // load the color map into the Bitmap, if it exists
            if (this._objTargaHeader.ColorMap.Count > 0)
            {
                // get the Bitmap's current palette
                ColorPalette pal = this._bmpTargaImage.Palette;

                // loop trough each color in the loaded file's color map
                for (int i = 0; i < this._objTargaHeader.ColorMap.Count; i++)
                {
                    // is the AttributesType 0 or 1 bit
                    if (this._objTargaExtensionArea.AttributesType == 0 ||
                        this._objTargaExtensionArea.AttributesType == 1)
                        // use 255 for alpha ( 255 = opaque/visible ) so we can see the image
                        pal.Entries[i] = Color.FromArgb(255, this._objTargaHeader.ColorMap[i].R, this._objTargaHeader.ColorMap[i].G, this._objTargaHeader.ColorMap[i].B);

                    else
                        // use whatever value is there
                        pal.Entries[i] = this._objTargaHeader.ColorMap[i];

                }

                // set the new palette back to the Bitmap object
                this._bmpTargaImage.Palette = pal;

                // set the palette to the thumbnail also, if there is one
                if (this._bmpImageThumbnail != null)
                {
                    this._bmpImageThumbnail.Palette = pal;
                }
            }
            else
            { // no color map


                // check to see if this is a Black and White (Greyscale)
                if (this._objTargaHeader.PixelDepth == 8 && (this._objTargaHeader.ImageType == ImageType.UncompressedBlackAndWhite ||
                    this._objTargaHeader.ImageType == ImageType.RunLengthEncodedBlackAndWhite))
                {
                    // get the current palette
                    ColorPalette pal = this._bmpTargaImage.Palette;

                    // create the Greyscale palette
                    for (int i = 0; i < 256; i++)
                    {
                        pal.Entries[i] = Color.FromArgb(i, i, i);
                    }

                    // set the new palette back to the Bitmap object
                    this._bmpTargaImage.Palette = pal;

                    // set the palette to the thumbnail also, if there is one
                    if (this._bmpImageThumbnail != null)
                    {
                        this._bmpImageThumbnail.Palette = pal;
                    }
                }
                

            }
            
        }

        /// <summary>
        /// Gets the PixelFormat to be used by the Image based on the Targa file's attributes
        /// </summary>
        /// <returns></returns>
        private PixelFormat GetPixelFormat()
        {
            
            PixelFormat pfTargaPixelFormat = PixelFormat.Undefined;

            // first off what is our Pixel Depth (bits per pixel)
            switch (this._objTargaHeader.PixelDepth)
            {
                case 8:
                    pfTargaPixelFormat = PixelFormat.Format8bppIndexed;
                    break;

                case 16:
                    //PixelFormat.Format16bppArgb1555
                    //PixelFormat.Format16bppRgb555
                    if (this.Format == TgaFormat.NewTga)
                    {
                        switch (this._objTargaExtensionArea.AttributesType)
                        {
                            case 0:
                            case 1:
                            case 2: // no alpha data
                                pfTargaPixelFormat = PixelFormat.Format16bppRgb555;
                                break;

                            case 3: // useful alpha data
                                pfTargaPixelFormat = PixelFormat.Format16bppArgb1555;
                                break;
                        }
                    }
                    else
                    {
                        pfTargaPixelFormat = PixelFormat.Format16bppRgb555;
                    }

                    break;

                case 24:
                    pfTargaPixelFormat = PixelFormat.Format24bppRgb;
                    break;

                case 32:
                    //PixelFormat.Format32bppArgb
                    //PixelFormat.Format32bppPArgb
                    //PixelFormat.Format32bppRgb
                    if (this.Format == TgaFormat.NewTga)
                    {
                        switch (this._objTargaExtensionArea.AttributesType)
                        {
                            
                            case 1:
                            case 2: // no alpha data
                                pfTargaPixelFormat = PixelFormat.Format32bppRgb;
                                break;

                            case 0:
                            case 3: // useful alpha data
                                pfTargaPixelFormat = PixelFormat.Format32bppArgb;
                                break;

                            case 4: // premultiplied alpha data
                                pfTargaPixelFormat = PixelFormat.Format32bppPArgb;
                                break;

                        }
                    }
                    else
                    {
                        pfTargaPixelFormat = PixelFormat.Format32bppRgb;
                        break;
                    }

                    
                    
                    break;
                
            }


            return pfTargaPixelFormat;
        }

        
        /// <summary>
        /// Loads the thumbnail of the loaded image file, if any.
        /// </summary>
        /// <param name="binReader">A BinaryReader that points the loaded file byte stream.</param>
        /// <param name="pfPixelFormat">A PixelFormat value indicating what pixel format to use when loading the thumbnail.</param>
        private void LoadThumbnail(BinaryReader binReader, PixelFormat pfPixelFormat)
        {

            // read the Thumbnail image data into a byte array
            // take into account stride has to be a multiple of 4
            // use padding to make sure multiple of 4    

            byte[] data = null;
            if (binReader != null && binReader.BaseStream != null && binReader.BaseStream.Length > 0 && binReader.BaseStream.CanSeek == true)
            {
                if (this.ExtensionArea.PostageStampOffset > 0)
                {

                    // seek to the beginning of the image data using the ImageDataOffset value
                    binReader.BaseStream.Seek(this.ExtensionArea.PostageStampOffset, SeekOrigin.Begin);

                    int iWidth = (int)binReader.ReadByte();
                    int iHeight = (int)binReader.ReadByte();

                    int iStride = ((iWidth * (int)this._objTargaHeader.PixelDepth + 31) & ~31) >> 3; // width in bytes
                    int iPadding = iStride - (((iWidth * (int)this._objTargaHeader.PixelDepth) + 7) / 8);

                    System.Collections.Generic.List<System.Collections.Generic.List<byte>> objRows = new System.Collections.Generic.List<System.Collections.Generic.List<byte>>();
                    System.Collections.Generic.List<byte> objRow = new System.Collections.Generic.List<byte>();




                    byte[] padding = new byte[iPadding];
                    MemoryStream msData = null;
                    bool blnEachRowReverse = false;
                    bool blnRowsReverse = false;

                    
                    using (msData = new MemoryStream())
                    {
                        // get the size in bytes of each row in the image
                        int intImageRowByteSize = iWidth * ((int)this._objTargaHeader.PixelDepth / 8);

                        // get the size in bytes of the whole image
                        int intImageByteSize = intImageRowByteSize * iHeight;

                        // thumbnails are never compressed
                        for (int i = 0; i < iHeight; i++)
                        {
                            for (int j = 0; j < intImageRowByteSize; j++)
                            {
                                objRow.Add(binReader.ReadByte());
                            }
                            objRows.Add(objRow);
                            objRow = new System.Collections.Generic.List<byte>();
                        }
                        
                        switch (this._objTargaHeader.FirstPixelDestination)
                        {
                            case FirstPixelDestination.TopLeft:
                                break;

                            case FirstPixelDestination.TopRight:
                                blnRowsReverse = false;
                                blnEachRowReverse = false;
                                break;

                            case FirstPixelDestination.BottomLeft:
                                break;

                            case FirstPixelDestination.BottomRight:
                            case FirstPixelDestination.UNKNOWN:
                                blnRowsReverse = true;
                                blnEachRowReverse = false;

                                break;
                        }

                        if (blnRowsReverse == true)
                            objRows.Reverse();

                        for (int i = 0; i < objRows.Count; i++)
                        {
                            if (blnEachRowReverse == true)
                                objRows[i].Reverse();

                            byte[] brow = objRows[i].ToArray();
                            msData.Write(brow, 0, brow.Length);
                            msData.Write(padding, 0, padding.Length);
                        }
                        data = msData.ToArray();
                    }

                    if (data != null && data.Length > 0)
                    {
                        this._thumbnailByteHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                        this._bmpImageThumbnail = new Bitmap(iWidth, iHeight, iStride, pfPixelFormat,
                                                        this._thumbnailByteHandle.AddrOfPinnedObject());

                    }
                                       

                }
                else
                {
                    if (this._bmpImageThumbnail != null)
                    {
                        this._bmpImageThumbnail.Dispose();
                        this._bmpImageThumbnail = null;
                    }
                }
            }
            else
            {
                if (this._bmpImageThumbnail != null)
                {
                    this._bmpImageThumbnail.Dispose();
                    this._bmpImageThumbnail = null;
                }
            }

        }

        /// <summary>
        /// Clears out all objects and resources.
        /// </summary>
        private void ClearAll()
        {
            if (this._bmpTargaImage != null)
            {
                this._bmpTargaImage.Dispose();
                this._bmpTargaImage = null;
            }
            if (this._imageByteHandle.IsAllocated)
                this._imageByteHandle.Free();

            if (this._thumbnailByteHandle.IsAllocated)
                this._thumbnailByteHandle.Free();

            this._objTargaHeader = new TargaHeader();
            this._objTargaExtensionArea = new TargaExtensionArea();
            this._objTargaFooter = new TargaFooter();
            this._eTgaFormat = TgaFormat.UNKNOWN;
            this._intStride = 0;
            this._intPadding = 0;
            this._rows.Clear();
            this._row.Clear();
            this._strFileName = string.Empty;
        
        }

        /// <summary>
        /// Loads a Targa image file into a Bitmap object.
        /// </summary>
        /// <param name="sFileName">The Targa image filename</param>
        /// <returns>A Bitmap object with the Targa image loaded into it.</returns>
        public static Bitmap LoadTargaImage(string sFileName)
        {
            Bitmap b = null;
            using (TargaImage ti = new TargaImage(sFileName))
            {
                b = new Bitmap(ti.Image);
            }
            
            return b;
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes all resources used by this instance of the TargaImage class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);

        }


        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference 
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">If true dispose all resources, else dispose only release unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    if (this._bmpTargaImage != null)
                    {
                        this._bmpTargaImage.Dispose();
                    }

                    if (this._bmpImageThumbnail != null)
                    {
                        this._bmpImageThumbnail.Dispose();
                    }

                    if (this._imageByteHandle != null)
                    {
                        if (this._imageByteHandle.IsAllocated)
                        {
                            this._imageByteHandle.Free();
                        }
                       
                    }

                    if (this._thumbnailByteHandle != null)
                    {
                        if (this._thumbnailByteHandle.IsAllocated)
                        {
                            this._thumbnailByteHandle.Free();
                        }

                    }
                }
                // Release unmanaged resources. If disposing is false, 
                // only the following code is executed.
                // ** release unmanged resources here **

                // Note that this is not thread safe.
                // Another thread could start disposing the object
                // after the managed resources are disposed,
                // but before the disposed flag is set to true.
                // If thread safety is necessary, it must be
                // implemented by the client.

            }
            _disposed = true;
        }


        #endregion
    }


    /// <summary>
    /// This class holds all of the header properties of a Targa image. 
    /// This includes the TGA File Header section the ImageID and the Color Map.
    /// </summary>
    public class TargaHeader
    {
        private byte _bImageIdLength = 0;
        private ColorMapType _eColorMapType = ColorMapType.NoColorMap;
        private ImageType _eImageType = ImageType.NoImageData;
        private short _sColorMapFirstEntryIndex = 0;
        private short _sColorMapLength = 0;
        private byte _bColorMapEntrySize = 0;
        private short _sXOrigin = 0;
        private short _sYOrigin = 0;
        private short _sWidth = 0;
        private short _sHeight = 0;
        private byte _bPixelDepth = 0;
        private byte _bImageDescriptor = 0;
        private VerticalTransferOrder _eVerticalTransferOrder = VerticalTransferOrder.UNKNOWN;
        private HorizontalTransferOrder _eHorizontalTransferOrder = HorizontalTransferOrder.UNKNOWN;
        private byte _bAttributeBits = 0;
        private string _strImageIdValue = string.Empty;
        private System.Collections.Generic.List<System.Drawing.Color> _cColorMap = new List<System.Drawing.Color>();
        
        /// <summary>
        /// Gets the number of bytes contained the ImageIDValue property. The maximum
        /// number of characters is 255. A value of zero indicates that no ImageIDValue is included with the
        /// image.
        /// </summary>
        public byte ImageIdLength
        {
            get { return this._bImageIdLength; }
        }

        /// <summary>
        /// Sets the ImageIDLength property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="bImageIdLength">The Image ID Length value read from the file.</param>
        internal protected void SetImageIdLength(byte bImageIdLength)
        {
            this._bImageIdLength = bImageIdLength;
        }

        /// <summary>
        /// Gets the type of color map (if any) included with the image. There are currently 2
        /// defined values for this field:
        /// NO_COLOR_MAP - indicates that no color-map data is included with this image.
        /// COLOR_MAP_INCLUDED - indicates that a color-map is included with this image.
        /// </summary>
        public ColorMapType ColorMapType
        {
            get { return this._eColorMapType; }
        }

        /// <summary>
        /// Sets the ColorMapType property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="eColorMapType">One of the ColorMapType enumeration values.</param>
        internal protected void SetColorMapType(ColorMapType eColorMapType)
        {
            this._eColorMapType = eColorMapType;
        }

        /// <summary>
        /// Gets one of the ImageType enumeration values indicating the type of Targa image read from the file.
        /// </summary>
        public ImageType ImageType
        {
            get { return this._eImageType; }
        }

        /// <summary>
        /// Sets the ImageType property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="eImageType">One of the ImageType enumeration values.</param>
        internal protected void SetImageType(ImageType eImageType)
        {
            this._eImageType = eImageType;
        }

        /// <summary>
        /// Gets the index of the first color map entry. ColorMapFirstEntryIndex refers to the starting entry in loading the color map.
        /// </summary>
        public short ColorMapFirstEntryIndex
        {
            get { return this._sColorMapFirstEntryIndex; }
        }

        /// <summary>
        /// Sets the ColorMapFirstEntryIndex property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="sColorMapFirstEntryIndex">The First Entry Index value read from the file.</param>
        internal protected void SetColorMapFirstEntryIndex(short sColorMapFirstEntryIndex)
        {
            this._sColorMapFirstEntryIndex = sColorMapFirstEntryIndex;
        }

        /// <summary>
        /// Gets total number of color map entries included.
        /// </summary>
        public short ColorMapLength
        {
            get { return this._sColorMapLength; }
        }

        /// <summary>
        /// Sets the ColorMapLength property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="sColorMapLength">The Color Map Length value read from the file.</param>
        internal protected void SetColorMapLength(short sColorMapLength)
        {
            this._sColorMapLength = sColorMapLength;
        }

        /// <summary>
        /// Gets the number of bits per entry in the Color Map. Typically 15, 16, 24 or 32-bit values are used.
        /// </summary>
        public byte ColorMapEntrySize
        {
            get { return this._bColorMapEntrySize; }
        }

        /// <summary>
        /// Sets the ColorMapEntrySize property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="bColorMapEntrySize">The Color Map Entry Size value read from the file.</param>
        internal protected void SetColorMapEntrySize(byte bColorMapEntrySize)
        {
            this._bColorMapEntrySize = bColorMapEntrySize;
        }

        /// <summary>
        /// Gets the absolute horizontal coordinate for the lower
        /// left corner of the image as it is positioned on a display device having
        /// an origin at the lower left of the screen (e.g., the TARGA series).
        /// </summary>
        public short XOrigin
        {
            get { return this._sXOrigin; }
        }

        /// <summary>
        /// Sets the XOrigin property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="sXOrigin">The X Origin value read from the file.</param>
        internal protected void SetXOrigin(short sXOrigin)
        {
            this._sXOrigin = sXOrigin;
        }

        /// <summary>
        /// These bytes specify the absolute vertical coordinate for the lower left
        /// corner of the image as it is positioned on a display device having an
        /// origin at the lower left of the screen (e.g., the TARGA series).
        /// </summary>
        public short YOrigin
        {
            get { return this._sYOrigin; }
        }

        /// <summary>
        /// Sets the YOrigin property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="sYOrigin">The Y Origin value read from the file.</param>
        internal protected void SetYOrigin(short sYOrigin)
        {
            this._sYOrigin = sYOrigin;
        }

        /// <summary>
        /// Gets the width of the image in pixels.
        /// </summary>
        public short Width
        {
            get { return this._sWidth; }
        }

        /// <summary>
        /// Sets the Width property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="sWidth">The Width value read from the file.</param>
        internal protected void SetWidth(short sWidth)
        {
            this._sWidth = sWidth;
        }

        /// <summary>
        /// Gets the height of the image in pixels.
        /// </summary>
        public short Height
        {
            get { return this._sHeight; }
        }

        /// <summary>
        /// Sets the Height property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="sHeight">The Height value read from the file.</param>
        internal protected void SetHeight(short sHeight)
        {
            this._sHeight = sHeight;
        }

        /// <summary>
        /// Gets the number of bits per pixel. This number includes
        /// the Attribute or Alpha channel bits. Common values are 8, 16, 24 and 32.
        /// </summary>
        public byte PixelDepth
        {
            get { return this._bPixelDepth; }
        }

        /// <summary>
        /// Sets the PixelDepth property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="bPixelDepth">The Pixel Depth value read from the file.</param>
        internal protected void SetPixelDepth(byte bPixelDepth)
        {
            this._bPixelDepth = bPixelDepth;
        }

        /// <summary>
        /// Gets or Sets the ImageDescriptor property. The ImageDescriptor is the byte that holds the 
        /// Image Origin and Attribute Bits values.
        /// Available only to objects in the same assembly as TargaHeader.
        /// </summary>
        internal protected byte ImageDescriptor
        {
            get { return this._bImageDescriptor; }
            set { this._bImageDescriptor = value; }
        }

        /// <summary>
        /// Gets one of the FirstPixelDestination enumeration values specifying the screen destination of first pixel based on VerticalTransferOrder and HorizontalTransferOrder
        /// </summary>
        public FirstPixelDestination FirstPixelDestination
        {
            get 
            {

                if (this._eVerticalTransferOrder == VerticalTransferOrder.UNKNOWN || this._eHorizontalTransferOrder == HorizontalTransferOrder.UNKNOWN)
                     return FirstPixelDestination.UNKNOWN; 
                else if (this._eVerticalTransferOrder == VerticalTransferOrder.Bottom && this._eHorizontalTransferOrder == HorizontalTransferOrder.Left)
                    return FirstPixelDestination.BottomLeft;
                else if (this._eVerticalTransferOrder == VerticalTransferOrder.Bottom && this._eHorizontalTransferOrder == HorizontalTransferOrder.Right)
                    return FirstPixelDestination.BottomRight;
                else if (this._eVerticalTransferOrder == VerticalTransferOrder.Top && this._eHorizontalTransferOrder == HorizontalTransferOrder.Left)
                    return FirstPixelDestination.TopLeft;
                else 
                    return FirstPixelDestination.TopRight;
               
            }
        }


        /// <summary>
        /// Gets one of the VerticalTransferOrder enumeration values specifying the top-to-bottom ordering in which pixel data is transferred from the file to the screen.
        /// </summary>
        public VerticalTransferOrder VerticalTransferOrder
        {
            get { return this._eVerticalTransferOrder; }
        }

        /// <summary>
        /// Sets the VerticalTransferOrder property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="eVerticalTransferOrder">One of the VerticalTransferOrder enumeration values.</param>
        internal protected void SetVerticalTransferOrder(VerticalTransferOrder eVerticalTransferOrder)
        {
            this._eVerticalTransferOrder = eVerticalTransferOrder;
        }

        /// <summary>
        /// Gets one of the HorizontalTransferOrder enumeration values specifying the left-to-right ordering in which pixel data is transferred from the file to the screen.
        /// </summary>
        public HorizontalTransferOrder HorizontalTransferOrder
        {
            get { return this._eHorizontalTransferOrder; }
        }

        /// <summary>
        /// Sets the HorizontalTransferOrder property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="eHorizontalTransferOrder">One of the HorizontalTransferOrder enumeration values.</param>
        internal protected void SetHorizontalTransferOrder(HorizontalTransferOrder eHorizontalTransferOrder)
        {
            this._eHorizontalTransferOrder = eHorizontalTransferOrder;
        }

        /// <summary>
        /// Gets the number of attribute bits per pixel.
        /// </summary>
        public byte AttributeBits
        {
            get { return this._bAttributeBits; }
        }

        /// <summary>
        /// Sets the AttributeBits property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="bAttributeBits">The Attribute Bits value read from the file.</param>
        internal protected void SetAttributeBits(byte bAttributeBits)
        {
            this._bAttributeBits = bAttributeBits;
        }

        /// <summary>
        /// Gets identifying information about the image. 
        /// A value of zero in ImageIDLength indicates that no ImageIDValue is included with the image.
        /// </summary>
        public string ImageIdValue
        {
            get { return this._strImageIdValue; }
        }

        /// <summary>
        /// Sets the ImageIDValue property, available only to objects in the same assembly as TargaHeader.
        /// </summary>
        /// <param name="strImageIdValue">The Image ID value read from the file.</param>
        internal protected void SetImageIdValue(string strImageIdValue)
        {
            this._strImageIdValue = strImageIdValue;
        }

        /// <summary>
        /// Gets the Color Map of the image, if any. The Color Map is represented by a list of System.Drawing.Color objects.
        /// </summary>
        public System.Collections.Generic.List<System.Drawing.Color> ColorMap
        {
            get { return this._cColorMap; }
        }

        /// <summary>
        /// Gets the offset from the beginning of the file to the Image Data.
        /// </summary>
        public int ImageDataOffset
        {
            get 
            {
                // calculate the image data offset

                // start off with the number of bytes holding the header info.
                int intImageDataOffset = TargaConstants.HeaderByteLength;

                // add the Image ID length (could be variable)
                intImageDataOffset += this._bImageIdLength;

                // determine the number of bytes for each Color Map entry
                int bytes = 0;
                switch (this._bColorMapEntrySize)
                {
                    case 15:
                        bytes = 2;
                        break;
                    case 16:
                        bytes = 2;
                        break;
                    case 24:
                        bytes = 3;
                        break;
                    case 32:
                        bytes = 4;
                        break;
                }

                // add the length of the color map
                intImageDataOffset += ((int)this._sColorMapLength * (int)bytes);

                // return result
                return intImageDataOffset; 
            }
        }

        /// <summary>
        /// Gets the number of bytes per pixel.
        /// </summary>
        public int BytesPerPixel
        {
            get
            {
                return (int)this._bPixelDepth / 8;
            }
        }
    }


    /// <summary>
    /// Holds Footer infomation read from the image file.
    /// </summary>
    public class TargaFooter
    {
        private int _intExtensionAreaOffset = 0;
        private int _intDeveloperDirectoryOffset = 0;
        private string _strSignature = string.Empty;
        private string _strReservedCharacter = string.Empty;
        
        /// <summary>
        /// Gets the offset from the beginning of the file to the start of the Extension Area. 
        /// If the ExtensionAreaOffset is zero, no Extension Area exists in the file.
        /// </summary>
        public int ExtensionAreaOffset
        {
            get { return this._intExtensionAreaOffset; }
        }

        /// <summary>
        /// Sets the ExtensionAreaOffset property, available only to objects in the same assembly as TargaFooter.
        /// </summary>
        /// <param name="intExtensionAreaOffset">The Extension Area Offset value read from the file.</param>
        internal protected void SetExtensionAreaOffset(int intExtensionAreaOffset)
        {
            this._intExtensionAreaOffset = intExtensionAreaOffset;
        }

        /// <summary>
        /// Gets the offset from the beginning of the file to the start of the Developer Area.
        /// If the DeveloperDirectoryOffset is zero, then the Developer Area does not exist
        /// </summary>
        public int DeveloperDirectoryOffset
        {
            get { return this._intDeveloperDirectoryOffset; }
        }

        /// <summary>
        /// Sets the DeveloperDirectoryOffset property, available only to objects in the same assembly as TargaFooter.
        /// </summary>
        /// <param name="intDeveloperDirectoryOffset">The Developer Directory Offset value read from the file.</param>
        internal protected void SetDeveloperDirectoryOffset(int intDeveloperDirectoryOffset)
        {
            this._intDeveloperDirectoryOffset = intDeveloperDirectoryOffset;
        }

        /// <summary>
        /// This string is formatted exactly as "TRUEVISION-XFILE" (no quotes). If the
        /// signature is detected, the file is assumed to be a New TGA format and MAY,
        /// therefore, contain the Developer Area and/or the Extension Areas. If the
        /// signature is not found, then the file is assumed to be an Original TGA format.
        /// </summary>
        public string Signature
        {
            get { return this._strSignature; }
        }

        /// <summary>
        /// Sets the Signature property, available only to objects in the same assembly as TargaFooter.
        /// </summary>
        /// <param name="strSignature">The Signature value read from the file.</param>
        internal protected void SetSignature(string strSignature)
        {
            this._strSignature = strSignature;
        }

        /// <summary>
        /// A New Targa format reserved character "." (period)
        /// </summary>
        public string ReservedCharacter
        {
            get { return this._strReservedCharacter; }
        }

        /// <summary>
        /// Sets the ReservedCharacter property, available only to objects in the same assembly as TargaFooter.
        /// </summary>
        /// <param name="strReservedCharacter">The ReservedCharacter value read from the file.</param>
        internal protected void SetReservedCharacter(string strReservedCharacter)
        {
            this._strReservedCharacter = strReservedCharacter;
        }

        /// <summary>
        /// Creates a new instance of the TargaFooter class.
        /// </summary>
        public TargaFooter()
        {}
        

    }


    /// <summary>
    /// This class holds all of the Extension Area properties of the Targa image. If an Extension Area exists in the file.
    /// </summary>
    public class TargaExtensionArea
    {
        int _intExtensionSize = 0;
        string _strAuthorName = string.Empty;
        string _strAuthorComments = string.Empty;
        DateTime _dtDateTimeStamp = DateTime.Now;
        string _strJobName = string.Empty;
        TimeSpan _dtJobTime = TimeSpan.Zero;
        string _strSoftwareId = string.Empty;
        string _strSoftwareVersion = string.Empty;
        Color _cKeyColor = Color.Empty;
        int _intPixelAspectRatioNumerator = 0;
        int _intPixelAspectRatioDenominator = 0;
        int _intGammaNumerator = 0;
        int _intGammaDenominator = 0;
        int _intColorCorrectionOffset = 0;
        int _intPostageStampOffset = 0;
        int _intScanLineOffset = 0;
        int _intAttributesType = 0;
        private System.Collections.Generic.List<int> _intScanLineTable = new List<int>();
        private System.Collections.Generic.List<System.Drawing.Color> _cColorCorrectionTable = new List<System.Drawing.Color>();

        /// <summary>
        /// Gets the number of Bytes in the fixed-length portion of the ExtensionArea. 
        /// For Version 2.0 of the TGA File Format, this number should be set to 495
        /// </summary>
        public int ExtensionSize
        {
            get { return this._intExtensionSize; }
        }

        /// <summary>
        /// Sets the ExtensionSize property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="intExtensionSize">The Extension Size value read from the file.</param>
        internal protected void SetExtensionSize(int intExtensionSize)
        {
            this._intExtensionSize = intExtensionSize;
        }

        /// <summary>
        /// Gets the name of the person who created the image.
        /// </summary>
        public string AuthorName
        {
            get { return this._strAuthorName; }
        }

        /// <summary>
        /// Sets the AuthorName property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="strAuthorName">The Author Name value read from the file.</param>
        internal protected void SetAuthorName(string strAuthorName)
        {
            this._strAuthorName = strAuthorName;
        }

        /// <summary>
        /// Gets the comments from the author who created the image.
        /// </summary>
        public string AuthorComments
        {
            get { return this._strAuthorComments; }
        }

        /// <summary>
        /// Sets the AuthorComments property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="strAuthorComments">The Author Comments value read from the file.</param>
        internal protected void SetAuthorComments(string strAuthorComments)
        {
            this._strAuthorComments = strAuthorComments;
        }

        /// <summary>
        /// Gets the date and time that the image was saved.
        /// </summary>
        public DateTime DateTimeStamp
        {
            get { return this._dtDateTimeStamp; }
        }

        /// <summary>
        /// Sets the DateTimeStamp property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="dtDateTimeStamp">The Date Time Stamp value read from the file.</param>
        internal protected void SetDateTimeStamp(DateTime dtDateTimeStamp)
        {
            this._dtDateTimeStamp = dtDateTimeStamp;
        }

        /// <summary>
        /// Gets the name or id tag which refers to the job with which the image was associated.
        /// </summary>
        public string JobName
        {
            get { return this._strJobName; }
        }

        /// <summary>
        /// Sets the JobName property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="strJobName">The Job Name value read from the file.</param>
        internal protected void SetJobName(string strJobName)
        {
            this._strJobName = strJobName;
        }

        /// <summary>
        /// Gets the job elapsed time when the image was saved.
        /// </summary>
        public TimeSpan JobTime
        {
            get { return this._dtJobTime; }
        }

        /// <summary>
        /// Sets the JobTime property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="dtJobTime">The Job Time value read from the file.</param>
        internal protected void SetJobTime(TimeSpan dtJobTime)
        {
            this._dtJobTime = dtJobTime;
        }

        /// <summary>
        /// Gets the Software ID. Usually used to determine and record with what program a particular image was created.
        /// </summary>
        public string SoftwareId
        {
            get { return this._strSoftwareId; }
        }

        /// <summary>
        /// Sets the SoftwareID property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="strSoftwareId">The Software ID value read from the file.</param>
        internal protected void SetSoftwareId(string strSoftwareId)
        {
            this._strSoftwareId = strSoftwareId;
        }

        /// <summary>
        /// Gets the version of software defined by the SoftwareID.
        /// </summary>
        public string SoftwareVersion
        {
            get { return this._strSoftwareVersion; }
        }

        /// <summary>
        /// Sets the SoftwareVersion property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="strSoftwareVersion">The Software Version value read from the file.</param>
        internal protected void SetSoftwareVersion(string strSoftwareVersion)
        {
            this._strSoftwareVersion = strSoftwareVersion;
        }

        /// <summary>
        /// Gets the key color in effect at the time the image is saved.
        /// The Key Color can be thought of as the "background color" or "transparent color".
        /// </summary>
        public Color KeyColor
        {
            get { return this._cKeyColor; }
        }

        /// <summary>
        /// Sets the KeyColor property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="cKeyColor">The Key Color value read from the file.</param>
        internal protected void SetKeyColor(Color cKeyColor)
        {
            this._cKeyColor = cKeyColor;
        }

        /// <summary>
        /// Gets the Pixel Ratio Numerator.
        /// </summary>
        public int PixelAspectRatioNumerator
        {
            get { return this._intPixelAspectRatioNumerator; }
        }

        /// <summary>
        /// Sets the PixelAspectRatioNumerator property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="intPixelAspectRatioNumerator">The Pixel Aspect Ratio Numerator value read from the file.</param>
        internal protected void SetPixelAspectRatioNumerator(int intPixelAspectRatioNumerator)
        {
            this._intPixelAspectRatioNumerator = intPixelAspectRatioNumerator;
        }

        /// <summary>
        /// Gets the Pixel Ratio Denominator.
        /// </summary>
        public int PixelAspectRatioDenominator
        {
            get { return this._intPixelAspectRatioDenominator; }
        }

        /// <summary>
        /// Sets the PixelAspectRatioDenominator property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="intPixelAspectRatioDenominator">The Pixel Aspect Ratio Denominator value read from the file.</param>
        internal protected void SetPixelAspectRatioDenominator(int intPixelAspectRatioDenominator)
        {
            this._intPixelAspectRatioDenominator = intPixelAspectRatioDenominator;
        }

        /// <summary>
        /// Gets the Pixel Aspect Ratio.
        /// </summary>
        public float PixelAspectRatio
        {
            get 
            {
                if (this._intPixelAspectRatioDenominator > 0)
                {
                    return (float)this._intPixelAspectRatioNumerator / (float)this._intPixelAspectRatioDenominator;
                }
                else
                    return 0.0F; 
            }
        }

        /// <summary>
        /// Gets the Gamma Numerator.
        /// </summary>
        public int GammaNumerator
        {
            get { return this._intGammaNumerator; }
        }

        /// <summary>
        /// Sets the GammaNumerator property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="intGammaNumerator">The Gamma Numerator value read from the file.</param>
        internal protected void SetGammaNumerator(int intGammaNumerator)
        {
            this._intGammaNumerator = intGammaNumerator;
        }

        /// <summary>
        /// Gets the Gamma Denominator.
        /// </summary>
        public int GammaDenominator
        {
            get { return this._intGammaDenominator; }
        }

        /// <summary>
        /// Sets the GammaDenominator property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="intGammaDenominator">The Gamma Denominator value read from the file.</param>
        internal protected void SetGammaDenominator(int intGammaDenominator)
        {
            this._intGammaDenominator = intGammaDenominator;
        }

        /// <summary>
        /// Gets the Gamma Ratio.
        /// </summary>
        public float GammaRatio
        {
            get
            {
                if (this._intGammaDenominator > 0)
                {
                    float ratio = (float)this._intGammaNumerator / (float)this._intGammaDenominator;
                    return (float)Math.Round(ratio, 1);
                }
                else
                    return 1.0F;
            }
        }

        /// <summary>
        /// Gets the offset from the beginning of the file to the start of the Color Correction table.
        /// </summary>
        public int ColorCorrectionOffset
        {
            get { return this._intColorCorrectionOffset; }
        }

        /// <summary>
        /// Sets the ColorCorrectionOffset property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="intColorCorrectionOffset">The Color Correction Offset value read from the file.</param>
        internal protected void SetColorCorrectionOffset(int intColorCorrectionOffset)
        {
            this._intColorCorrectionOffset = intColorCorrectionOffset;
        }

        /// <summary>
        /// Gets the offset from the beginning of the file to the start of the Postage Stamp image data.
        /// </summary>
        public int PostageStampOffset
        {
            get { return this._intPostageStampOffset; }
        }

        /// <summary>
        /// Sets the PostageStampOffset property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="intPostageStampOffset">The Postage Stamp Offset value read from the file.</param>
        internal protected void SetPostageStampOffset(int intPostageStampOffset)
        {
            this._intPostageStampOffset = intPostageStampOffset;
        }

        /// <summary>
        /// Gets the offset from the beginning of the file to the start of the Scan Line table.
        /// </summary>
        public int ScanLineOffset
        {
            get { return this._intScanLineOffset; }
        }

        /// <summary>
        /// Sets the ScanLineOffset property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="intScanLineOffset">The Scan Line Offset value read from the file.</param>
        internal protected void SetScanLineOffset(int intScanLineOffset)
        {
            this._intScanLineOffset = intScanLineOffset;
        }

        /// <summary>
        /// Gets the type of Alpha channel data contained in the file.
        /// 0: No Alpha data included.
        /// 1: Undefined data in the Alpha field, can be ignored
        /// 2: Undefined data in the Alpha field, but should be retained
        /// 3: Useful Alpha channel data is present
        /// 4: Pre-multiplied Alpha (see description below)
        /// 5-127: RESERVED
        /// 128-255: Un-assigned
        /// </summary>
        public int AttributesType
        {
            get { return this._intAttributesType; }
        }

        /// <summary>
        /// Sets the AttributesType property, available only to objects in the same assembly as TargaExtensionArea.
        /// </summary>
        /// <param name="intAttributesType">The Attributes Type value read from the file.</param>
        internal protected void SetAttributesType(int intAttributesType)
        {
            this._intAttributesType = intAttributesType;
        }

        /// <summary>
        /// Gets a list of offsets from the beginning of the file that point to the start of the next scan line, 
        /// in the order that the image was saved 
        /// </summary>
        public System.Collections.Generic.List<int> ScanLineTable
        {
            get { return this._intScanLineTable; }
        }

        /// <summary>
        /// Gets a list of Colors where each Color value is the desired Color correction for that entry.
        /// This allows the user to store a correction table for image remapping or LUT driving.
        /// </summary>
        public System.Collections.Generic.List<System.Drawing.Color> ColorCorrectionTable
        {
            get { return this._cColorCorrectionTable; }
        }

    }
    

    /// <summary>
    /// Utilities functions used by the TargaImage class.
    /// </summary>
    static class Utilities
    {

        /// <summary>
        /// Gets an int value representing the subset of bits from a single Byte.
        /// </summary>
        /// <param name="b">The Byte used to get the subset of bits from.</param>
        /// <param name="offset">The offset of bits starting from the right.</param>
        /// <param name="count">The number of bits to read.</param>
        /// <returns>
        /// An int value representing the subset of bits.
        /// </returns>
        /// <remarks>
        /// Given -> b = 00110101 
        /// A call to GetBits(b, 2, 4)
        /// GetBits looks at the following bits in the byte -> 00{1101}00
        /// Returns 1101 as an int (13)
        /// </remarks>
        internal static int GetBits(byte b, int offset, int count)
        {
            return (b >> offset) & ((1 << count) - 1);
        }

        /// <summary>
        /// Reads ARGB values from the 16 bits of two given Bytes in a 1555 format.
        /// </summary>
        /// <param name="one">The first Byte.</param>
        /// <param name="two">The Second Byte.</param>
        /// <returns>A System.Drawing.Color with a ARGB values read from the two given Bytes</returns>
        /// <remarks>
        /// Gets the ARGB values from the 16 bits in the two bytes based on the below diagram
        /// |   BYTE 1   |  BYTE 2   |
        /// | A RRRRR GG | GGG BBBBB |
        /// </remarks>
        internal static Color GetColorFrom2Bytes(byte one, byte two)
        {
            // get the 5 bits used for the RED value from the first byte
            int r1 = Utilities.GetBits(one, 2, 5);
            int r = r1 << 3;

            // get the two high order bits for GREEN from the from the first byte
            int bit = Utilities.GetBits(one, 0, 2);
            // shift bits to the high order
            int g1 = bit << 6;

            // get the 3 low order bits for GREEN from the from the second byte
            bit = Utilities.GetBits(two, 5, 3);
            // shift the low order bits
            int g2 = bit << 3;
            // add the shifted values together to get the full GREEN value
            int g = g1 + g2;

            // get the 5 bits used for the BLUE value from the second byte
            int b1 = Utilities.GetBits(two, 0, 5);
            int b = b1 << 3;

            // get the 1 bit used for the ALPHA value from the first byte
            int a1 = Utilities.GetBits(one, 7, 1);
            int a = a1 * 255;

            // return the resulting Color
            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Gets a 32 character binary string of the specified Int32 value.
        /// </summary>
        /// <param name="n">The value to get a binary string for.</param>
        /// <returns>A string with the resulting binary for the supplied value.</returns>
        /// <remarks>
        /// This method was used during debugging and is left here just for fun.
        /// </remarks>
        internal static string GetIntBinaryString(Int32 n)
        {
            char[] b = new char[32];
            int pos = 31;
            int i = 0;

            while (i < 32)
            {
                if ((n & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return new string(b);
        }

        /// <summary>
        /// Gets a 16 character binary string of the specified Int16 value.
        /// </summary>
        /// <param name="n">The value to get a binary string for.</param>
        /// <returns>A string with the resulting binary for the supplied value.</returns>
        /// <remarks>
        /// This method was used during debugging and is left here just for fun.
        /// </remarks>
        internal static string GetInt16BinaryString(Int16 n)
        {
            char[] b = new char[16];
            int pos = 15;
            int i = 0;

            while (i < 16)
            {
                if ((n & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return new string(b);
        }

    }
}
