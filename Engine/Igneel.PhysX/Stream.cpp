#include "Stdafx.h"
#include "Stream.h"

UserStream::UserStream(const char* filename, bool load) : fp(NULL)
	{
	fp = fopen(filename, load ? "rb" : "wb");
	}

UserStream::~UserStream()
	{
	if(fp)	fclose(fp);
	}

// Loading API
NxU8 UserStream::readByte() const
	{
	NxU8 b;
	size_t r = fread(&b, sizeof(NxU8), 1, fp);
	NX_ASSERT(r);
	return b;
	}

NxU16 UserStream::readWord() const
	{
	NxU16 w;
	size_t r = fread(&w, sizeof(NxU16), 1, fp);
	NX_ASSERT(r);
	return w;
	}

NxU32 UserStream::readDword() const
	{
	NxU32 d;
	size_t r = fread(&d, sizeof(NxU32), 1, fp);
	NX_ASSERT(r);
	return d;
	}

float UserStream::readFloat() const
	{
	NxReal f;
	size_t r = fread(&f, sizeof(NxReal), 1, fp);
	NX_ASSERT(r);
	return f;
	}

double UserStream::readDouble() const
	{
	NxF64 f;
	size_t r = fread(&f, sizeof(NxF64), 1, fp);
	NX_ASSERT(r);
	return f;
	}

void UserStream::readBuffer(void* buffer, NxU32 size)	const
	{
	size_t w = fread(buffer, size, 1, fp);
	NX_ASSERT(w);
	}

// Saving API
NxStream& UserStream::storeByte(NxU8 b)
	{
	size_t w = fwrite(&b, sizeof(NxU8), 1, fp);
	NX_ASSERT(w);
	return *this;
	}

NxStream& UserStream::storeWord(NxU16 w)
	{
	size_t ww = fwrite(&w, sizeof(NxU16), 1, fp);
	NX_ASSERT(ww);
	return *this;
	}

NxStream& UserStream::storeDword(NxU32 d)
	{
	size_t w = fwrite(&d, sizeof(NxU32), 1, fp);
	NX_ASSERT(w);
	return *this;
	}

NxStream& UserStream::storeFloat(NxReal f)
	{
	size_t w = fwrite(&f, sizeof(NxReal), 1, fp);
	NX_ASSERT(w);
	return *this;
	}

NxStream& UserStream::storeDouble(NxF64 f)
	{
	size_t w = fwrite(&f, sizeof(NxF64), 1, fp);
	NX_ASSERT(w);
	return *this;
	}

NxStream& UserStream::storeBuffer(const void* buffer, NxU32 size)
	{
	size_t w = fwrite(buffer, size, 1, fp);
	NX_ASSERT(w);
	return *this;
	}




MemoryWriteBuffer::MemoryWriteBuffer() : currentSize(0), maxSize(0), data(NULL)
	{
	}

MemoryWriteBuffer::~MemoryWriteBuffer()
	{
	NX_DELETE_ARRAY(data);
	}

void MemoryWriteBuffer::clear()
	{
	currentSize = 0;
	}

NxStream& MemoryWriteBuffer::storeByte(NxU8 b)
	{
	storeBuffer(&b, sizeof(NxU8));
	return *this;
	}
NxStream& MemoryWriteBuffer::storeWord(NxU16 w)
	{
	storeBuffer(&w, sizeof(NxU16));
	return *this;
	}
NxStream& MemoryWriteBuffer::storeDword(NxU32 d)
	{
	storeBuffer(&d, sizeof(NxU32));
	return *this;
	}
NxStream& MemoryWriteBuffer::storeFloat(NxReal f)
	{
	storeBuffer(&f, sizeof(NxReal));
	return *this;
	}
NxStream& MemoryWriteBuffer::storeDouble(NxF64 f)
	{
	storeBuffer(&f, sizeof(NxF64));
	return *this;
	}
	NxStream& MemoryWriteBuffer::storeBuffer(const void* buffer, NxU32 size)
	{
		NxU32 expectedSize = currentSize + size;
		if(expectedSize > maxSize)
		{
			maxSize = expectedSize + 4096;

			NxU8* newData = new NxU8[maxSize];
			NX_ASSERT(newData!=NULL);

			if(data)
				{
				memcpy(newData, data, currentSize);
				delete[] data;
				}
			data = newData;
		}
		memcpy(data+currentSize, buffer, size);
		currentSize += size;
		return *this;
	}


MemoryReadBuffer::MemoryReadBuffer(const NxU8* data) : buffer(data)
	{
	}

MemoryReadBuffer::~MemoryReadBuffer()
	{
	// We don't own the data => no delete
	}

NxU8 MemoryReadBuffer::readByte() const
	{
	NxU8 b;
	memcpy(&b, buffer, sizeof(NxU8));
	buffer += sizeof(NxU8);
	return b;
	}

NxU16 MemoryReadBuffer::readWord() const
	{
	NxU16 w;
	memcpy(&w, buffer, sizeof(NxU16));
	buffer += sizeof(NxU16);
	return w;
	}

NxU32 MemoryReadBuffer::readDword() const
	{
	NxU32 d;
	memcpy(&d, buffer, sizeof(NxU32));
	buffer += sizeof(NxU32);
	return d;
	}

float MemoryReadBuffer::readFloat() const
	{
	float f;
	memcpy(&f, buffer, sizeof(float));
	buffer += sizeof(float);
	return f;
	}

double MemoryReadBuffer::readDouble() const
	{
	double f;
	memcpy(&f, buffer, sizeof(double));
	buffer += sizeof(double);
	return f;
	}

void MemoryReadBuffer::readBuffer(void* dest, NxU32 size) const
	{
	memcpy(dest, buffer, size);
	buffer += size;
	}


WrapperStream::WrapperStream(BinaryReader^ reader, BinaryWriter^ writter)
	{
		 this->reader = gcroot<BinaryReader^>(reader);
		 this->writer = gcroot<BinaryWriter^>(writter);
	}

WrapperStream::~WrapperStream()
{
	reader->Close();
	writer->Close();
}

// Loading API
NxU8 WrapperStream::readByte() const
{
	return reader->ReadByte();	
}

NxU16 WrapperStream::readWord() const
{
	return reader->ReadUInt16();	
}

NxU32 WrapperStream::readDword() const
	{
	  return reader->ReadUInt32();
	}

float WrapperStream::readFloat() const
	{
		return reader->ReadSingle();
	}

double WrapperStream::readDouble() const
	{
	 return reader->ReadDouble();	
	}

void WrapperStream::readBuffer(void* buffer, NxU32 size)	const
	{		
		auto data = reader->ReadBytes(size);
		Marshal::Copy(data,  0, static_cast<IntPtr>(buffer), data->Length);
	}

// Saving API
NxStream& WrapperStream::storeByte(NxU8 b)
	{
		writer->Write(b);
		return *this;
	}

NxStream& WrapperStream::storeWord(NxU16 w)
	{
		writer->Write(w);	
		return *this;
	}

NxStream& WrapperStream::storeDword(NxU32 d)
	{
	writer->Write(d);
	return *this;
	}

NxStream& WrapperStream::storeFloat(NxReal f)
	{
		writer->Write(f);
		return *this;
	}

NxStream& WrapperStream::storeDouble(NxF64 f)
	{
	writer->Write(f);
	return *this;
	}

NxStream& WrapperStream::storeBuffer(const void* buffer, NxU32 size)
	{
		auto arr = gcnew array<Byte>(size);
		Marshal::Copy(static_cast<IntPtr>((void*)buffer), arr,0,size);
		writer->Write(arr);

		return *this;
	}
