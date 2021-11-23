using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

namespace App1
{
    internal class Renderer
    {

        internal D3D11.Device2 m_Device;
        internal D3D11.DeviceContext m_DeviceContext;
        internal DXGI.SwapChain2 m_SwapChain;
        internal SwapChainPanel m_SwapChainPanel;

        internal Renderer(SwapChainPanel _swapChainPanel)
        {
            m_SwapChainPanel = _swapChainPanel;

            var swapChainDescription = new DXGI.SwapChainDescription1()
            {
                BufferCount = 2,
                Format = DXGI.Format.R8G8B8A8_UNorm,
                Height = 400,
                Width = 400,
                SampleDescription = new DXGI.SampleDescription(1, 0),
                Scaling = DXGI.Scaling.Stretch,
                Stereo = false,
                SwapEffect = DXGI.SwapEffect.FlipSequential,
                Usage = DXGI.Usage.RenderTargetOutput
            };

            using (var defaultDevice = new D3D11.Device(SharpDX.Direct3D.DriverType.Hardware, D3D11.DeviceCreationFlags.Debug))
            {
                m_Device = defaultDevice.QueryInterface<D3D11.Device2>();
                m_DeviceContext = m_Device.ImmediateContext2;
            }

            using (var dxgiDevice3 = m_Device.QueryInterface<DXGI.Device3>())
            using (var dxgiFactory3 = dxgiDevice3.Adapter.GetParent<DXGI.Factory3>())
            using (var swapChain1 = new DXGI.SwapChain1(dxgiFactory3, m_Device, ref swapChainDescription))
                m_SwapChain = swapChain1.QueryInterface<DXGI.SwapChain2>();

            using (var nativeObject = ComObject.As<DXGI.ISwapChainPanelNative>(m_SwapChainPanel))
                nativeObject.SwapChain = m_SwapChain;
        }
    }
}
