using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace AndriiCo.ConverterClient.ConverterApp.DataAccess
{
    /// <summary>
    /// Wraps a service client so you can use "using" without worrying of getting
    /// your business exception swallowed. Usage:
    /// <example>
    /// <code>
    /// using(var serviceClient = new ServiceClientWrapper&lt;IServiceContract&gt;)
    /// {
    /// serviceClient.Channel.YourServiceCall(request);
    /// }
    /// </code>
    /// </example>
    /// Sources copied from http://blog.2mas.xyz/disposible-wcf-client-wrapper-2/
    /// </summary>
    /// <typeparam name="T">The type of the service type.</typeparam>
    public sealed class ServiceClientWrapper<T> : IDisposable
    {
        private T _channel;
        /// <summary>
        /// Gets the channel.
        /// </summary>
        /// <value>The channel.</value>
        public T Channel
        {
            get { return _channel; }
        }

        private static ChannelFactory<T> _channelFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClientWrapper&lt;ServiceType&gt;"/> class.
        /// As a default the endpoint that is used is the one named after the contracts full name.
        /// </summary>
        public ServiceClientWrapper() : this(typeof(T).FullName)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClientWrapper&lt;ServiceType&gt;"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        public ServiceClientWrapper(string endpoint)
        {
            if (_channelFactory == null)
                _channelFactory = new ChannelFactory<T>(endpoint);
            _channel = _channelFactory.CreateChannel();
            ((IChannel)_channel).Open();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                ((IChannel)_channel).Close();
            }
            catch (CommunicationException)
            {
                ((IChannel)_channel).Abort();
            }
            catch (TimeoutException)
            {
                ((IChannel)_channel).Abort();
            }
            catch (Exception)
            {
                ((IChannel)_channel).Abort();
            }
        }
    }
}
