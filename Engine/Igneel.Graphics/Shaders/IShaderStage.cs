using Igneel.Collections;
using System;
namespace Igneel.Graphics
{
    
    public interface IShaderStage
    {
        ResourceCollecion<SamplerStateStack> SamplerStacks { get; }

        ResourceCollecion<SamplerState> Samplers { get; }

        ResourceCollecion<IShaderResource> Resources { get; }        

        void SetResource(int index, IShaderResource resource);        

        void SetResources(int index, int numResources, IShaderResource[] resources);

        void SetSampler(int index, SamplerState state);

        void SetSamplers(int index, int numSamplers, SamplerState[] states);
    }

    public interface IShaderStage<T> :IShaderStage, IShaderFactory<T>      
    {

    }

    public interface IVertexShaderStage : IShaderStage<VertexShader>
    {

    }

    public interface IPixelShaderStage : IShaderStage<PixelShader>
    {

    }
   

    public interface IGeometryShaderStage : IShaderStage<GeometryShader>
    {
        int NumberOfSOBuffers { get; }

        GeometryShader CreateShaderWithSO(ShaderCode bytecode, StreamOutDeclaration[] declaration, int[] bufferStrides, bool rasterizedStream0 = false);

        void SetSOBuffer(GraphicBuffer buffer, int offset = 0);

        void SetSOBuffer(GraphicBuffer[] buffers, int[] offsets = null);

        void GetSOBuffer(GraphicBuffer[] buffers, int[] offsets = null);
    }

    public interface IHullShaderStage : IShaderStage<HullShader>
    {

    }

    public interface IDomainShaderStage : IShaderStage<DomainShader>
    {

    }

    public interface IComputeShaderStage:IShaderStage<ComputeShader>
    {
        void DispatchCompute(int groupCountX, int groupCountY, int groupCountZ);
    }
}
